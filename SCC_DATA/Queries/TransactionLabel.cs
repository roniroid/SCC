﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class TransactionLabel
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string DESCRIPTION = "Description";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_TransactionLabelDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_TransactionLabelInsert]";

				public struct Parameters
				{
					public const string DESCRIPTION = "@description";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_TransactionLabelSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string DESCRIPTION = "Description";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_TransactionLabelUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string DESCRIPTION = "@description";
				}
			}

		}
	}
}