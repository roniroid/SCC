using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class ComparativeByProgram
    {
        public int TransactionID { get; set; } = 0;
        public int? GeneralFinalUserCriticalErrorResultID { get; set; } = 0;
        public int? GeneralBusinessCriticalErrorResultID { get; set; } = 0;
        public int? GeneralFulfillmentCriticalErrorResultID { get; set; } = 0;
        public int? GeneralResultID { get; set; } = 0;
        public int ProgramID { get; set; } = 0;

        public ComparativeByProgram(int transactionID, int generalFinalUserCriticalErrorResultID, int generalBusinessCriticalErrorResultID, int generalFulfillmentCriticalErrorResultID, int generalResultID, int programID)
        {
            this.TransactionID = transactionID;
            this.GeneralFinalUserCriticalErrorResultID = generalFinalUserCriticalErrorResultID;
            this.GeneralBusinessCriticalErrorResultID = generalBusinessCriticalErrorResultID;
            this.GeneralFulfillmentCriticalErrorResultID = generalFulfillmentCriticalErrorResultID;
            this.GeneralResultID = generalResultID;
            this.ProgramID = programID;
        }
    }
}
