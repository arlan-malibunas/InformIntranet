using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace ActiveDirectoryHelper
{
	public class ActiveDirectoryHelper
	{
		private DirectoryEntry _directoryEntry;

		private DirectoryEntry SearchRoot
		{
			get
			{
				if (_directoryEntry == null)
				{
					_directoryEntry = new DirectoryEntry(LDAPPath, LDAPUser, LDAPPassword, AuthenticationTypes.Secure);
				}
				return _directoryEntry;
			}
		}

		private string LDAPPath
		{
			get { return ConfigurationManager.AppSettings["LDAPPath"]; }
		}

		private string LDAPUser
		{
			get { return ConfigurationManager.AppSettings["LDAPUser"]; }
		}

		private string LDAPPassword
		{
			get { return ConfigurationManager.AppSettings["LDAPPassword"]; }
		}

		private string LDAPDomain
		{
			get { return ConfigurationManager.AppSettings["LDAPDomain"]; }
		}

		internal UserDetail GetUserByFullName(string userName)
		{
			try
			{
				_directoryEntry = null;
				var directorySearch = new DirectorySearcher(SearchRoot);
				directorySearch.Filter = "(&(objectClass=user)(cn=" + userName + "))";
				var results = directorySearch.FindOne();

				if (results != null)
				{
					var user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
					return UserDetail.GetUser(user);
				}
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public UserDetail GetUserByLoginName(string userName)
		{
			try
			{
				_directoryEntry = null;
				var directorySearch = new DirectorySearcher(SearchRoot);
				directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
				var results = directorySearch.FindOne();

				if (results != null)
				{
					var user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
					return UserDetail.GetUser(user);
				}
				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		/// <summary>
		///     This function will take a DL or Group name and return list of users
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public List<UserDetail> GetUserFromGroup(string groupName)
		{
			var userlist = new List<UserDetail>();
			try
			{
				_directoryEntry = null;
				var directorySearch = new DirectorySearcher(SearchRoot);
				directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + groupName + "))";
				var results = directorySearch.FindOne();
				if (results != null)
				{
					var deGroup = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
					var pColl = deGroup.Properties;
					var count = pColl["member"].Count;


					for (var i = 0; i < count; i++)
					{
						var respath = results.Path;
						var pathnavigate = respath.Split("CN".ToCharArray());
						respath = pathnavigate[0];
						var objpath = pColl["member"][i].ToString();
						var path = respath + objpath;


						var user = new DirectoryEntry(path, LDAPUser, LDAPPassword);
						var userobj = UserDetail.GetUser(user);
						userlist.Add(userobj);
						user.Close();
					}
				}
				return userlist;
			}
			catch (Exception ex)
			{
				return userlist;
			}
		}

		#region Get user with First Name

		public List<UserDetail> GetUsersByFirstName(string fName)
		{
			//UserProfile user;
			var userlist = new List<UserDetail>();
			var filter = "";

			_directoryEntry = null;
			var directorySearch = new DirectorySearcher(SearchRoot);
			directorySearch.Asynchronous = true;
			directorySearch.CacheResults = true;
			filter = string.Format("(givenName={0}*", fName);
			//            filter = "(&(objectClass=user)(objectCategory=person)(givenName="+fName+ "*))";


			directorySearch.Filter = filter;

			var userCollection = directorySearch.FindAll();
			foreach (SearchResult users in userCollection)
			{
				var userEntry = new DirectoryEntry(users.Path, LDAPUser, LDAPPassword);
				var userInfo = UserDetail.GetUser(userEntry);

				userlist.Add(userInfo);
			}

			directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + fName + "*))";
			var results = directorySearch.FindAll();
			if (results != null)
			{
				foreach (SearchResult r in results)
				{
					var deGroup = new DirectoryEntry(r.Path, LDAPUser, LDAPPassword);

					var agroup = UserDetail.GetUser(deGroup);
					userlist.Add(agroup);
				}
			}
			return userlist;
		}

		#endregion

		#region RemoveUserToGroup

		public bool RemoveUserToGroup(string userlogin, string groupName)
		{
			try
			{
				_directoryEntry = null;
				var Manager = new Manager("xxx", LDAPUser, LDAPPassword);
				Manager.RemoveUserFromGroup(userlogin, groupName);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		#endregion

		#region AddUserToGroup

		public bool AddUserToGroup(string userlogin, string groupName)
		{
			try
			{
				_directoryEntry = null;
				var Manager = new Manager(LDAPDomain, LDAPUser, LDAPPassword);
				Manager.AddUserToGroup(userlogin, groupName);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool IsInGroup(string userName, string groupName)
		{
			var context = new PrincipalContext(ContextType.Domain, "informoffice.au");
			var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
			if (user != null)
			{
				var groups = user.GetAuthorizationGroups();
				return groups.Any(g => g.Name == groupName);
			}
			return false;
		}

		#endregion
	}
}