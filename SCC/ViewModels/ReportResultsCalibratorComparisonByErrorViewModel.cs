using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportResultsCalibratorComparisonByErrorViewModel
    {
        public ReportCalibratorComparisonByErrorViewModel RequestObject { get; set; } = new ReportCalibratorComparisonByErrorViewModel();

        public List<SCC_BL.Reports.Results.CalibratorComparisonByError> CalibratorComparisonByErrorResultList { get; set; } = new List<SCC_BL.Reports.Results.CalibratorComparisonByError>();
        public List<ResultsByCalibrator> ResultsByCalibratortList { get; set; } = new List<ResultsByCalibrator>();

        public ReportResultsCalibratorComparisonByErrorViewModel()
        {
        }

        public ReportResultsCalibratorComparisonByErrorViewModel(List<SCC_BL.Reports.Results.CalibratorComparisonByError> calibratorComparisonByErrorResultList, ReportCalibratorComparisonByErrorViewModel requestObject)
        {
            this.CalibratorComparisonByErrorResultList = calibratorComparisonByErrorResultList;
            this.RequestObject = requestObject;

            this.RequestObject.SetDescriptiveData();

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

            //----------------------------------------------------------------------------------

            public int GlobalAccurateResultCountSuccess { get; set; } = 0;
            public int GlobalAccurateFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalAccurateNonCriticalErrorCountSuccess { get; set; } = 0;

            public int GlobalAccurateResultCountFail { get; set; } = 0;
            public int GlobalAccurateFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GlobalAccurateNonCriticalErrorCountFail { get; set; } = 0;

            //----------------------------------------------------------------------------------

            public int GlobalControllableResultCountSuccess { get; set; } = 0;
            public int GlobalControllableFinalUserCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableBusinessCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableFulfillmentCriticalErrorCountSuccess { get; set; } = 0;
            public int GlobalControllableNonCriticalErrorCountSuccess { get; set; } = 0;

            public int GlobalControllableResultCountFail { get; set; } = 0;
            public int GlobalControllableFinalUserCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableBusinessCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableFulfillmentCriticalErrorCountFail { get; set; } = 0;
            public int GlobalControllableNonCriticalErrorCountFail { get; set; } = 0;
        }

        public void ProcessData()
        {
            List<SCC_BL.Reports.Results.CalibratorComparisonByError> expertAttributeList = new List<SCC_BL.Reports.Results.CalibratorComparisonByError>();
            List<SCC_BL.Reports.Results.CalibratorComparisonByError> calibratorAttributeList = new List<SCC_BL.Reports.Results.CalibratorComparisonByError>();

            expertAttributeList =
                this.CalibratorComparisonByErrorResultList
                    .Where(e => e.IsExpertsCalibration)
                    .ToList();

            calibratorAttributeList =
                this.CalibratorComparisonByErrorResultList
                    .Where(e => !e.IsExpertsCalibration)
                    .ToList();

            int[] calibratorIDArray =
                calibratorAttributeList
                    .Select(e => e.CalibratorUserID)
                    .GroupBy(e => e)
                    .Select(e => e.First())
                    .ToArray();

            int[] transactionIDArray =
                this.CalibratorComparisonByErrorResultList
                    .Select(e => e.TransactionID)
                    .GroupBy(e => e)
                    .Select(e => e.First())
                    .ToArray();

            for (int i = 0; i < calibratorIDArray.Length; i++)
            {
                ResultsByCalibrator resultsByCalibrator = new ResultsByCalibrator();

                List<SCC_BL.Reports.Results.CalibratorComparisonByError> listByCalibratorUserID =
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

                for (int k = 0; k < transactionIDArray.Length; k++)
                {
                    if (!listByCalibratorUserID.Select(e => e.TransactionID).Contains(transactionIDArray[k]))
                        continue;

                    if (!expertAttributeList.Select(e => e.TransactionID).Contains(transactionIDArray[k]))
                        continue;

                    SCC_BL.Reports.Results.CalibratorComparisonByError currentExpertResult =
                        expertAttributeList
                            .Where(e =>
                                e.TransactionID == transactionIDArray[k])
                            .FirstOrDefault();

                    SCC_BL.Reports.Results.CalibratorComparisonByError currentCalibratorResult =
                        listByCalibratorUserID
                            .Where(e =>
                                e.TransactionID == transactionIDArray[k])
                            .FirstOrDefault();

                    if (currentCalibratorResult.GlobalGeneralFinalUserCriticalErrorResultID == currentExpertResult.GlobalGeneralFinalUserCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalGeneralFinalUserCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalGeneralFinalUserCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalAccurateFinalUserCriticalErrorResultID == currentExpertResult.GlobalAccurateFinalUserCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalAccurateFinalUserCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalAccurateFinalUserCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalControllableFinalUserCriticalErrorResultID == currentExpertResult.GlobalControllableFinalUserCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalControllableFinalUserCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalControllableFinalUserCriticalErrorCountFail += 1;
                    }

                    //---------------------------------------------------------------------------------------------------------------------

                    if (currentCalibratorResult.GlobalGeneralBusinessCriticalErrorResultID == currentExpertResult.GlobalGeneralBusinessCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalGeneralBusinessCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalGeneralBusinessCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalAccurateBusinessCriticalErrorResultID == currentExpertResult.GlobalAccurateBusinessCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalAccurateBusinessCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalAccurateBusinessCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalControllableBusinessCriticalErrorResultID == currentExpertResult.GlobalControllableBusinessCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalControllableBusinessCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalControllableBusinessCriticalErrorCountFail += 1;
                    }

                    //---------------------------------------------------------------------------------------------------------------------

                    if (currentCalibratorResult.GlobalGeneralFulfillmentCriticalErrorResultID == currentExpertResult.GlobalGeneralFulfillmentCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalGeneralFulfillmentCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalGeneralFulfillmentCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalAccurateFulfillmentCriticalErrorResultID == currentExpertResult.GlobalAccurateFulfillmentCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalAccurateFulfillmentCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalAccurateFulfillmentCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalControllableFulfillmentCriticalErrorResultID == currentExpertResult.GlobalControllableFulfillmentCriticalErrorResultID)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalControllableFulfillmentCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalControllableFulfillmentCriticalErrorCountFail += 1;
                    }

                    //---------------------------------------------------------------------------------------------------------------------

                    if (currentCalibratorResult.GlobalGeneralNonCriticalErrorResult == currentExpertResult.GlobalGeneralNonCriticalErrorResult)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalGeneralNonCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE))
                        {
                            resultsByCalibrator.GlobalGeneralResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalGeneralNonCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalAccurateNonCriticalErrorResult == currentExpertResult.GlobalAccurateNonCriticalErrorResult)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalAccurateNonCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE))
                        {
                            resultsByCalibrator.GlobalAccurateResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalAccurateNonCriticalErrorCountFail += 1;
                    }

                    if (currentCalibratorResult.GlobalControllableNonCriticalErrorResult == currentExpertResult.GlobalControllableNonCriticalErrorResult)
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountSuccess += 1;
                        }

                        resultsByCalibrator.GlobalControllableNonCriticalErrorCountSuccess += 1;
                    }
                    else
                    {
                        if (this.RequestObject.ErrorTypeIDArray.Contains((int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE))
                        {
                            resultsByCalibrator.GlobalControllableResultCountFail += 1;
                        }

                        resultsByCalibrator.GlobalControllableNonCriticalErrorCountFail += 1;
                    }
                }

                this.ResultsByCalibratortList.Add(resultsByCalibrator);
            }
        }
    }
}