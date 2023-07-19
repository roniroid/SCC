using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class UserSupervisorCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string USERID = "UserID";
			public const string SUPERVISORID = "SupervisorID";
			public const string STARTDATE = "StartDate";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_UserSupervisorCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_UserSupervisorCatalogInsert]";

				public struct Parameters
				{
					public const string USERID = "@userID";
					public const string SUPERVISORID = "@supervisorID";
					public const string STARTDATE = "@startDate";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByUserID
			{
				public const string NAME = "[dbo].[usp_UserSupervisorCatalogSelectByUserID]";

				public struct Parameters
				{
					public const string USERID = "@userID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string SUPERVISORID = "SupervisorID";
					public const string STARTDATE = "StartDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_UserSupervisorCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string USERID = "@userID";
					public const string SUPERVISORID = "@supervisorID";
					public const string STARTDATE = "@startDate";
				}
			}

		}
	}
}