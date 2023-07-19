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

            public int GeneralFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GeneralBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GeneralFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GeneralResultCountSuccess { get; set; } = 0;

            public int GeneralFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GeneralBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GeneralFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GeneralResultCountFail { get; set; } = 0;
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
                    resultsByUser.GeneralResultCountSuccess =
                        listByUserID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
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
                    resultsByUser.GeneralResultCountFail =
                        listByUserID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();

                    this.ResultsByUserList.Add(resultsByUser);
                }
            }
        }
    }
}