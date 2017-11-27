using System.DirectoryServices;
using System.Linq;

namespace ActiveDirectoryHelper
{
	public class UserDetail
	{
		private readonly string _manager;


		private UserDetail(DirectoryEntry directoryUser)
		{
			string domainAddress;
			string domainName;
			Name = GetProperty(directoryUser, Properties.DISPLAYNAME);
			FirstName = GetProperty(directoryUser, Properties.FIRSTNAME);
			MiddleName = GetProperty(directoryUser, Properties.MIDDLENAME);
			LastName = GetProperty(directoryUser, Properties.LASTNAME);
			LoginName = GetProperty(directoryUser, Properties.LOGINNAME);
			var userPrincipalName = GetProperty(directoryUser, Properties.USERPRINCIPALNAME);
			if (!string.IsNullOrEmpty(userPrincipalName))
			{
				domainAddress = userPrincipalName.Split('@')[1];
			}
			else
			{
				domainAddress = string.Empty;
			}

			if (!string.IsNullOrEmpty(domainAddress))
			{
				domainName = domainAddress.Split('.').First();
			}
			else
			{
				domainName = string.Empty;
			}
			LoginNameWithDomain = string.Format(@"{0}\{1}", domainName, LoginName);
			StreetAddress = GetProperty(directoryUser, Properties.STREETADDRESS);
			City = GetProperty(directoryUser, Properties.CITY);
			State = GetProperty(directoryUser, Properties.STATE);
			PostalCode = GetProperty(directoryUser, Properties.POSTALCODE);
			Country = GetProperty(directoryUser, Properties.COUNTRY);
			Company = GetProperty(directoryUser, Properties.COMPANY);
			Department = GetProperty(directoryUser, Properties.DEPARTMENT);
			HomePhone = GetProperty(directoryUser, Properties.HOMEPHONE);
			Extension = GetProperty(directoryUser, Properties.EXTENSION);
			Mobile = GetProperty(directoryUser, Properties.MOBILE);
			Fax = GetProperty(directoryUser, Properties.FAX);
			Pager = GetProperty(directoryUser, Properties.PAGER);
			EmailAddress = GetProperty(directoryUser, Properties.EMAILADDRESS);
			Title = GetProperty(directoryUser, Properties.TITLE);
			_manager = GetProperty(directoryUser, Properties.MANAGER);
			Logoncount = GetProperty(directoryUser, Properties.LOGONCOUNT);
			if (!string.IsNullOrEmpty(_manager))
			{
				var managerArray = _manager.Split(',');
				ManagerName = managerArray[0].Replace("CN=", "");
			}
		}

		public string Department { get; }

		public string FirstName { get; }

		public string MiddleName { get; }

		public string LastName { get; }

		public string Name { get; }

		public string LoginName { get; }

		public string LoginNameWithDomain { get; }

		public string StreetAddress { get; }

		public string City { get; }

		public string State { get; }

		public string PostalCode { get; }

		public string Country { get; }

		public string HomePhone { get; }

		public string Extension { get; }

		public string Mobile { get; }

		public string Pager { get; }

		public string Fax { get; }

		public string EmailAddress { get; }

		public string Title { get; }

		public string Company { get; }

		public string Logoncount { get; }

		public UserDetail Manager
		{
			get
			{
				if (!string.IsNullOrEmpty(ManagerName))
				{
					var ad = new ActiveDirectoryHelper();
					return ad.GetUserByFullName(ManagerName);
				}
				return null;
			}
		}

		public string ManagerName { get; }


		private static string GetProperty(DirectoryEntry userDetail, string propertyName)
		{
			if (userDetail.Properties.Contains(propertyName))
			{
				return userDetail.Properties[propertyName][0].ToString();
			}
			return string.Empty;
		}

		public static UserDetail GetUser(DirectoryEntry directoryUser)
		{
			return new UserDetail(directoryUser);
		}
	}
}