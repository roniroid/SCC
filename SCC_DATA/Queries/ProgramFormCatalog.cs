using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class ProgramFormCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string PROGRAMID = "ProgramID";
			public const string FORMID = "FormID";
			public const string STARTDATE = "StartDate";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogInsert]";

				public struct Parameters
				{
					public const string PROGRAMID = "@programID";
					public const string FORMID = "@formID";
					public const string STARTDATE = "@startDate";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMID = "ProgramID";
					public const string FORMID = "FormID";
					public const string STARTDATE = "StartDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByProgramID
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogSelectByProgramID]";

				public struct Parameters
				{
					public const string PROGRAMID = "@programID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMID = "ProgramID";
					public const string FORMID = "FormID";
					public const string STARTDATE = "StartDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMID = "ProgramID";
					public const string FORMID = "FormID";
					public const string STARTDATE = "StartDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByFormID
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogSelectByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string PROGRAMID = "ProgramID";
					public const string FORMID = "FormID";
					public const string STARTDATE = "StartDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_ProgramFormCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string PROGRAMID = "@programID";
					public const string FORMID = "@formID";
					public const string STARTDATE = "@startDate";
				}
			}

		}
	}
}