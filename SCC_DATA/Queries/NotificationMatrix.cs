using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class NotificationMatrix
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string ENTITYID = "EntityID";
			public const string ACTIONID = "ActionID";
		}

		public struct StoredProcedures
		{
			public struct DeleteAll
			{
				public const string NAME = "[dbo].[usp_NotificationMatrixDeleteAll]";
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_NotificationMatrixInsert]";

				public struct Parameters
				{
					public const string ENTITYID = "@entityID";
					public const string ACTIONID = "@actionID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_NotificationMatrixSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string ENTITYID = "EntityID";
					public const string ACTIONID = "ActionID";
				}
			}

		}
	}
}