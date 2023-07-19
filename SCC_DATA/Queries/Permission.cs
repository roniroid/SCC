using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class Permission
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
				public const string NAME = "[dbo].[usp_PermissionDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_PermissionInsert]";

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
				public const string NAME = "[dbo].[usp_PermissionSelectByID]";

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

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_PermissionSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string DESCRIPTION = "Description";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectTotalPermissionsByUser
			{
				public const string NAME = "[dbo].[usp_PermissionSelectTotalPermissionsByUserID]";

				public struct Parameters
				{
					public const string USERID = "@userID";
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
				public const string NAME = "[dbo].[usp_PermissionUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string DESCRIPTION = "@description";
				}
			}

		}
	}
}