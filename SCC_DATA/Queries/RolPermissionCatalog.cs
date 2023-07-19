using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class RolPermissionCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string ROLID = "RolID";
			public const string PERMISSIONID = "PermissionID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_RolPermissionCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_RolPermissionCatalogInsert]";

				public struct Parameters
				{
					public const string ROLID = "@rolID";
					public const string PERMISSIONID = "@permissionID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByRolID
			{
				public const string NAME = "[dbo].[usp_RolPermissionCatalogSelectByRolID]";

				public struct Parameters
				{
					public const string ROLID = "@rolID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string ROLID = "RolID";
					public const string PERMISSIONID = "PermissionID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_RolPermissionCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string ROLID = "@rolID";
					public const string PERMISSIONID = "@permissionID";
				}
			}

		}
	}
}