using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Settings
{
    public class Overall
    {
        public const int WRITE_LOG_MAX_ATTEMPTS = 10;
        public const string DEFAULT_LOG_NAME = "SCC";
        public const string LOG_PATH = "LOG_PATH";
        public const string TEST_MAIL = "TEST_MAIL";
        public const string LOG_DATE_FORMAT = "_yyyyMMdd";
        public const string SHOW_MESSAGE = "(() => { ShowMessage(%title%, %content%, %footerContent%, %type%); })();";

        public const int MAX_FILE_SIZE_MB = 1000;
        public static string[] ALLOWED_FILE_EXTENSIONS = { ".xls", ".xlsx", ".csv", ".xlt" };

        public const string DEFAULT_PASSWORD = ".Mu5T.ch4Ng3.p455W0rD.";
        public const int DEFAULT_PASSWORD_LENGTH = 15;

        public const int DEFAULT_ORDER_LENGTH = 5;

        public const int MIN_NON_EXISTING_ATTRIBUTE_GHOST_ID = 1000;
        public const int MIN_EXISTING_ATTRIBUTE_GHOST_ID = 4000;

        public static readonly string[] NEUTRAL_VALUES = {
                "NA",
                "N/A",
                "ND",
                "N/D",
                "VACIO",
                "VACÍO",
                "PENDIENTE",
                "No visible",
                "NO VISIBLE",
                string.Empty
        };

        public const int MAX_NUMBER_OF_ROWS_TO_CHECK = 10;

        public struct ErrorType
        {
            public const string ECUF = "ECUF";
            public const string ECN = "ECN";
            public const string ECC = "ECC";
            public const string ENC = "ENC";

            public const string BCE = "BCE";
            public const string CCE = "CCE";
            public const string NCE = "NCE";
            public const string UCE = "UCE";
            public const string EUCE = "EUCE";

            public static string[] List = { ECUF, ECN, ECC, ENC, BCE, CCE, NCE, UCE, EUCE };
        }

        public const int InitialAttributeGhostID = 1000;

        public struct ImportTasks
        {
            public struct Transaction
            {
                public const string REPEATED_COLUMN_COUNT_NUMBER = "%count%";
                public const string REPEATED_COLUMN_COUNT = "_" + REPEATED_COLUMN_COUNT_NUMBER;
                public const string REPEATED_COLUMN = "_REPEATED";
            }
        }
    }
}
