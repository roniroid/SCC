using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class AccuracyTrend
    {
        public int TransactionID { get; set; } = 0;
        public DateTime TransactionStartDate { get; set; }

        public int GeneralGlobalCriticalErrorResultID { get; set; }
        public int GeneralFinalUserCriticalErrorResultID { get; set; }
        public int GeneralBusinessCriticalErrorResultID { get; set; }
        public int GeneralFulfilmentCriticalErrorResultID { get; set; }

        public int AccurateGlobalCriticalErrorResultID { get; set; }
        public int AccurateFinalUserCriticalErrorResultID { get; set; }
        public int AccurateBusinessCriticalErrorResultID { get; set; }
        public int AccurateFulfilmentCriticalErrorResultID { get; set; }

        public int ControllableGlobalCriticalErrorResultID { get; set; }
        public int ControllableFinalUserCriticalErrorResultID { get; set; }
        public int ControllableBusinessCriticalErrorResultID { get; set; }
        public int ControllableFulfilmentCriticalErrorResultID { get; set; }

        public AccuracyTrend(
            int transactionID, 
            DateTime transactionStartDate,

            int generalGlobalCriticalErrorSuccessResultID, 
            int generalFinalUserCriticalErrorSuccessResultID, 
            int generalBusinessCriticalErrorSuccessResultID, 
            int generalFulfilmentCriticalErrorSuccessResultID,

            int accurateGlobalCriticalErrorSuccessResultID, 
            int accurateFinalUserCriticalErrorSuccessResultID, 
            int accurateBusinessCriticalErrorSuccessResultID, 
            int accurateFulfilmentCriticalErrorSuccessResultID,

            int controllableGlobalCriticalErrorSuccessResultID, 
            int controllableFinalUserCriticalErrorSuccessResultID, 
            int controllableBusinessCriticalErrorSuccessResultID, 
            int controllableFulfilmentCriticalErrorSuccessResultID)
        {
            this.TransactionID = transactionID;
            this.TransactionStartDate = transactionStartDate;

            this.GeneralGlobalCriticalErrorResultID = generalGlobalCriticalErrorSuccessResultID;
            this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorSuccessResultID;
            this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorSuccessResultID;
            this.GeneralFulfilmentCriticalErrorResultID = generalFulfilmentCriticalErrorSuccessResultID;

            this.AccurateGlobalCriticalErrorResultID = accurateGlobalCriticalErrorSuccessResultID;
            this.AccurateFinalUserCriticalErrorResultID = accurateFinalUserCriticalErrorSuccessResultID;
            this.AccurateBusinessCriticalErrorResultID = accurateBusinessCriticalErrorSuccessResultID;
            this.AccurateFulfilmentCriticalErrorResultID = accurateFulfilmentCriticalErrorSuccessResultID;

            this.ControllableGlobalCriticalErrorResultID = controllableGlobalCriticalErrorSuccessResultID;
            this.ControllableFinalUserCriticalErrorResultID = controllableFinalUserCriticalErrorSuccessResultID;
            this.ControllableBusinessCriticalErrorResultID = controllableBusinessCriticalErrorSuccessResultID;
            this.ControllableFulfilmentCriticalErrorResultID = controllableFulfilmentCriticalErrorSuccessResultID;
        }
    }
}
