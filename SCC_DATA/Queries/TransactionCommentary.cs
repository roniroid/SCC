using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class TransactionCommentary
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string TYPEID = "TypeID";
			public const string TRANSACTIONID = "TransactionID";
			public const string COMMENT = "Comment";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionCommentaryDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionCommentaryInsert]";

				public struct Parameters
				{
					public const string TYPEID = "@typeID";
					public const string TRANSACTIONID = "@transactionID";
					public const string COMMENT = "@comment";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_TransactionCommentarySelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TYPEID = "TypeID";
					public const string TRANSACTIONID = "TransactionID";
					public const string COMMENT = "Comment";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByTransactionID
			{
				public const string NAME = "[dbo].[usp_TransactionCommentarySelectByTransactionID]";

				public struct Parameters
				{
					public const string TRANSACTIONID = "@transactionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string TYPEID = "TypeID";
					public const string TRANSACTIONID = "TransactionID";
					public const string COMMENT = "Comment";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionCommentaryUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string TYPEID = "@typeID";
					public const string TRANSACTIONID = "@transactionID";
					public const string COMMENT = "@comment";
				}
			}

		}
	}
}