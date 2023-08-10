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
        public bool IsControllable { get; set; }

        public AccuracyByAttribute(int transactionAttributeID, int attributeID, string attributeName, bool successFulResult)
        {
            this.TransactionAttributeID = transactionAttributeID;
            this.AttributeID = attributeID;
            this.AttributeName = attributeName;
            this.SuccessFulResult = successFulResult;
            this.SetIsControllable();
        }

        public void SetIsControllable()
        {
            List<SCC_BL.Attribute> levelOneAttributeList = new List<SCC_BL.Attribute>();
            int[] parentIDArray = new int[0];

            using (SCC_BL.Attribute attribute = new SCC_BL.Attribute(this.AttributeID))
            {
                levelOneAttributeList = attribute.SelectByLevel(1);
                parentIDArray = attribute.SelectParentIDArrayByID();
                parentIDArray.Append(this.AttributeID);
            }

            for (int i = 0; i < parentIDArray.Length; i++)
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
