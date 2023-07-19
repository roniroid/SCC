﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class Group
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string NAME = "Name";
			public const string APPLICABLEMODULEID = "ApplicableModuleID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_GroupDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_GroupInsert]";

				public struct Parameters
				{
					public const string NAME = "@name";
					public const string APPLICABLEMODULEID = "@applicableModuleID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_GroupSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string APPLICABLEMODULEID = "ApplicableModuleID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_GroupSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string APPLICABLEMODULEID = "ApplicableModuleID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_GroupUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string NAME = "@name";
					public const string APPLICABLEMODULEID = "@applicableModuleID";
				}
			}

		}
	}
}