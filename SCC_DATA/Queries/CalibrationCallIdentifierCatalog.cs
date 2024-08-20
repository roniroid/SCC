using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
    public class CalibrationCallIdentifierCatalog
    {
        public struct Fields
        {
            public const string ID = "ID";
            public const string CALIBRATIONID = "CalibrationID";
            public const string CALL_IDENTIFIER = "CallIdentifier";
            public const string PROGRAM_ID = "ProgramID";
            public const string BASICINFOID = "BasicInfoID";
        }

        public struct StoredProcedures
        {
            public struct DeleteByID
            {
                public const string NAME = "[dbo].[usp_CalibrationCallIdentifierCatalogDeleteByID]";

                public struct Parameters
                {
                    public const string ID = "@id";
                }
            }

            public struct Insert
            {
                public const string NAME = "[dbo].[usp_CalibrationCallIdentifierCatalogInsert]";

                public struct Parameters
                {
                    public const string CALIBRATIONID = "@calibrationID";
                    public const string CALL_IDENTIFIER = "@callIdentifier";
                    public const string PROGRAM_ID = "@programID";
                    public const string BASICINFOID = "@basicInfoID";
                    //programID
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                }
            }

            public struct SelectByCalibrationID
            {
                public const string NAME = "[dbo].[usp_CalibrationCallIdentifierCatalogSelectByCalibrationID]";

                public struct Parameters
                {
                    public const string CALIBRATIONID = "@calibrationID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string CALIBRATIONID = "CalibrationID";
                    public const string CALL_IDENTIFIER = "CallIdentifier";
                    public const string PROGRAM_ID = "ProgramID";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct SelectByCallIdentifier
            {
                public const string NAME = "[dbo].[usp_CalibrationCallIdentifierCatalogSelectByCallIdentifier]";

                public struct Parameters
                {
                    public const string CALL_IDENTIFIER = "@callIdentifier";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string CALIBRATIONID = "CalibrationID";
                    public const string CALL_IDENTIFIER = "CallIdentifier";
                    public const string PROGRAM_ID = "ProgramID";
                    public const string BASICINFOID = "BasicInfoID";
                }
            }

            public struct Update
            {
                public const string NAME = "[dbo].[usp_CalibrationCallIdentifierCatalogUpdate]";

                public struct Parameters
                {
                    public const string ID = "@id";
                    public const string CALIBRATIONID = "@calibrationID";
                    public const string CALL_IDENTIFIER = "@callIdentifier";
                    public const string PROGRAM_ID = "@programID";
                }
            }

        }
    }
}