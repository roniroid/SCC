using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ProgramGroupManagementViewModel
    {
        public SCC_BL.ProgramGroup ProgramGroup { get; set; } = new SCC_BL.ProgramGroup();
        public List<SCC_BL.ProgramGroup> ProgramGroupList { get; set; } = new List<SCC_BL.ProgramGroup>();
    }
}