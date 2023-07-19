using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsAccuracyTrendViewModel
    {
        public ReportAccuracyTrendViewModel RequestObject { get; set; } = new ReportAccuracyTrendViewModel();

        public SCC_BL.DBValues.Catalog.TIME_INTERVAL IntervalTypeID { get; set; }
        public List<SCC_BL.Reports.Results.AccuracyTrend> AccuracyTrendResultList { get; set; } = new List<SCC_BL.Reports.Results.AccuracyTrend>();
        public List<AccuracyTrendByPeriod> AccuracyTrendByPeriodList { get; set; } = new List<AccuracyTrendByPeriod>();

        public class AccuracyTrendByPeriod
        {
            public DateTime Period { get; set; }

            public Double FinalUserCriticalErrorPercentage { get; set; }
            public Double BusinessCriticalErrorPercentage { get; set; }
            public Double FulfillmentCriticalErrorPercentage { get; set; }
            public Double GeneralCriticalErrorPercentage { get; set; }

            public int TransactionCount { get; set; }

            public AccuracyTrendByPeriod(DateTime period, Double finalUserCriticalErrorPercentage, Double businessCriticalErrorPercentage, Double fulfillmentCriticalErrorPercentage, Double generalCriticalErrorPercentage, int transactionCount)
            {
                this.Period = period;

                this.FinalUserCriticalErrorPercentage = finalUserCriticalErrorPercentage;
                this.BusinessCriticalErrorPercentage = businessCriticalErrorPercentage;
                this.FulfillmentCriticalErrorPercentage = fulfillmentCriticalErrorPercentage;
                this.GeneralCriticalErrorPercentage = generalCriticalErrorPercentage;

                this.TransactionCount = transactionCount;
            }
        }

        public ReportResultsAccuracyTrendViewModel()
        {
        }

        public ReportResultsAccuracyTrendViewModel(List<SCC_BL.Reports.Results.AccuracyTrend> accuracyTrendResultList, SCC_BL.DBValues.Catalog.TIME_INTERVAL intervalTypeID)
        {
            this.IntervalTypeID = intervalTypeID;
            this.AccuracyTrendResultList = accuracyTrendResultList;

            ProcessData();
        }

        public void ProcessData()
        {
            DateTime minDate = this.AccuracyTrendResultList.Min(e => e.TransactionStartDate);
            DateTime maxDate = this.AccuracyTrendResultList.Max(e => e.TransactionStartDate);

            minDate = minDate.AddDays((minDate.Day - 1) * -1);

            while (minDate < maxDate)
            {
                List<SCC_BL.Reports.Results.AccuracyTrend> tempAccuracyTrendResultList = new List<SCC_BL.Reports.Results.AccuracyTrend>();

                DateTime newMinDate = minDate;

                switch (this.IntervalTypeID)
                {
                    case SCC_BL.DBValues.Catalog.TIME_INTERVAL.DAY:
                        newMinDate = newMinDate.AddDays(1);
                        break;
                    case SCC_BL.DBValues.Catalog.TIME_INTERVAL.WEEK:
                        newMinDate = newMinDate.AddDays(7);
                        break;
                    case SCC_BL.DBValues.Catalog.TIME_INTERVAL.MONTH:
                        newMinDate = newMinDate.AddMonths(1);
                        break;
                    case SCC_BL.DBValues.Catalog.TIME_INTERVAL.QUARTER:
                        newMinDate = newMinDate.AddMonths(3);
                        break;
                    case SCC_BL.DBValues.Catalog.TIME_INTERVAL.YEAR:
                        newMinDate = newMinDate.AddYears(1);
                        break;
                    default:
                        break;
                }

                bool firstStep = true;

                if (newMinDate > maxDate && firstStep)
                {
                    newMinDate = maxDate;
                    firstStep = false;
                    break;
                }

                tempAccuracyTrendResultList = 
                    this.AccuracyTrendResultList
                        .Where(e =>
                            e.TransactionStartDate >= minDate &&
                            e.TransactionStartDate <= newMinDate)
                        .ToList();

                Double percentageFinalUserCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.FinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageBusinessCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.BusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageFulfilmentCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.FulfilmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageGlobalCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.GlobalCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                AccuracyTrendByPeriod accuracyTrendByPeriod = new AccuracyTrendByPeriod(
                        minDate,
                        percentageFinalUserCriticalError > 0 ? percentageFinalUserCriticalError : 0,
                        percentageBusinessCriticalError > 0 ? percentageBusinessCriticalError : 0,
                        percentageFulfilmentCriticalError > 0 ? percentageFulfilmentCriticalError : 0,
                        percentageGlobalCriticalError > 0 ? percentageGlobalCriticalError : 0,
                        tempAccuracyTrendResultList.Count()
                    );

                this.AccuracyTrendByPeriodList.Add(accuracyTrendByPeriod);

                minDate = newMinDate;
            }
        }
    }
}