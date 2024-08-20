using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class TransactionBIFieldCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string TRANSACTIONID = "TransactionID";
			public const string BIFIELDID = "BIFieldID";
			public const string COMMENT = "Comment";
			/*public const string VALUEID = "ValueID";*/
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionBIFieldCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionBIFieldCatalogInsert]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
					public const string BIFIELDID = "@bIFieldID";
					public const string COMMENT = "@comment";
					/*public const string VALUEID = "@valueID";*/
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
                public const string NAME = "[dbo].[usp_TransactionBIFieldCatalogSelectByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }

                public struct ResultFields
                {
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string BIFIELDID = "BIFieldID";
					public const string COMMENT = "Comment";
					/*public const string VALUEID = "ValueID";*/
					public const string BASICINFOID = "BasicInfoID";
					public const string CHECKED = "Checked";
                }
            }

            public struct SelectByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionBIFieldCatalogSelectByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TRANSACTIONID = "TransactionID";
					public const string BIFIELDID = "BIFieldID";
					public const string COMMENT = "Comment";
					/*public const string VALUEID = "ValueID";*/
					public const string BASICINFOID = "BasicInfoID";
					public const string CHECKED = "Checked";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionBIFieldCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string TRANSACTIONID = "@transactionID";
					public const string BIFIELDID = "@bIFieldID";
					public const string COMMENT = "@comment";
					/*public const string VALUEID = "@valueID";*/
					public const string CHECKED = "@checked";
				}
			}

		}
	}
}