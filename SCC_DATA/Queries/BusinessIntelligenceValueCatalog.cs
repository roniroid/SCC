using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class BusinessIntelligenceValueCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string BIFIELDID = "BIFieldID";
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
				public const string NAME = "[dbo].[usp_BusinessIntelligenceValueCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceValueCatalogInsert]";

				public struct Parameters
				{
					public const string BIFIELDID = "@bIFieldID";
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

			public struct SelectByBIFieldID
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceValueCatalogSelectByBIFieldID]";

				public struct Parameters
				{
					public const string BIFIELDID = "@bIFieldID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string BIFIELDID = "BIFieldID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string TRIGGERSCHILDVISUALIZATION = "TriggersChildVisualization";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceValueCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string BIFIELDID = "BIFieldID";
					public const string NAME = "Name";
					public const string VALUE = "Value";
					public const string TRIGGERSCHILDVISUALIZATION = "TriggersChildVisualization";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceValueCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string BIFIELDID = "@bIFieldID";
					public const string NAME = "@name";
					public const string VALUE = "@value";
					public const string TRIGGERSCHILDVISUALIZATION = "@triggersChildVisualization";
					public const string ORDER = "@order";
				}
			}

		}
	}
}