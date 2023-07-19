using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.DBValues
{
    public class Catalog
    {
        public enum Category
        {
            CATEGORY = 1,
            NOTIFICATION_MATRIX_ACTION = 3,
            NOTIFICATION_MATRIX_ENTITY = 4,
            ATTRIBUTE_ERROR_TYPE = 5,
            FORM_TYPE = 6,
            TRANSACTION_GENERAL_RESULT_FINAL = 7,
            TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR = 8,
            TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR = 9,
            TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR = 10,
            TRANSACTION_ACCURATE_RESULT_FINAL = 11,
            TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR = 12,
            TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR = 13,
            TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR = 14,
            TRANSACTION_CONTROLLABLE_RESULT_FINAL = 15,
            TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR = 16,
            TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR = 17,
            TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR = 18,
            MODULE = 19,
            CONTROL_TYPE = 20,
            CUSTOM_CONTROL_TYPE = 21,
            TRANSACTION_COMMENT_TYPE = 22,
            CALIBRATION_TYPE = 23,
            USER_LANGUAGE = 24, 
            STATUS_ATTRIBUTE = 25,
            STATUS_ATTRIBUTE_VALUE_CATALOG = 26,
            STATUS_BI_FIELD = 27,
            STATUS_BI_FIELD_VALUE_CATALOG = 28,
            STATUS_CALIBRATION = 29,
            STATUS_CALIBRATION_GROUP_CATALOG = 30,
            STATUS_CALIBRATION_TRANSACTION_CATALOG = 31,
            STATUS_CALIBRATION_USER_CATALOG = 32,
            STATUS_CUSTOM_CONTROL = 33,
            STATUS_CUSTOM_CONTROL_VALUE_CATALOG = 34,
            STATUS_CUSTOM_FIELD = 35,
            STATUS_FORM = 36,
            STATUS_GROUP = 37,
            STATUS_LOG = 38,
            STATUS_PERMISSION = 39,
            STATUS_PERSON = 40,
            STATUS_PROGRAM = 41,
            STATUS_PROGRAM_FORM_CATALOG = 42,
            STATUS_ROL_PERMISSION_CATALOG = 43,
            STATUS_TRANSACTION = 44,
            STATUS_TRANSACTION_ATTRIBUTE_CATALOG = 45,
            STATUS_TRANSACTION_BI_FIELD_CATALOG = 46,
            STATUS_TRANSACTION_COMMENTARY = 47,
            STATUS_TRANSACTION_CUSTOM_FIELD_CATALOG = 48,
            STATUS_TRANSACTION_FILE_CATALOG = 49,
            STATUS_TRANSACTION_LABEL = 50,
            STATUS_TRANSACTION_LABEL_CATALOG = 51,
            STATUS_UPLOADED_FILE = 52,
            STATUS_USER = 53,
            STATUS_USER_GROUP_CATALOG = 54,
            STATUS_USER_PERMISSION_CATALOG = 55,
            STATUS_USER_PROGRAM_CATALOG = 56,
            STATUS_USER_ROLE_CATALOG = 57,
            STATUS_USER_SUPERVISOR_CATALOG = 58,
            STATUS_USER_WORKSPACE_CATALOG = 59,
            STATUS_WORKSPACE = 60,
            CATALOG = 241,
            ELEMENT = 1241,
            STATUS_PROGRAM_GROUP = 1311,
            STATUS_PROGRAM_GROUP_PROGRAM_CATALOG = 1312,
            STATUS_USER_PROGRAM_GROUP_CATALOG = 1324,
            STATUS_FORM_BI_FIELD_CATALOG = 1336,
            TRANSACTION_SEARCH_STRING_TYPE = 1378,
            TRANSACTION_SEARCH_TIME_UNIT_TYPE = 1382,
            REPORT_TYPE = 1390,
            TIME_INTERVAL = 1392,
            TRANSACTION_TYPE = 1403,
            NOTIFICATION_TYPE = 1410,
            STATUS_NOTIFICATION = 1423,
            STATUS_NOTIFICATION_URL = 1429,
            STATUS_NOTIFICATION_URL_CATALOG = 1435,
            PERSON_COUNTRY = 2410,
        }

        public enum ELEMENT
        {
            ELEMENT_ATTRIBUTE = 1242,
            ELEMENT_ATTRIBUTEVALUECATALOG = 1243,
            ELEMENT_BUSINESSINTELLIGENCEFIELD = 1244,
            ELEMENT_BUSINESSINTELLIGENCEVALUECATALOG = 1245,
            ELEMENT_CALIBRATION = 1246,
            ELEMENT_CALIBRATIONGROUPCATALOG = 1247,
            ELEMENT_CALIBRATIONTRANSACTIONCATALOG = 1248,
            ELEMENT_CALIBRATIONUSERCATALOG = 1249,
            ELEMENT_CUSTOMCONTROL = 1250,
            ELEMENT_CUSTOMCONTROLVALUECATALOG = 1251,
            ELEMENT_CUSTOMFIELD = 1252,
            ELEMENT_FORM = 1253,
            ELEMENT_GROUP = 1254,
            ELEMENT_LOG = 1255,
            ELEMENT_PERMISSION = 1256,
            ELEMENT_PERSON = 1257,
            ELEMENT_PROGRAM = 1258,
            ELEMENT_PROGRAMFORMCATALOG = 1259,
            ELEMENT_ROLPERMISSIONCATALOG = 1260,
            ELEMENT_TRANSACTION = 1261,
            ELEMENT_TRANSACTIONATTRIBUTECATALOG = 1262,
            ELEMENT_TRANSACTIONBIFIELDCATALOG = 1263,
            ELEMENT_TRANSACTIONCOMMENTARY = 1264,
            ELEMENT_TRANSACTIONCUSTOMFIELDCATALOG = 1265,
            ELEMENT_TRANSACTIONFILECATALOG = 1266,
            ELEMENT_TRANSACTIONLABEL = 1267,
            ELEMENT_TRANSACTIONLABELCATALOG = 1268,
            ELEMENT_UPLOADEDFILE = 1269,
            ELEMENT_USER = 1270,
            ELEMENT_USERGROUPCATALOG = 1271,
            ELEMENT_USERPERMISSIONCATALOG = 1272,
            ELEMENT_USERPROGRAMCATALOG = 1273,
            ELEMENT_USERROLECATALOG = 1274,
            ELEMENT_USERSUPERVISORCATALOG = 1275,
            ELEMENT_USERWORKSPACECATALOG = 1276,
            ELEMENT_WORKSPACE = 1277,
            ELEMENT_NO_ELEMENT = 1278,
            ELEMENT_BASICINFO = 1279,
            ELEMENT_CATALOG = 1280,
            ELEMENT_FORMIMPORTHISTORY = 1281,
            ELEMENT_NOTIFICATIONMATRIX = 1282,
            ELEMENT_ROLE = 1296,
            ELEMENT_PROGRAMGROUP = 1309,
            ELEMENT_PROGRAMGROUPPROGRAMCATALOG = 1310,
            ELEMENT_USERPROGRAMGROUPCATALOG = 1323,
            ELEMENT_FORMBIFIELDCATALOG = 1335,
        }

        public enum Permission
        {
            CAN_ASIGN_PERMISSIONS_TO_ROLES = 1,
            CAN_ASIGN_PERMISSIONS_TO_USERS = 2,
            CAN_MASSIVELY_IMPORT_USERS = 3,
            CAN_CALIBRATE_IN_CALIBRATION_SESSIONS = 4,
            CAN_MODIFY_OTHER_USER_PASSWORDS = 5,
            CAN_CREATE_BI_QUESTIONS = 6,
            CAN_CREATE_TEMPLATE_FORMS = 7,
            CAN_CREATE_GROUPS = 8,
            CAN_CREATE_CALIBRATION_OPTIONS = 9,
            CAN_CREATE_CALIBRATION_SESSIONS = 10,
            CAN_CREATE_CUSTOM_FIELDS = 11,
            CAN_CREATE_DEVOLUTION_OPTIONS = 12,
            CAN_CREATE_WORKSPACES = 13,
            CAN_CREATE_ROLES = 14,
            CAN_CREATE_USERS = 15,
            CAN_DELETE_CALIBRATIONS = 16,
            CAN_DELETE_WORKSPACES = 17,
            CAN_DELETE_ROLES = 18,
            CAN_DELETE_USERS = 19,
            CAN_DISPUTE_TRANSACTIONS = 20,
            CAN_MODIFY_WORKSPACES = 21,
            CAN_MODIFY_ROLES = 22,
            CAN_MODIFY_TRANSACTIONS = 23,
            CAN_MODIFY_USERS = 24,
            CAN_GIVE_DEVOLUTIONS = 25,
            CAN_INVALIDATE_TRANSACTIONS = 26,
            CAN_MONITOR = 27,
            CAN_SEARCH_TRANSACTIONS = 28,
            CAN_SEARCH_USERS = 29,
            CAN_SEE_ALL_CALIBRATION_SESSIONS = 30,
            CAN_CHANGE_NOTIFICATION_ALARMS = 31,
            CAN_SEE_ALL_USERS = 32,
            CAN_SEE_CALIBRATION_RESULTS = 33,
            CAN_SEE_OWN_MONITORING = 34,
            CAN_SEE_PROGRAMS = 35,
            CAN_SEE_REPORTS = 36,
            CAN_SEE_ROLES = 37,
            CAN_MODIFY_USER_TRANSACTION = 38,
            CAN_SEE_ALL_PROGRAMS = 39,
            CAN_SEE_DESCENDANT_USERS = 40,
            CAN_EDIT_RECOLECTION = 41,
            CAN_RECOLECT = 42,
        }

        public enum USER_LANGUAGE
        {
            SPANISH = 1288
        }

        public enum STATUS_ATTRIBUTE
        {
            CREATED = 61,
            UPDATED = 62,
            DELETED = 63,
            ENABLED = 64,
            DISABLED = 65,
        }

        public enum STATUS_ATTRIBUTE_VALUE_CATALOG
        {
            CREATED = 66,
            UPDATED = 67,
            DELETED = 68,
            ENABLED = 69,
            DISABLED = 70,
        }

        public enum STATUS_BI_FIELD
        {
            CREATED = 71,
            UPDATED = 72,
            DELETED = 73,
            ENABLED = 74,
            DISABLED = 75,
        }

        public enum STATUS_BI_FIELD_VALUE_CATALOG
        {
            CREATED = 76,
            UPDATED = 77,
            DELETED = 78,
            ENABLED = 79,
            DISABLED = 80,
        }

        public enum STATUS_CALIBRATION
        {
            CREATED = 81,
            UPDATED = 82,
            DELETED = 83,
            ENABLED = 84,
            DISABLED = 85,
        }

        public enum STATUS_CALIBRATION_GROUP_CATALOG
        {
            CREATED = 86,
            UPDATED = 87,
            DELETED = 88,
            ENABLED = 89,
            DISABLED = 90,
        }

        public enum STATUS_CALIBRATION_TRANSACTION_CATALOG
        {
            CREATED = 91,
            UPDATED = 92,
            DELETED = 93,
            ENABLED = 94,
            DISABLED = 95,
        }

        public enum STATUS_CALIBRATION_USER_CATALOG
        {
            CREATED = 96,
            UPDATED = 97,
            DELETED = 98,
            ENABLED = 99,
            DISABLED = 100,
        }

        public enum STATUS_CUSTOM_CONTROL
        {
            CREATED = 101,
            UPDATED = 102,
            DELETED = 103,
            ENABLED = 104,
            DISABLED = 105,
        }

        public enum STATUS_CUSTOM_CONTROL_VALUE_CATALOG
        {
            CREATED = 106,
            UPDATED = 107,
            DELETED = 108,
            ENABLED = 109,
            DISABLED = 110,
        }

        public enum STATUS_CUSTOM_FIELD
        {
            CREATED = 111,
            UPDATED = 112,
            DELETED = 113,
            ENABLED = 114,
            DISABLED = 115,
        }

        public enum STATUS_FORM
        {
            CREATED = 116,
            UPDATED = 117,
            DELETED = 118,
            ENABLED = 119,
            DISABLED = 120,
        }

        public enum STATUS_GROUP
        {
            CREATED = 121,
            UPDATED = 122,
            DELETED = 123,
            ENABLED = 124,
            DISABLED = 125,
        }

        public enum STATUS_LOG
        {
            CREATED = 126,
            UPDATED = 127,
            DELETED = 128,
            ENABLED = 129,
            DISABLED = 130,
        }

        public enum STATUS_PERMISSION
        {
            CREATED = 131,
            UPDATED = 132,
            DELETED = 133,
            ENABLED = 134,
            DISABLED = 135,
        }

        public enum STATUS_PERSON
        {
            CREATED = 136,
            UPDATED = 137,
            DELETED = 138,
            ENABLED = 139,
            DISABLED = 140,
        }

        public enum STATUS_PROGRAM
        {
            CREATED = 141,
            UPDATED = 142,
            DELETED = 143,
            ENABLED = 144,
            DISABLED = 145,
        }

        public enum STATUS_PROGRAM_FORM_CATALOG
        {
            CREATED = 146,
            UPDATED = 147,
            DELETED = 148,
            ENABLED = 149,
            DISABLED = 150,
        }

        public enum STATUS_ROL_PERMISSION_CATALOG
        {
            CREATED = 151,
            UPDATED = 152,
            DELETED = 153,
            ENABLED = 154,
            DISABLED = 155,
        }

        public enum STATUS_TRANSACTION
        {
            CREATED = 156,
            UPDATED = 157,
            DELETED = 158,
            ENABLED = 159,
            DISABLED = 160,
            CALIBRATION_CREATED = 1400,
        }

        public enum STATUS_TRANSACTION_ATTRIBUTE_CATALOG
        {
            CREATED = 161,
            UPDATED = 162,
            DELETED = 163,
            ENABLED = 164,
            DISABLED = 165,
        }

        public enum STATUS_TRANSACTION_BI_FIELD_CATALOG
        {
            CREATED = 166,
            UPDATED = 167,
            DELETED = 168,
            ENABLED = 169,
            DISABLED = 170,
        }

        public enum STATUS_TRANSACTION_COMMENTARY
        {
            CREATED = 171,
            UPDATED = 172,
            DELETED = 173,
            ENABLED = 174,
            DISABLED = 175,
        }

        public enum STATUS_TRANSACTION_CUSTOM_FIELD_CATALOG
        {
            CREATED = 176,
            UPDATED = 177,
            DELETED = 178,
            ENABLED = 179,
            DISABLED = 180,
        }

        public enum STATUS_TRANSACTION_FILE_CATALOG
        {
            CREATED = 181,
            UPDATED = 182,
            DELETED = 183,
            ENABLED = 184,
            DISABLED = 185,
        }

        public enum STATUS_TRANSACTION_LABEL
        {
            CREATED = 186,
            UPDATED = 187,
            DELETED = 188,
            ENABLED = 189,
            DISABLED = 190,
        }

        public enum STATUS_TRANSACTION_LABEL_CATALOG
        {
            CREATED = 191,
            UPDATED = 192,
            DELETED = 193,
            ENABLED = 194,
            DISABLED = 195,
        }

        public enum STATUS_UPLOADED_FILE
        {
            CREATED = 196,
            UPDATED = 197,
            DELETED = 198,
            ENABLED = 199,
            DISABLED = 200,
            LOADED_FILE_USER_IMPORT = 1295,
            LOADED_FILE_UPLOAD_FORM = 1347,
            LOADED_FILE_DATA_IMPORT = 1409,
        }

        public enum STATUS_USER
        {
            CREATED = 201,
            UPDATED = 202,
            DELETED = 203,
            ENABLED = 204,
            DISABLED = 205,
        }

        public enum STATUS_USER_GROUP_CATALOG
        {
            CREATED = 206,
            UPDATED = 207,
            DELETED = 208,
            ENABLED = 209,
            DISABLED = 210,
        }

        public enum STATUS_USER_PERMISSION_CATALOG
        {
            CREATED = 211,
            UPDATED = 212,
            DELETED = 213,
            ENABLED = 214,
            DISABLED = 215,
        }

        public enum STATUS_USER_PROGRAM_CATALOG
        {
            CREATED = 216,
            UPDATED = 217,
            DELETED = 218,
            ENABLED = 219,
            DISABLED = 220,
        }

        public enum STATUS_USER_ROLE_CATALOG
        {
            CREATED = 221,
            UPDATED = 222,
            DELETED = 223,
            ENABLED = 224,
            DISABLED = 225,
        }

        public enum STATUS_USER_SUPERVISOR_CATALOG
        {
            CREATED = 226,
            UPDATED = 227,
            DELETED = 228,
            ENABLED = 229,
            DISABLED = 230,
        }

        public enum STATUS_USER_WORKSPACE_CATALOG
        {
            CREATED = 231,
            UPDATED = 232,
            DELETED = 233,
            ENABLED = 234,
            DISABLED = 235,
        }

        public enum STATUS_WORKSPACE
        {
            CREATED = 236,
            UPDATED = 237,
            DELETED = 238,
            ENABLED = 239,
            DISABLED = 240,
        }

        public enum STATUS_ROLE
        {
            CREATED = 1290,
            UPDATED = 1291,
            DELETED = 1292,
            ENABLED = 1293,
            DISABLED = 1294,
        }

        public enum STATUS_PROGRAM_GROUP
        {
            CREATED = 1313,
            UPDATED = 1314,
            DELETED = 1315,
            ENABLED = 1316,
            DISABLED = 1317,
        }

        public enum STATUS_PROGRAM_GROUP_PROGRAM_CATALOG
        {
            CREATED = 1318,
            UPDATED = 1319,
            DELETED = 1320,
            ENABLED = 1321,
            DISABLED = 1322,
        }

        public enum STATUS_USER_PROGRAM_GROUP_CATALOG
        {
            CREATED = 1325,
            UPDATED = 1326,
            DELETED = 1327,
            ENABLED = 1328,
            DISABLED = 1329,
        }

        public enum STATUS_FORM_BI_FIELD_CATALOG
        {
            CREATED = 1337,
            UPDATED = 1338,
            DELETED = 1339,
            ENABLED = 1340,
            DISABLED = 1341,
        }

        public enum MODULE
        {
            ANALYTICS_FILE_ACCESS = 1297,
            CALIBRATION = 1298,
        }

        public enum USER_ROLE
        {
            SUPERUSER = 1,
            ADMINISTRATOR = 2,
            COLABORATOR = 3,
            SUPERVISOR = 4,
            CALIBRATOR = 5,
            MONITOR = 7,
            FACILITATOR = 9,
        }

        public enum NOTIFICATION_MATRIX_ENTITY
        {
            DIRECT_SUPERVISOR = 1299,
            INDIRECT_SUPERVISOR = 1300,
            MONITORED_USER = 1301,
            MONITORING_USER = 1302,
        }

        public enum NOTIFICATION_MATRIX_ACTION
        {
            TRANSACTION_DISPUTE = 1303,
            TRANSACTION_DEVOLUTION = 1304,
            TRANSACTION_INVALIDATION = 1305,
            TRANSACTION_CONFIRMATION = 1306,
            TEAM_MONITORING_WITH_SCORE_ZERO = 1307,
            CALIBRATION = 1308,
        }

        public enum CUSTOM_CONTROL_TYPE
        {
            TEXT_BOX = 1330,
            TEXT_AREA = 1331,
            CHECKBOX = 1332,
            RADIO_BUTTON = 1333,
            SELECT_LIST = 1334,
        }

        public enum ATTRIBUTE_ERROR_TYPE
        {
            FUCE = 1343,
            BCE = 1344,
            FCE = 1345,
            NCE = 1346,
        }

        public enum TRANSACTION_COMMENT_TYPE
        {
            DISPUTE = 1348,
            INVALIDATION = 1349,
            DEVOLUTION_COMMENTARIES = 1350,
            DEVOLUTION_USER_STRENGTHS = 1351,
            DEVOLUTION_IMPROVEMENT_STEPS = 1352,
        }

        public enum TRANSACTION_GENERAL_RESULT_FINAL
        {
            SUCCESS = 1353,
            FAIL = 1354,
        }

        public enum TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR
        {
            SUCCESS = 1355,
            FAIL = 1356,
        }

        public enum TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR
        {
            SUCCESS = 1357,
            FAIL = 1358,
        }

        public enum TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR
        {
            SUCCESS = 1359,
            FAIL = 1360,
        }

        public enum TRANSACTION_ACCURATE_RESULT_FINAL
        {
            SUCCESS = 1361,
            FAIL = 1362,
        }

        public enum TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR
        {
            SUCCESS = 1363,
            FAIL = 1364,
        }

        public enum TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR
        {
            SUCCESS = 1365,
            FAIL = 1366,
        }

        public enum TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR
        {
            SUCCESS = 1367,
            FAIL = 1368,
        }

        public enum TRANSACTION_CONTROLLABLE_RESULT_FINAL
        {
            SUCCESS = 1369,
            FAIL = 1370,
        }

        public enum TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR
        {
            SUCCESS = 1371,
            FAIL = 1372,
        }

        public enum TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR
        {
            SUCCESS = 1373,
            FAIL = 1374,
        }

        public enum TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR
        {
            SUCCESS = 1375,
            FAIL = 1376,
        }

        public enum TRANSACTION_SEARCH_STRING_TYPE
        {
            STARTS_WITH = 1379,
            INCLUDES = 1380,
            ENDS_WITH = 1381,
        }

        public enum TRANSACTION_SEARCH_TIME_UNIT_TYPE
        {
            HOURS = 1383,
            DAYS = 1384,
            WEEKS = 1385,
            MONTHS = 1386,
        }

        public enum CALIBRATION_TYPE
        {
            STANDARD = 1387,
        }

        public enum REPORT_TYPE
        {
            OVERALL_ACCURACY = 1391,
            ACCURACY_TREND = 1398,
            CALIBRATOR_COMPARISON = 1399,
            ACCURACY_BY_ATTRIBUTE = 1401,
            ACCURACY_TREND_BY_ATTRIBUTE = 1402,
            COMPARATIVE_BY_USER = 1406,
            COMPARATIVE_BY_PROGRAM = 1407,
            PARETO_BI = 1408,
        }

        public enum TIME_INTERVAL
        {
            DAY = 1393,
            WEEK = 1394,
            MONTH = 1395,
            QUARTER = 1396,
            YEAR = 1397,
        }

        public enum TRANSACTION_TYPE
        {
            EVALUATION = 1404,
            CALIBRATION = 1405,
        }

        public enum FORM_TYPE
        {
            EVALUATION = 1342,
        }

        public enum NOTIFICATION_TYPE
        {
            DISPUTE_AGENT = 1411,
            DISPUTE_OTHERS = 1412,

            DEVOLUTION_AGENT = 1413,
            DEVOLUTION_OTHERS = 1414,

            INVALIDATION_AGENT = 1415,
            INVALIDATION_OTHERS = 1416,

            TRANSACTION_ACCORDANCE_AGENT = 1417,
            TRANSACTION_ACCORDANCE_OTHERS = 1418,

            MONITORED_TEAM_WITH_NOTE_ZERO_AGENT = 1419,
            MONITORED_TEAM_WITH_NOTE_ZERO_OTHERS = 1420,

            CALIBRATION_AGENT = 1421,
            CALIBRATION_OTHERS = 1422,
        }

        public enum STATUS_NOTIFICATION
        {
            CREATED = 1424,
            UPDATED = 1425,
            DELETED = 1426,
            ENABLED = 1427,
            DISABLED = 1428,
        }

        public enum STATUS_NOTIFICATION_URL
        {
            CREATED = 1430,
            UPDATED = 1431,
            DELETED = 1432,
            ENABLED = 1433,
            DISABLED = 1434,
        }

        public enum STATUS_NOTIFICATION_URL_CATALOG
        {
            CREATED = 1436,
            UPDATED = 1437,
            DELETED = 1438,
            ENABLED = 1439,
            DISABLED = 1440,
        }

        public enum PERSON_COUNTRY
        {
            COSTA_RICA = 2411,
            PANAMA = 2412,
            COLOMBIA = 2413,
        }
    }
}
