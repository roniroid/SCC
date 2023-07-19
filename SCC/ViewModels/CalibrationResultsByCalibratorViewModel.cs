using SCC_BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SCC.ViewModels
{
    public class CalibrationResultsByCalibratorViewModel
    {
        public SCC_BL.Calibration CalibrationSession { get; set; } = new SCC_BL.Calibration();
        public List<SCC_BL.Transaction> CalibratedTransactionList { get; set; } = new List<SCC_BL.Transaction>();
        public List<SCC_BL.Transaction> CalibrationList { get; set; } = new List<SCC_BL.Transaction>();
        public List<SCC_BL.Transaction> ExpertEvaluationList { get; set; } = new List<SCC_BL.Transaction>();

        //------------------------------------------------------------------------------------------------------------

        public Result FUCE { get; set; }
        public Result BCE { get; set; }
        public Result FCE { get; set; }
        public Result NCE { get; set; }
        public decimal? OverallAccuracy { get; set; } = null;
        public Result SubAttributeAccuracy { get; set; } = null;
        public Result BIAccuracy { get; set; } = null;

        //------------------------------------------------------------------------------------------------------------

        public List<ResultsByCalibrator> ResultsByCalibratorList { get; set; } = new List<ResultsByCalibrator>();
        public List<ResultByAttribute> ResultByAttributeList { get; set; } = new List<ResultByAttribute>();
        public Form Form { get; set; }

        public CalibrationResultsByCalibratorViewModel(Calibration calibrationSession, List<SCC_BL.Transaction> calibrationList, List<SCC_BL.Transaction> calibratedTransactionList)
        {
            this.CalibrationSession = calibrationSession;
            this.CalibrationList = calibrationList;
            this.CalibratedTransactionList = calibratedTransactionList;

            this.ExpertEvaluationList =
                calibrationList
                    .Where(e =>
                        e.EvaluatorUserID == this.CalibrationSession.ExperiencedUserID)
                    .ToList();

            this.CalibrationList =
                this.CalibrationList
                    .Where(e => !this.ExpertEvaluationList.Select(s => s.ID).Contains(e.ID))
                    .ToList();

            this.Calculate();
        }

        public class Result
        {
            public int Success { get; set; } = 0;
            public int Total { get; set; } = 0;
            public decimal PercentageScore { get; set; } = 0;

            //------------------------------------------

            public SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE ErrorType { get; set; }
            public List<SCC_BL.Transaction> CalibrationList { get; set; }
            public List<SCC_BL.Transaction> ExpertEvaluationList { get; set; }
            public Form Form { get; set; }

            public Result()
            {

            }

            public Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE errorType, List<SCC_BL.Transaction> calibrationList, List<SCC_BL.Transaction> expertEvaluationList, int totalTransactionCount, Form form)
            {
                this.ErrorType = errorType;
                this.CalibrationList = calibrationList;
                this.ExpertEvaluationList = expertEvaluationList;

                this.Form = form;

                Calculate(totalTransactionCount);
            }

            public void Calculate(int totalTransactionCount)
            {
                List<SCC_BL.Attribute> auxAttributeList = 
                    this.Form.AttributeList
                        .Where(e => 
                            e.ParentAttributeID == null ||
                            e.ParentAttributeID == 0)
                        .ToList();

                switch (this.ErrorType)
                {
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE:
                        auxAttributeList = 
                            auxAttributeList
                                .Where(e => 
                                    e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE)
                                .ToList();
                        break;
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE:
                        auxAttributeList =
                            auxAttributeList
                                .Where(e =>
                                    e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE)
                                .ToList();
                        break;
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE:
                        auxAttributeList =
                            auxAttributeList
                                .Where(e =>
                                    e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE)
                                .ToList();
                        break;
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE:
                        auxAttributeList =
                            auxAttributeList
                                .Where(e =>
                                    e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE)
                                .ToList();
                        break;
                    default:
                        break;
                }

                this.Total = auxAttributeList.Count() * totalTransactionCount;

                if (this.Total <= 0)
                {
                    this.PercentageScore = 100;
                    return;
                }

                switch (this.ErrorType)
                {
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE:
                        foreach (SCC_BL.Attribute attribute in auxAttributeList)
                        {
                            foreach (Transaction calibration in this.CalibrationList)
                            {
                                TransactionAttributeCatalog calibrationTransactionAttributeCatalog =
                                    calibration.AttributeList
                                        .Where(e =>
                                            e.AttributeID == attribute.ID)
                                        .FirstOrDefault();

                                Transaction expertTransaction =
                                    this.ExpertEvaluationList
                                        .Where(f =>
                                            f.CalibratedTransactionID == calibration.CalibratedTransactionID)
                                        .FirstOrDefault();

                                TransactionAttributeCatalog expertTransactionAttributeCatalog =
                                    expertTransaction.AttributeList
                                        .Where(e =>
                                            e.AttributeID == attribute.ID)
                                        .FirstOrDefault();

                                if (calibrationTransactionAttributeCatalog != null)
                                {
                                    if (calibrationTransactionAttributeCatalog.ScoreValue != null)
                                    {
                                        if (calibrationTransactionAttributeCatalog.ScoreValue == expertTransactionAttributeCatalog.ScoreValue)
                                            this.Success++;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        foreach (SCC_BL.Attribute attribute in auxAttributeList)
                        {
                            foreach (Transaction calibration in this.CalibrationList)
                            {
                                TransactionAttributeCatalog calibrationTransactionAttributeCatalog =
                                    calibration.AttributeList
                                        .Where(e =>
                                            e.AttributeID == attribute.ID)
                                        .FirstOrDefault();

                                Transaction expertTransaction =
                                    this.ExpertEvaluationList
                                        .Where(f =>
                                            f.CalibratedTransactionID == calibration.CalibratedTransactionID)
                                        .FirstOrDefault();

                                TransactionAttributeCatalog expertTransactionAttributeCatalog =
                                    expertTransaction.AttributeList
                                        .Where(e =>
                                            e.AttributeID == attribute.ID)
                                        .FirstOrDefault();

                                if (calibrationTransactionAttributeCatalog != null)
                                {
                                    if (calibrationTransactionAttributeCatalog.ValueID != null)
                                    {
                                        if (calibrationTransactionAttributeCatalog.ValueID == expertTransactionAttributeCatalog.ValueID)
                                            this.Success++;
                                    }
                                    else
                                    {
                                        if (calibrationTransactionAttributeCatalog.Checked == expertTransactionAttributeCatalog.Checked)
                                            this.Success++;
                                    }
                                }
                            }
                        }
                        break;
                }

                this.PercentageScore = (Convert.ToDecimal(this.Success) / this.Total) * 100;
            }

            /*public void Calculate(int totalTransactionCount)
            {
                //this.Total = this.CalibrationList.Count();
                this.Total = totalTransactionCount;

                switch (this.ErrorType)
                {
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE:
                        this.Success =
                            this.CalibrationList
                                .Where(e =>
                                    e.ControllableFinalUserCriticalErrorResultID ==
                                    this.ExpertEvaluationList
                                        .Where(f =>
                                            f.CalibratedTransactionID == e.CalibratedTransactionID)
                                        .FirstOrDefault()
                                        .ControllableFinalUserCriticalErrorResultID)
                                .Count();
                        break;
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE:
                        this.Success =
                            this.CalibrationList
                                .Where(e =>
                                    e.ControllableBusinessCriticalErrorResultID ==
                                    this.ExpertEvaluationList
                                        .Where(f =>
                                            f.CalibratedTransactionID == e.CalibratedTransactionID)
                                        .FirstOrDefault()
                                        .ControllableBusinessCriticalErrorResultID)
                                .Count();
                        break;
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE:
                        this.Success =
                            this.CalibrationList
                                .Where(e =>
                                    e.ControllableFulfillmentCriticalErrorResultID ==
                                    this.ExpertEvaluationList
                                        .Where(f =>
                                            f.CalibratedTransactionID == e.CalibratedTransactionID)
                                        .FirstOrDefault()
                                        .ControllableFulfillmentCriticalErrorResultID)
                                .Count();
                        break;
                    case SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE:
                        this.Success =
                            this.CalibrationList
                                .Where(e =>
                                    e.ControllableNonCriticalErrorResult ==
                                    this.ExpertEvaluationList
                                        .Where(f =>
                                            f.CalibratedTransactionID == e.CalibratedTransactionID)
                                        .FirstOrDefault()
                                        .ControllableNonCriticalErrorResult)
                                .Count();
                        break;
                    default:
                        break;
                }

                this.PercentageScore = (Convert.ToDecimal(this.Success) / this.Total) * 100;
            }*/
        }

        public class ResultsByCalibrator
        {
            public User CalibratorUser { get; set; }
            public List<SCC_BL.Transaction> CalibratedTransactionList { get; set; } = new List<SCC_BL.Transaction>();
            public List<SCC_BL.Transaction> CalibrationList { get; set; } = new List<SCC_BL.Transaction>();
            public List<SCC_BL.Transaction> ExpertEvaluationList { get; set; } = new List<SCC_BL.Transaction>();
            public Form Form { get; set; }

            //----------------------------------------------------------------------------------------------------

            public Result FUCE { get; set; } = new Result();
            public Result BCE { get; set; } = new Result();
            public Result FCE { get; set; } = new Result();
            public Result NCE { get; set; } = new Result();
            public decimal? OverallAccuracy { get; set; } = 0;
            public Result SubAttributeAccuracy { get; set; } = null;
            public Result BIAccuracy { get; set; } = null;

            public List<ResultByAttribute> ResultByAttributeList { get; set; } = new List<ResultByAttribute>();

            public ResultsByCalibrator(User calibratorUser, List<SCC_BL.Transaction> calibrationList, List<SCC_BL.Transaction> calibratedTransactionList, List<SCC_BL.Transaction> expertEvaluationList, Form form, int totalTransactionCount)
            {
                this.CalibratedTransactionList = calibratedTransactionList;
                this.CalibrationList = calibrationList;
                this.ExpertEvaluationList = expertEvaluationList;

                this.CalibratorUser = calibratorUser;

                this.Form = form;

                this.Calculate(totalTransactionCount);
            }

            public void Calculate(int totalTransactionCount)
            {
                //if (this.CalibrationList.Count() <= 0) return;

                this.FUCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE, this.CalibrationList, this.ExpertEvaluationList, totalTransactionCount, this.Form);
                this.BCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE, this.CalibrationList, this.ExpertEvaluationList, totalTransactionCount, this.Form);
                this.FCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE, this.CalibrationList, this.ExpertEvaluationList, totalTransactionCount, this.Form);
                this.NCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE, this.CalibrationList, this.ExpertEvaluationList, totalTransactionCount, this.Form);

                this.OverallAccuracy = GetOverallCalibratorAccuracy();

                foreach (SCC_BL.Attribute attribute in this.Form.AttributeList)
                {
                    ResultByAttribute resultByAttribute =
                        new ResultByAttribute(
                            attribute.ID,
                            this.CalibrationList,
                            this.ExpertEvaluationList,
                            totalTransactionCount);

                    this.ResultByAttributeList.Add(resultByAttribute);
                }
            }

            decimal GetOverallCalibratorAccuracy()
            {
                int total = 0;
                int success = 0;
                decimal percentageScore = 0;

                success += this.FUCE.Success;
                success += this.BCE.Success;
                success += this.FCE.Success;
                //success += this.NCE.Success;

                total += this.FUCE.Total;
                total += this.BCE.Total;
                total += this.FCE.Total;
                //total += this.NCE.Total;

                percentageScore = (Convert.ToDecimal(success) / total) * 100;

                return percentageScore;
            }

            public class ResultByAttribute
            {
                public int AttributeID { get; set; }
                public int Success { get; set; } = 0;
                public int Total { get; set; }
                public decimal PercentageScore { get; set; }

                public ResultByAttribute(int attributeID, List<SCC_BL.Transaction> calibrationList, List<SCC_BL.Transaction> expertEvaluationList, int totalTransactionCount)
                {
                    this.AttributeID = attributeID;

                    //this.Total = calibrationList.Count();
                    this.Total = totalTransactionCount;

                    /*foreach (TransactionAttributeCatalog transactionAttributeCatalog in tempTransactionAttributeCatalogList)
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
                    }*/

                    foreach (SCC_BL.Transaction calibration in calibrationList)
                    {
                        SCC_BL.Transaction expertEvaluation =
                            expertEvaluationList
                                .Where(e =>
                                    e.CalibratedTransactionID == calibration.CalibratedTransactionID.Value)
                                .FirstOrDefault();

                        TransactionAttributeCatalog transactionAttributeCatalog =
                            calibration.AttributeList
                                .Where(e => e.AttributeID == attributeID)
                                .FirstOrDefault();

                        TransactionAttributeCatalog expertTransactionAttributeCatalog =
                            expertEvaluation.AttributeList
                                .Where(e => e.AttributeID == attributeID)
                                .FirstOrDefault();

                        if (transactionAttributeCatalog != null)
                        {
                            if (transactionAttributeCatalog.ValueID != null)
                            {
                                if (transactionAttributeCatalog.ValueID == expertTransactionAttributeCatalog.ValueID)
                                    this.Success++;
                            }
                            else
                            {
                                if (transactionAttributeCatalog.Checked == expertTransactionAttributeCatalog.Checked)
                                    this.Success++;
                            }
                        }
                    }

                    this.PercentageScore = (Convert.ToDecimal(this.Success) / this.Total) * 100;
                }
            }
        }

        void Calculate()
        {
            this.Form = new Form(this.CalibrationList.FirstOrDefault().FormID);
            this.Form.SetDataByID();

            List<User> userList = new List<User>();
            userList = this.CalibrationSession.GetUserList();

            int totalTransactionCount = this.CalibrationSession.TransactionList.Count;
            int totalExpected = totalTransactionCount * userList.Count();

            foreach (User user in userList)
            {
                List<Transaction> calibrationList = new List<Transaction>();
                calibrationList = this.CalibrationList
                    .Where(e =>
                        e.EvaluatorUser.ID == user.ID)
                    .ToList();

                List<Transaction> calibratedTransactionList = new List<Transaction>();
                calibratedTransactionList = this.CalibratedTransactionList
                    .Where(e =>
                        calibrationList.Select(s => s.CalibratedTransactionID).Contains(e.ID))
                    .ToList();

                ResultsByCalibrator resultsByCalibrator = new ResultsByCalibrator(
                    user,
                    calibrationList,
                    calibratedTransactionList,
                    this.ExpertEvaluationList,
                    this.Form,
                    totalTransactionCount);

                this.ResultsByCalibratorList.Add(resultsByCalibrator);
            }

            this.FUCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FUCE, this.CalibrationList, this.ExpertEvaluationList, totalExpected, this.Form);
            this.BCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.BCE, this.CalibrationList, this.ExpertEvaluationList, totalExpected, this.Form);
            this.FCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.FCE, this.CalibrationList, this.ExpertEvaluationList, totalExpected, this.Form);
            this.NCE = new Result(SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE, this.CalibrationList, this.ExpertEvaluationList, totalExpected, this.Form);

            this.OverallAccuracy = GetOverallAccuracy();

            foreach (SCC_BL.Attribute attribute in this.Form.AttributeList)
            {
                ResultByAttribute resultByAttribute =
                    new ResultByAttribute(
                        attribute.ID,
                        this.CalibrationList.Where(e => !this.ExpertEvaluationList.Select(s => s.ID).Contains(e.ID)).ToList(),
                        this.ExpertEvaluationList,
                        totalExpected);

                this.ResultByAttributeList.Add(resultByAttribute);
            }
        }

        decimal GetOverallAccuracy()
        {
            int total = 0;
            int success = 0;
            decimal percentageScore = 0;

            success += this.FUCE.Success;
            success += this.BCE.Success;
            success += this.FCE.Success;
            ///success += this.NCE.Success;

            total += this.FUCE.Total;
            total += this.BCE.Total;
            total += this.FCE.Total;
            //total += this.NCE.Total;

            percentageScore = (Convert.ToDecimal(success) / total) * 100;

            return percentageScore;
        }

        public class ResultByAttribute
        {
            public int AttributeID { get; set; }
            public int Success { get; set; } = 0;
            public int Total { get; set; }
            public decimal PercentageScore { get; set; }

            public ResultByAttribute(int attributeID, List<SCC_BL.Transaction> calibrationList, List<SCC_BL.Transaction> expertEvaluationList, int totalTransactionCount)
            {
                this.AttributeID = attributeID;

                //this.Total = calibrationList.Count();
                this.Total = totalTransactionCount;

                foreach (SCC_BL.Transaction calibration in calibrationList)
                {
                    SCC_BL.Transaction expertEvaluation =
                        expertEvaluationList
                            .Where(e =>
                                e.CalibratedTransactionID == calibration.CalibratedTransactionID.Value)
                            .FirstOrDefault();

                    TransactionAttributeCatalog transactionAttributeCatalog =
                        calibration.AttributeList
                            .Where(e => e.AttributeID == attributeID)
                            .FirstOrDefault();

                    TransactionAttributeCatalog expertTransactionAttributeCatalog =
                        expertEvaluation.AttributeList
                            .Where(e => e.AttributeID == attributeID)
                            .FirstOrDefault();

                    if (transactionAttributeCatalog != null)
                    {
                        if (transactionAttributeCatalog.ValueID != null)
                        {
                            if (transactionAttributeCatalog.ValueID == expertTransactionAttributeCatalog.ValueID)
                                this.Success++;
                        }
                        else
                        {
                            if (transactionAttributeCatalog.Checked == expertTransactionAttributeCatalog.Checked)
                                this.Success++;
                        }
                    }
                }

                this.PercentageScore = (Convert.ToDecimal(this.Success) / this.Total) * 100;
            }
        }
    }
}