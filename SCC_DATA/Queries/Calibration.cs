using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class Calibration
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string STARTDATE = "StartDate";
			public const string ENDDATE = "EndDate";
			public const string DESCRIPTION = "Description";
			public const string TYPEID = "TypeID";
			public const string EXPERIENCEDUSERID = "ExperiencedUserID";
			public const string HASNOTIFICATIONTOBESENT = "HasNotificationToBeSent";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_CalibrationDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CalibrationInsert]";

				public struct Parameters
				{
					public const string STARTDATE = "@startDate";
					public const string ENDDATE = "@endDate";
					public const string DESCRIPTION = "@description";
					public const string TYPEID = "@typeID";
					public const string EXPERIENCEDUSERID = "@experiencedUserID";
					public const string HASNOTIFICATIONTOBESENT = "@hasNotificationToBeSent";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_CalibrationSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string DESCRIPTION = "Description";
					public const string TYPEID = "TypeID";
					public const string EXPERIENCEDUSERID = "ExperiencedUserID";
					public const string HASNOTIFICATIONTOBESENT = "HasNotificationToBeSent";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByProgramID
			{
				public const string NAME = "[dbo].[usp_CalibrationSelectByProgramID]";

                public struct Parameters
                {
                    public const string PROGRAM_ID = "@programID";
                }

                public struct ResultFields
				{
					public const string ID = "ID";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string DESCRIPTION = "Description";
					public const string TYPEID = "TypeID";
					public const string EXPERIENCEDUSERID = "ExperiencedUserID";
					public const string HASNOTIFICATIONTOBESENT = "HasNotificationToBeSent";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByUserID
			{
				public const string NAME = "[dbo].[usp_CalibrationSelectByUserID]";

                public struct Parameters
                {
                    public const string USER_ID = "@userID";
                }

                public struct ResultFields
				{
					public const string ID = "ID";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string DESCRIPTION = "Description";
					public const string TYPEID = "TypeID";
					public const string EXPERIENCEDUSERID = "ExperiencedUserID";
					public const string HASNOTIFICATIONTOBESENT = "HasNotificationToBeSent";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_CalibrationSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string STARTDATE = "StartDate";
					public const string ENDDATE = "EndDate";
					public const string DESCRIPTION = "Description";
					public const string TYPEID = "TypeID";
					public const string EXPERIENCEDUSERID = "ExperiencedUserID";
					public const string HASNOTIFICATIONTOBESENT = "HasNotificationToBeSent";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CalibrationUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string STARTDATE = "@startDate";
					public const string ENDDATE = "@endDate";
					public const string DESCRIPTION = "@description";
					public const string TYPEID = "@typeID";
					public const string EXPERIENCEDUSERID = "@experiencedUserID";
					public const string HASNOTIFICATIONTOBESENT = "@hasNotificationToBeSent";
				}
			}

		}
	}
}