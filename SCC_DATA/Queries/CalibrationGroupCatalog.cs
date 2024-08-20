using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class CalibrationGroupCatalog
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string CALIBRATIONID = "CalibrationID";
			public const string GROUPID = "GroupID";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_CalibrationGroupCatalogDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CalibrationGroupCatalogInsert]";

				public struct Parameters
				{
					public const string CALIBRATIONID = "@calibrationID";
					public const string GROUPID = "@groupID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByCalibrationID
			{
				public const string NAME = "[dbo].[usp_CalibrationGroupCatalogSelectByCalibrationID]";

				public struct Parameters
				{
					public const string CALIBRATIONID = "@calibrationID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string CALIBRATIONID = "CalibrationID";
					public const string GROUPID = "GroupID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CalibrationGroupCatalogUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string CALIBRATIONID = "@calibrationID";
					public const string GROUPID = "@groupID";
				}
			}

		}
	}
}