using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class CustomControlManagement
    {
        public SCC_BL.CustomControl CustomControl { get; set; } = new SCC_BL.CustomControl();
        public List<SCC_BL.CustomControl> CustomControlList { get; set; } = new List<SCC_BL.CustomControl>();
    }
}