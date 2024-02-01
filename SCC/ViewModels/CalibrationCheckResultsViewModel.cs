using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class CalibrationCheckResultsViewModel
    {
        public SCC_BL.Calibration CalibrationSession { get; set; } = new SCC_BL.Calibration();
        public SCC_BL.Transaction CalibratedTransaction { get; set; } = new SCC_BL.Transaction();
        public List<SCC_BL.Transaction> CalibrationList { get; set; } = new List<SCC_BL.Transaction>();
        public SCC_BL.Transaction SelectedCalibration { get; set; } = new SCC_BL.Transaction();
        public SCC_BL.Form Form { get; set; } = new SCC_BL.Form();
        public SCC_BL.User ExperiencedUser { get; set; } = new SCC_BL.User();
        public SCC_BL.User EvaluatorUser { get; set; } = new SCC_BL.User();
    }
}