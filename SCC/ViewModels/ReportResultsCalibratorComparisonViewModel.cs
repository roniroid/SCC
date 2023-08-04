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

            public int GlobalGeneralFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralResultCountSuccess { get; set; } = 0;

            public int GlobalGeneralFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralResultCountFail { get; set; } = 0;

            //----------------------------------------------------------------------------------

            public int GlobalAccurateFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateResultCountSuccess { get; set; } = 0;

            public int GlobalAccurateFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateResultCountFail { get; set; } = 0;

            //----------------------------------------------------------------------------------

            public int GlobalControllableFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableResultCountSuccess { get; set; } = 0;

            public int GlobalControllableFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableResultCountFail { get; set; } = 0;
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

                    resultsByCalibrator.GlobalGeneralResultCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalGeneralFinalUserCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalGeneralBusinessCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalGeneralFulfillmentCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByCalibrator.GlobalGeneralResultCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalGeneralFinalUserCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalGeneralBusinessCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalGeneralFulfillmentCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalGeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    //------------------------------------------------------------------------------------------------------------------------------------------------------------

                    resultsByCalibrator.GlobalAccurateResultCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalAccurateFinalUserCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalAccurateBusinessCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalAccurateFulfillmentCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByCalibrator.GlobalAccurateResultCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalAccurateFinalUserCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalAccurateBusinessCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalAccurateFulfillmentCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalAccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    //------------------------------------------------------------------------------------------------------------------------------------------------------------

                    resultsByCalibrator.GlobalControllableResultCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalControllableFinalUserCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalControllableBusinessCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByCalibrator.GlobalControllableFulfillmentCriticalErrorCountSuccess =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByCalibrator.GlobalControllableResultCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalControllableFinalUserCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalControllableBusinessCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByCalibrator.GlobalControllableFulfillmentCriticalErrorCountFail =
                        listByCalibratorUserID
                            .Where(e =>
                                e.GlobalControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    ResultsByCalibratortList.Add(resultsByCalibrator);
                }
            }
        }
    }
}