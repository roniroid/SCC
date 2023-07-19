using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsOverallAccuracyViewModel
    {
        public ReportOverallAccuracyViewModel RequestObject { get; set; } = new ReportOverallAccuracyViewModel();

        public List<SCC_BL.Reports.Results.OverallAccuracy> OverallAccuracyResultList { get; set; } = new List<SCC_BL.Reports.Results.OverallAccuracy>();

        public int TotalTransactions { get; set; } = 0;

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

        public ReportResultsOverallAccuracyViewModel()
        {
        }

        public ReportResultsOverallAccuracyViewModel(List<SCC_BL.Reports.Results.OverallAccuracy> overallAccuracyResultList)
        {
            this.OverallAccuracyResultList = overallAccuracyResultList;

            ProcessData();
        }

        public void ProcessData()
        {
            this.TotalTransactions = OverallAccuracyResultList.Count();

            this.GeneralResultCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                    .Count();
            this.GeneralFinalUserCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                    .Count();
            this.GeneralBusinessCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                    .Count();
            this.GeneralFulfillmentCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                    .Count();

            this.GeneralResultCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                    .Count();
            this.GeneralFinalUserCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                    .Count();
            this.GeneralBusinessCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                    .Count();
            this.GeneralFulfillmentCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                    .Count();

            this.GeneralNonCriticalErrorAverageResult =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.GeneralNonCriticalErrorResult != null)
                    .Sum(e => e.GeneralNonCriticalErrorResult.Value) /
                    this.TotalTransactions;

            //----------------------------------------------------------------------------------------------------------------------------------------------------------

            this.AccurateResultCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.SUCCESS)
                    .Count();
            this.AccurateFinalUserCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                    .Count();
            this.AccurateBusinessCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                    .Count();
            this.AccurateFulfillmentCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                    .Count();

            this.AccurateResultCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.FAIL)
                    .Count();
            this.AccurateFinalUserCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                    .Count();
            this.AccurateBusinessCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                    .Count();
            this.AccurateFulfillmentCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                    .Count();

            this.AccurateNonCriticalErrorAverageResult =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.AccurateNonCriticalErrorResult != null)
                    .Sum(e => e.AccurateNonCriticalErrorResult.Value) /
                    this.TotalTransactions;

            //----------------------------------------------------------------------------------------------------------------------------------------------------------

            this.ControllableResultCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.SUCCESS)
                    .Count();
            this.ControllableFinalUserCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                    .Count();
            this.ControllableBusinessCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                    .Count();
            this.ControllableFulfillmentCriticalErrorCountSuccess =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                    .Count();

            this.ControllableResultCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.FAIL)
                    .Count();
            this.ControllableFinalUserCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                    .Count();
            this.ControllableBusinessCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                    .Count();
            this.ControllableFulfillmentCriticalErrorCountFail =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                    .Count();

            this.ControllableNonCriticalErrorAverageResult =
                this.OverallAccuracyResultList
                    .Where(e =>
                        e.ControllableNonCriticalErrorResult != null)
                    .Sum(e => e.ControllableNonCriticalErrorResult.Value) /
                    this.TotalTransactions;
        }
    }
}