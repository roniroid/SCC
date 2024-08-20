using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class ProgramGroup
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string IDENTIFIER = "Identifier";
			public const string NAME = "Name";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_ProgramGroupDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_ProgramGroupInsert]";

				public struct Parameters
				{
					public const string IDENTIFIER = "@identifier";
					public const string NAME = "@name";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_ProgramGroupSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFIER = "Identifier";
					public const string NAME = "Name";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_ProgramGroupSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFIER = "Identifier";
					public const string NAME = "Name";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_ProgramGroupUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string IDENTIFIER = "@identifier";
					public const string NAME = "@name";
				}
			}

		}
	}
}