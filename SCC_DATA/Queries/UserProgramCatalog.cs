using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class UserProgramCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string USERID = "UserID";
			public const string PROGRAMID = "ProgramID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_UserProgramCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_UserProgramCatalogInsert]";

				public struct Parameters
				{
					public const string USERID = "@userID";
					public const string PROGRAMID = "@programID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByUserID
			{
				public const string NAME = "[dbo].[usp_UserProgramCatalogSelectByUserID]";

				public struct Parameters
				{
					public const string USERID = "@userID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string USERID = "UserID";
					public const string PROGRAMID = "ProgramID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_UserProgramCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string USERID = "@userID";
					public const string PROGRAMID = "@programID";
				}
			}

		}
	}
}