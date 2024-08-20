using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
            public struct CheckExistence
            {
                public const string NAME = "[dbo].[usp_UserCustomCheckExistence]";

                public struct Parameters
                {
                    public const string USERNAME = "@username";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

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

            public struct SelectByProgramID
            {
                public const string NAME = "[dbo].[usp_UserSelectByProgramID]";

                public struct Parameters
                {
                    public const string PROGRAM_ID = "@programID";
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

			public struct SelectByPermissionID
			{
				public const string NAME = "[dbo].[usp_UserSelectByPermissionID]";

				public struct Parameters
				{
					public const string ROLE_ID = "@permissionID";
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

            public struct Search
            {
                public const string NAME = "[dbo].[usp_UserSearch]";

                public struct Parameters
                {
                    public const string IDENTIFICATION_TYPE = "@identificationType";
                    public const string IDENTIFICATION = "@identification";
                    public const string FIRST_NAME_TYPE = "@firstNameType";
                    public const string FIRST_NAME = "@firstName";
                    public const string SUR_NAME_TYPE = "@surNameType";
                    public const string SUR_NAME = "@surName";
                    public const string COUNTRY_ID_LIST = "@countryIDList";
                    public const string LANGUAGE_ID_LIST = "@languageIDList";

                    public const string USERNAME_TYPE = "@usernameType";
                    public const string USERNAME = "@username";
                    public const string EMAIL_TYPE = "@emailType";
                    public const string EMAIL = "@email";
                    public const string HAS_PASS_PERMISSION = "@hasPassPermission";
                    public const string USER_STATUS_ID = "@userStatusIDList";
                    public const string GROUP_ID_LIST = "@groupIDList";
                    public const string PERMISSION_ID_LIST = "@permissionIDList";
                    public const string PROGRAM_ID_LIST = "@programIDList";
                    public const string ROLE_ID_LIST = "@roleIDList";
                    public const string SUPERVISOR_ID_LIST = "@supervisorIDList";
                    public const string WORKSPACE_ID_LIST = "@workspaceIDList";
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
        }
	}
}