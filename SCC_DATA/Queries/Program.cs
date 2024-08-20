using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class Program
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string NAME = "Name";
			public const string STARTDATE = "StartDate";
			public const string ENDDATE = "EndDate";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_ProgramDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_ProgramInsert]";

				public struct Parameters
				{
					public const string NAME = "@name";
					public const string STARTDATE = "@startDate";
					public const string ENDDATE = "@endDate";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_ProgramSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByName
			{
				public const string NAME = "[dbo].[usp_ProgramSelectByName]";

				public struct Parameters
				{
					public const string NAME = "@name";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_ProgramSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectWithForm
			{
				public const string NAME = "[dbo].[usp_ProgramSelectWithForm]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_ProgramUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string NAME = "@name";
					public const string STARTDATE = "@startDate";
					public const string ENDDATE = "@endDate";
				}
			}

		}
	}
}