using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class Form
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string NAME = "Name";
			public const string TYPEID = "TypeID";
			public const string COMMENT = "Comment";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_FormDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_FormInsert]";

				public struct Parameters
				{
					public const string NAME = "@name";
					public const string TYPEID = "@typeID";
					public const string COMMENT = "@comment";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_FormSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string TYPEID = "TypeID";
					public const string COMMENT = "Comment";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByName
			{
				public const string NAME = "[dbo].[usp_FormSelectByName]";

				public struct Parameters
				{
					public const string NAME = "@name";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string TYPEID = "TypeID";
					public const string COMMENT = "Comment";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_FormSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string TYPEID = "TypeID";
					public const string COMMENT = "Comment";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_FormUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string NAME = "@name";
					public const string TYPEID = "@typeID";
					public const string COMMENT = "@comment";
				}
			}

		}
	}
}