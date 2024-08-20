using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        public bool IsControllable { get; set; } = true;

        public AccuracyTrendByAttribute(int transactionAttributeID, int transactionID, DateTime transactionDate, int attributeID, string attributeName, bool successfulResult, bool mustBeControllable)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.TransactionID = transactionID;
            this.TransactionDate = transactionDate;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessfulResult = successfulResult;

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
                            this.SuccessfulResult = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
