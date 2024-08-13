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
        public int[] ChildrenAttributeIDList { get; set; } = new int[0];
        public bool IsControllable { get; set; } = true;

        public AccuracyByAttribute(int transactionAttributeID, int transactionID, int attributeID, string attributeName, bool successFulResult, bool mustBeControllable, string childrenAttributeIDList)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.TransactionID = transactionID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessFulResult = successFulResult;

            this.ChildrenAttributeIDList = 
                !string.IsNullOrEmpty(childrenAttributeIDList) 
                    ? childrenAttributeIDList.Split(',').Select(e => Convert.ToInt32(e)).ToArray()
                    : new int[0];

            if (mustBeControllable)
                this.SetIsControllable();
        }

        public void SetIsControllable()
        {
            List<SCC_BL.Attribute> childrenAttributeList = new List<SCC_BL.Attribute>();

            using (SCC_BL.Attribute attribute = SCC_BL.Attribute.AttributeWithParentAttributeID(this.AttributeID))
                childrenAttributeList = attribute.SelectByParentAttributeID();

            childrenAttributeList = childrenAttributeList.Where(e => !e.IsControllable).ToList();

            bool attributeIsControllable = childrenAttributeList.Count() <= 0;

            if (!attributeIsControllable)
            {
                foreach (SCC_BL.Attribute currentChildAttribute in childrenAttributeList)
                {
                    using (TransactionAttributeCatalog transactionAttributeCatalog = TransactionAttributeCatalog.TransactionAttributeCatalogWithTransactionIDAndAttributeID(this.TransactionID, currentChildAttribute.ID))
                    {
                        transactionAttributeCatalog.SetDataByTransactionIDAndAttributeID();

                        if (transactionAttributeCatalog.Checked)
                        {
                            this.IsControllable = false;
                            this.SuccessFulResult = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
