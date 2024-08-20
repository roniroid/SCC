using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Reports.Results
{
    public class CalibratorComparison
    {
        public int TransactionID { get; set; } = 0;

        public int? GlobalGeneralResultID { get; set; } = 0;
        public int? GlobalGeneralFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GlobalGeneralBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GlobalGeneralFulfillmentCriticalErrorResultID { get; set; } = 0;

        //----------------------------------------------------------------------------

        public int? GlobalAccurateResultID { get; set; } = 0;
        public int? GlobalAccurateFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GlobalAccurateBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GlobalAccurateFulfillmentCriticalErrorResultID { get; set; } = 0;

        //----------------------------------------------------------------------------

        public int? GlobalControllableResultID { get; set; } = 0;
        public int? GlobalControllableFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GlobalControllableBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GlobalControllableFulfillmentCriticalErrorResultID { get; set; } = 0;


        public int CalibratorUserID { get; set; } = 0;

        public CalibratorComparison(
            int transactionID,

            int globalGeneralResultID,
            int globalGeneralFinalUserCriticalErrorResultID, 
            int globalGeneralBusinessCriticalErrorResultID, 
            int globalGeneralFulfillmentCriticalErrorResultID, 

            int globalAccurateResultID,
            int globalAccurateFinalUserCriticalErrorResultID, 
            int globalAccurateBusinessCriticalErrorResultID, 
            int globalAccurateFulfillmentCriticalErrorResultID, 

            int globalControllableResultID,
            int globalControllableFinalUserCriticalErrorResultID, 
            int globalControllableBusinessCriticalErrorResultID, 
            int globalControllableFulfillmentCriticalErrorResultID, 

            int calibratorUserID)
        {
            this.TransactionID = transactionID;

            this.GlobalGeneralResultID = globalGeneralResultID;
            this.GlobalGeneralFinalUserCriticalErrorResultID = globalGeneralFinalUserCriticalErrorResultID;
            this.GlobalGeneralBusinessCriticalErrorResultID = globalGeneralBusinessCriticalErrorResultID;
            this.GlobalGeneralFulfillmentCriticalErrorResultID = globalGeneralFulfillmentCriticalErrorResultID;

            //----------------------------------------------------------------------------------

            this.GlobalAccurateResultID = globalAccurateResultID;
            this.GlobalAccurateFinalUserCriticalErrorResultID = globalAccurateFinalUserCriticalErrorResultID;
            this.GlobalAccurateBusinessCriticalErrorResultID = globalAccurateBusinessCriticalErrorResultID;
            this.GlobalAccurateFulfillmentCriticalErrorResultID = globalAccurateFulfillmentCriticalErrorResultID;

            //----------------------------------------------------------------------------------

            this.GlobalControllableResultID = globalControllableResultID;
            this.GlobalControllableFinalUserCriticalErrorResultID = globalControllableFinalUserCriticalErrorResultID;
            this.GlobalControllableBusinessCriticalErrorResultID = globalControllableBusinessCriticalErrorResultID;
            this.GlobalControllableFulfillmentCriticalErrorResultID = globalControllableFulfillmentCriticalErrorResultID;

            this.CalibratorUserID = calibratorUserID;
        }
    }
}
