using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class Log
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string CATEGORYID = "CategoryID";
			public const string ITEMID = "ItemID";
			public const string DESCRIPTION = "Description";
			public const string STATUSID = "StatusID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_LogDelete]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct DeleteByCategoryIDAndItemID
			{
				public const string NAME = "[dbo].[usp_LogDeleteByItemID]";

				public struct Parameters
				{
					public const string CATEGORYID = "@categoryID";
					public const string ITEMID = "@itemID";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_LogInsert]";

				public struct Parameters
				{
					public const string CATEGORYID = "@categoryID";
					public const string ITEMID = "@itemID";
					public const string DESCRIPTION = "@description";
					public const string STATUSID = "@statusID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByCategoryIDAndItemID
			{
				public const string NAME = "[dbo].[usp_LogSelectByItemID]";

				public struct Parameters
				{
					public const string CATEGORYID = "@categoryID";
					public const string ITEMID = "@itemID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CATEGORYID = "CategoryID";
					public const string ITEMID = "ItemID";
					public const string DESCRIPTION = "Description";
					public const string STATUSID = "StatusID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

		}
	}
}