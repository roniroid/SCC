using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class UserManagementViewModel
    {
        public SCC_BL.Person Person { get; set; } = new SCC_BL.Person();
        public SCC_BL.User User { get; set; } = new SCC_BL.User();
        public List<SCC_BL.User> UserList { get; set; } = new List<SCC_BL.User>();
    }
}