using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsComparativeByUserViewModel
    {
        public ReportComparativeByUserViewModel RequestObject { get; set; } = new ReportComparativeByUserViewModel();

        public List<SCC_BL.Reports.Results.ComparativeByUser> ComparativeByUserResultList { get; set; } = new List<SCC_BL.Reports.Results.ComparativeByUser>();
        public List<ResultsByUser> ResultsByUserList { get; set; } = new List<ResultsByUser>();

        public ReportResultsComparativeByUserViewModel()
        {
        }

        public ReportResultsComparativeByUserViewModel(List<SCC_BL.Reports.Results.ComparativeByUser> comparativeByUserResultList)
        {
            this.ComparativeByUserResultList = comparativeByUserResultList;

            ProcessData();
        }

        public class ResultsByUser
        {
            public int UserID { get; set; } = 0;

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
        }

        public void ProcessData()
        {
            foreach (SCC_BL.Reports.Results.ComparativeByUser comparativeByUserResult in this.ComparativeByUserResultList)
            {
                if (!this.ResultsByUserList.Select(e => e.UserID).Contains(comparativeByUserResult.UserID))
                {
                    ResultsByUser resultsByUser = new ResultsByUser();

                    List<SCC_BL.Reports.Results.ComparativeByUser> listByUserID =
                        this.ComparativeByUserResultList
                            .Where(e =>
                                e.UserID == comparativeByUserResult.UserID)
                            .ToList();

                    resultsByUser.UserID = comparativeByUserResult.UserID;

                    resultsByUser.TotalTransactions = listByUserID.Count();

                    resultsByUser.GeneralResultCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByUser.GeneralFinalUserCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByUser.GeneralBusinessCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByUser.GeneralFulfillmentCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByUser.GeneralResultCountFail =
                        listByUserID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByUser.GeneralFinalUserCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByUser.GeneralBusinessCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByUser.GeneralFulfillmentCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    resultsByUser.GeneralNonCriticalErrorAverageResult =
                        listByUserID
                            .Where(e =>
                                e.GeneralNonCriticalErrorAverageResult != null)
                            .Sum(e => e.GeneralNonCriticalErrorAverageResult.Value) /
                            resultsByUser.TotalTransactions;

                    //------------------------------------------------------------------------------------------------------------------------------------------------------------

                    resultsByUser.AccurateResultCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.AccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByUser.AccurateFinalUserCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.AccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByUser.AccurateBusinessCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.AccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByUser.AccurateFulfillmentCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.AccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByUser.AccurateResultCountFail =
                        listByUserID
                            .Where(e =>
                                e.AccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByUser.AccurateFinalUserCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.AccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByUser.AccurateBusinessCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.AccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByUser.AccurateFulfillmentCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.AccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    resultsByUser.AccurateNonCriticalErrorAverageResult =
                        listByUserID
                            .Where(e =>
                                e.AccurateNonCriticalErrorAverageResult != null)
                            .Sum(e => e.AccurateNonCriticalErrorAverageResult.Value) /
                            resultsByUser.TotalTransactions;

                    //------------------------------------------------------------------------------------------------------------------------------------------------------------

                    resultsByUser.ControllableResultCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.ControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByUser.ControllableFinalUserCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.ControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByUser.ControllableBusinessCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.ControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByUser.ControllableFulfillmentCriticalErrorCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.ControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByUser.ControllableResultCountFail =
                        listByUserID
                            .Where(e =>
                                e.ControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByUser.ControllableFinalUserCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.ControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByUser.ControllableBusinessCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.ControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByUser.ControllableFulfillmentCriticalErrorCountFail =
                        listByUserID
                            .Where(e =>
                                e.ControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    resultsByUser.ControllableNonCriticalErrorAverageResult =
                        listByUserID
                            .Where(e =>
                                e.ControllableNonCriticalErrorAverageResult != null)
                            .Sum(e => e.ControllableNonCriticalErrorAverageResult.Value) /
                            resultsByUser.TotalTransactions;

                    this.ResultsByUserList.Add(resultsByUser);
                }
            }
        }
    }
}