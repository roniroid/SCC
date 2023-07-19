using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsCalibratorComparisonViewModel
    {
        public ReportCalibratorComparisonViewModel RequestObject { get; set; } = new ReportCalibratorComparisonViewModel();

        public List<SCC_BL.Reports.Results.CalibratorComparison> CalibratorComparisonResultList { get; set; } = new List<SCC_BL.Reports.Results.CalibratorComparison>();
        public List<ResultsByCalibrator> ResultsByCalibratortList { get; set; } = new List<ResultsByCalibrator>();

        public ReportResultsCalibratorComparisonViewModel()
        {
        }

        public ReportResultsCalibratorComparisonViewModel(List<SCC_BL.Reports.Results.CalibratorComparison> calibratorComparisonResultList)
        {
            this.CalibratorComparisonResultList = calibratorComparisonResultList;

            ProcessData();
        }

        public class ResultsByCalibrator
        {
            public int CalibratorUserID { get; set; } = 0;

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
            foreach (SCC_BL.Reports.Results.CalibratorComparison calibratorComparisonResult in this.CalibratorComparisonResultList)
            {
                if (!this.ResultsByCalibratortList.Select(e => e.CalibratorUserID).Contains(calibratorComparisonResult.CalibratorUserID))
                {
                    ResultsByCalibrator resultsByCalibrator = new ResultsByCalibrator();

                    List<SCC_BL.Reports.Results.CalibratorComparison> listByCalibratorUserID =
                        this.CalibratorComparisonResultList
                            .Where(e =>
                                e.CalibratorUserID == calibratorComparisonResult.CalibratorUserID)
                            .ToList();

                    resultsByCalibrator.CalibratorUserID = calibratorComparisonResult.CalibratorUserID;

                    resultsByCalibrator.TotalTransactions = listByCalibratorUserID.Count();

                    resultsByCalibrator.GeneralFinalUserCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GeneralBusinessCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GeneralFulfillmentCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GeneralResultCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                            .Count();

                    resultsByCalibrator.GeneralFinalUserCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GeneralBusinessCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GeneralFulfillmentCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GeneralResultCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();

                    ResultsByCalibratortList.Add(resultsByCalibrator);
                }
            }
        }
    }
}