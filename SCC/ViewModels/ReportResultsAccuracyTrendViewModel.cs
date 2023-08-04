﻿using System;
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

            public int TransactionCount { get; set; }

            public int GeneralResultCountSuccess { get; set; } = 0;
            public int GeneralFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GeneralBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GeneralFulfillmentCriticalErrorCountSuccess { get; set; } = 0;

            public int GeneralResultCountFail { get; set; } = 0;
            public int GeneralFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GeneralBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GeneralFulfillmentCriticalErrorCountFail { get; set; } = 0;

            public double GeneralNonCriticalErrorAverageResult { get; set; } = 0;

            public int AccurateResultCountSuccess { get; set; } = 0;
            public int AccurateFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int AccurateBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int AccurateFulfillmentCriticalErrorCountSuccess { get; set; } = 0;

            public int AccurateResultCountFail { get; set; } = 0;
            public int AccurateFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int AccurateBusinessCriticalErrorCountFail { get; set; } = 0;
            public int AccurateFulfillmentCriticalErrorCountFail { get; set; } = 0;

            public double AccurateNonCriticalErrorAverageResult { get; set; } = 0;

            public int ControllableResultCountSuccess { get; set; } = 0;
            public int ControllableFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int ControllableBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int ControllableFulfillmentCriticalErrorCountSuccess { get; set; } = 0;

            public int ControllableResultCountFail { get; set; } = 0;
            public int ControllableFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int ControllableBusinessCriticalErrorCountFail { get; set; } = 0;
            public int ControllableFulfillmentCriticalErrorCountFail { get; set; } = 0;

            public double ControllableNonCriticalErrorAverageResult { get; set; } = 0;

            //RESULTS:

            public Double GeneralCriticalErrorPercentage { get; set; }
            public Double GeneralFinalUserCriticalErrorPercentage { get; set; }
            public Double GeneralBusinessCriticalErrorPercentage { get; set; }
            public Double GeneralFulfillmentCriticalErrorPercentage { get; set; }

            public Double AccurateCriticalErrorPercentage { get; set; }
            public Double AccurateFinalUserCriticalErrorPercentage { get; set; }
            public Double AccurateBusinessCriticalErrorPercentage { get; set; }
            public Double AccurateFulfillmentCriticalErrorPercentage { get; set; }

            public Double ControllableCriticalErrorPercentage { get; set; }
            public Double ControllableFinalUserCriticalErrorPercentage { get; set; }
            public Double ControllableBusinessCriticalErrorPercentage { get; set; }
            public Double ControllableFulfillmentCriticalErrorPercentage { get; set; }

            public AccuracyTrendByPeriod(
                DateTime period,
                Double generalCriticalErrorPercentage,
                Double generalFinalUserCriticalErrorPercentage, 
                Double generalBusinessCriticalErrorPercentage, 
                Double generalFulfillmentCriticalErrorPercentage,

                Double accurateCriticalErrorPercentage,
                Double accurateFinalUserCriticalErrorPercentage,
                Double accurateBusinessCriticalErrorPercentage,
                Double accurateFulfillmentCriticalErrorPercentage,

                Double controllableCriticalErrorPercentage,
                Double controllableFinalUserCriticalErrorPercentage,
                Double controllableBusinessCriticalErrorPercentage,
                Double controllableFulfillmentCriticalErrorPercentage,
                int transactionCount)
            {
                this.Period = period;

                this.GeneralCriticalErrorPercentage = generalCriticalErrorPercentage;
                this.GeneralFinalUserCriticalErrorPercentage = generalFinalUserCriticalErrorPercentage;
                this.GeneralBusinessCriticalErrorPercentage = generalBusinessCriticalErrorPercentage;
                this.GeneralFulfillmentCriticalErrorPercentage = generalFulfillmentCriticalErrorPercentage;

                this.AccurateCriticalErrorPercentage = accurateCriticalErrorPercentage;
                this.AccurateFinalUserCriticalErrorPercentage = accurateFinalUserCriticalErrorPercentage;
                this.AccurateBusinessCriticalErrorPercentage = accurateBusinessCriticalErrorPercentage;
                this.AccurateFulfillmentCriticalErrorPercentage = accurateFulfillmentCriticalErrorPercentage;

                this.ControllableCriticalErrorPercentage = controllableCriticalErrorPercentage;
                this.ControllableFinalUserCriticalErrorPercentage = controllableFinalUserCriticalErrorPercentage;
                this.ControllableBusinessCriticalErrorPercentage = controllableBusinessCriticalErrorPercentage;
                this.ControllableFulfillmentCriticalErrorPercentage = controllableFulfillmentCriticalErrorPercentage;

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

                Double percentageGeneralGlobalCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.GeneralGlobalCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageGeneralFinalUserCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.GeneralFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageGeneralBusinessCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.GeneralBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageGeneralFulfilmentCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.GeneralFulfilmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;


                Double percentageAccurateGlobalCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.AccurateGlobalCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageAccurateFinalUserCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.AccurateFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageAccurateBusinessCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.AccurateBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageAccurateFulfilmentCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.AccurateFulfilmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;


                Double percentageControllableGlobalCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.ControllableGlobalCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageControllableFinalUserCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.ControllableFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageControllableBusinessCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.ControllableBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;

                Double percentageControllableFulfilmentCriticalError =
                    ((Double)tempAccuracyTrendResultList
                        .Where(e =>
                            e.ControllableFulfilmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                        .Count() / tempAccuracyTrendResultList.Count()) * 100;


                AccuracyTrendByPeriod accuracyTrendByPeriod = new AccuracyTrendByPeriod(
                        minDate,
                        percentageGeneralGlobalCriticalError > 0 ? percentageGeneralGlobalCriticalError : 0,
                        percentageGeneralFinalUserCriticalError > 0 ? percentageGeneralFinalUserCriticalError : 0,
                        percentageGeneralBusinessCriticalError > 0 ? percentageGeneralBusinessCriticalError : 0,
                        percentageGeneralFulfilmentCriticalError > 0 ? percentageGeneralFulfilmentCriticalError : 0,

                        percentageAccurateGlobalCriticalError > 0 ? percentageAccurateGlobalCriticalError : 0,
                        percentageAccurateFinalUserCriticalError > 0 ? percentageAccurateFinalUserCriticalError : 0,
                        percentageAccurateBusinessCriticalError > 0 ? percentageAccurateBusinessCriticalError : 0,
                        percentageAccurateFulfilmentCriticalError > 0 ? percentageAccurateFulfilmentCriticalError : 0,

                        percentageControllableGlobalCriticalError > 0 ? percentageControllableGlobalCriticalError : 0,
                        percentageControllableFinalUserCriticalError > 0 ? percentageControllableFinalUserCriticalError : 0,
                        percentageControllableBusinessCriticalError > 0 ? percentageControllableBusinessCriticalError : 0,
                        percentageControllableFulfilmentCriticalError > 0 ? percentageControllableFulfilmentCriticalError : 0,
                        tempAccuracyTrendResultList.Count()
                    );

                this.AccuracyTrendByPeriodList.Add(accuracyTrendByPeriod);

                minDate = newMinDate;
            }
        }
    }
}