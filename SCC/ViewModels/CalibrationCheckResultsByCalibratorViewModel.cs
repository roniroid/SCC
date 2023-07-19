using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SCC.ViewModels.CalibrationCheckResultsByCalibratorViewModel.ResultsByCalibrator;

namespace SCC.ViewModels
{
    public class CalibrationCheckResultsByCalibratorViewModel
    {
        public SCC_BL.Calibration CalibrationSession { get; set; } = new SCC_BL.Calibration();
        public List<SCC_BL.Transaction> CalibratedTransactionList { get; set; } = new List<SCC_BL.Transaction>();
        public List<SCC_BL.Transaction> CalibrationList { get; set; } = new List<SCC_BL.Transaction>();
        public SCC_BL.Transaction ExpertEvaluation { get; set; } = new SCC_BL.Transaction();

        //------------------------------------------------------------------------------------------------------------

        public List<ResultsByCalibrator> ResultsByCalibratorList { get; set; } = new List<ResultsByCalibrator>();
        public Form Form { get; set; }

        //------------------------------------------------------------------------------------------------------------

        public Result FUCE { get; set; }
        public Result BCE { get; set; }
        public Result FCE { get; set; }
        public Result NCE { get; set; }
        public decimal? OverallCalibratorAccuracy { get; set; } = null;
        public Result SubAttributeAccuracy { get; set; } = null;
        public Result BIAccuracy { get; set; } = null;

        public class ResultsByCalibrator
        {
            public User CalibratorUser { get; set; }
            public List<SCC_BL.Transaction> CalibratedTransactionList { get; set; } = new List<SCC_BL.Transaction>();
            public List<SCC_BL.Transaction> CalibrationList { get; set; } = new List<SCC_BL.Transaction>();

            //----------------------------------------------------------------------------------------------------

            public Result FUCE { get; set; }
            public Result BCE { get; set; }
            public Result FCE { get; set; }
            public Result NCE { get; set; }
            public decimal? OverallCalibratorAccuracy { get; set; } = null;
            public Result SubAttributeAccuracy { get; set; } = null;
            public Result BIAccuracy { get; set; } = null;

            public List<ResultByAttribute> ResultByAttributeList { get; set; } = new List<ResultByAttribute>();

            public class Result
            {
                public int Success { get; set; } = 0;
                public int Total { get; set; } = 0;
                public decimal PercentageScore { get; set; } = 0;

                //------------------------------------------

                public SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE ErrorType { get; set; }
                public List<SCC_BL.Transaction> CalibrationList { get; set; }

                public Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE errorType, List<SCC_BL.Transaction> calibrationList)
                {
                    this.ErrorType = errorType;
                    this.CalibrationList = calibrationList;

                    Calculate();
                }

                public void Calculate()
                {
                    this.Total = this.CalibrationList.Count();

                    switch (this.ErrorType)
                    {
                        case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE:
                            this.Success =
                                this.CalibrationList
                                    .Where(e =>
                                        e.ControllableFinalUserCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS)
                                    .Count();
                            break;
                        case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE:
                            this.Success =
                                this.CalibrationList
                                    .Where(e =>
                                        e.ControllableBusinessCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS)
                                    .Count();
                            break;
                        case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE:
                            this.Success =
                                this.CalibrationList
                                    .Where(e =>
                                        e.ControllableFulfillmentCriticalErrorResultID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS)
                                    .Count();
                            break;
                        case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE:
                            this.Success =
                                this.CalibrationList
                                    .Where(e =>
                                        e.ControllableNonCriticalErrorResult >= SCC_BL.Settings.AppValues.TRANSACTION_MINIMUM_NCE_SCORE)
                                    .Count();
                            break;
                        default:
                            break;
                    }

                    this.PercentageScore = (Convert.ToDecimal(this.Success) / this.Total) * 100;
                }
            }

            public class ResultByAttribute
            {
                public int AttributeID { get; set; }
                public int Success { get; set; } = 0;
                public int Total { get; set; }
                public decimal PercentageScore { get; set; }
                public decimal? SessionAccuracy { get; set; }

                public ResultByAttribute(int attributeID, List<SCC_BL.Transaction> calibrationList)
                {
                    this.AttributeID = attributeID;

                    this.Total = 
                        calibrationList
                            .Where(e => 
                                e.AttributeList.Select(s => s.AttributeID).Contains(attributeID))
                            .Count();

                    List<TransactionAttributeCatalog> tempTransactionAttributeCatalogList = 
                        calibrationList
                            .SelectMany(e => 
                                e.AttributeList)
                            .Where(e => 
                                e.AttributeID == attributeID)
                            .ToList();

                    foreach (TransactionAttributeCatalog transactionAttributeCatalog in tempTransactionAttributeCatalogList)
                    {
                        if (transactionAttributeCatalog.ValueID != null)
                        {
                            AttributeValueCatalog attributeValueCatalog = new AttributeValueCatalog(transactionAttributeCatalog.ValueID.Value);
                            attributeValueCatalog.SetDataByID();

                            if (!attributeValueCatalog.TriggersChildVisualization)
                                this.Success++;
                        }
                        else
                        {
                            if (!transactionAttributeCatalog.Checked)
                                this.Success++;
                        }
                    }

                    this.PercentageScore = (Convert.ToDecimal(this.Success) / this.Total) * 100;
                }
            }

            public ResultsByCalibrator(User calibratorUser, List<SCC_BL.Transaction> calibrationList, List<SCC_BL.Transaction> calibratedTransactionList, Form form)
            {
                this.CalibratedTransactionList = calibratedTransactionList;
                this.CalibrationList = calibrationList;
                this.CalibratorUser = calibratorUser;

                this.Calculate(form);
            }

            decimal GetOverallCalibratorAccuracy()
            {
                int total = 0;
                int success = 0;
                decimal percentageScore = 0;

                success += this.FUCE.Success;
                success += this.BCE.Success;
                success += this.FCE.Success;
                success += this.NCE.Success;

                total += this.FUCE.Total;
                total += this.BCE.Total;
                total += this.FCE.Total;
                total += this.NCE.Total;

                percentageScore = (Convert.ToDecimal(success) / total) * 100;

                return percentageScore;
            }

            public void Calculate(Form form)
            {
                if (this.CalibrationList.Count() <= 0) return;

                this.FUCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE, this.CalibrationList);
                this.BCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE, this.CalibrationList);
                this.FCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE, this.CalibrationList);
                this.NCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE, this.CalibrationList);

                this.OverallCalibratorAccuracy = GetOverallCalibratorAccuracy();

                foreach (SCC_BL.Attribute attribute in form.AttributeList)
                {
                    ResultByAttribute resultByAttribute = new ResultByAttribute(attribute.ID, this.CalibrationList);
                    this.ResultByAttributeList.Add(resultByAttribute);
                }
            }
        }

        public void Calculate()
        {
            this.Form = new Form(this.CalibrationList.FirstOrDefault().FormID);
            this.Form.SetDataByID();

            List<User> userList = new List<User>();
            userList = this.CalibrationSession.GetUserList();

            foreach (User user in userList)
            {
                List<Transaction> calibrationList = new List<Transaction>();
                calibrationList = this.CalibrationList
                    .Where(e =>
                        e.EvaluatorUser.ID == user.ID)
                    .ToList();

                List<Transaction> calibratedTransactionList = new List<Transaction>();
                calibrationList = this.CalibratedTransactionList
                    .Where(e =>
                        calibrationList.Select(s => s.CalibratedTransactionID).Contains(e.ID))
                    .ToList();

                ResultsByCalibrator resultsByCalibrator = new ResultsByCalibrator(
                    user,
                    calibrationList,
                    calibratedTransactionList,
                    this.Form);

                this.ResultsByCalibratorList.Add(resultsByCalibrator);
            }

            this.FUCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE, this.CalibrationList);
            this.BCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE, this.CalibrationList);
            this.FCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE, this.CalibrationList);
            this.NCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE, this.CalibrationList);

            this.OverallCalibratorAccuracy = GetOverallCalibratorAccuracy();
        }

        decimal GetOverallCalibratorAccuracy()
        {
            int total = 0;
            int success = 0;
            decimal percentageScore = 0;

            success += this.FUCE.Success;
            success += this.BCE.Success;
            success += this.FCE.Success;
            success += this.NCE.Success;

            total += this.FUCE.Total;
            total += this.BCE.Total;
            total += this.FCE.Total;
            total += this.NCE.Total;

            percentageScore = (Convert.ToDecimal(success) / total) * 100;

            return percentageScore;
        }
    }
}