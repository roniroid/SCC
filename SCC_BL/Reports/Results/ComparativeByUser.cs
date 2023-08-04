﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class ComparativeByUser
    {
        public int TransactionID { get; set; } = 0;

        public int? GeneralResultID { get; set; } = 0;
        public int? GeneralFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GeneralBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GeneralFulfillmentCriticalErrorResultID { get; set; } = 0;

        public int? AccurateResultID { get; set; } = 0;
        public int? AccurateFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? AccurateBusinessCriticalErrorResultID { get; set; } = 0;
        public int? AccurateFulfillmentCriticalErrorResultID { get; set; } = 0;

        public int? ControllableResultID { get; set; } = 0;
        public int? ControllableFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? ControllableBusinessCriticalErrorResultID { get; set; } = 0;
        public int? ControllableFulfillmentCriticalErrorResultID { get; set; } = 0;

        public int UserID { get; set; } = 0;

        public ComparativeByUser(
            int transactionID,

            int generalResultID,
            int generalFinalUserCriticalErrorResultID,
            int generalBusinessCriticalErrorResultID,
            int generalFulfillmentCriticalErrorResultID, 

            int accurateResultID,
            int accurateFinalUserCriticalErrorResultID, 
            int accurateBusinessCriticalErrorResultID, 
            int accurateFulfillmentCriticalErrorResultID, 

            int controllableResultID,
            int controllableFinalUserCriticalErrorResultID, 
            int controllableBusinessCriticalErrorResultID, 
            int controllableFulfillmentCriticalErrorResultID, 
            
            int userID)
        {
            this.TransactionID = transactionID;

            this.GeneralResultID = generalResultID;
            this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorResultID;
            this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorResultID;
            this.GeneralFulfillmentCriticalErrorResultID = generalFulfillmentCriticalErrorResultID;

            this.AccurateResultID = accurateResultID;
            this.AccurateFinalUserCriticalErrorResultID = accurateFinalUserCriticalErrorResultID;
            this.AccurateBusinessCriticalErrorResultID = accurateBusinessCriticalErrorResultID;
            this.AccurateFulfillmentCriticalErrorResultID = accurateFulfillmentCriticalErrorResultID;

            this.ControllableResultID = controllableResultID;
            this.ControllableFinalUserCriticalErrorResultID = controllableFinalUserCriticalErrorResultID;
            this.ControllableBusinessCriticalErrorResultID = controllableBusinessCriticalErrorResultID;
            this.ControllableFulfillmentCriticalErrorResultID = controllableFulfillmentCriticalErrorResultID;

            this.UserID = userID;
        }
    }
}
