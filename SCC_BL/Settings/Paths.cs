using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Settings
{
    public class Paths
    {
        public class ActionProtocol
        {
            public string ProtocolName { get; set; }
            public string ActionName { get; set; }

            public ActionProtocol(string protocolName, string actionName)
            {
                this.ProtocolName = protocolName;
                this.ActionName = actionName;
            }
        }

        public struct User
        {
            public static readonly string[] NotUserActionList = { "LogIn", "PasswordRecovery", "SignIn", "ForgottenPassword" };
            public static readonly ActionProtocol[] NotAdminActionList = 
                {
                    new ActionProtocol("Protocol", "Index")
                };

            public const string LOGIN = "~/User/LogIn";

            public const string MASSIVE_IMPORT_FOLDER = "~/Content/files/uploaded/UserMassiveImports/";

            public const string DB_PROCESSED_FILES_FOLDER = "~/Content/files/uploaded/DBProcessedFiles/";
        }

        public struct Form
        {
            public const string FORM_UPLOAD_FOLDER = "~/Content/files/uploaded/FormUpload/";
        }

        public struct Transaction
        {
            public const string TRANSACTION_IMPORT_FOLDER = "~/Content/files/uploaded/ImportData/";
            public const string TRANSACTION_EXPORT_FOLDER = "~/Content/files/generated/Transaction/Export/";
            public const string TRANSACTION_GENERATED_IMPORT_RESULTS_FOLDER = "~/Content/files/generated/Transaction/Import/";
        }

        public struct Excel
        {
            public struct User
            {
                public const string MASIVE_IMPORT_METHOD_NAME = "GetFromExcel";
            }
        }
    }
}
