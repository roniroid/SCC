using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class FormImportHistory
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string FORMID = "FormID";
			public const string UPLOADEDFILEID = "UploadedFileID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_FormImportHistoryDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_FormImportHistoryInsert]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
					public const string UPLOADEDFILEID = "@uploadedFileID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByFormID
			{
				public const string NAME = "[dbo].[usp_FormImportHistorySelectByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string UPLOADEDFILEID = "UploadedFileID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_FormImportHistoryUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string FORMID = "@formID";
					public const string UPLOADEDFILEID = "@uploadedFileID";
				}
			}

		}
	}
}