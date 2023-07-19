using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class TransactionLabelCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string TRANSACTIONID = "TransactionID";
			public const string LABELID = "LabelID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionLabelCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionLabelCatalogInsert]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
					public const string LABELID = "@labelID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionLabelCatalogSelectByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string LABELID = "LabelID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionLabelCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string TRANSACTIONID = "@transactionID";
					public const string LABELID = "@labelID";
				}
			}

		}
	}
}