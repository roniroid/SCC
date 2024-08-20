using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
    public class UserNotification
    {
        public struct Fields
        {
            public const string ID = "ID";
            public const string USERID = "UserID";
            public const string MESSAGE = "Message";
            public const string TYPEID = "TypeID";
            public const string BASICINFOID = "BasicInfoID";
        }

        public struct StoredProcedures
        {
            public struct DeleteByID
            {
                public const string NAME = "[dbo].[usp_UserNotificationDeleteByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }
            }

            public struct Insert
            {
                public const string NAME = "[dbo].[usp_UserNotificationInsert]";

                public struct Parameters
                {
                    public const string USERID = "@userID";
                    public const string MESSAGE = "@message";
                    public const string TYPEID = "@typeID";
                    public const string BASICINFOID = "@basicInfoID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

            public struct SelectByID
            {
                public const string NAME = "[dbo].[usp_UserNotificationSelectByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string USERID = "UserID";
                    public const string MESSAGE = "Message";
                    public const string TYPEID = "TypeID";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct SelectByUserID
            {
                public const string NAME = "[dbo].[usp_UserNotificationSelectByUserID]";

                public struct Parameters
                {
                    public const string USER_ID = "@userID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string USERID = "UserID";
                    public const string MESSAGE = "Message";
                    public const string TYPEID = "TypeID";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct Update
            {
                public const string NAME = "[dbo].[usp_UserNotificationUpdate]";

                public struct Parameters
                {
                    public const string ID = "@id";
                    public const string USERID = "@userID";
                    public const string MESSAGE = "@message";
                    public const string TYPEID = "@typeID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

        }
    }
}