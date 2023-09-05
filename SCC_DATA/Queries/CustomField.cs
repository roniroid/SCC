using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class CustomField
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string FORMID = "FormID";
			public const string CUSTOMCONTROLID = "CustomControlID";
			public const string ORDER = "Order";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_CustomFieldDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CustomFieldInsert]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
					public const string CUSTOMCONTROLID = "@customControlID";
					public const string ORDER = "@order";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByFormID
			{
				public const string NAME = "[dbo].[usp_CustomFieldSelectByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string CUSTOMCONTROLID = "CustomControlID";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_CustomFieldSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string CUSTOMCONTROLID = "CustomControlID";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_CustomFieldSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string CUSTOMCONTROLID = "CustomControlID";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CustomFieldUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string FORMID = "@formID";
					public const string CUSTOMCONTROLID = "@customControlID";
					public const string ORDER = "@order";
				}
			}

		}
	}
}