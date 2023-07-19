using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class CalibrationTypeViewModel
    {
        public SCC_BL.Catalog Catalog { get; set; } = new SCC_BL.Catalog();
        public List<SCC_BL.Catalog> CalibrationTypeList { get; set; } = new List<SCC_BL.Catalog>();
    }
}