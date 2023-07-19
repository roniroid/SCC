using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class AccuracyTrendByAttribute
    {
        public int TransactionAttributeID { get; set; }
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public bool SuccessfulResult { get; set; }

        public AccuracyTrendByAttribute(int transactionAttributeID, int transactionID, DateTime transactionDate, int attributeID, string attributeName, bool successfulResult)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.TransactionID = transactionID;
            this.TransactionDate = transactionDate;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessfulResult = successfulResult;
        }
    }
}
