using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class TransactionAttributeCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string TRANSACTIONID = "TransactionID";
			public const string ATTRIBUTEID = "AttributeID";
			public const string COMMENT = "Comment";
			public const string VALUEID = "ValueID";
			public const string SCORE_VALUE = "ScoreValue";
			public const string CHECKED = "Checked";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionAttributeCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionAttributeCatalogInsert]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
					public const string ATTRIBUTEID = "@attributeID";
					public const string COMMENT = "@comment";
					public const string VALUEID = "@valueID";
					public const string SCORE_VALUE = "@scoreValue";
					public const string CHECKED = "@checked";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_TransactionAttributeCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string ATTRIBUTEID = "AttributeID";
					public const string COMMENT = "Comment";
					public const string VALUEID = "ValueID";
					public const string SCORE_VALUE = "ScoreValue";
					public const string CHECKED = "Checked";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionAttributeCatalogSelectByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string ATTRIBUTEID = "AttributeID";
					public const string COMMENT = "Comment";
					public const string VALUEID = "ValueID";
					public const string SCORE_VALUE = "ScoreValue";
					public const string CHECKED = "Checked";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAttributeIDListByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionAttributeCatalogSelectAttributeIDListByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ATTRIBUTE_ID = "AttributeID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionAttributeCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string TRANSACTIONID = "@transactionID";
					public const string ATTRIBUTEID = "@attributeID";
					public const string COMMENT = "@comment";
					public const string VALUEID = "@valueID";
					public const string SCORE_VALUE = "@scoreValue";
					public const string CHECKED = "@checked";
				}
			}

		}
	}
}