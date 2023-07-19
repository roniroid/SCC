using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ProgramManagementViewModel
    {
        public SCC_BL.Program Program { get; set; } = new SCC_BL.Program();
        public List<SCC_BL.Program> ProgramList { get; set; } = new List<SCC_BL.Program>();
    }
}