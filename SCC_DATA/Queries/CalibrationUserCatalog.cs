using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class CalibrationUserCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string CALIBRATIONID = "CalibrationID";
			public const string USERID = "UserID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_CalibrationUserCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CalibrationUserCatalogInsert]";

				public struct Parameters
				{
					public const string CALIBRATIONID = "@calibrationID";
					public const string USERID = "@userID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByCalibrationID
			{
				public const string NAME = "[dbo].[usp_CalibrationUserCatalogSelectByCalibrationID]";

				public struct Parameters
				{
					public const string CALIBRATIONID = "@calibrationID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CALIBRATIONID = "CalibrationID";
					public const string USERID = "UserID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CalibrationUserCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string CALIBRATIONID = "@calibrationID";
					public const string USERID = "@userID";
				}
			}

		}
	}
}