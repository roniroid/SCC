using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class Catalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string CATEGORYID = "CategoryID";
			public const string DESCRIPTION = "Description";
			public const string ACTIVE = "Active";
		}

		public struct StoredProcedures
		{
			public struct Delete
			{
				public const string NAME = "[dbo].[usp_CatalogDelete]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CatalogInsert]";

				public struct Parameters
				{
					public const string CATEGORYID = "@categoryID";
					public const string DESCRIPTION = "@description";
					public const string ACTIVE = "@active";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct Select
			{
				public const string NAME = "[dbo].[usp_CatalogSelect]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CATEGORYID = "CategoryID";
					public const string DESCRIPTION = "Description";
					public const string ACTIVE = "Active";
				}
			}

			public struct SelectByDescription
			{
				public const string NAME = "[dbo].[usp_CatalogSelectByDescription]";

				public struct Parameters
				{
					public const string DESCRIPTION = "@description";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CATEGORYID = "CategoryID";
					public const string DESCRIPTION = "Description";
					public const string ACTIVE = "Active";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_CatalogSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CATEGORYID = "CategoryID";
					public const string DESCRIPTION = "Description";
					public const string ACTIVE = "Active";
				}
			}

			public struct SelectByCategoryID
			{
				public const string NAME = "[dbo].[usp_CatalogSelectByCategoryID]";

				public struct Parameters
				{
					public const string CATEGORYID = "@categoryID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CATEGORYID = "CategoryID";
					public const string DESCRIPTION = "Description";
					public const string ACTIVE = "Active";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string CATEGORYID = "@categoryID";
					public const string DESCRIPTION = "@description";
					public const string ACTIVE = "@active";
				}
			}

		}
	}
}