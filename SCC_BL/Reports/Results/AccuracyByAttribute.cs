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
        public int TransactionID { get; set; } = 0;
        public int AttributeID { get; set; } = 0;
        public string AttributeName { get; set; }
        public bool SuccessFulResult { get; set; }
        public bool IsControllable { get; set; }

        public AccuracyByAttribute(int transactionAttributeID, int transactionID, int attributeID, string attributeName, bool successFulResult)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.TransactionID = transactionID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessFulResult = successFulResult;
            this.SetIsControllable();
        }

        public void SetIsControllable()
        {
            List<SCC_BL.Attribute> childrenAttributeList = new List<SCC_BL.Attribute>();

            using (SCC_BL.Attribute attribute = SCC_BL.Attribute.AttributeWithParentAttributeID(this.AttributeID))
                childrenAttributeList = attribute.SelectByParentAttributeID();

            this.IsControllable = true;

            bool attributeIsControllable = childrenAttributeList.Where(e => !e.IsControllable).Count() <= 0;

            if (!attributeIsControllable)
            {
                using (TransactionAttributeCatalog transactionAttributeCatalog = TransactionAttributeCatalog.TransactionAttributeCatalogWithTransactionIDAndAttributeID(this.TransactionID, this.AttributeID))
                {
                    transactionAttributeCatalog.SetDataByTransactionIDAndAttributeID();

                    if (transactionAttributeCatalog.Checked)
                        this.IsControllable = false;
                }
            }
        }
    }
}
