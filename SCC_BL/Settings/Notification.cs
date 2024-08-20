using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Settings
{
    public class Notification
    {
        public const string LOCAL_LOG_DATE_AND_TIME = "%dateAndTime%";
        public const string LOCAL_LOG_STACK_LEVEL = "%stackLevel%";
        public const string LOCAL_LOG_LEVEL = "%level%";
        public const string LOCAL_LOG_METHOD_NAME = "%methodName%";
        public const string LOCAL_LOG_USERNAME = "%username%";
        public const string LOCAL_LOG_MESSAGE = "%message%";

        public const string LOCAL_LOG_FORMAT = LOCAL_LOG_DATE_AND_TIME + "\t" + LOCAL_LOG_STACK_LEVEL + "\t" + LOCAL_LOG_LEVEL + "\t" + LOCAL_LOG_METHOD_NAME + "\t" + LOCAL_LOG_USERNAME + "\t\"" + LOCAL_LOG_MESSAGE + "\"\r\n";

        public enum Type
        {
            SUCCESS,
            INFO,
            QUESTION,
            WARNING,
            ERROR,
            NONE
        }

        public enum LogLevel
        {
            TRACE = 1,
            DEBUG = 2,
            INFO = 3,
            NOTICE = 4,
            WARN = 5,
            ERROR = 6,
            FATAL = 7
        }
    }
}
