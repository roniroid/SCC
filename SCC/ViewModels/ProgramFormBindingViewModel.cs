using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ProgramFormBindingViewModel
    {
        public SCC_BL.Form Form { get; set; } = new SCC_BL.Form();
        public DateTime StartDate { get; set; } = DateTime.Now;
        public List<SCC_BL.Form> FormList { get; set; } = new List<SCC_BL.Form>();

    }
}