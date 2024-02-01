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
        public int TransactionID { get; set; } = 0;
        public int AttributeID { get; set; } = 0;
        public string AttributeName { get; set; }
        public bool SuccessfulResult { get; set; }
        public bool HasChildren { get; set; }
        public int ErrorTypeID { get; set; }
        public bool IsControllable { get; set; }

        public AccuracyBySubattribute(int transactionAttributeID, int transactionID, int attributeID, string attributeName, bool successfulResult, bool hasChildren, int errorTypeID, bool mustBeControllable)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.TransactionID = transactionID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessfulResult = successfulResult;
            this.HasChildren = hasChildren;
            this.ErrorTypeID = errorTypeID;

            if (mustBeControllable)
                this.SetIsControllable();
        }

        public void SetIsControllable()
        {
            List<SCC_BL.Attribute> levelOneAttributeList = new List<SCC_BL.Attribute>();
            List<int> parentIDArray = new List<int>();

            using (SCC_BL.Attribute attribute = new SCC_BL.Attribute(this.AttributeID))
            {
                levelOneAttributeList = attribute.SelectByLevel(1);
                parentIDArray = attribute.SelectParentIDArrayByID().ToList();
                parentIDArray.Add(this.AttributeID);
            }

            for (int i = 0; i < parentIDArray.Count; i++)
            {
                if (!levelOneAttributeList.Select(s => s.ID).Contains(parentIDArray[i]))
                    continue;

                Attribute currentParentAttribute = levelOneAttributeList.Where(e => e.ID == parentIDArray[i]).FirstOrDefault();

                if (!currentParentAttribute.IsControllable)
                {
                    this.IsControllable = false;
                    this.SuccessfulResult = true;
                }

                break;
            }
        }

        /*public void SetIsControllable()
        {
            List<SCC_BL.Attribute> levelOneAttributeList = new List<SCC_BL.Attribute>();
            List<int> parentIDArray = new List<int>();

            using (SCC_BL.Attribute attribute = new SCC_BL.Attribute(this.AttributeID))
            {
                levelOneAttributeList = attribute.SelectByLevel(1);
                parentIDArray = attribute.SelectParentIDArrayByID().ToList();
                parentIDArray.Add(this.AttributeID);
            }

            for (int i = 0; i < parentIDArray.Count; i++)
            {
                if (!levelOneAttributeList.Select(s => s.ID).Contains(parentIDArray[i]))
                    continue;

                List<SCC_BL.Attribute> childrenAttributeList = new List<SCC_BL.Attribute>();

                using (SCC_BL.Attribute attribute = SCC_BL.Attribute.AttributeWithParentAttributeID(parentIDArray[i]))
                    childrenAttributeList = attribute.SelectByParentAttributeID();

                childrenAttributeList = childrenAttributeList.Where(e => !e.IsControllable).ToList();

                bool attributeIsControllable = childrenAttributeList.Count() <= 0;

                if (attributeIsControllable)
                    continue;

                foreach (SCC_BL.Attribute currentChildAttribute in childrenAttributeList)
                {
                    using (TransactionAttributeCatalog transactionAttributeCatalog = TransactionAttributeCatalog.TransactionAttributeCatalogWithTransactionIDAndAttributeID(this.TransactionID, currentChildAttribute.ID))
                    {
                        transactionAttributeCatalog.SetDataByTransactionIDAndAttributeID();

                        if (!transactionAttributeCatalog.Checked)
                            continue;

                        this.IsControllable = false;
                        this.SuccessfulResult = true;
                        break;
                    }
                }

                break;
            }
        }*/
    }
}
