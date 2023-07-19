using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class ParetoBI
    {
        public int TransactionBIFieldID { get; set; } = 0;
        public int BusinessIntelligenceFieldID { get; set; } = 0;
        public string BusinessIntelligenceFieldName { get; set; }
        public bool SuccessfulResult { get; set; }
        public bool HasChildren { get; set; }

        public ParetoBI(int transactionBIFieldID, int businessIntelligenceFieldID, string businessIntelligenceFieldName, bool successfulResult, bool hasChildren)
        {
            this.TransactionBIFieldID = transactionBIFieldID;
            this.BusinessIntelligenceFieldID = businessIntelligenceFieldID;
            this.BusinessIntelligenceFieldName = businessIntelligenceFieldName;
            this.SuccessfulResult = successfulResult;
            this.HasChildren = hasChildren;
        }
    }
}
