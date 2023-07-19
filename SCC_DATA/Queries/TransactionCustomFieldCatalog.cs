using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class TransactionCustomFieldCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string TRANSACTIONID = "TransactionID";
			public const string CUSTOMFIELDID = "CustomFieldID";
			public const string COMMENT = "Comment";
			public const string VALUEID = "ValueID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionCustomFieldCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionCustomFieldCatalogInsert]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
					public const string CUSTOMFIELDID = "@customFieldID";
					public const string COMMENT = "@comment";
					public const string VALUEID = "@valueID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionCustomFieldCatalogSelectByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string CUSTOMFIELDID = "CustomFieldID";
					public const string COMMENT = "Comment";
					public const string VALUEID = "ValueID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_TransactionCustomFieldCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string CUSTOMFIELDID = "CustomFieldID";
					public const string COMMENT = "Comment";
					public const string VALUEID = "ValueID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionCustomFieldCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string TRANSACTIONID = "@transactionID";
					public const string CUSTOMFIELDID = "@customFieldID";
					public const string COMMENT = "@comment";
					public const string VALUEID = "@valueID";
				}
			}

		}
	}
}