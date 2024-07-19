using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Settings
{
    public class SMTP
    {
        public struct Test
        {
            public const string NAME = "SCC";
            public const string HOST = "smtp-relay.sendinblue.com\r\n";
            public const int PORT = 587;
            public const bool ENABLE_SSL = false;
            public const string SYSTEM_EMAIL_ADDRESS = "";
            public const string PASSWORD = "";
            public const bool IS_BODY_HTML = true;
        }
    }
}
