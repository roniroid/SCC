using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsCalibratorComparisonWithAttributesViewModel
    {
        public ReportCalibratorComparisonWithAttributesViewModel RequestObject { get; set; } = new ReportCalibratorComparisonWithAttributesViewModel();

        public List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes> CalibratorComparisonWithAttributesResultList { get; set; } = new List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes>();
        public List<ResultsByCalibrator> ResultsByCalibratortList { get; set; } = new List<ResultsByCalibrator>();

        public ReportResultsCalibratorComparisonWithAttributesViewModel()
        {
        }

        public ReportResultsCalibratorComparisonWithAttributesViewModel(List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes> calibratorComparisonWithAttributesResultList)
        {
            this.CalibratorComparisonWithAttributesResultList = calibratorComparisonWithAttributesResultList;

            ProcessData();
        }

        public class ResultsByCalibrator
        {
            public int CalibratorUserID { get; set; } = 0;
            public User CalibratorUser { get; set; } = new User();

            public int TotalTransactions { get; set; } = 0;

            public int GlobalGeneralResultCountSuccess { get; set; } = 0;
            public int GlobalGeneralFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalGeneralNonCriticalErrorCountSuccess { get; set; } = 0;

            public int GlobalGeneralResultCountFail { get; set; } = 0;
            public int GlobalGeneralFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GlobalGeneralNonCriticalErrorCountFail { get; set; } = 0;
        }

        public void ProcessData()
        {
            List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes> expertAttributeList = new List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes>();
            List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes> calibratorAttributeList = new List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes>();

            expertAttributeList =
                this.CalibratorComparisonWithAttributesResultList
                    .Where(e => e.IsExpertsCalibration)
                    .ToList();

            calibratorAttributeList =
                this.CalibratorComparisonWithAttributesResultList
                    .Where(e => !e.IsExpertsCalibration)
                    .ToList();

            int[] calibratorIDArray =
                calibratorAttributeList
                    .Select(e => e.CalibratorUserID)
                    .GroupBy(e => e)
                    .Select(e => e.First())
                    .ToArray();

            int[] attributeIDArray =
                this.CalibratorComparisonWithAttributesResultList
                    .Select(e => e.AttributeID)
                    .GroupBy(e => e)
                    .Select(e => e.First())
                    .ToArray();

            int[] transactionIDArray =
                this.CalibratorComparisonWithAttributesResultList
                    .Select(e => e.TransactionID)
                    .GroupBy(e => e)
                    .Select(e => e.First())
                    .ToArray();

            for (int i = 0; i < calibratorIDArray.Length; i++)
            {
                ResultsByCalibrator resultsByCalibrator = new ResultsByCalibrator();

                List<SCC_BL.Reports.Results.CalibratorComparisonWithAttributes> listByCalibratorUserID =
                    calibratorAttributeList
                        .Where(e =>
                            e.CalibratorUserID == calibratorIDArray[i])
                        .ToList();

                resultsByCalibrator.CalibratorUserID = calibratorIDArray[i];

                using (User user = new User(resultsByCalibrator.CalibratorUserID))
                {
                    user.SetDataByID(true);
                    resultsByCalibrator.CalibratorUser = user;
                }

                resultsByCalibrator.TotalTransactions = transactionIDArray.Length;

                for (int j = 0; j < attributeIDArray.Length; j++)
                {
                    for (int k = 0; k < transactionIDArray.Length; k++)
                    {
                        if (!listByCalibratorUserID.Select(e => e.TransactionID).Contains(transactionIDArray[k]))
                            continue;

                        if (!expertAttributeList.Select(e => e.TransactionID).Contains(transactionIDArray[k]))
                            continue;

                        SCC_BL.Reports.Results.CalibratorComparisonWithAttributes currentExpertResult =
                            expertAttributeList
                                .Where(e =>
                                    e.AttributeID == attributeIDArray[j] &&
                                    e.TransactionID == transactionIDArray[k])
                                .FirstOrDefault();

                        SCC_BL.Reports.Results.CalibratorComparisonWithAttributes currentCalibratorResult =
                            listByCalibratorUserID
                                .Where(e =>
                                    e.AttributeID == attributeIDArray[j] &&
                                    e.TransactionID == transactionIDArray[k])
                                .FirstOrDefault();

                        bool isCalibrated = false;

                        if (currentCalibratorResult.ValueID != null)
                        {
                            if (currentCalibratorResult.ValueID == currentExpertResult.ValueID)
                                isCalibrated = true;
                        }
                        else
                        {
                            if (currentCalibratorResult.Checked == currentExpertResult.Checked)
                                isCalibrated = true;
                        }

                        switch ((SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE)currentCalibratorResult.ErrorTypeID)
                        {
                            case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE:
                                if (isCalibrated)
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                                    resultsByCalibrator.GlobalGeneralFinalUserCriticalErrorCountSuccess += 1;
                                }
                                else
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                                    resultsByCalibrator.GlobalGeneralFinalUserCriticalErrorCountFail += 1;
                                }
                                break;
                            case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE:
                                if (isCalibrated)
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                                    resultsByCalibrator.GlobalGeneralBusinessCriticalErrorCountSuccess += 1;
                                }
                                else
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                                    resultsByCalibrator.GlobalGeneralBusinessCriticalErrorCountFail += 1;
                                }
                                break;
                            case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE:
                                if (isCalibrated)
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                                    resultsByCalibrator.GlobalGeneralFulfillmentCriticalErrorCountSuccess += 1;
                                }
                                else
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                                    resultsByCalibrator.GlobalGeneralFulfillmentCriticalErrorCountFail += 1;
                                }
                                break;
                            case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE:
                                if (isCalibrated)
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                                    resultsByCalibrator.GlobalGeneralNonCriticalErrorCountSuccess += 1;
                                }
                                else
                                {
                                    resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                                    resultsByCalibrator.GlobalGeneralNonCriticalErrorCountFail += 1;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                this.ResultsByCalibratortList.Add(resultsByCalibrator);
            }
        }
    }
}