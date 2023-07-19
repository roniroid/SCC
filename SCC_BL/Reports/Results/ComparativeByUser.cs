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
        public int? GeneralFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GeneralBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GeneralFulfillmentCriticalErrorResultID { get; set; } = 0;
        public int? GeneralResultID { get; set; } = 0;
        public int UserID { get; set; } = 0;

        public ComparativeByUser(int transactionID, int generalFinalUserCriticalErrorResultID, int generalBusinessCriticalErrorResultID, int generalFulfillmentCriticalErrorResultID, int generalResultID, int userID)
        {
            this.TransactionID = transactionID;
            this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorResultID;
            this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorResultID;
            this.GeneralFulfillmentCriticalErrorResultID = generalFulfillmentCriticalErrorResultID;
            this.GeneralResultID = generalResultID;
            this.UserID = userID;
        }
    }
}
