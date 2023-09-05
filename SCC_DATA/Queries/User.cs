﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class User
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string PERSONID = "PersonID";
			public const string USERNAME = "Username";
			public const string PASSWORD = "Password";
			public const string SALT = "Salt";
			public const string EMAIL = "Email";
			public const string STARTDATE = "StartDate";
			public const string LANGUAGEID = "LanguageID";
			public const string HASPASSPERMISSION = "HasPassPermission";
			public const string LASTLOGINDATE = "LastLoginDate";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct GetEmailByUsername
			{
				public const string NAME = "[dbo].[usp_UserCustomGetEmailByUsername]";

				public struct Parameters
				{
					public const string USERNAME = "@username";
				}

				public struct ResultFields
				{
					public const string EMAIL = "Email";
				}
			}

			public struct GetSaltByUsername
			{
				public const string NAME = "[dbo].[usp_UserCustomGetSaltByUsername]";

				public struct Parameters
				{
					public const string USERNAME = "@username";
				}

				public struct ResultFields
				{
					public const string SALT = "Salt";
				}
			}

			public struct ValidateLogIn
			{
				public const string NAME = "[dbo].[usp_UserCustomValidateLogIn]";

				public struct Parameters
				{
					public const string USERNAME = "@username";
					public const string PASSWORD = "@password";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_UserDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_UserInsert]";

				public struct Parameters
				{
					public const string PERSONID = "@personID";
					public const string USERNAME = "@username";
					public const string PASSWORD = "@password";
					public const string SALT = "@salt";
					public const string EMAIL = "@email";
					public const string STARTDATE = "@startDate";
					public const string LANGUAGEID = "@languageID";
					public const string HASPASSPERMISSION = "@hasPassPermission";
					public const string LASTLOGINDATE = "@lastLoginDate";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_UserSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PERSONID = "PersonID";
					public const string USERNAME = "Username";
					public const string PASSWORD = "Password";
					public const string SALT = "Salt";
					public const string EMAIL = "Email";
					public const string STARTDATE = "StartDate";
					public const string LANGUAGEID = "LanguageID";
					public const string HASPASSPERMISSION = "HasPassPermission";
					public const string LASTLOGINDATE = "LastLoginDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByRoleID
			{
				public const string NAME = "[dbo].[usp_UserSelectByRoleID]";

				public struct Parameters
				{
					public const string ROLE_ID = "@roleID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PERSONID = "PersonID";
					public const string USERNAME = "Username";
					public const string PASSWORD = "Password";
					public const string SALT = "Salt";
					public const string EMAIL = "Email";
					public const string STARTDATE = "StartDate";
					public const string LANGUAGEID = "LanguageID";
					public const string HASPASSPERMISSION = "HasPassPermission";
					public const string LASTLOGINDATE = "LastLoginDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectEvaluatorList
			{
				public const string NAME = "[dbo].[usp_UserEvaluatorSelect]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PERSONID = "PersonID";
					public const string USERNAME = "Username";
					public const string EMAIL = "Email";
					public const string STARTDATE = "StartDate";
					public const string LANGUAGEID = "LanguageID";
					public const string HASPASSPERMISSION = "HasPassPermission";
					public const string LASTLOGINDATE = "LastLoginDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_UserSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PERSONID = "PersonID";
					public const string USERNAME = "Username";
					public const string PASSWORD = "Password";
					public const string SALT = "Salt";
					public const string EMAIL = "Email";
					public const string STARTDATE = "StartDate";
					public const string LANGUAGEID = "LanguageID";
					public const string HASPASSPERMISSION = "HasPassPermission";
					public const string LASTLOGINDATE = "LastLoginDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByName
			{
				public const string NAME = "[dbo].[usp_UserSelectByName]";

				public struct Parameters
				{
					public const string FIRST_NAME = "@firstName";
					public const string SUR_NAME = "@surName";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PERSONID = "PersonID";
					public const string USERNAME = "Username";
					public const string PASSWORD = "Password";
					public const string SALT = "Salt";
					public const string EMAIL = "Email";
					public const string STARTDATE = "StartDate";
					public const string LANGUAGEID = "LanguageID";
					public const string HASPASSPERMISSION = "HasPassPermission";
					public const string LASTLOGINDATE = "LastLoginDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByUsername
			{
				public const string NAME = "[dbo].[usp_UserSelectByUsername]";

				public struct Parameters
				{
					public const string USERNAME = "@username";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PERSONID = "PersonID";
					public const string USERNAME = "Username";
					public const string PASSWORD = "Password";
					public const string SALT = "Salt";
					public const string EMAIL = "Email";
					public const string STARTDATE = "StartDate";
					public const string LANGUAGEID = "LanguageID";
					public const string HASPASSPERMISSION = "HasPassPermission";
					public const string LASTLOGINDATE = "LastLoginDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_UserUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string USERNAME = "@username";
					public const string EMAIL = "@email";
					public const string STARTDATE = "@startDate";
					public const string LANGUAGEID = "@languageID";
					public const string HASPASSPERMISSION = "@hasPassPermission";
				}
			}

			public struct UpdatePassword
			{
				public const string NAME = "[dbo].[usp_UserCustomUpdatePassword]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string PASSWORD = "@password";
				}
			}

			public struct UpdateLastLogin
			{
				public const string NAME = "[dbo].[usp_UserCustomUpdateLasLogin]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

		}
	}
}