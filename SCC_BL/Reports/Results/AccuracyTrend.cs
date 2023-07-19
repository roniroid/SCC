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
        public int FinalUserCriticalErrorResultID { get; set; }
        public int BusinessCriticalErrorResultID { get; set; }
        public int FulfilmentCriticalErrorResultID { get; set; }
        public int GlobalCriticalErrorResultID { get; set; }

        public AccuracyTrend(int transactionID, DateTime transactionStartDate, int finalUserCriticalErrorSuccessResultID, int businessCriticalErrorSuccessResultID, int fulfilmentCriticalErrorSuccessResultID, int globalCriticalErrorSuccessResultID)
        {
            this.TransactionID = transactionID;
            this.TransactionStartDate = transactionStartDate;
            this.FinalUserCriticalErrorResultID = finalUserCriticalErrorSuccessResultID;
            this.BusinessCriticalErrorResultID = businessCriticalErrorSuccessResultID;
            this.FulfilmentCriticalErrorResultID = fulfilmentCriticalErrorSuccessResultID;
            this.GlobalCriticalErrorResultID = globalCriticalErrorSuccessResultID;
        }
    }
}
