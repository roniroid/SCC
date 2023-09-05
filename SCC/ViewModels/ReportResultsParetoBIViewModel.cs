using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsParetoBIViewModel
    {
        public ReportParetoBIViewModel RequestObject { get; set; } = new ReportParetoBIViewModel();

        public List<SCC_BL.Reports.Results.ParetoBI> ParetoBIResultList { get; set; } = new List<SCC_BL.Reports.Results.ParetoBI>();
        public List<ResultByBIField> ResultByBIFieldList { get; set; } = new List<ResultByBIField>();
        public List<OrderHelper> OrderHelperList { get; set; } = new List<OrderHelper>();

        public int TotalTransactions { get; set; } = 0;

        public ReportResultsParetoBIViewModel()
        {
        }

        public ReportResultsParetoBIViewModel(List<SCC_BL.Reports.Results.ParetoBI> paretoBIResultList, int totalTransactions)
        {
            this.TotalTransactions = totalTransactions;
            this.ParetoBIResultList = paretoBIResultList;

            this.ResultByBIFieldList = new List<ResultByBIField>();

            foreach (SCC_BL.Reports.Results.ParetoBI paretoBIResult in this.ParetoBIResultList.Where(e => !e.SuccessfulResult).OrderBy(e => e.BusinessIntelligenceFieldID))
            {
                if (this.ResultByBIFieldList.Select(e => e.BusinessIntelligenceFieldID).Where(e => e == paretoBIResult.BusinessIntelligenceFieldID).Count() <= 0)
                {
                    int failedResultCount = this.ParetoBIResultList.Where(e => e.BusinessIntelligenceFieldID == paretoBIResult.BusinessIntelligenceFieldID && !e.SuccessfulResult).Count();

                    ResultByBIField resultByBIField = new ResultByBIField();

                    resultByBIField.TransactionBIFieldID = paretoBIResult.TransactionBIFieldID;
                    resultByBIField.BusinessIntelligenceFieldID = paretoBIResult.BusinessIntelligenceFieldID;
                    resultByBIField.BusinessIntelligenceFieldName = paretoBIResult.BusinessIntelligenceFieldName;
                    resultByBIField.Quantity = failedResultCount;
                    resultByBIField.HasChildren = paretoBIResult.HasChildren;

                    this.ResultByBIFieldList.Add(resultByBIField);
                }
            }

            this.ParetoBIResultList
                .Where(e => !e.SuccessfulResult)
                .ToList()
                .ForEach(e => {
                    if (!this.OrderHelperList.Select(f => f.BusinessIntelligenceFieldID).Contains(e.BusinessIntelligenceFieldID))
                    {
                        this.OrderHelperList.Add(
                            new OrderHelper()
                            {
                                BusinessIntelligenceFieldID = e.BusinessIntelligenceFieldID,
                                Quantity = this.ParetoBIResultList.Where(g => g.BusinessIntelligenceFieldID == e.BusinessIntelligenceFieldID && !g.SuccessfulResult).Count()
                            });
                    }
                });

            this.ResultByBIFieldList =
                this.ResultByBIFieldList
                    .Where(e => e.Quantity > 0)
                    .OrderBy(e => e.Quantity)
                    .ToList();
        }

        public class OrderHelper
        {
            public int BusinessIntelligenceFieldID { get; set; }
            public int Quantity { get; set; }
        }

        public class ResultByBIField
        {
            public int TransactionBIFieldID { get; set; }
            public int BusinessIntelligenceFieldID { get; set; }
            public string BusinessIntelligenceFieldName { get; set; }
            public int Quantity { get; set; }
            public bool HasChildren { get; set; }
        }
    }
}