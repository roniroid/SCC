using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class FormBIFieldCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string FORMID = "FormID";
			public const string BIFIELDID = "BIFieldID";
			public const string BASICINFOID = "BasicInfoID";
			public const string ORDER = "Order";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_FormBIFieldCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_FormBIFieldCatalogInsert]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
					public const string BIFIELDID = "@bIFieldID";
					public const string ORDER = "@order";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_FormBIFieldCatalogSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string BIFIELDID = "BIFieldID";
					public const string BASICINFOID = "BasicInfoID";
					public const string ORDER = "Order";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_FormBIFieldCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string BIFIELDID = "BIFieldID";
					public const string BASICINFOID = "BasicInfoID";
					public const string ORDER = "Order";
				}
			}

			public struct SelectByFormID
			{
				public const string NAME = "[dbo].[usp_FormBIFieldCatalogSelectByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string BIFIELDID = "BIFieldID";
					public const string BASICINFOID = "BasicInfoID";
					public const string ORDER = "Order";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_FormBIFieldCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string FORMID = "@formID";
					public const string BIFIELDID = "@bIFieldID";
					public const string ORDER = "@order";
				}
			}

		}
	}
}