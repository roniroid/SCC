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

        public ReportResultsAccuracyTrendByAttributeViewModel(
            List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> accuracyTrendByAttributeResultList, 
            SCC_BL.DBValues.Catalog.TIME_INTERVAL intervalTypeID,
            ReportAccuracyTrendByAttributeViewModel requestObject = null)
        {
            this.IntervalTypeID = intervalTypeID;
            this.AccuracyTrendByAttributeResultList = accuracyTrendByAttributeResultList;
            this.RequestObject = requestObject;

            ProcessData();
        }

        public void ProcessData()
        {
            if (RequestObject.TransactionStartDate == null && RequestObject.TransactionEndDate == null)
                return;

            DateTime minDate = this.AccuracyTrendByAttributeResultList.Count() > 0 ? this.AccuracyTrendByAttributeResultList.Min(e => e.TransactionDate) : RequestObject.TransactionStartDate.Value;
            DateTime maxDate = this.AccuracyTrendByAttributeResultList.Count() > 0 ? this.AccuracyTrendByAttributeResultList.Max(e => e.TransactionDate) : RequestObject.TransactionStartDate.Value;

            switch (this.IntervalTypeID)
            {
                case SCC_BL.DBValues.Catalog.TIME_INTERVAL.DAY:
                    int dayOfWeek = (int)maxDate.DayOfWeek;
                    maxDate = maxDate.AddDays(7 - dayOfWeek);
                    break;
                case SCC_BL.DBValues.Catalog.TIME_INTERVAL.WEEK:
                    //Set maxDate to the end of month
                    maxDate = new DateTime(maxDate.Year, maxDate.Month + 1, 1, 23, 59, 59, 999);
                    maxDate = maxDate.AddDays(-1);
                    break;
                case SCC_BL.DBValues.Catalog.TIME_INTERVAL.MONTH:
                    //Set maxDate to the end of year
                    maxDate = new DateTime(maxDate.Year + 1, 1, 1, 23, 59, 59, 999);
                    maxDate = maxDate.AddDays(-1);
                    break;
                case SCC_BL.DBValues.Catalog.TIME_INTERVAL.QUARTER:
                    //Set maxDate to the end of month
                    maxDate = new DateTime(maxDate.Year, maxDate.Month + 1, 1, 23, 59, 59, 999);
                    maxDate = maxDate.AddDays(-1);
                    break;
                case SCC_BL.DBValues.Catalog.TIME_INTERVAL.YEAR:
                    maxDate = new DateTime(maxDate.Year, 12, 31, 23, 59, 59, 999);
                    break;
                default:
                    break;
            }

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

            //Take the ones that have data
            this.AccuracyTrendByPeriodList =
                this.AccuracyTrendByPeriodList
                    .Where(e => e.TransactionCount > 0)
                    .ToList();
        }
    }
}