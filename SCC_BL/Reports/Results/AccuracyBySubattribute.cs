using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Results
{
    public class AccuracyBySubattribute
    {
        public int TransactionAttributeID { get; set; } = 0;
        public int AttributeID { get; set; } = 0;
        public string AttributeName { get; set; }
        public bool SuccessfulResult { get; set; }
        public bool HasChildren { get; set; }
        public int ErrorTypeID { get; set; }

        public AccuracyBySubattribute(int transactionAttributeID, int attributeID, string attributeName, bool successfulResult, bool hasChildren, int errorTypeID)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessfulResult = successfulResult;
            this.HasChildren = hasChildren;
            this.ErrorTypeID = errorTypeID;
        }
    }
}
