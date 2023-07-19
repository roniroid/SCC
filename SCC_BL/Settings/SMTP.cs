using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Settings
{
    public class SMTP
    {
        public struct SCC
        {
            public const string NAME = "SCC";
            public const string HOST = "smtp.office365.com";
            public const int PORT = 587;
            public const bool ENABLE_SSL = true;
            public const string SYSTEM_EMAIL_ADDRESS = "scc@netcom.com.pa";
            public const string PASSWORD = "iM178*B4kCyJe0";
            public const bool IS_BODY_HTML = true;
        }

        public struct SCC_Test
        {
            public const string NAME = "SCC_Test";
            public const string HOST = "smtp.office365.com";
            public const int PORT = 587;
            public const bool ENABLE_SSL = true;
            public const string SYSTEM_EMAIL_ADDRESS = "pruebascorreo@netcombcc.com";
            public const string PASSWORD = "nN410%ajyBPM*3";
            public const bool IS_BODY_HTML = true;
        }

        public struct Test
        {
            public const string NAME = "SCC";
            public const string HOST = "smtp-relay.sendinblue.com\r\n";
            public const int PORT = 587;
            public const bool ENABLE_SSL = false;
            public const string SYSTEM_EMAIL_ADDRESS = "jenalo7376@fitwl.com";
            public const string PASSWORD = "?284R1Dy35..?@";
            public const bool IS_BODY_HTML = true;
        }
    }
}
