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
        public bool IsControllable { get; set; }

        public AccuracyBySubattribute(int transactionAttributeID, int attributeID, string attributeName, bool successfulResult, bool hasChildren, int errorTypeID)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessfulResult = successfulResult;
            this.HasChildren = hasChildren;
            this.ErrorTypeID = errorTypeID;
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
                if (levelOneAttributeList.Select(s => s.ID).Contains(parentIDArray[i]))
                {
                    this.IsControllable = true;
                    break;
                }
            }
        }
    }
}
