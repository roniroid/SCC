using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class AttributeValueCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string ATTRIBUTEID = "AttributeID";
			public const string NAME = "Name";
			public const string VALUE = "Value";
			public const string TRIGGERSCHILDVISUALIZATION = "TriggersChildVisualization";
			public const string ORDER = "Order";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_AttributeValueCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_AttributeValueCatalogInsert]";

				public struct Parameters
				{
					public const string ATTRIBUTEID = "@attributeID";
					public const string NAME = "@name";
					public const string VALUE = "@value";
					public const string TRIGGERSCHILDVISUALIZATION = "@triggersChildVisualization";
					public const string ORDER = "@order";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByAttributeID
			{
				public const string NAME = "[dbo].[usp_AttributeValueCatalogSelectByAttributeID]";

				public struct Parameters
				{
					public const string ATTRIBUTEID = "@attributeID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string ATTRIBUTEID = "AttributeID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string TRIGGERSCHILDVISUALIZATION = "TriggersChildVisualization";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_AttributeValueCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string ATTRIBUTEID = "AttributeID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string TRIGGERSCHILDVISUALIZATION = "TriggersChildVisualization";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByAttributeIDAndValue
			{
				public const string NAME = "[dbo].[usp_AttributeValueCatalogSelectByAttributeIDAndValue]";

				public struct Parameters
				{
					public const string ATTRIBUTE_ID = "@attributeID";
					public const string VALUE = "@value";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string ATTRIBUTEID = "AttributeID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string TRIGGERSCHILDVISUALIZATION = "TriggersChildVisualization";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_AttributeValueCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string ATTRIBUTEID = "@attributeID";
					public const string NAME = "@name";
					public const string VALUE = "@value";
					public const string TRIGGERSCHILDVISUALIZATION = "@triggersChildVisualization";
					public const string ORDER = "@order";
				}
			}

		}
	}
}