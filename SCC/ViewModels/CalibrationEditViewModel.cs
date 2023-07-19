using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class CalibrationEditViewModel
    {
        public SCC_BL.Calibration Calibration = new SCC_BL.Calibration();
        public List<SCC_BL.Transaction> TransactionList { get; set; } = new List<SCC_BL.Transaction>();
    }
}