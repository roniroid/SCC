using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class RoleManagementViewModel
    {
        public SCC_BL.Role Role { get; set; } = new SCC_BL.Role();
        public List<SCC_BL.Role> RoleList { get; set; } = new List<SCC_BL.Role>();
    }
}