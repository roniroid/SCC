using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Settings
{ 
    public class Constants
    {
        public const int DEFAULT_TIMEOUT = 6000;

        //PRODUCTION
        //public const string DEFAULT_CONNECTION = "Data Source=172.24.89.25;Initial Catalog=NETCOM_SCC;Persist Security Info=True;User ID=sa;Password=Netcom240";

        //PRODUCTION LOCAL
        //public const string DEFAULT_CONNECTION = "Data Source=RONIROID;Initial Catalog=NETCOM_SCC_PRODUCTION;Integrated Security=True";

        //NETCOM PC
        //public const string DEFAULT_CONNECTION = "Data Source=NET-ZPTID002;Initial Catalog=NETCOM_SCC;Integrated Security=True";

        //LOCAL PC
        public const string DEFAULT_CONNECTION = "Data Source=RONIROID;Initial Catalog=NETCOM_SCC_PRODUCTION;Integrated Security=True";

        //LOCAL LAPTOP
        //public const string DEFAULT_CONNECTION = "Data Source=RONIROID-LAPTOP;Initial Catalog=NETCOM_SCC_PRODUCTION;Integrated Security=True";
    }
}
