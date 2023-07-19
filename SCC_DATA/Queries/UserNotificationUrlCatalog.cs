using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
    public class UserNotificationUrlCatalog
    {
        public struct Fields
        {
            public const string ID = "ID";
            public const string USERNOTIFICATIONID = "UserNotificationID";
            public const string USERNOTIFICATIONURLID = "UserNotificationUrlID";
            public const string BASICINFOID = "BasicInfoID";
        }

        public struct StoredProcedures
        {
            public struct DeleteByID
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlCatalogDeleteByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }
            }

            public struct Insert
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlCatalogInsert]";

                public struct Parameters
                {
                    public const string USERNOTIFICATIONID = "@userNotificationID";
                    public const string USERNOTIFICATIONURLID = "@userNotificationUrlID";
                    public const string BASICINFOID = "@basicInfoID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

            public struct SelectByID
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlCatalogSelectByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string USERNOTIFICATIONID = "UserNotificationID";
                    public const string USERNOTIFICATIONURLID = "UserNotificationUrlID";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct SelectByUserNotificationID
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlCatalogSelectByUserNotificationID]";

                public struct Parameters
                {
                    public const string USER_NOTIFICATION_ID = "@userNotificationID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string USERNOTIFICATIONID = "UserNotificationID";
                    public const string USERNOTIFICATIONURLID = "UserNotificationUrlID";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct Update
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlCatalogUpdate]";

                public struct Parameters
                {
                    public const string ID = "@id";
                    public const string USERNOTIFICATIONID = "@userNotificationID";
                    public const string USERNOTIFICATIONURLID = "@userNotificationUrlID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

        }
    }
}