using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class AccuracyByAttribute
    {
        public int TransactionAttributeID { get; set; } = 0;
        public int AttributeID { get; set; } = 0;
        public string AttributeName { get; set; }
        public bool SuccessFulResult { get; set; }

        public AccuracyByAttribute(int transactionAttributeID, int attributeID, string attributeName, bool successFulResult)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessFulResult = successFulResult;
        }
    }
}
