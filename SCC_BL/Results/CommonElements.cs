using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Results
{
    public class CommonElements
    {
        public const string REPLACE_USERNAME = "%username%";
        public const string REPLACE_JSON_INFO = "%jsonInfo%";
        public const string REPLACE_EXCEPTION_MESSAGE = "%exceptionMessage%";

        public const string REPLACE_LOCAL_LOG_MESSAGE = "%message%";
        public const string REPLACE_LOCAL_LOG_STACK_TRACE = "%stackTrace%";

        public const string REPLACE_CUSTOM_CONTENT = "%customContent%";
        public const string REPLACE_CUSTOM_CONTENT_2 = "%customContent2%";
    }
}
