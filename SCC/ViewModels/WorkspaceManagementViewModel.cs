using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class WorkspaceManagementViewModel
    {
        public SCC_BL.Workspace Workspace { get; set; } = new SCC_BL.Workspace();
        public List<SCC_BL.Workspace> WorkspaceList { get; set; } = new List<SCC_BL.Workspace>();
    }
}