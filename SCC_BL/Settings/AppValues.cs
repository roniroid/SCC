using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Settings
{
    public class AppValues
    {
        public struct Session
        {
            public const string GLOBAL_TEMP_MESSAGE = "TEMP_MESSAGE";
            public const string GLOBAL_ACTUAL_USER = "ACTUAL_USER";
            public const string GLOBAL_CATALOG_LIST = "CATALOG_LIST";
            public const string GLOBAL_CATEGORY_LIST = "TABLE_CATEGORY_LIST";

            public const string ERROR_COUNT = "ERROR_COUNT";

            public struct Transaction
            {
                public const string LAST_GENERATED_IMPORT_RESULTS_FILE = "LAST_GENERATED_IMPORT_RESULTS_FILE";
            }

            public struct Report
            {
                public const string OVERALL_ACCURACY_REQUEST = "OVERALL_ACCURACY_REQUEST";
            }
        }

        public const string DELETED_SUFIX_COUNT = "%count%";
        public const string DELETED_SUFIX = " (eliminado " + DELETED_SUFIX_COUNT + ")";

        public enum MailTopic
        {
            FORGOTTEN_PASSWORD,
            USER_CREATION,
            CHANGE_PASSWORD,
            CALIBRATION_CREATED,
            CALIBRATION_SESSION_CREATED,
            DISPUTE,
            INVALIDATION,
            DEVOLUTION
        }

        public static string[] POSITIVE_ANSWERS = { "SI", "SÍ", "YES", "si", "sí", "yes", "1" };
        public static string[] NEGATIVE_ANSWERS = { "NO", "no", "0" };

        public const int TRANSACTION_MINIMUM_NCE_SCORE = 80;

        public struct File
        {
            public struct ContentType
            {
                public enum List
                {
                    EXCEL_FILES = 0
                }

                public const string EXCEL_FILES_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                public const string EXCEL_FILES_XLS = "application/vnd.ms-excel";
                public const string TEXT_FILES = "text/plain";
                public const string OCTET_STREAM = "application/octet-stream";
            }
        }

        public struct ExcelTasks
        {
            public struct User
            {
                public struct MassiveImport
                {
                    public enum Fields
                    {
                        IDENTIFICATION = 0,
                        FIRST_NAME = 1,
                        LAST_NAME = 2,
                        EMAIL = 3,
                        START_DATE = 4,
                        END_DATE = 5,
                        LANGUAGE = 6,
                        WORKSPACE = 7,
                        SUPERVISOR = 8,
                        SUPERVISOR_START_DATE = 9,
                        ROLE = 10,
                        GROUP = 11,
                        PROGRAM = 12,
                        HAS_PASS_PERMISSION = 13,
                    }
                }
            }

            public struct Form
            {
                public struct UploadForm
                {
                    public enum AttributeFields
                    {
                        DESCRIPTION = 0,
                        MAX_SCORE = 1,
                        TOP_DOWN_SCORE = 2,
                        FORCE_COMMENT = 3,
                        DEFINE_ANSWER_TYPE = 4,
                        APPLY = 5,
                        KNOWN = 6,
                        CONTROLLABLE = 7,
                        SCORABLE = 8,
                    }
                }
            }

            public struct Transaction
            {
                public struct ImportData
                {
                    public const string FILE_NAME_PREFIX = "carga_de_transacciones";
                    public const string DEFAULT_EXTENSION = ".xlsx";

                    public const int CONSTANT_START_INDEX = 0;
                    public const int CONSTANT_END_INDEX = 49;

                    public struct User
                    {
                        public enum ExcelFields
                        {
                            AGENT_IDENTIFICATION = 1,
                            AGENT_NAME = 2,
                            SUPERVISOR_NAME = 3,
                            EVALUATOR_NAME = 4,
                        }
                    }

                    public struct Form
                    {
                        public const int TYPE_ID = (int)SCC_BL.DBValues.Catalog.FORM_TYPE.TEMPLATE;
                        public const string COMMENT_PREFIX = "Importación de transacción - Identificador viejo de la transacción: ";

                        public enum ExcelFields
                        {
                            NAME = 5,
                        }
                    }

                    public struct Program
                    {
                        public enum ExcelFields
                        {
                            NAME = 5,
                        }
                    }

                    public struct Transaction
                    {
                        public const string IDENTIFIER_PREFIX = "Identificador anterior: ";

                        public struct ExcelFields
                        {
                            public enum BaseInfo
                            {
                                OLD_IDENTIFIER = 0,
                                LABELS = 6,
                                TRANSACTION_DATE = 7,
                                EVALUATION_DATE = 8,
                                LOAD_DATE = 9,
                                MODIFICATION_DATE = 10,
                                MODIFICATION_USER = 11,
                                TIME_ELAPSED = 12,
                                COMMENT = 28,
                            }

                            public struct Results
                            {
                                public enum General
                                {
                                    GENERAL_RESULT = 13,
                                    FINAL_USER_CRITICAL_ERROR = 14,
                                    BUSINESS_CRITICAL_ERROR = 15,
                                    FULFILMENT_CRITICAL_ERROR = 16,
                                    NON_CRITICAL_ERROR = 17,
                                }

                                public enum Accurate
                                {
                                    FINAL_USER_CRITICAL_ERROR = 22,
                                    BUSINESS_CRITICAL_ERROR = 23,
                                    FULFILMENT_CRITICAL_ERROR = 24,
                                }

                                public enum Controllable
                                {
                                    FINAL_USER_CRITICAL_ERROR = 25,
                                    BUSINESS_CRITICAL_ERROR = 26,
                                    FULFILMENT_CRITICAL_ERROR = 27,
                                }
                            }

                            public enum ActionPlan
                            {
                                ACTION_DATE = 39,
                            }

                            public enum Tracing
                            {
                                TRACING_DATE = 42,
                            }

                            public enum Coaching
                            {
                                SENDING_DATE = 48,
                                READING_DATE = 49,
                            }

                            public struct Commentaries
                            {
                                public enum Disputation
                                {
                                    COMMENT = 30,
                                    CREATION_DATE = 32,
                                }

                                public enum Invalidation
                                {
                                    COMMENT = 47,
                                    CREATION_DATE = 46,
                                }

                                public struct Devolution
                                {
                                    public enum General
                                    {
                                        COMMENT = 34,
                                        CREATION_DATE = 33,
                                    }

                                    public enum UserStrengths
                                    {
                                        COMMENT = 35,
                                    }

                                    public enum ImprovementSteps
                                    {
                                        COMMENT = 36,
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public struct Masks
        {
            public enum MaskID
            {
                DATE_1 = 1,
                TIME_1 = 2,
                PHONE_1 = 3,
                PHONE_2 = 4,
                ALPHANUMERIC_1 = 5,
                NAME_1 = 6,
                LAST_NAME_1 = 7,
                EMAIL_1 = 8,
            }

            public struct Date1
            {
                public const string MASK = "Fecha: mm/dd/yyyy";
                public const string PATTERN = "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$";
            }

            public struct Time1
            {
                public const string MASK = "Tiempo: hh:mm:ss";
                public const string PATTERN = "^(?:([01]?\\d|2[0-3]):([0-5]?\\d):)?([0-5]?\\d)$";
            }

            public struct PhoneNumber1
            {
                public const string MASK = "Teléfono 1: (999) 999-9999";
                public const string PATTERN = "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$";
            }

            public struct PhoneNumber2
            {
                public const string MASK = "Teléfono 2: (999) 9999-9999";
                public const string PATTERN = "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{4}[-\\s\\.]?[0-9]{4,6}$";
            }

            public struct Alphanumeric1
            {
                public const string MASK = "Alfanumérico: [0-9][a-z][A-Z]";
                public const string PATTERN = "^[A-Za-z0-9\\s]+$";
            }

            public struct Name1
            {
                public const string MASK = "Nombre";
                public const string PATTERN = "^[a-zA-ZÀ-ÿñÑ]+(\\s*[a-zA-ZÀ-ÿñÑ]*)*[a-zA-ZÀ-ÿñÑ]+$";
            }

            public struct LastName1
            {
                public const string MASK = "Apellido";
                public const string PATTERN = "^[a-zA-ZÀ-ÿñÑ]+$";
            }

            public struct Email1
            {
                public const string MASK = "Email";
                public const string PATTERN = "^[a-zA-Z0-9.!#$%&'*+\\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(\\.[a-zA-Z0-9-]+){1,}$";
                //public const string PATTERN = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$";
            }
        }

        public struct ViewData
        {
            public const string CATEGORIES = "CATEGORIES";

            public struct Log
            {
                public struct Index
                {
                    public struct CategoryCatalog
                    {
                        public const string NAME = "CATEGORY_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct Workspace
            {
                public const string WORKSPACES = "WORKSPACES";

                public struct Manage
                {
                }
            }

            public struct CustomControl
            {
                public const string CUSTOM_CONTROLS = "CUSTOM_CONTROLS";

                public struct Manage
                {
                    public const string MODEL_ID = "CUSTOM_CONTROL_MODEL_ID";
                }

                public struct CustomControlTypeView
                {
                    public const string MASK_ID = "CUSTOM_CONTROL_TYPE_VIEW_CUSTOM_CONTROL_MASK_ID";

                    public struct Module
                    {
                        public const string NAME = "CUSTOM_CONTROL_TYPE_VIEW_CUSTOM_CONTROL_MODULE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct BIField
            {
                public const string BI_FIELDS = "BI_FIELDS";
            }

            public struct Group
            {
                public const string GROUPS = "GROUPS";

                public struct Manage
                {
                    public struct UserList
                    {
                        public const string NAME = "MANAGE_GROUP_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct Module
                    {
                        public const string NAME = "MANAGE_GROUP_MODULE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct Transaction
            {
                public const string TRANSACTIONS = "TRANSACTIONS";

                public struct Edit
                {
                    public struct ProgramList
                    {
                        public const string NAME = "EDIT_TRANSACTION_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct FormView
                {
                    public struct UserList
                    {
                        public const string NAME = "EDIT_TRANSACTION_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct HasDisputation
                    {
                        public const string NAME = "EDIT_TRANSACTION_HAS_DISPUTATION";
                    }

                    public struct HasInvalidation
                    {
                        public const string NAME = "EDIT_TRANSACTION_HAS_INVALIDATION";
                    }

                    public struct HasDevolution
                    {
                        public const string NAME = "EDIT_TRANSACTION_HAS_DEVOLUTION";
                    }
                }

                public struct Search
                {
                    public struct StringTypeID
                    {
                        public const string NAME = "SEARCH_TRANSACTION_STRING_TYPE_ID";
                    }

                    public struct TimeUnitTypeID
                    {
                        public const string NAME = "SEARCH_TRANSACTION_TIME_UNIT_TYPE_ID";
                    }

                    public struct YesNoQuestion
                    {
                        public enum Values
                        {
                            NO_VALUE,
                            YES = 1,
                            NO = 0
                        }

                        public const string NAME = "SEARCH_TRANSACTION_YES_NO_QUESTION_ID";
                    }

                    public struct UserStatus
                    {
                        public const string NAME = "SEARCH_TRANSACTION_USER_STATUS";
                    }

                    public struct Workspace
                    {
                        public const string NAME = "SEARCH_TRANSACTION_WORKSPACE";
                    }

                    public struct ProgramList
                    {
                        public const string NAME = "SEARCH_TRANSACTION_PROGRAM_LIST";
                    }

                    public struct UserList
                    {
                        public const string NAME = "SEARCH_TRANSACTION_USER_LIST";
                    }
                }
            }

            public struct Calibration
            {
                public const string CALIBRATIONS = "CALIBRATIONS";

                public struct Search
                {
                    public struct StringTypeID
                    {
                        public const string NAME = "SEARCH_CALIBRATION_STRING_TYPE_ID";
                    }

                    public struct TimeUnitTypeID
                    {
                        public const string NAME = "SEARCH_CALIBRATION_TIME_UNIT_TYPE_ID";
                    }

                    public struct YesNoQuestion
                    {
                        public enum Values
                        {
                            NO_VALUE,
                            YES = 1,
                            NO = 0
                        }

                        public const string NAME = "SEARCH_CALIBRATION_YES_NO_QUESTION_ID";
                    }

                    public struct UserStatus
                    {
                        public const string NAME = "SEARCH_CALIBRATION_USER_STATUS";
                    }

                    public struct Workspace
                    {
                        public const string NAME = "SEARCH_CALIBRATION_WORKSPACE";
                    }

                    public struct ProgramList
                    {
                        public const string NAME = "SEARCH_CALIBRATION_PROGRAM_LIST";
                    }
                }

                public struct Edit
                {
                    public struct CalibratorUserGroupList
                    {
                        public const string NAME = "EDIT_CALIBRATION_CALIBRATOR_USER_GROUP_LIST";
                    }
                    public struct ExpertUserList
                    {
                        public const string NAME = "EDIT_CALIBRATION_EXPERT_USER_LIST";
                    }
                    public struct CalibratorUserList
                    {
                        public const string NAME = "EDIT_CALIBRATION_CALIBRATOR_USER_LIST";
                    }
                    public struct CalibrationTypeList
                    {
                        public const string NAME = "EDIT_CALIBRATION_CALIBRATION_TYPE_LIST";
                    }
                    public struct TransactionList
                    {
                        public const string NAME = "EDIT_CALIBRATION_TRANSACTION_LIST";
                    }
                }
            }

            public struct ProgramGroup
            {
                public const string PROGRAM_GROUPS = "PROGRAM_GROUPS";

                public struct Manage
                {
                    public struct ProgramList
                    {
                        public const string NAME = "MANAGE_PROGRAM_GROUP_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct Role
            {
                public const string ROLES = "ROLES";

                public struct Manage
                {
                    public struct PermissionList
                    {
                        public const string NAME = "MANAGE_ROLE_PERMISSION_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct Form
            {
                public const string FORMS = "FORMS";

                public struct Edit
                {
                    public struct TypeList
                    {
                        public const string NAME = "EDIT_FORM_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct CustomControlList
                    {
                        public const string NAME = "EDIT_FORM_CUSTOM_CONTROL_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Label";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct BIFieldList
                    {
                        public const string NAME = "EDIT_FORM_BUSINESS_INTELLIGENCE_FIELD_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "EDIT_FORM_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct UploadForm
                {
                    public struct TypeList
                    {
                        public const string NAME = "UPLOAD_FORM_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct FormBinding
                {
                    public struct FormList
                    {
                        public const string NAME = "FORM_BINDING_FORM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }
                    public struct ProgramList
                    {
                        public const string NAME = "FORM_BINDING_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct User
            {
                public const string USERS = "USERS";

                public struct AsignRolesAndPermissions
                {
                    public struct PermissionList
                    {
                        public const string NAME = "ASIGN_ROLES_AND_PERMISSIONS_PERMISSION_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct RoleList
                    {
                        public const string NAME = "ASIGN_ROLES_AND_PERMISSIONS_ROLE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "ASIGN_ROLES_AND_PERMISSIONS_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct AsignProgramsAndProgramGroups
                {
                    public struct ProgramList
                    {
                        public const string NAME = "ASIGN_PROGRAMS_AND_PROGRAM_GROUPS_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ProgramGroupList
                    {
                        public const string NAME = "ASIGN_PROGRAMS_AND_PROGRAM_GROUPS_PROGRAM_GROUP_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "ASIGN_PROGRAMS_AND_PROGRAM_GROUPS_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct MassivePasswordChange
                {
                    public struct User
                    {
                        public const string NAME = "MASSIVE_PASSWORD_CHANGE_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct SignIn
                {
                    public struct Catalog
                    {
                        public struct LanguageCatalog
                        {
                            public const string NAME = "LANGUAGE_CATALOG";

                            public struct SelectList
                            {
                                public const string TEXT = "Description";
                                public const string VALUE = "ID";
                            }
                        }

                        public struct CountryCatalog
                        {
                            public const string NAME = "COUNTRY_CATALOG";

                            public struct SelectList
                            {
                                public const string TEXT = "Description";
                                public const string VALUE = "ID";
                            }
                        }
                    }
                }

                public struct Edit
                {
                    public struct LanguageCatalog
                    {
                        public const string NAME = "LANGUAGE_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct CountryCatalog
                    {
                        public const string NAME = "COUNTRY_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct RoleCatalog
                    {
                        public const string NAME = "ROLE_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct StatusCatalog
                    {
                        public const string NAME = "STATUS";

                        public struct SelectList
                        {
                            public const string TEXT = "Description";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct Supervisor
                    {
                        public const string NAME = "SUPERVISOR_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct Workspace
                    {
                        public const string NAME = "WORKSPACE_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct Group
                    {
                        public const string NAME = "GROUP_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct Program
                    {
                        public const string NAME = "PROGRAM_CATALOG";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }

            public struct Catalog
            {
                public struct CategoryList 
                {
                    public const string NAME = "CATEGORY_LIST";

                    public struct SelectList 
                    {
                        public const string TEXT = "Name";
                        public const string VALUE = "ID";
                    }
                }
            }

            public struct Report
            {
                public struct _OverallAccuracy
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_OVERALL_ACCURACY_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_OVERALL_ACCURACY_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_OVERALL_ACCURACY_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_OVERALL_ACCURACY_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_OVERALL_ACCURACY_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }
                
                public struct _ParetoBI
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_PARETO_BI_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_PARETO_BI_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_PARETO_BI_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_PARETO_BI_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct BusinessIntelligenceFieldList
                    {
                        public const string NAME = "REPORT_PARETO_BI_BUSINESS_INTELLIGENCE_FIELD_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }
                
                public struct _ComparativeByUser
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_USER_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_USER_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_USER_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_USER_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_USER_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }
                
                public struct _ComparativeByProgram
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_PROGRAM_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_PROGRAM_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_PROGRAM_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_PROGRAM_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_COMPARATIVE_BY_PROGRAM_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }
                
                public struct _AccuracyByAttribute
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_ATTRIBUTE_ACCURACY_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_ATTRIBUTE_ACCURACY_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_ATTRIBUTE_ACCURACY_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_ATTRIBUTE_ACCURACY_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_ATTRIBUTE_ACCURACY_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct AttributeList
                    {
                        public const string NAME = "REPORT_ATTRIBUTE_ACCURACY_ATTRIBUTE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }
                
                public struct AccuracyByAttribute
                {
                    public const string IS_CONTROLLABLE = "IS_CONTROLLABLE";
                }

                public struct _CalibratorComparison
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_CALIBRATOR_COMPARISON_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct CalibratedUserList
                    {
                        public const string NAME = "REPORT_CALIBRATOR_COMPARISON_CALIBRATED_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct CalibratedSupervisorList
                    {
                        public const string NAME = "REPORT_CALIBRATOR_COMPARISON_CALIBRATED_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct CalibratorUserList
                    {
                        public const string NAME = "REPORT_CALIBRATOR_COMPARISON_CALIBRATOR_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct CalibrationTypeList
                    {
                        public const string NAME = "REPORT_CALIBRATOR_COMPARISON_CALIBRATION_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_CALIBRATOR_COMPARISON_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct _AccuracyTrend
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct IntervalTypeList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_INTERVAL_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }

                public struct _AccuracyTrendByAttribute
                {
                    public struct ProgramList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_PROGRAM_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "Name";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct UserList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_USER_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct SupervisorList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_SUPERVISOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct EvaluatorList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_EVALUATOR_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct ErrorTypeList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_ERROR_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct IntervalTypeList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_INTERVAL_TYPE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }

                    public struct AttributeList
                    {
                        public const string NAME = "REPORT_ACCURACY_TREND_BY_ATTRIBUTE_ATTRIBUTE_LIST";

                        public struct SelectList
                        {
                            public const string TEXT = "FullName";
                            public const string VALUE = "ID";
                        }
                    }
                }
            }
        }
    }
}
