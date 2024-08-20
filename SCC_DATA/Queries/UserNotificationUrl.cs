using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
    public class UserNotificationUrl
    {
        public struct Fields
        {
            public const string ID = "ID";
            public const string CONTENT = "Content";
            public const string DESCRIPTION = "Description";
            public const string BASICINFOID = "BasicInfoID";
        }

        public struct StoredProcedures
        {
            public struct DeleteByID
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlDeleteByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }
            }

            public struct Insert
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlInsert]";

                public struct Parameters
                {
                    public const string CONTENT = "@content";
                    public const string DESCRIPTION = "@description";
                    public const string BASICINFOID = "@basicInfoID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

            public struct SelectByID
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlSelectByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string CONTENT = "Content";
                    public const string DESCRIPTION = "Description";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct Update
            {
                public const string NAME = "[dbo].[usp_UserNotificationUrlUpdate]";

                public struct Parameters
                {
                    public const string ID = "@id";
                    public const string CONTENT = "@content";
                    public const string DESCRIPTION = "@description";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

        }
    }
}