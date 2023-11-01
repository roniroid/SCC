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

        /*public ReportResultsAccuracyByAttributeViewModel(List<SCC_BL.Reports.Results.AccuracyByAttribute> accuracyByAttributeResultList, int totalTransactions)
        {
            this.TotalTransactions = totalTransactions;
            this.AccuracyByAttributeResultList = accuracyByAttributeResultList;

            this.ResultByAttributeList = new List<ResultByAttribute>();

            foreach(SCC_BL.Reports.Results.AccuracyByAttribute accuracyByAttributeResult in this.AccuracyByAttributeResultList.OrderBy(e => e.AttributeID))
            {
                if (this.ResultByAttributeList.Select(e => e.AttributeID).Where(e => e == accuracyByAttributeResult.AttributeID).Count() <= 0)
                {
                    int successfulResultCount = 
                        this.AccuracyByAttributeResultList
                            .Where(e => 
                                e.AttributeID == accuracyByAttributeResult.AttributeID && 
                                e.SuccessFulResult)
                            .Count();

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
        }*/

        public ReportResultsAccuracyByAttributeViewModel(List<SCC_BL.Reports.Results.AccuracyByAttribute> accuracyByAttributeResultList, int totalTransactions)
        {
            this.TotalTransactions = totalTransactions;
            this.AccuracyByAttributeResultList = accuracyByAttributeResultList;

            this.ResultByAttributeList = new List<ResultByAttribute>();

            foreach (SCC_BL.Reports.Results.AccuracyByAttribute accuracyByAttributeResult in this.AccuracyByAttributeResultList.OrderBy(e => e.AttributeName))
            {
                if (this.ResultByAttributeList.Select(e => e.AttributeName).Where(e => e.Equals(accuracyByAttributeResult.AttributeName)).Count() <= 0)
                {
                    int successfulResultCount =
                        this.AccuracyByAttributeResultList
                            .Where(e =>
                                e.AttributeName == accuracyByAttributeResult.AttributeName &&
                                e.SuccessFulResult)
                            .Count();

                    ResultByAttribute resultByAttribute = new ResultByAttribute();

                    resultByAttribute.TransactionAttributeID = 
                        this.AccuracyByAttributeResultList
                            .Where(e => 
                                e.AttributeName.Equals(accuracyByAttributeResult.AttributeName))
                            .Select(e => e.TransactionAttributeID)
                            .ToArray();

                    resultByAttribute.AttributeID = 
                        this.AccuracyByAttributeResultList
                            .Where(e => 
                                e.AttributeName.Equals(accuracyByAttributeResult.AttributeName))
                            .Select(e => e.AttributeID)
                            .ToArray();

                    resultByAttribute.AttributeName = accuracyByAttributeResult.AttributeName;
                    resultByAttribute.Quantity = successfulResultCount;

                    this.ResultByAttributeList.Add(resultByAttribute);
                }
            }
        }

        public class ResultByAttribute
        {
            public int[] TransactionAttributeID { get; set; }
            public int[] AttributeID { get; set; }
            public string AttributeName { get; set; }
            public int Quantity { get; set; }
        }
    }
}