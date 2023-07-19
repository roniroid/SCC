using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class UserGroupCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string USERID = "UserID";
			public const string GROUPID = "GroupID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_UserGroupCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_UserGroupCatalogInsert]";

				public struct Parameters
				{
					public const string USERID = "@userID";
					public const string GROUPID = "@groupID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByUserID
			{
				public const string NAME = "[dbo].[usp_UserGroupCatalogSelectByUserID]";

				public struct Parameters
				{
					public const string USERID = "@userID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string GROUPID = "GroupID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByGroupID
			{
				public const string NAME = "[dbo].[usp_UserGroupCatalogSelectByGroupID]";

				public struct Parameters
				{
					public const string GROUPID = "@groupID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string GROUPID = "GroupID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_UserGroupCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string USERID = "@userID";
					public const string GROUPID = "@groupID";
				}
			}

		}
	}
}