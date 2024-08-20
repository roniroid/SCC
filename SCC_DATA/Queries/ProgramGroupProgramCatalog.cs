using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class ProgramGroupProgramCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string PROGRAMGROUPID = "ProgramGroupID";
			public const string PROGRAMID = "ProgramID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_ProgramGroupProgramCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_ProgramGroupProgramCatalogInsert]";

				public struct Parameters
				{
					public const string PROGRAMGROUPID = "@programGroupID";
					public const string PROGRAMID = "@programID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_ProgramGroupProgramCatalogSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMGROUPID = "ProgramGroupID";
					public const string PROGRAMID = "ProgramID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_ProgramGroupProgramCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMGROUPID = "ProgramGroupID";
					public const string PROGRAMID = "ProgramID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByProgramGroupID
			{
				public const string NAME = "[dbo].[usp_ProgramGroupProgramCatalogSelectByProgramGroupID]";

				public struct Parameters
				{
					public const string PROGRAMGROUPID = "@programGroupID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMGROUPID = "ProgramGroupID";
					public const string PROGRAMID = "ProgramID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_ProgramGroupProgramCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string PROGRAMGROUPID = "@programGroupID";
					public const string PROGRAMID = "@programID";
				}
			}

		}
	}
}