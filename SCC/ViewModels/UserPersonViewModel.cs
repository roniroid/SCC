using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class UserPersonViewModel
    {
        public SCC_BL.User User { get; set; }
        public SCC_BL.Person Person { get; set; }
    }
}