using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsAccuracyBySubattributeViewModel
    {
        public List<SCC_BL.Reports.Results.AccuracyBySubattribute> AccuracyBySubattributeResultList { get; set; } = new List<SCC_BL.Reports.Results.AccuracyBySubattribute>();
        public List<ResultBySubattribute> ResultBySubattributeList { get; set; } = new List<ResultBySubattribute>();
        public List<OrderHelper> OrderHelperList { get; set; } = new List<OrderHelper>();

        public int TotalTransactions { get; set; } = 0;

        public ReportResultsAccuracyBySubattributeViewModel()
        { 
        }

        public ReportResultsAccuracyBySubattributeViewModel(List<SCC_BL.Reports.Results.AccuracyBySubattribute> accuracyBySubattributeResultList, int totalTransactions)
        {
            this.TotalTransactions = totalTransactions;
            this.AccuracyBySubattributeResultList = accuracyBySubattributeResultList;

            this.ResultBySubattributeList = new List<ResultBySubattribute>();

            foreach (SCC_BL.Reports.Results.AccuracyBySubattribute accuracyBySubattributeResult in this.AccuracyBySubattributeResultList.Where(e => !e.SuccessfulResult).OrderBy(e => e.AttributeID))
            {
                if (this.ResultBySubattributeList.Select(e => e.AttributeID).Where(e => e == accuracyBySubattributeResult.AttributeID).Count() <= 0)
                {
                    int successfulResultCount = AccuracyBySubattributeResultList.Where(e => e.AttributeID == accuracyBySubattributeResult.AttributeID && e.SuccessfulResult).Count();

                    ResultBySubattribute resultBySubattribute = new ResultBySubattribute();

                    resultBySubattribute.TransactionAttributeID = accuracyBySubattributeResult.TransactionAttributeID;
                    resultBySubattribute.AttributeID = accuracyBySubattributeResult.AttributeID;
                    resultBySubattribute.AttributeName = accuracyBySubattributeResult.AttributeName;
                    resultBySubattribute.Quantity = successfulResultCount;
                    resultBySubattribute.HasChildren = accuracyBySubattributeResult.HasChildren;
                    resultBySubattribute.ErrorTypeID = accuracyBySubattributeResult.ErrorTypeID;

                    this.ResultBySubattributeList.Add(resultBySubattribute);
                }
            }

            this.AccuracyBySubattributeResultList
                .ToList()
                .ForEach(e => {
                    if (!OrderHelperList.Select(f => f.AttributeID).Contains(e.AttributeID))
                    {
                        OrderHelperList.Add(
                            new OrderHelper() { 
                                AttributeID = e.AttributeID,
                                Quantity = this.AccuracyBySubattributeResultList.Where(g => g.AttributeID == e.AttributeID && g.SuccessfulResult).Count()
                            });
                    }
                });

            this.ResultBySubattributeList =
                this.ResultBySubattributeList
                    .OrderBy(e => e.Quantity)
                    .ToList();
        }

        public class OrderHelper
        {
            public int AttributeID { get; set; }
            public int Quantity { get; set; }
        }

        public class ResultBySubattribute
        {
            public int TransactionAttributeID { get; set; }
            public int AttributeID { get; set; }
            public string AttributeName { get; set; }
            public int Quantity { get; set; }
            public bool HasChildren { get; set; }
            public int ErrorTypeID { get; set; }
        }
    }
}