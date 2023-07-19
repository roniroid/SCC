using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class GroupManagementViewModel
    {
        public SCC_BL.Group Group { get; set; } = new SCC_BL.Group();
        public List<SCC_BL.Group> GroupList { get; set; } = new List<SCC_BL.Group>();
    }
}