using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsAccuracyByAttributeViewModel
    {
        public ReportAccuracyByAttributeViewModel RequestObject { get; set; } = new ReportAccuracyByAttributeViewModel();

        public List<SCC_BL.Reports.Results.AccuracyByAttribute> AccuracyByAttributeResultList { get; set; } = new List<SCC_BL.Reports.Results.AccuracyByAttribute>();
        public List<ResultByAttribute> ResultByAttributeList { get; set; } = new List<ResultByAttribute>();

        public int TotalTransactions { get; set; } = 0;

        public ReportResultsAccuracyByAttributeViewModel()
        {
        }

        public ReportResultsAccuracyByAttributeViewModel(List<SCC_BL.Reports.Results.AccuracyByAttribute> accuracyByAttributeResultList, int totalTransactions)
        {
            this.TotalTransactions = totalTransactions;
            this.AccuracyByAttributeResultList = accuracyByAttributeResultList;

            this.ResultByAttributeList = new List<ResultByAttribute>();

            foreach(SCC_BL.Reports.Results.AccuracyByAttribute accuracyByAttributeResult in this.AccuracyByAttributeResultList.OrderBy(e => e.AttributeID))
            {
                if (this.ResultByAttributeList.Select(e => e.AttributeID).Where(e => e == accuracyByAttributeResult.AttributeID).Count() <= 0)
                {
                    int successfulResultCount = AccuracyByAttributeResultList.Where(e => e.AttributeID == accuracyByAttributeResult.AttributeID && e.SuccessFulResult).Count();

                    ResultByAttribute resultByAttribute = new ResultByAttribute();

                    resultByAttribute.TransactionAttributeID = accuracyByAttributeResult.TransactionAttributeID;
                    resultByAttribute.AttributeID = accuracyByAttributeResult.AttributeID;
                    resultByAttribute.AttributeName = accuracyByAttributeResult.AttributeName;
                    resultByAttribute.Quantity = successfulResultCount;

                    this.ResultByAttributeList.Add(resultByAttribute);
                }
            }
        }

        public class ResultByAttribute
        {
            public int TransactionAttributeID { get; set;}
            public int AttributeID { get; set;}
            public string AttributeName { get; set;}
            public int Quantity { get; set;}
            public bool IsControllable { get; set;}

            public void SetIsControllable()
            {
                List<SCC_BL.Attribute> levelOneAttributeList = new List<SCC_BL.Attribute>();
                int[] parentIDArray = new int[0];

                using (SCC_BL.Attribute attribute = new SCC_BL.Attribute(this.AttributeID))
                {
                    levelOneAttributeList = attribute.SelectByLevel(1);
                    parentIDArray = attribute.SelectParentIDArrayByID();
                }

                levelOneAttributeList = levelOneAttributeList.Where(e => e.IsControllable).ToList();

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
}