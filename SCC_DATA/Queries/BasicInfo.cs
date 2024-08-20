using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
    public class BasicInfo
    {
        public struct Fields
        {
            public const string ID = "ID";
            public const string CREATION_DATE = "CreationDate";
            public const string MODIFICATION_DATE = "ModificationDate";
            public const string CREATION_USER_ID = "CreationUserID";
            public const string MODIFICATION_USER_ID = "ModificationUserID";
            public const string STATUS_ID = "StatusID";
        }

        public struct StoredProcedures
        {
            public struct Select
            {
                public const string NAME = "[dbo].[usp_BasicInfoSelect]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string CREATION_DATE = "CreationDate";
                    public const string MODIFICATION_DATE = "ModificationDate";
                    public const string CREATION_USER_ID = "CreationUserID";
                    public const string MODIFICATION_USER_ID = "ModificationUserID";
                    public const string STATUS_ID = "StatusID";
                }
            }

            public struct Insert
            {
                public const string NAME = "[dbo].[usp_BasicInfoInsert]";

                public struct Parameters
                {
                    public const string CREATION_USER_ID = "@creationUserID";
                    public const string STATUS_ID = "@statusID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

            public struct Update
            {
                public const string NAME = "[dbo].[usp_BasicInfoUpdate]";

                public struct Parameters
                {
                    public const string ID = "@id";
                    public const string MODIFICATION_USER_ID = "@modificationUserID";
                    public const string STATUS_ID = "@statusID";
                }
            }

            public struct Delete
            {
                public const string NAME = "[dbo].[usp_BasicInfoDelete]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }
            }
        }
    }
}
