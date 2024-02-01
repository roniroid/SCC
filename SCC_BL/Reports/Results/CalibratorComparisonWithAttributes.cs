using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class CalibratorComparisonWithAttributes
    {
        public int TransactionID { get; set; } = 0;

        public int ErrorTypeID { get; set; } = 0;
        public int AttributeID { get; set; } = 0;
        public int? ValueID { get; set; } = 0;
        public bool Checked { get; set; } = false;

        public int CalibratorUserID { get; set; } = 0;
        public bool IsExpertsCalibration { get; set; } = false;

        public CalibratorComparisonWithAttributes(
            int transactionID,

            int errorTypeID,
            int attributeID,
            int? valueID,
            bool @checked,

            int calibratorUserID,
            bool isExpertsCalibration)
        {
            this.TransactionID = transactionID;

            this.ErrorTypeID = errorTypeID;
            this.AttributeID = attributeID;
            this.ValueID = valueID;
            this.Checked = @checked;

            this.CalibratorUserID = calibratorUserID;
            this.IsExpertsCalibration = isExpertsCalibration;
        }
    }
}
