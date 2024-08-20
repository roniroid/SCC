using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class TransactionFileCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string TRANSACTIONID = "TransactionID";
			public const string UPLOADEDFILEID = "UploadedFileID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionFileCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionFileCatalogInsert]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
					public const string UPLOADEDFILEID = "@uploadedFileID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionFileCatalogSelectByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string UPLOADEDFILEID = "UploadedFileID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionFileCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string TRANSACTIONID = "@transactionID";
					public const string UPLOADEDFILEID = "@uploadedFileID";
				}
			}

		}
	}
}