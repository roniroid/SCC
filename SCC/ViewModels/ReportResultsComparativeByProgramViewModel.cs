using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsComparativeByProgramViewModel
    {
        public ReportComparativeByProgramViewModel RequestObject { get; set; } = new ReportComparativeByProgramViewModel();

        public List<SCC_BL.Reports.Results.ComparativeByProgram> ComparativeByProgramResultList { get; set; } = new List<SCC_BL.Reports.Results.ComparativeByProgram>();
        public List<ResultsByProgram> ResultsByProgramList { get; set; } = new List<ResultsByProgram>();

        public ReportResultsComparativeByProgramViewModel()
        {
        }

        public ReportResultsComparativeByProgramViewModel(List<SCC_BL.Reports.Results.ComparativeByProgram> comparativeByProgramResultList)
        {
            this.ComparativeByProgramResultList = comparativeByProgramResultList;

            ProcessData();
        }

        public class ResultsByProgram
        {
            public int ProgramID { get; set; } = 0;

            public int TotalTransactions { get; set; } = 0;

            public int GlobalGeneralResultCountSuccess { get; set; } = 0;
            public int GlobalGeneralFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralFulfillmentCriticalErrorCountSuccess { get; set; } = 0;

            public int GlobalGeneralResultCountFail { get; set; } = 0;
            public int GlobalGeneralFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralFulfillmentCriticalErrorCountFail { get; set; } = 0;

            //-------------------------------------------------------------------------

            public int GlobalAccurateResultCountSuccess { get; set; } = 0;
            public int GlobalAccurateFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
                       
            public int GlobalAccurateResultCountFail { get; set; } = 0;
            public int GlobalAccurateFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateFulfillmentCriticalErrorCountFail { get; set; } = 0;

            //-------------------------------------------------------------------------

            public int GlobalControllableResultCountSuccess { get; set; } = 0;
            public int GlobalControllableFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
                       
            public int GlobalControllableResultCountFail { get; set; } = 0;
            public int GlobalControllableFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableFulfillmentCriticalErrorCountFail { get; set; } = 0;
        }

        public void ProcessData()
        {
            foreach (SCC_BL.Reports.Results.ComparativeByProgram comparativeByProgramResult in this.ComparativeByProgramResultList)
            {
                if (!this.ResultsByProgramList.Select(e => e.ProgramID).Contains(comparativeByProgramResult.ProgramID))
                {
                    ResultsByProgram resultsByProgram = new ResultsByProgram();

                    List<SCC_BL.Reports.Results.ComparativeByProgram> listByProgramID =
                        this.ComparativeByProgramResultList
                            .Where(e =>
                                e.ProgramID == comparativeByProgramResult.ProgramID)
                            .ToList();

                    resultsByProgram.ProgramID = comparativeByProgramResult.ProgramID;

                    resultsByProgram.TotalTransactions = listByProgramID.Count();


                    resultsByProgram.GlobalGeneralResultCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalGeneralFinalUserCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalGeneralBusinessCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalGeneralFulfillmentCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByProgram.GlobalGeneralResultCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByProgram.GlobalGeneralFinalUserCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GlobalGeneralBusinessCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GlobalGeneralFulfillmentCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalGeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    //--------------------------------------------------------------------------------------------------------------------------------------------------------

                    resultsByProgram.GlobalAccurateResultCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalAccurateFinalUserCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalAccurateBusinessCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalAccurateFulfillmentCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByProgram.GlobalAccurateResultCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByProgram.GlobalAccurateFinalUserCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GlobalAccurateBusinessCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GlobalAccurateFulfillmentCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalAccurateFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    //--------------------------------------------------------------------------------------------------------------------------------------------------------

                    resultsByProgram.GlobalControllableResultCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalControllableFinalUserCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalControllableBusinessCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GlobalControllableFulfillmentCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();

                    resultsByProgram.GlobalControllableResultCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL.FAIL)
                            .Count();
                    resultsByProgram.GlobalControllableFinalUserCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GlobalControllableBusinessCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GlobalControllableFulfillmentCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GlobalControllableFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();

                    this.ResultsByProgramList.Add(resultsByProgram);
                }
            }
        }
    }
}