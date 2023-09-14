using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class CalibratorComparisonByError
    {
        public int TransactionID { get; set; } = 0;

        public int? GlobalGeneralResultID { get; set; } = 0;
        public int? GlobalGeneralFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GlobalGeneralBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GlobalGeneralFulfillmentCriticalErrorResultID { get; set; } = 0;
        public int? GlobalGeneralNonCriticalErrorResult { get; set; } = 0;

        //----------------------------------------------------------------------------

        public int? GlobalAccurateResultID { get; set; } = 0;
        public int? GlobalAccurateFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GlobalAccurateBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GlobalAccurateFulfillmentCriticalErrorResultID { get; set; } = 0;
        public int? GlobalAccurateNonCriticalErrorResult { get; set; } = 0;

        //----------------------------------------------------------------------------

        public int? GlobalControllableResultID { get; set; } = 0;
        public int? GlobalControllableFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GlobalControllableBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GlobalControllableFulfillmentCriticalErrorResultID { get; set; } = 0;
        public int? GlobalControllableNonCriticalErrorResult { get; set; } = 0;

        public int CalibratorUserID { get; set; } = 0;
        public bool IsExpertsCalibration { get; set; } = false;

        public CalibratorComparisonByError(
            int transactionID,

            int globalGeneralResultID,
            int globalGeneralFinalUserCriticalErrorResultID,
            int globalGeneralBusinessCriticalErrorResultID,
            int globalGeneralFulfillmentCriticalErrorResultID,
            int globalGeneralNonCriticalErrorResult,

            int globalAccurateResultID,
            int globalAccurateFinalUserCriticalErrorResultID,
            int globalAccurateBusinessCriticalErrorResultID,
            int globalAccurateFulfillmentCriticalErrorResultID,
            int globalAccurateNonCriticalErrorResult,

            int globalControllableResultID,
            int globalControllableFinalUserCriticalErrorResultID,
            int globalControllableBusinessCriticalErrorResultID,
            int globalControllableFulfillmentCriticalErrorResultID,
            int globalControllableNonCriticalErrorResult,

            int calibratorUserID,
            bool isExpertsCalibration)
        {
            this.TransactionID = transactionID;

            this.GlobalGeneralResultID = globalGeneralResultID;
            this.GlobalGeneralFinalUserCriticalErrorResultID = globalGeneralFinalUserCriticalErrorResultID;
            this.GlobalGeneralBusinessCriticalErrorResultID = globalGeneralBusinessCriticalErrorResultID;
            this.GlobalGeneralFulfillmentCriticalErrorResultID = globalGeneralFulfillmentCriticalErrorResultID;
            this.GlobalGeneralNonCriticalErrorResult = globalGeneralNonCriticalErrorResult;

            //----------------------------------------------------------------------------------

            this.GlobalAccurateResultID = globalAccurateResultID;
            this.GlobalAccurateFinalUserCriticalErrorResultID = globalAccurateFinalUserCriticalErrorResultID;
            this.GlobalAccurateBusinessCriticalErrorResultID = globalAccurateBusinessCriticalErrorResultID;
            this.GlobalAccurateFulfillmentCriticalErrorResultID = globalAccurateFulfillmentCriticalErrorResultID;
            this.GlobalAccurateNonCriticalErrorResult = globalAccurateNonCriticalErrorResult;

            //----------------------------------------------------------------------------------

            this.GlobalControllableResultID = globalControllableResultID;
            this.GlobalControllableFinalUserCriticalErrorResultID = globalControllableFinalUserCriticalErrorResultID;
            this.GlobalControllableBusinessCriticalErrorResultID = globalControllableBusinessCriticalErrorResultID;
            this.GlobalControllableFulfillmentCriticalErrorResultID = globalControllableFulfillmentCriticalErrorResultID;
            this.GlobalControllableNonCriticalErrorResult = globalControllableNonCriticalErrorResult;

            this.CalibratorUserID = calibratorUserID;
            this.IsExpertsCalibration = isExpertsCalibration;
        }
    }
}
