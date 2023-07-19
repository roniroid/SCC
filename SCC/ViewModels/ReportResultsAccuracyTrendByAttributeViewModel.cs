using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsAccuracyTrendByAttributeViewModel
    {
        public ReportAccuracyTrendByAttributeViewModel RequestObject { get; set; } = new ReportAccuracyTrendByAttributeViewModel();

        public SCC_BL.DBValues.Catalog.TIME_INTERVAL IntervalTypeID { get; set; }
        public List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> AccuracyTrendByAttributeResultList { get; set; } = new List<SCC_BL.Reports.Results.AccuracyTrendByAttribute>();
        public List<AccuracyTrendByPeriod> AccuracyTrendByPeriodList { get; set; } = new List<AccuracyTrendByPeriod>();

        public class AccuracyTrendByPeriod
        {
            public DateTime Period { get; set; }
            public List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> AccuracyTrendByAttributeInPeriod { get; set; }
            public int TransactionCount { get; set; }

            public AccuracyTrendByPeriod(DateTime period, List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> accuracyTrendByAttributeInPeriod, int transactionCount)
            {
                this.Period = period;
                this.AccuracyTrendByAttributeInPeriod = accuracyTrendByAttributeInPeriod;
                this.TransactionCount = transactionCount;
            }
        }

        public ReportResultsAccuracyTrendByAttributeViewModel()
        {
        }

        public ReportResultsAccuracyTrendByAttributeViewModel(List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> accuracyTrendByAttributeResultList, SCC_BL.DBValues.Catalog.TIME_INTERVAL intervalTypeID)
        {
            this.IntervalTypeID = intervalTypeID;
            this.AccuracyTrendByAttributeResultList = accuracyTrendByAttributeResultList;

            ProcessData();
        }

        public void ProcessData()
        {
            DateTime minDate = this.AccuracyTrendByAttributeResultList.Min(e => e.TransactionDate);
            DateTime maxDate = this.AccuracyTrendByAttributeResultList.Max(e => e.TransactionDate);

            minDate = minDate.AddDays((minDate.Day - 1) * -1);

            while (minDate < maxDate)
            {
                List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> tempAccuracyTrendByAttributeResultList = new List<SCC_BL.Reports.Results.AccuracyTrendByAttribute>();

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

                tempAccuracyTrendByAttributeResultList =
                    this.AccuracyTrendByAttributeResultList
                        .Where(e =>
                            e.TransactionDate >= minDate &&
                            e.TransactionDate <= newMinDate)
                        .ToList();

                AccuracyTrendByPeriod accuracyTrendByPeriod = new AccuracyTrendByPeriod(
                        minDate,
                        tempAccuracyTrendByAttributeResultList,
                        tempAccuracyTrendByAttributeResultList.Count()
                    );

                this.AccuracyTrendByPeriodList.Add(accuracyTrendByPeriod);

                minDate = newMinDate;
            }
        }
    }
}