using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class UploadedFile
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string FILENAME = "FileName";
			public const string EXTENSION = "Extension";
			public const string DATA = "Data";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_UploadedFileDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_UploadedFileInsert]";

				public struct Parameters
				{
					public const string FILENAME = "@fileName";
					public const string EXTENSION = "@extension";
					public const string DATA = "@data";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_UploadedFileSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FILENAME = "FileName";
					public const string EXTENSION = "Extension";
					public const string DATA = "Data";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_UploadedFileSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FILENAME = "FileName";
					public const string EXTENSION = "Extension";
					//public const string DATA = "Data";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_UploadedFileUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string FILENAME = "@fileName";
					public const string EXTENSION = "@extension";
					public const string DATA = "@data";
				}
			}

		}
	}
}