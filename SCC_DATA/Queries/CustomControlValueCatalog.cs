using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class CustomControlValueCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string CUSTOMCONTROLID = "CustomControlID";
			public const string NAME = "Name";
			public const string VALUE = "Value";
			public const string ISDEFAULTVALUE = "IsDefaultValue";
			public const string ORDER = "Order";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_CustomControlValueCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CustomControlValueCatalogInsert]";

				public struct Parameters
				{
					public const string CUSTOMCONTROLID = "@customControlID";
					public const string NAME = "@name";
					public const string VALUE = "@value";
					public const string ISDEFAULTVALUE = "@isDefaultValue";
					public const string ORDER = "@order";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByCustomControlID
			{
				public const string NAME = "[dbo].[usp_CustomControlValueCatalogSelectByCustomControlID]";

				public struct Parameters
				{
					public const string CUSTOMCONTROLID = "@customControlID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CUSTOMCONTROLID = "CustomControlID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string ISDEFAULTVALUE = "IsDefaultValue";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_CustomControlValueCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CUSTOMCONTROLID = "CustomControlID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string ISDEFAULTVALUE = "IsDefaultValue";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByCustomControlIDAndValue
			{
				public const string NAME = "[dbo].[usp_CustomControlValueCatalogSelectByCustomControlIDAndValue]";

				public struct Parameters
				{
					public const string CUSTOM_CONTROL_ID = "@customControlID";
					public const string VALUE = "@value";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CUSTOMCONTROLID = "CustomControlID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string ISDEFAULTVALUE = "IsDefaultValue";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CustomControlValueCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string CUSTOMCONTROLID = "@customControlID";
					public const string NAME = "@name";
					public const string VALUE = "@value";
					public const string ISDEFAULTVALUE = "@isDefaultValue";
					public const string ORDER = "@order";
				}
			}

		}
	}
}