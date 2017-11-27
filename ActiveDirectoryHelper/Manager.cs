using System.DirectoryServices.AccountManagement;

namespace ActiveDirectoryHelper
{
	public class Manager
	{
		readonly PrincipalContext context;

		public Manager()
		{
			context = new PrincipalContext(ContextType.Machine, "xxx", "xxx", "xxx");
		}


		public Manager(string domain, string container)
		{
			context = new PrincipalContext(ContextType.Domain, domain, container);
		}

		public Manager(string domain, string username, string password)
		{
//			context = new PrincipalContext(ContextType.Domain, username, password);
			context = new PrincipalContext(ContextType.Domain, null);
		}

		public bool AddUserToGroup(string userName, string groupName)
		{
			var done = false;
			var group = GroupPrincipal.FindByIdentity(context, groupName);
			if (group == null)
			{
				group = new GroupPrincipal(context, groupName);
			}
			var user = UserPrincipal.FindByIdentity(context, userName);
			if (user != null & group != null)
			{
				group.Members.Add(user);
				group.Save();
				done = (user.IsMemberOf(group));
			}
			return done;
		}

		public bool IsMemberOfGroup(string username, string groupName)
		{
			var group = GroupPrincipal.FindByIdentity(context, groupName);
			var user = UserPrincipal.FindByIdentity(context, username);

			if (user.IsMemberOf(group))
			{
				return true;
			}
			return false;
		}


		public bool RemoveUserFromGroup(string userName, string groupName)
		{
			var done = false;
			var user = UserPrincipal.FindByIdentity(context, userName);
			var group = GroupPrincipal.FindByIdentity(context, groupName);
			if (user != null & group != null)
			{
				group.Members.Remove(user);
				group.Save();
				done = !(user.IsMemberOf(group));
			}
			return done;
		}
	}
}