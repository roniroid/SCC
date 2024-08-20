using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class UserProgramGroupCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string USERID = "UserID";
			public const string PROGRAMGROUPID = "ProgramGroupID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_UserProgramGroupCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_UserProgramGroupCatalogInsert]";

				public struct Parameters
				{
					public const string USERID = "@userID";
					public const string PROGRAMGROUPID = "@programGroupID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_UserProgramGroupCatalogSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string PROGRAMGROUPID = "ProgramGroupID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_UserProgramGroupCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string PROGRAMGROUPID = "ProgramGroupID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByUserID
			{
				public const string NAME = "[dbo].[usp_UserProgramGroupCatalogSelectByUserID]";

				public struct Parameters
				{
					public const string USERID = "@userID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string PROGRAMGROUPID = "ProgramGroupID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_UserProgramGroupCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string USERID = "@userID";
					public const string PROGRAMGROUPID = "@programGroupID";
				}
			}

		}
	}
}