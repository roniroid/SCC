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

                    resultsByProgram.GeneralFinalUserCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GeneralBusinessCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GeneralFulfillmentCriticalErrorCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                            .Count();
                    resultsByProgram.GeneralResultCountSuccess =
                        listByProgramID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS)
                            .Count();

                    resultsByProgram.GeneralFinalUserCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GeneralFinalUserCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GeneralBusinessCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GeneralBusinessCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GeneralFulfillmentCriticalErrorCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GeneralFulfillmentCriticalErrorResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL)
                            .Count();
                    resultsByProgram.GeneralResultCountFail =
                        listByProgramID
                            .Where(e =>
                                e.GeneralResultID.Value == (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL)
                            .Count();

                    this.ResultsByProgramList.Add(resultsByProgram);
                }
            }
        }
    }
}