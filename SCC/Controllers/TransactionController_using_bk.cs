using SCC.ViewModels;
using SCC_BL;
using SCC_BL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class TransactionController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(TransactionController));

        public ActionResult Edit(int? transactionID, int? calibratedTransactionID = null, int typeID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.EVALUATION)
        {
            int transactionProgram = 0;
            Transaction transaction = new Transaction((SCC_BL.DBValues.Catalog.TRANSACTION_TYPE)typeID, calibratedTransactionID);
            transaction.EvaluatorUserID = GetActualUser().ID;
            transaction.SetIdentifier();

            if (transactionID != null && transactionID > 0)
            {
                transaction = new Transaction(transactionID.Value);
                transaction.SetDataByID();

                using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(transaction.FormID))
                {
                    transactionProgram = programFormCatalog.SelectByFormID().FirstOrDefault().ProgramID;
                }
            }

            List<Program> programList = new List<Program>();

            using (Program program = new Program())
                programList =
                    program.SelectWithForm()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Edit.ProgramList.NAME] =
                new SelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    transactionProgram);

            return View(transaction);
        }

        [HttpGet]
        public ActionResult _FormView(int? programID = null, int? formID = null, int? transactionID = null, bool hasDisputation = false, bool hasInvalidation = false, bool hasDevolution = false, int? calibratedTransactionID = null, int typeID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.EVALUATION)
        {
            try
            {
                List<User> userList = new List<User>();

                using (User user = new User())
                    userList =
                        user.SelectAll()
                            .Where(e =>
                                e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                                e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                            .OrderBy(o => o.Person.SurName)
                            .ThenBy(o => o.Person.LastName)
                            .ThenBy(o => o.Person.FirstName)
                            .ToList();

                TransactionFormViewModel transactionFormViewModel = new TransactionFormViewModel();
                transactionFormViewModel.Transaction.SetIdentifier();
                transactionFormViewModel.Transaction.EvaluatorUserID = GetActualUser().ID;
                transactionFormViewModel.Transaction.CalibratedTransactionID = calibratedTransactionID;
                transactionFormViewModel.Transaction.TypeID = typeID;

                if (transactionID != null)
                {
                    transactionFormViewModel.Transaction = new Transaction(transactionID.Value);
                    transactionFormViewModel.Transaction.SetDataByID();

                    transactionFormViewModel.Form = new Form(transactionFormViewModel.Transaction.FormID);
                    transactionFormViewModel.Form.SetDataByID();
                }

                int? userToEvaluate = null;

                if (transactionFormViewModel.Transaction.UserToEvaluateID > 0)
                    userToEvaluate = transactionFormViewModel.Transaction.UserToEvaluateID;

                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.UserList.NAME] =
                    new SelectList(
                        userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                        "Key",
                        "Value",
                        userToEvaluate);

                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.HasDisputation.NAME] = hasDisputation;
                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.HasInvalidation.NAME] = hasInvalidation;
                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.HasDevolution.NAME] = hasDevolution;

                if (transactionID != null)
                    return PartialView(transactionFormViewModel);

                if (formID != null)
                {
                    transactionFormViewModel.Form = new Form(formID.Value);
                    transactionFormViewModel.Form.SetDataByID();
                    return PartialView(transactionFormViewModel);
                }
                else
                if (programID != null)
                {
                    using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithProgramID(programID.Value))
                    {
                        transactionFormViewModel.Form.ID = programFormCatalog.SelectByProgramID().FirstOrDefault().FormID;
                        transactionFormViewModel.Form.SetDataByID();
                        return PartialView(transactionFormViewModel);
                    }
                }

                SaveProcessingInformation<SCC_BL.Results.Transaction.FormView.Error>();
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.FormView.Error>(null, null, null, ex);
            }

            return Json(new { error_url = Url.Action(nameof(TransactionController.Edit), _mainControllerName) }, JsonRequestBehavior.AllowGet);

            //return RedirectToAction();
        }

        [HttpPost]
        public ActionResult Create(
            Transaction transaction,
            List<SCC_BL.TransactionAttributeCatalog> transactionAttributeList,
            List<SCC_BL.TransactionCustomFieldCatalog> transactionCustomFieldIDList,
            List<SCC_BL.TransactionBIFieldCatalog> transactionBIFieldList,
            List<SCC_BL.TransactionCommentary> transactionCommentaryList,
            string[] transactionLabelArray)
        {
            Transaction newTransaction = new Transaction(
                transaction.Identifier,
                transaction.UserToEvaluateID,
                transaction.EvaluatorUserID,
                transaction.EvaluationDate,
                transaction.TransactionDate,
                transaction.FormID,
                transaction.Comment,
                transaction.GeneralResultID,
                transaction.GeneralFinalUserCriticalErrorResultID,
                transaction.GeneralBusinessCriticalErrorResultID,
                transaction.GeneralFulfillmentCriticalErrorResultID,
                transaction.GeneralNonCriticalErrorResult,
                transaction.AccurateResultID,
                transaction.AccurateFinalUserCriticalErrorResultID,
                transaction.AccurateBusinessCriticalErrorResultID,
                transaction.AccurateFulfillmentCriticalErrorResultID,
                transaction.AccurateNonCriticalErrorResult,
                transaction.ControllableResultID,
                transaction.ControllableFinalUserCriticalErrorResultID,
                transaction.ControllableBusinessCriticalErrorResultID,
                transaction.ControllableFulfillmentCriticalErrorResultID,
                transaction.ControllableNonCriticalErrorResult,
                transaction.TimeElapsed,
                GetActualUser().ID,
                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION.CREATED,
                transaction.TypeID,
                transaction.CalibratedTransactionID);

            try
            {
                int result = newTransaction.Insert();

                if (result > 0)
                {
                    newTransaction.SetDataByID();

                    UpdateAttributeList(newTransaction, transactionAttributeList);

                    UpdateDisputeCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE)
                            .ToList());

                    UpdateInvalidationCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION)
                            .ToList());

                    UpdateDevolutionCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
                            .ToList());

                    UpdateTransactionLabelList(newTransaction, transactionLabelArray);

                    SaveProcessingInformation<SCC_BL.Results.Transaction.Insert.Success>(newTransaction.ID, newTransaction.BasicInfo.StatusID, newTransaction);
                }
                else
                {
                    switch ((SCC_BL.Results.Transaction.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Transaction.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Transaction.Insert.Error>(null, null, newTransaction);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.Insert.Error>(null, null, null, ex);
            }

            return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName) });
        }

        void UpdateAttributeList(Transaction transaction, List<SCC_BL.TransactionAttributeCatalog> transactionAttributeList)
        {
            try
            {
                switch (transaction.UpdateAttributeList(transactionAttributeList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateAttributeList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateAttributeList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateAttributeList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateDisputeCommentList(Transaction transaction, List<SCC_BL.TransactionCommentary> transactionCommentaryList)
        {
            try
            {
                switch (transaction.UpdateDisputeCommentList(transactionCommentaryList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateDisputeCommentList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDisputeCommentList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDisputeCommentList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateInvalidationCommentList(Transaction transaction, List<SCC_BL.TransactionCommentary> transactionCommentaryList)
        {
            try
            {
                switch (transaction.UpdateInvalidationCommentList(transactionCommentaryList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateInvalidationCommentList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateInvalidationCommentList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateInvalidationCommentList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateDevolutionCommentList(Transaction transaction, List<SCC_BL.TransactionCommentary> transactionCommentaryList)
        {
            try
            {
                switch (transaction.UpdateDevolutionCommentList(transactionCommentaryList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateDevolutionCommentList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDevolutionCommentList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDevolutionCommentList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateTransactionLabelList(Transaction transaction, string[] labelList)
        {
            try
            {
                switch (transaction.UpdateTransactionLabelList(labelList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateTransactionLabelList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateTransactionLabelList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateTransactionLabelList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        [HttpPost]
        public ActionResult Edit(
            Transaction transaction,
            List<SCC_BL.TransactionAttributeCatalog> transactionAttributeList,
            List<SCC_BL.TransactionCustomFieldCatalog> transactionCustomFieldIDList,
            List<SCC_BL.TransactionBIFieldCatalog> transactionBIFieldList,
            List<SCC_BL.TransactionCommentary> transactionCommentaryList,
            string[] transactionLabelArray)
        {
            Transaction oldTransaction = new Transaction(transaction.ID);
            oldTransaction.SetDataByID();

            Transaction newTransaction = new Transaction(
                transaction.ID,
                transaction.Identifier,
                transaction.UserToEvaluateID,
                transaction.EvaluatorUserID,
                transaction.EvaluationDate,
                transaction.TransactionDate,
                transaction.FormID,
                transaction.Comment,
                transaction.GeneralResultID,
                transaction.GeneralFinalUserCriticalErrorResultID,
                transaction.GeneralBusinessCriticalErrorResultID,
                transaction.GeneralFulfillmentCriticalErrorResultID,
                transaction.GeneralNonCriticalErrorResult,
                transaction.AccurateResultID,
                transaction.AccurateFinalUserCriticalErrorResultID,
                transaction.AccurateBusinessCriticalErrorResultID,
                transaction.AccurateFulfillmentCriticalErrorResultID,
                transaction.AccurateNonCriticalErrorResult,
                transaction.ControllableResultID,
                transaction.ControllableFinalUserCriticalErrorResultID,
                transaction.ControllableBusinessCriticalErrorResultID,
                transaction.ControllableFulfillmentCriticalErrorResultID,
                transaction.ControllableNonCriticalErrorResult,
                transaction.TimeElapsed,
                transaction.BasicInfoID,
                GetActualUser().ID,
                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION.UPDATED);

            try
            {
                int result = newTransaction.Update();

                if (result > 0)
                {
                    newTransaction.SetDataByID();

                    UpdateAttributeList(newTransaction, transactionAttributeList);

                    UpdateDisputeCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE)
                            .ToList());

                    UpdateInvalidationCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION)
                            .ToList());

                    UpdateDevolutionCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
                            .ToList());

                    UpdateTransactionLabelList(newTransaction, transactionLabelArray);

                    SaveProcessingInformation<SCC_BL.Results.Transaction.Update.Success>(newTransaction.ID, newTransaction.BasicInfo.StatusID, oldTransaction);
                }
                else
                {
                    switch ((SCC_BL.Results.Transaction.Update.CODE)result)
                    {
                        case SCC_BL.Results.Transaction.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Transaction.Update.Error>(oldTransaction.ID, oldTransaction.BasicInfo.StatusID, newTransaction);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.Update.Error>(oldTransaction.ID, oldTransaction.BasicInfo.StatusID, oldTransaction, ex);
            }

            return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName) });
        }

        public ActionResult Search(SCC_BL.Helpers.TransactionSearchHelper transactionSearchHelper = null)
        {
            TransactionSearchViewModel transactionSearchViewModel = new TransactionSearchViewModel();

            bool hasHelper = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in transactionSearchHelper.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(transactionSearchHelper) != null)
                {
                    hasHelper = true;
                    break;
                }
            }

            if (hasHelper)
            {
                transactionSearchViewModel.TransactionSearchHelper = transactionSearchHelper;

                using (Transaction transaction = new Transaction())
                {
                    transactionSearchViewModel.TransactionList = transaction.Search(transactionSearchHelper);
                }
            }

            List<Catalog> catalogSearchStringType = new List<Catalog>();
            List<Catalog> catalogSearchTimeUnitType = new List<Catalog>();
            List<Catalog> catalogUserStatus = new List<Catalog>();
            List<Workspace> workspaceList = new List<Workspace>();
            List<Program> programList = new List<Program>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_SEARCH_STRING_TYPE))
                catalogSearchStringType =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_SEARCH_TIME_UNIT_TYPE))
                catalogSearchTimeUnitType =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.STATUS_USER))
                catalogUserStatus =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (Workspace workspace = new Workspace())
                workspaceList =
                    workspace.SelectAll();

            using (Program program = new Program())
                programList =
                    program.SelectAll();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.StringTypeID.NAME] =
                new SelectList(
                    catalogSearchStringType,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.TimeUnitTypeID.NAME] =
                new SelectList(
                    catalogSearchTimeUnitType,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.UserStatus.NAME] =
                new SelectList(
                    catalogUserStatus,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    transactionSearchHelper.UserStatusID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.Workspace.NAME] =
                new MultiSelectList(
                    workspaceList,
                    nameof(Workspace.ID),
                    nameof(Workspace.Name),
                    transactionSearchHelper.WorkspaceIDList != null
                        ? transactionSearchHelper.WorkspaceIDList.Select(e => e)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    transactionSearchHelper.ProgramIDList != null
                        ? transactionSearchHelper.ProgramIDList.Select(e => e)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.YesNoQuestion.NAME] =
                new SelectList(
                    new SelectListItem[]
                    {
                        new SelectListItem()
                        {
                            Text = "No importa",
                            Value = null
                        },
                        new SelectListItem()
                        {
                            Text = "Sí",
                            Value = ((int)SCC_BL.Settings.AppValues.ViewData.Transaction.Search.YesNoQuestion.Values.YES).ToString()
                        },
                        new SelectListItem()
                        {
                            Text = "No",
                            Value = ((int)SCC_BL.Settings.AppValues.ViewData.Transaction.Search.YesNoQuestion.Values.NO).ToString()
                        }
                    },
                    nameof(SelectListItem.Value),
                    nameof(SelectListItem.Text));

            return View(transactionSearchViewModel);
        }

        public static object GetValueFromTransaction(int valueID, SCC_BL.DBValues.Catalog.ELEMENT category)
        {
            switch (category)
            {
                case SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_ATTRIBUTE:
                    using (AttributeValueCatalog attributeValueCatalog = new AttributeValueCatalog(valueID))
                    {
                        attributeValueCatalog.SetDataByID();
                        return attributeValueCatalog;
                    }
                case SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_BUSINESSINTELLIGENCEFIELD:
                    using (BusinessIntelligenceValueCatalog businessIntelligenceValueCatalog = new BusinessIntelligenceValueCatalog(valueID))
                    {
                        businessIntelligenceValueCatalog.SetDataByID();
                        return businessIntelligenceValueCatalog;
                    }
                case SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_CUSTOMCONTROL:
                    using (CustomControlValueCatalog customControlValueCatalog = new CustomControlValueCatalog(valueID))
                    {
                        customControlValueCatalog.SetDataByID();
                        return customControlValueCatalog;
                    }
                default:
                    return null;
            }
        }

        List<ViewModels.TransactionImportViewModel.Error> _transactionImportErrorList = new List<TransactionImportViewModel.Error>();

        public ActionResult ImportData(int programID)
        {
            List<UploadedFile> uploadedFileList = new List<UploadedFile>();

            using (UploadedFile uploadedFile = new UploadedFile())
            {
                uploadedFileList =
                    uploadedFile.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_DATA_IMPORT)
                        .ToList();

            }

            ViewModels.TransactionImportViewModel transactionImportViewModel = new ViewModels.TransactionImportViewModel();
            transactionImportViewModel.UploadedFileList = uploadedFileList;
            transactionImportViewModel.ErrorList = _transactionImportErrorList;

            return View(transactionImportViewModel);
        }

        [HttpPost]
        public ActionResult ImportDataAction(HttpPostedFileBase file, int programID)
        {
            _transactionImportErrorList = new List<TransactionImportViewModel.Error>();

            string filePath = SaveUploadedFile(file, SCC_BL.Settings.Paths.Transaction.TRANSACTION_IMPORT_FOLDER);

            if (!string.IsNullOrEmpty(filePath))
            {
                int uploadedFileID = 0;

                uploadedFileID = SaveFileInDatabase(filePath, (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_DATA_IMPORT);

                if (uploadedFileID > 0)
                {
                    using (UploadedFile uploadedFile = new UploadedFile(uploadedFileID))
                    {
                        uploadedFile.SetDataByID();
                        uploadedFile.Data = new byte[0];
                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.Insert.Success>(uploadedFile.ID, uploadedFile.BasicInfo.StatusID, uploadedFile);
                    }

                    try
                    {
                        SCC_BL.Results.UploadedFile.TransactionImport.CODE result = ProcessImportExcel(filePath, programID);

                        switch (result)
                        {
                            case SCC_BL.Results.UploadedFile.TransactionImport.CODE.SUCCESS:
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Success>();
                                break;
                            case SCC_BL.Results.UploadedFile.TransactionImport.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>(null, null, null, ex);
                    }
                }
            }
            else
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>();
            }

            return RedirectToAction(nameof(TransactionController.ImportData), new { programID = programID });
        }

        public SCC_BL.Results.UploadedFile.TransactionImport.CODE ProcessImportExcel(string filePath, int programID)
        {
            try
            {
                using (DocumentFormat.OpenXml.Packaging.SpreadsheetDocument document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(filePath, false))
                {
                    DocumentFormat.OpenXml.Packaging.WorkbookPart wbPart = document.WorkbookPart;
                    var workSheet = wbPart.Workbook.Descendants<DocumentFormat.OpenXml.Spreadsheet.Sheet>().FirstOrDefault();
                    if (workSheet != null)
                    {
                        DocumentFormat.OpenXml.Packaging.WorksheetPart wsPart = (DocumentFormat.OpenXml.Packaging.WorksheetPart)(wbPart.GetPartById(workSheet.Id));
                        IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rows = wsPart.Worksheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();

                        int headersCount = rows.ElementAt(0).Count();

                        /*DateTime? minDate = GetMinDate(rows);
                        DateTime? maxDate = GetMaxDate(rows);*/
                        
                        DateTime? minDate = DateTime.Now;
                        DateTime? maxDate = DateTime.Now;

                        DocumentFormat.OpenXml.Spreadsheet.Cell[] headerRow = new DocumentFormat.OpenXml.Spreadsheet.Cell[0];

                        using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                            headerRow = excelParser.GetRowCells(rows.ElementAt(0), headersCount).ToArray();

                        Parallel.ForEach(rows.Skip(1), (row) => {
                            DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentCells = new DocumentFormat.OpenXml.Spreadsheet.Cell[0];

                            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                                auxCurrentCells = excelParser.GetRowCells(row, headersCount).ToArray();

                            ProcessRow(headerRow, auxCurrentCells, rows.ToList().IndexOf(row), minDate, maxDate, programID);
                        });

                        /*for (int i = 1; i < rows.Count(); i++)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentCells = excelParser.GetRowCells(rows.ElementAt(i), headersCount).ToArray();
                            ProcessRow(headerRow, auxCurrentCells, i + 1, minDate, maxDate, programID);
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(ex);
            }

            if (_transactionImportErrorList.Count() > 0)
            {
                string lineList = string.Empty;

                _transactionImportErrorList
                    .ForEach(e => {
                        if (!string.IsNullOrEmpty(lineList))
                            lineList += ", ";

                        lineList += e.RowIndex.ToString();
                    });

                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(
                    new Exception(
                        SCC_BL.Results.UploadedFile.FormUpload.ErrorSingleRow.CUSTOM_ERROR_EXCEL_LINES
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, lineList)
                    )
                );

                return SCC_BL.Results.UploadedFile.TransactionImport.CODE.ERROR;
            }

            return SCC_BL.Results.UploadedFile.TransactionImport.CODE.SUCCESS;
        }

        void ProcessRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] headerRow, DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCells, int currentRowCount, DateTime? minDate, DateTime? maxDate, int programID)
        {
            bool hasError = false;

            int headersCount = headerRow.Count();

            int firstAttributeIndex = 0;
            int firstAttributeCommentaryIndex = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.CONSTANT_END_INDEX + 1;

            int lastAttributeIndex = 0;

            for (int i = firstAttributeCommentaryIndex; i < headerRow.Length; i++)
            {
                string currentValue = string.Empty;

                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    currentValue = excelParser.GetCellValue(headerRow[i]).ToString().Trim();

                if (currentValue.Equals("Comentarios_1"))
                {
                    firstAttributeIndex = i - 1;
                    //break;
                }

                if (currentValue.Length >= 13)
                {
                    if (currentValue.Substring(0, 13).Equals("Subatributos_"))
                        lastAttributeIndex = i;
                }
            }

            int customControlStartIndex = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.CONSTANT_END_INDEX + 1;
            int customControlEndIndex = firstAttributeIndex - 1;

            int biFieldStartIndex = lastAttributeIndex + 1;
            int biFieldEndIndex = headersCount - 1;

            User agentUser = new User();
            User supervisorUser = new User();
            User evaluatorUser = new User();

            Program program = new Program();

            Form form = new Form();

            List<ImportTransactionAttributeHelper> importTransactionAttributeHelperList = new List<ImportTransactionAttributeHelper>();

            List<ImportTransactionCustomControlHelper> importTransactionCustomControlHelperList = new List<ImportTransactionCustomControlHelper>();

            List<ImportTransactionBusinessIntelligenceFieldHelper> importTransactionBusinessIntelligenceFieldHelperList = new List<ImportTransactionBusinessIntelligenceFieldHelper>();

            ProgramFormCatalog programFormCatalog = new ProgramFormCatalog();

            Transaction transaction = new Transaction();

            List<string> labelList = new List<string>();
            List<TransactionLabelCatalog> transactionLabelCatalogList = new List<TransactionLabelCatalog>();

            List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

            //BEGIN TO ENCAPSULATE OBJECTS

            string agentUserIdentification = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                agentUserIdentification = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.AGENT_IDENTIFICATION]).ToString().Trim();
            agentUser = GetAgentUserByIdentification(agentUserIdentification, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.AGENT_IDENTIFICATION);

            string supervisorUserName = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                supervisorUserName = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.SUPERVISOR_NAME]).ToString().Trim();
            supervisorUser = GetSupervisorUserByName(supervisorUserName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.SUPERVISOR_NAME);

            string evaluatorUserName = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                evaluatorUserName = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.EVALUATOR_NAME]).ToString().Trim();
            evaluatorUser = GetEvaluatorUserByName(evaluatorUserName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.EVALUATOR_NAME);

            string programName = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                programName = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME]).ToString().Trim();
            //program = GetProgramByName(programName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME, minDate, maxDate);
            program = GetProgramByID(programID);

            programFormCatalog = GetProgramFormCatalogByProgramID(program.ID, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME);

            string formName = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                formName = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Form.ExcelFields.NAME]).ToString().Trim();

            if (programFormCatalog != null)
                form = GetFormByID(programFormCatalog.FormID);
            else
                form = GetFormByName(formName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Form.ExcelFields.NAME);

            importTransactionAttributeHelperList = GetAttributesFromRow(headerRow, currentCells, firstAttributeIndex, lastAttributeIndex, form.ID, currentRowCount);

            importTransactionCustomControlHelperList = GetCustomControlsFromRow(headerRow, currentCells, customControlStartIndex, customControlEndIndex, currentRowCount);

            importTransactionBusinessIntelligenceFieldHelperList = GetBusinessIntelligenceFieldsFromRow(headerRow, currentCells, biFieldStartIndex, biFieldEndIndex, currentRowCount);

            string transactionIdentifier = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
            {
                transactionIdentifier =
                SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.IDENTIFIER_PREFIX +
                excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER]).ToString().Trim();
            }
            transaction = GetTransactionByIdentifier(transactionIdentifier, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER);

            string labelExcelField = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                labelExcelField = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.LABELS]).ToString().Trim();
            labelList.Add(labelExcelField);

            transactionCommentaryList = GetTransactionCommentaryListFromRow(currentCells);

            //CHECK IF EVERYTHING IS OK

            if (agentUser == null) hasError = true;
            if (supervisorUser == null) hasError = true;
            if (evaluatorUser == null) hasError = true;
            if (program == null) hasError = true;
            if (form == null) hasError = true;
            //if (programFormCatalog == null) hasError = true;
            if (importTransactionAttributeHelperList == null) hasError = true;
            if (importTransactionCustomControlHelperList == null) hasError = true;
            if (importTransactionBusinessIntelligenceFieldHelperList == null) hasError = true;

            if (hasError) return;

            if (transaction == null)
                transaction = CreateTransactionFromRow(currentCells, agentUser.ID, supervisorUser.ID, evaluatorUser.ID, form.ID, currentRowCount);

            if (transaction == null) hasError = true;

            if (!hasError)
            {
                transaction.Insert();

                if (transaction.ID > 0)
                {
                    List<TransactionAttributeCatalog> transactionAttributeCatalogList = new List<TransactionAttributeCatalog>();

                    foreach (ImportTransactionAttributeHelper importTransactionAttributeHelper in importTransactionAttributeHelperList)
                    {
                        int? tempAttributeValueCatalogID = null;

                        if (importTransactionAttributeHelper.AttributeValueCatalog != null)
                            tempAttributeValueCatalogID = importTransactionAttributeHelper.AttributeValueCatalog.ID;

                        TransactionAttributeCatalog transactionAttributeCatalog = 
                            new TransactionAttributeCatalog(
                                transaction.ID, 
                                importTransactionAttributeHelper.Attribute.ID, 
                                importTransactionAttributeHelper.AttributeTransactionComment,
                                tempAttributeValueCatalogID,
                                0,
                                true,
                                GetActualUser().ID,
                                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_ATTRIBUTE_CATALOG.CREATED);

                        transactionAttributeCatalogList.Add(transactionAttributeCatalog);
                    }

                    //PENDIENTES BI y CAMPOS PERSONALIZADOS

                    UpdateAttributeList(transaction, transactionAttributeCatalogList);

                    UpdateDisputeCommentList(
                        transaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE)
                            .ToList());

                    UpdateInvalidationCommentList(
                        transaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION)
                            .ToList());

                    UpdateDevolutionCommentList(
                        transaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
                            .ToList());

                    transactionLabelCatalogList = TransactionLabelCatalog.TransactionLabelCatalogWithTransactionID(transaction.ID).SelectByTransactionID();

                    foreach (TransactionLabelCatalog transactionLabelCatalog in transactionLabelCatalogList)
                    {
                        using (TransactionLabel transactionLabel = new TransactionLabel(transactionLabelCatalog.LabelID))
                        {
                            transactionLabel.SetDataByID();
                            labelList.Add(transactionLabel.Description);
                        }
                    }

                    UpdateTransactionLabelList(transaction, labelList.ToArray());

                    for (int i = 0; i < transactionCommentaryList.Count(); i++)
                    {
                        transactionCommentaryList[i].TransactionID = transaction.ID;
                        transactionCommentaryList[i].Insert();
                    }
                }
            }
        }

        List<TransactionCommentary> GetTransactionCommentaryListFromRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCells)
        {
            List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

            string disputationComment = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                disputationComment = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Disputation.COMMENT]).ToString().Trim();

            string invalidationComment = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                invalidationComment = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Invalidation.COMMENT]).ToString().Trim();

            string devolutionGeneralComment = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                devolutionGeneralComment = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.COMMENT]).ToString().Trim();

            string devolutionImprovementStepsComment = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                devolutionImprovementStepsComment = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.ImprovementSteps.COMMENT]).ToString().Trim();

            string devolutionUserStrengthsComment = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                devolutionUserStrengthsComment = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.UserStrengths.COMMENT]).ToString().Trim();

            if (!disputationComment.Equals("-") && disputationComment.Length > 1)
            {
                string disputationCreationDateExcelField = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    disputationCreationDateExcelField = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Disputation.CREATION_DATE]).ToString().Trim();

                DateTime? disputationCreationDate = null;
                if (!string.IsNullOrEmpty(disputationCreationDateExcelField))
                {
                    using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                        disputationCreationDate = Convert.ToDateTime(excelParser.FormatDate(disputationCreationDateExcelField));
                }

                TransactionCommentary disputationCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE,
                        0,
                        disputationComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                disputationCommentary.BasicInfo.CreationDate = disputationCreationDate != null ? disputationCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(disputationCommentary);
            }

            if (!invalidationComment.Equals("-") && invalidationComment.Length > 1)
            {
                string invalidationCreationDateExcelField = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    invalidationCreationDateExcelField = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Invalidation.CREATION_DATE]).ToString().Trim();

                DateTime? invalidationCreationDate = null;
                if (!string.IsNullOrEmpty(invalidationCreationDateExcelField))
                {
                    using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                        invalidationCreationDate = Convert.ToDateTime(excelParser.FormatDate(invalidationCreationDateExcelField));
                }

                TransactionCommentary invalidationCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION,
                        0,
                        invalidationComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                invalidationCommentary.BasicInfo.CreationDate = invalidationCreationDate != null ? invalidationCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(invalidationCommentary);
            }

            if (!devolutionGeneralComment.Equals("-") && devolutionGeneralComment.Length > 1)
            {
                string devolutionGeneralCreationDateExcelField = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    devolutionGeneralCreationDateExcelField = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE]).ToString().Trim();

                DateTime? devolutionGeneralCreationDate = null;
                if (!string.IsNullOrEmpty(devolutionGeneralCreationDateExcelField))
                {
                    using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                        devolutionGeneralCreationDate = Convert.ToDateTime(excelParser.FormatDate(devolutionGeneralCreationDateExcelField));
                }

                TransactionCommentary devolutionGeneralCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES,
                        0,
                        devolutionGeneralComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                devolutionGeneralCommentary.BasicInfo.CreationDate = devolutionGeneralCreationDate != null ? devolutionGeneralCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(devolutionGeneralCommentary);
            }

            if (!devolutionImprovementStepsComment.Equals("-") && devolutionImprovementStepsComment.Length > 1)
            {
                string devolutionImprovementStepsCreationDateExcelField = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    devolutionImprovementStepsCreationDateExcelField = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE]).ToString().Trim();

                DateTime? devolutionImprovementStepsCreationDate = null;
                if (!string.IsNullOrEmpty(devolutionImprovementStepsCreationDateExcelField))
                {
                    using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                        devolutionImprovementStepsCreationDate = Convert.ToDateTime(excelParser.FormatDate(devolutionImprovementStepsCreationDateExcelField));
                }

                TransactionCommentary devolutionImprovementStepsCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS,
                        0,
                        devolutionImprovementStepsComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                devolutionImprovementStepsCommentary.BasicInfo.CreationDate = devolutionImprovementStepsCreationDate != null ? devolutionImprovementStepsCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(devolutionImprovementStepsCommentary);
            }

            if (!devolutionUserStrengthsComment.Equals("-") && devolutionUserStrengthsComment.Length > 1)
            {
                string devolutionUserStrengthsCreationDateExcelField = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    devolutionUserStrengthsCreationDateExcelField = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE]).ToString().Trim();

                DateTime? devolutionUserStrengthsCreationDate = null;
                if (!string.IsNullOrEmpty(devolutionUserStrengthsCreationDateExcelField))
                {
                    using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                        devolutionUserStrengthsCreationDate = Convert.ToDateTime(excelParser.FormatDate(devolutionUserStrengthsCreationDateExcelField));
                }

                TransactionCommentary devolutionUserStrengthsCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS,
                        0,
                        devolutionUserStrengthsComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                devolutionUserStrengthsCommentary.BasicInfo.CreationDate = devolutionUserStrengthsCreationDate != null ? devolutionUserStrengthsCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(devolutionUserStrengthsCommentary);
            }

            return transactionCommentaryList;
        }

        DateTime? GetMinDate(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rows)
        {
            DateTime? minDate = null;

            int headersCount = rows.ElementAt(0).Count();

            rows = rows.Skip(1);

            for (int i = 0; i < rows.Count(); i++)
            {
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentCells = excelParser.GetRowCells(rows.ElementAt(i), headersCount).ToArray();

                    string excelFieldStartDate =
                        excelParser.GetCellValue(auxCurrentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE]).ToString().Trim();

                    DateTime? currentDate = null;

                    if (!string.IsNullOrEmpty(excelFieldStartDate))
                        currentDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldStartDate));

                    if (currentDate != null)
                    {
                        if (minDate == null)
                            minDate = currentDate;
                        else
                            minDate = currentDate < minDate ? currentDate : minDate;
                    }
                }
            }

            return minDate;
        }

        DateTime? GetMaxDate(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rows)
        {
            DateTime? maxDate = null;

            int headersCount = rows.ElementAt(0).Count();

            rows = rows.Skip(1);

            for (int i = 0; i < rows.Count(); i++)
            {
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentCells = excelParser.GetRowCells(rows.ElementAt(i), headersCount).ToArray();

                    string excelFieldStartDate =
                        excelParser.GetCellValue(auxCurrentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE]).ToString().Trim();

                    DateTime? currentDate = null;

                    if (!string.IsNullOrEmpty(excelFieldStartDate))
                        currentDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldStartDate));

                    if (currentDate != null)
                    {
                        if (maxDate == null)
                            maxDate = currentDate;
                        else
                            maxDate = currentDate > maxDate ? currentDate : maxDate;
                    }
                }
            }

            return maxDate;
        }

        User GetAgentUserByIdentification(string userIdentification, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "User";

            if (string.IsNullOrEmpty(userIdentification))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.NO_IDENTIFICATION_ENTERED,
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            using (User user = new SCC_BL.User(userIdentification))
            {
                try
                {
                    user.SetDataByUsername();

                    if (user.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.NO_IDENTIFICATION_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userIdentification),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }

                return user;
            }
        }

        User GetSupervisorUserByName(string userName, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "User";

            if (string.IsNullOrEmpty(userName))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            if (!userName.Contains(','))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_VALID_FORMAT
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            using (User user = new SCC_BL.User(userName))
            {
                try
                {
                    string firstName = userName.Split(',')[1].Trim();
                    string surName = userName.Split(',')[0].Split(' ')[0].Trim();
                    string lastName = userName.Split(',')[0].Split(' ')[1].Trim();

                    user.SetDataByName(firstName, surName, lastName);

                    if (user.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }

                return user;
            }
        }

        User GetEvaluatorUserByName(string userName, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "User";

            if (string.IsNullOrEmpty(userName))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            if (!userName.Contains(','))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_VALID_FORMAT
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            using (User user = new SCC_BL.User(userName))
            {
                try
                {
                    string firstName = userName.Split(',')[1].Trim();
                    string surName = userName.Split(',')[0].Split(' ')[0].Trim();
                    string lastName = userName.Split(',')[0].Split(' ')[1].Trim();

                    user.SetDataByName(firstName, surName, lastName);

                    if (user.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }

                return user;
            }
        }

        ProgramFormCatalog GetProgramFormCatalogByProgramID(int programID, int currentRowCount, int currentColumnCount)
        {
            ProgramFormCatalog programFormCatalogResult = null;

            string transactionImportErrorElementName = "ProgramFormCatalog";

            using (ProgramFormCatalog programFormCatalog = SCC_BL.ProgramFormCatalog.ProgramFormCatalogWithProgramID(programID))
            {
                try
                {
                    List<ProgramFormCatalog> programFormCatalogList = programFormCatalog.SelectByProgramID();

                    if (programFormCatalogList.Count() <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.ProgramFormCatalog.NO_PROGRAM_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, programID.ToString()),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }

                    programFormCatalogResult = programFormCatalogList.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }

                return programFormCatalogResult;
            }
        }

        Program GetProgramByName(string programName, int currentRowCount, int currentColumnCount, DateTime? minDate, DateTime? maxDate)
        {
            string transactionImportErrorElementName = "Program";

            if (string.IsNullOrEmpty(programName))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Program.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            using (Program program = new SCC_BL.Program(programName))
            {
                try
                {
                    program.SetDataByName();

                    if (program.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Program.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, programName),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Program.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    //Se podría crear si se necesitase

                    return null;
                }

                return program;
            }
        }

        Program GetProgramByID(int programID)
        {
            string transactionImportErrorElementName = "Program";

            using (Program program = new SCC_BL.Program(programID))
            {
                try
                {
                    program.SetDataByID();
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Program.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            0,
                            0));

                    //Se podría crear si se necesitase

                    return null;
                }

                return program;
            }
        }

        Form GetFormByName(string formName, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "Form";

            if (string.IsNullOrEmpty(formName))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Form.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            using (Form form = new SCC_BL.Form(formName))
            {
                try
                {
                    form.SetDataByName();

                    if (form.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Form.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, formName),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Form.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    //Se podría crear si se necesitase

                    return null;
                }

                return form;
            }
        }

        Form GetFormByID(int formID)
        {
            string transactionImportErrorElementName = "Form";

            using (Form form = new SCC_BL.Form(formID))
            {
                try
                {
                    form.SetDataByID();
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Form.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            0,
                            0));

                    //Se podría crear si se necesitase

                    return null;
                }

                return form;
            }
        }

        Transaction CreateTransactionFromRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCells, int userToEvaluateID, int supervisorUserID, int evaluatorUserID, int formID, int currentRowCount)
        {
            bool hasError = false;

            string transactionImportErrorElementName = "Transaction";

            string rawIdentifier = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                rawIdentifier = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER]).ToString().Trim();

            if (string.IsNullOrEmpty(rawIdentifier))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_IDENTIFIER_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER));

                hasError = true;
            }

            string identifier = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.IDENTIFIER_PREFIX + rawIdentifier;

            string excelFieldEvaluationDate = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                excelFieldEvaluationDate = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.EVALUATION_DATE]).ToString().Trim();

            string excelFieldTransactionDate = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                excelFieldTransactionDate = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE]).ToString().Trim();

            if (string.IsNullOrEmpty(excelFieldEvaluationDate))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_EVALUATION_DATE_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.EVALUATION_DATE));

                hasError = true;
            }

            if (string.IsNullOrEmpty(excelFieldTransactionDate))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_TRANSACTION_DATE_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE));

                hasError = true;
            }

            DateTime evaluationDate = DateTime.Now;
            DateTime transactionDate = DateTime.Now;

            if (!string.IsNullOrEmpty(excelFieldEvaluationDate))
            {
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    evaluationDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldEvaluationDate));
            }

            if (!string.IsNullOrEmpty(excelFieldTransactionDate))
            {
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    transactionDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldTransactionDate));
            }

            string comment = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                comment = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.COMMENT]).ToString().Trim();

            if (string.IsNullOrEmpty(comment))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_COMMENT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.COMMENT));

                hasError = true;
            }

            //GENERAL RESULTS

            string generalResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                generalResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.GENERAL_RESULT]).ToString().Trim();

            if (string.IsNullOrEmpty(generalResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_GENERAL_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.GENERAL_RESULT));

                hasError = true;
            }

            int generalResultID = 0;

            switch (generalResult)
            {
                case "Pasó":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS;
                    break;
                case "Falló":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL;
                    break;
                case "100%":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS;
                    break;
                case "0%":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL;
                    break;
                default:
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL;
                    break;
            }

            string generalFUCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                generalFUCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FINAL_USER_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(generalFUCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_FUCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FINAL_USER_CRITICAL_ERROR));

                hasError = true;
            }

            int generalFUCEResultID = 0;

            switch (generalFUCEResult)
            {
                case "Pasó":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
            }

            string generalBCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                generalBCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.BUSINESS_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(generalBCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_BCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.BUSINESS_CRITICAL_ERROR));

                hasError = true;
            }

            int generalBCEResultID = 0;

            switch (generalBCEResult)
            {
                case "Pasó":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
            }

            string generalFCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                generalFCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FULFILMENT_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(generalFCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_FCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FULFILMENT_CRITICAL_ERROR));

                hasError = true;
            }

            int generalFCEResultID = 0;

            switch (generalFCEResult)
            {
                case "Pasó":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
            }

            string generalNCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                generalNCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.NON_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(generalNCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_NCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.NON_CRITICAL_ERROR));

                hasError = true;
            }

            int generalNCEResultValue = 0;

            if (!SCC_BL.Settings.Overall.NEUTRAL_VALUES.Contains(generalNCEResult))
            {
                try
                {
                    generalNCEResultValue = Convert.ToInt32(generalNCEResult.ToUpper().Replace("PTS", ""));
                }
                catch (Exception ex)
                {
                }
            }

            //ACCURATE RESULTS

            int accurateResultID = generalResultID;

            string accurateFUCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                accurateFUCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FINAL_USER_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(accurateFUCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Accurate.NO_FUCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FINAL_USER_CRITICAL_ERROR));

                hasError = true;
            }

            int accurateFUCEResultID = 0;

            switch (accurateFUCEResult)
            {
                case "Pasó":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
            }

            string accurateBCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                accurateBCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.BUSINESS_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(accurateBCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Accurate.NO_BCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.BUSINESS_CRITICAL_ERROR));

                hasError = true;
            }

            int accurateBCEResultID = 0;

            switch (accurateBCEResult)
            {
                case "Pasó":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
            }

            string accurateFCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                accurateFCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FULFILMENT_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(accurateFCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Accurate.NO_FCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FULFILMENT_CRITICAL_ERROR));

                hasError = true;
            }

            int accurateFCEResultID = 0;

            switch (accurateFCEResult)
            {
                case "Pasó":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
            }

            int accurateNCEResultValue = generalNCEResultValue;

            //CONTROLLABLE RESULTS

            int controllableResultID = generalResultID;

            string controllableFUCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                controllableFUCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FINAL_USER_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(controllableFUCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Controllable.NO_FUCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FINAL_USER_CRITICAL_ERROR));

                hasError = true;
            }

            int controllableFUCEResultID = 0;

            switch (controllableFUCEResult)
            {
                case "Pasó":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
            }

            string controllableBCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                controllableBCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.BUSINESS_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(controllableBCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Controllable.NO_BCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.BUSINESS_CRITICAL_ERROR));

                hasError = true;
            }

            int controllableBCEResultID = 0;

            switch (controllableBCEResult)
            {
                case "Pasó":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
            }

            string controllableFCEResult = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                controllableFCEResult = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FULFILMENT_CRITICAL_ERROR]).ToString().Trim();

            if (string.IsNullOrEmpty(controllableFCEResult))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Controllable.NO_FCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FULFILMENT_CRITICAL_ERROR));

                hasError = true;
            }

            int controllableFCEResultID = 0;

            switch (controllableFCEResult)
            {
                case "Pasó":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
            }

            int controllableNCEResultValue = generalNCEResultValue;

            string timeElapsed = string.Empty;
            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                timeElapsed = excelParser.GetCellValue(currentCells[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TIME_ELAPSED]).ToString().Trim();

            if (string.IsNullOrEmpty(timeElapsed))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_TIME_ELAPSED_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TIME_ELAPSED));

                hasError = true;
            }

            TimeSpan timeElapsedValue = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (!string.IsNullOrEmpty(timeElapsed))
            {
                int hours = Convert.ToInt32(timeElapsed.Split(':')[0]);
                int minutes = Convert.ToInt32(timeElapsed.Split(':')[1]);
                int seconds = Convert.ToInt32(timeElapsed.Split(':')[2]);

                timeElapsedValue = new TimeSpan(hours, minutes, seconds);
            }

            Transaction transaction = new Transaction(identifier, userToEvaluateID, evaluatorUserID, evaluationDate, transactionDate, formID, comment, generalResultID, generalFUCEResultID, generalBCEResultID, generalFCEResultID, generalNCEResultValue, accurateResultID, accurateFUCEResultID, accurateBCEResultID, accurateFCEResultID, accurateNCEResultValue, controllableResultID, controllableFUCEResultID, controllableBCEResultID, controllableFCEResultID, controllableNCEResultValue, timeElapsedValue, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION.CREATED, (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.EVALUATION, null);

            return hasError ? null : transaction;
        }

        Transaction GetTransactionByIdentifier(string transactionIdentifier, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "Transaction";

            if (string.IsNullOrEmpty(transactionIdentifier))
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_IDENTIFIER_ENTERED,
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            using (Transaction transaction = new SCC_BL.Transaction(transactionIdentifier))
            {
                try
                {
                    transaction.SetDataByIdentifier();

                    if (transaction.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_IDENTIFIER_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, transactionIdentifier),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    //Se podría crear si se necesitase

                    return null;
                }

                return transaction;
            }
        }

        string Replace(string currentValue, string[] oldValueArray, string newValueArray)
        {
            for (int i = 0; i < oldValueArray.Length; i++)
            {
                currentValue = currentValue.Replace(oldValueArray[i], newValueArray);
            }

            return currentValue;
        }

        class ImportTransactionAttributeHelper
        {
            public SCC_BL.Attribute Attribute { get; set; }
            public SCC_BL.AttributeValueCatalog AttributeValueCatalog { get; set; }
            public string AttributeTransactionComment { get; set; }

            public ImportTransactionAttributeHelper(SCC_BL.Attribute attribute, SCC_BL.AttributeValueCatalog attributeValueCatalog, string attributeTransactionComment)
            {
                if (attributeTransactionComment.Length <= 1) attributeTransactionComment = string.Empty;

                this.Attribute = attribute;
                this.AttributeValueCatalog = attributeValueCatalog;
                this.AttributeTransactionComment = attributeTransactionComment;
            }
        }

        List<ImportTransactionAttributeHelper> GetAttributesFromRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] headerRow, DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCells, int firstAttributeIndex, int lastAttributeIndex, int formID, int currentRowCount)
        {
            List<ImportTransactionAttributeHelper> importTransactionAttributeHelperList = new List<ImportTransactionAttributeHelper>();

            string transactionImportErrorElementName = "Attribute";

            if (currentCells.Length < 3)
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName, 
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_COLUMNS, 
                        currentRowCount,
                        firstAttributeIndex));

                return null;
            }

            int currentCellIndex = firstAttributeIndex;

            while (currentCellIndex < lastAttributeIndex)
            {
                void _setNextAttributeIndex()
                {
                    currentCellIndex += 3;
                }

                string attributeName = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    attributeName = excelParser.GetCellValue(headerRow[(int)currentCellIndex]).ToString().Trim();

                string attributeValue = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    attributeValue = excelParser.GetCellValue(currentCells[(int)currentCellIndex]).ToString().Trim();

                string attributeTransactionComment = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    attributeTransactionComment = excelParser.GetCellValue(currentCells[(int)currentCellIndex + 1]).ToString().Trim();

                string attributeSubattributes = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    attributeSubattributes = excelParser.GetCellValue(currentCells[(int)currentCellIndex + 2]).ToString().Trim();

                attributeName = Replace(attributeName, SCC_BL.Settings.Overall.ErrorType.List.Select(e => "(" + e + ")").ToArray(), "");
                attributeName = attributeName.Trim();

                if (attributeValue.Equals("-")) attributeValue = string.Empty;
                attributeValue = attributeValue.Trim();

                if (string.IsNullOrEmpty(attributeValue))
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_VALUE_ENTERED,
                            currentRowCount,
                            currentCellIndex));

                    _setNextAttributeIndex();
                    continue;
                }

                if (attributeSubattributes.Equals("~")) attributeSubattributes = string.Empty;

                if (attributeSubattributes.Length > 1 && attributeSubattributes.ElementAt(0) == '~') attributeSubattributes = attributeSubattributes.Substring(1);

                using (SCC_BL.Attribute attribute = SCC_BL.Attribute.AttributeWithFormIDAndName(formID, attributeName))
                {
                    try
                    {
                        attribute.SetDataByName();

                        if (attribute.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new TransactionImportViewModel.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_NAME_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeName),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextAttributeIndex();
                            continue;
                        }

                        SCC_BL.AttributeValueCatalog attributeValueCatalog = new SCC_BL.AttributeValueCatalog(attribute.ID, attributeValue);
                        attributeValueCatalog.SetDataByAttributeIDAndValue();

                        if (attributeValueCatalog.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new TransactionImportViewModel.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_VALUE_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeValue),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextAttributeIndex();
                            continue;
                        }

                        importTransactionAttributeHelperList.Add(
                            new ImportTransactionAttributeHelper(
                                attribute,
                                attributeValueCatalog,
                                attributeTransactionComment
                            )
                        );

                        string[] subattributeArray = attributeSubattributes.Split('~');

                        List<SCC_BL.Attribute> tempAttributeHierarchy = new List<SCC_BL.Attribute>() { attribute };

                        for (int i = 0; i < subattributeArray.Length; i++)
                        {
                            string currentSubattributeName = subattributeArray[i].Trim();

                            if (string.IsNullOrEmpty(currentSubattributeName)) continue;

                            if (currentSubattributeName.Length > 1 && currentSubattributeName.ElementAt(0) == '~') currentSubattributeName = currentSubattributeName.Substring(1);

                            using (SCC_BL.Attribute subattribute = SCC_BL.Attribute.AttributeWithParentAttributeIDAndName(tempAttributeHierarchy.LastOrDefault().ID, currentSubattributeName))
                            {
                                try
                                {
                                    subattribute.SetSubattributeDataByName();

                                    if (subattribute.ID <= 0)
                                    {
                                        _transactionImportErrorList.Add(
                                            new TransactionImportViewModel.Error(
                                                transactionImportErrorElementName,
                                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_SUBATTRIBUTE_NAME_FOUND
                                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubattributeName),
                                                currentRowCount,
                                                currentCellIndex + 2));

                                        break;
                                    }

                                    importTransactionAttributeHelperList.Add(
                                        new ImportTransactionAttributeHelper(
                                            subattribute,
                                            null,
                                            string.Empty
                                        )
                                    );

                                    tempAttributeHierarchy.Add(subattribute);
                                }
                                catch (Exception ex)
                                {
                                    _transactionImportErrorList.Add(
                                        new TransactionImportViewModel.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                            currentRowCount,
                                            currentCellIndex + 2));

                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                currentRowCount,
                                currentCellIndex));

                        //Se podría crear si se necesitase

                        _setNextAttributeIndex();
                        continue;
                    }
                }

                _setNextAttributeIndex();
            }

            return importTransactionAttributeHelperList;
        }

        class ImportTransactionCustomControlHelper
        {
            public SCC_BL.CustomControl CustomControl { get; set; }
            public SCC_BL.CustomControlValueCatalog CustomControlValueCatalog { get; set; }

            public ImportTransactionCustomControlHelper(SCC_BL.CustomControl customControl, SCC_BL.CustomControlValueCatalog customControlValueCatalog)
            {
                this.CustomControl = customControl;
                this.CustomControlValueCatalog = customControlValueCatalog;
            }
        }

        List<ImportTransactionCustomControlHelper> GetCustomControlsFromRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] headerRow, DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCells, int customControlStartIndex, int customControlEndIndex, int currentRowCount)
        {
            List<ImportTransactionCustomControlHelper> importTransactionCustomControlHelperList = new List<ImportTransactionCustomControlHelper>();

            string transactionImportErrorElementName = "CustomControl";

            if (currentCells.Length < 1)
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName, 
                        SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_COLUMNS, 
                        currentRowCount,
                        customControlStartIndex));

                return null;
            }

            int currentCellIndex = customControlStartIndex;

            while (currentCellIndex < customControlEndIndex)
            {
                void _setNextCustomControlIndex()
                {
                    currentCellIndex += 1;
                }

                string customControlName = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    customControlName = excelParser.GetCellValue(headerRow[(int)currentCellIndex]).ToString().Trim();

                string customControlValue = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    customControlValue = excelParser.GetCellValue(currentCells[(int)currentCellIndex]).ToString().Trim();

                if (customControlValue.Equals("-")) customControlValue = string.Empty;
                customControlValue = customControlValue.Trim();

                if (string.IsNullOrEmpty(customControlValue))
                {
                    _transactionImportErrorList.Add(
                        new TransactionImportViewModel.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_VALUE_ENTERED,
                            currentRowCount,
                            currentCellIndex));

                    _setNextCustomControlIndex();
                    continue;
                }

                using (SCC_BL.CustomControl customControl = new SCC_BL.CustomControl(customControlName))
                {
                    try
                    {
                        customControl.SetDataByLabel();

                        if (customControl.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new TransactionImportViewModel.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_NAME_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlName),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextCustomControlIndex();
                            continue;
                        }

                        SCC_BL.CustomControlValueCatalog customControlValueCatalog = new SCC_BL.CustomControlValueCatalog(customControl.ID, customControlValue);
                        customControlValueCatalog.SetDataByCustomControlIDAndValue();

                        if (customControlValueCatalog.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new TransactionImportViewModel.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_VALUE_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlValue),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextCustomControlIndex();
                            continue;
                        }

                        importTransactionCustomControlHelperList.Add(
                            new ImportTransactionCustomControlHelper(
                                customControl,
                                customControlValueCatalog
                            )
                        );
                    }
                    catch (Exception ex)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.UNKNOWN
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                currentRowCount,
                                currentCellIndex));

                        //Se podría crear si se necesitase

                        _setNextCustomControlIndex();
                        continue;
                    }
                }

                _setNextCustomControlIndex();
            }

            return importTransactionCustomControlHelperList;
        }

        class ImportTransactionBusinessIntelligenceFieldHelper
        {
            public SCC_BL.BusinessIntelligenceField BusinessIntelligenceField { get; set; }
            public string BusinessIntelligenceValueTransactionComment { get; set; }

            public ImportTransactionBusinessIntelligenceFieldHelper(SCC_BL.BusinessIntelligenceField businessIntelligenceField, string businessIntelligenceValueTransactionComment)
            {
                if (businessIntelligenceValueTransactionComment.Length <= 1) businessIntelligenceValueTransactionComment = string.Empty;

                this.BusinessIntelligenceField = businessIntelligenceField;
                this.BusinessIntelligenceValueTransactionComment = businessIntelligenceValueTransactionComment;
            }
        }

        List<ImportTransactionBusinessIntelligenceFieldHelper> GetBusinessIntelligenceFieldsFromRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] headerRow, DocumentFormat.OpenXml.Spreadsheet.Cell[] currentCells, int biFieldStartIndex, int biFieldEndIndex, int currentRowCount)
        {
            List<ImportTransactionBusinessIntelligenceFieldHelper> importTransactionBusinessIntelligenceFieldHelperList = new List<ImportTransactionBusinessIntelligenceFieldHelper>();

            string transactionImportErrorElementName = "BusinessIntelligenceField";

            if (currentCells.Length < 2)
            {
                _transactionImportErrorList.Add(
                    new TransactionImportViewModel.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_COLUMNS,
                        currentRowCount,
                        biFieldStartIndex));

                return null;
            }

            int currentCellIndex = biFieldStartIndex;

            while (currentCellIndex < biFieldEndIndex)
            {
                void _setNextBusinessIntelligenceFieldIndex()
                {
                    currentCellIndex += 2;
                }

                string businessIntelligenceFieldName = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    businessIntelligenceFieldName = excelParser.GetCellValue(headerRow[(int)currentCellIndex]).ToString().Trim();

                string businessIntelligenceFieldSubFields = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    businessIntelligenceFieldSubFields = excelParser.GetCellValue(currentCells[(int)currentCellIndex]).ToString().Trim();

                string businessIntelligenceFieldTransactionComment = string.Empty;
                using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                    businessIntelligenceFieldTransactionComment = excelParser.GetCellValue(currentCells[(int)currentCellIndex + 1]).ToString().Trim();

                if (businessIntelligenceFieldName.Substring(0, 4).Equals("BI: "))
                    businessIntelligenceFieldName = businessIntelligenceFieldName.Substring(4);

                businessIntelligenceFieldName = businessIntelligenceFieldName.Trim();

                if (businessIntelligenceFieldSubFields.Equals("-")) businessIntelligenceFieldSubFields = string.Empty;

                using (SCC_BL.BusinessIntelligenceField businessIntelligenceField = new SCC_BL.BusinessIntelligenceField(businessIntelligenceFieldName))
                {
                    try
                    {
                        businessIntelligenceField.SetDataByParentIDAndName();

                        if (businessIntelligenceField.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new TransactionImportViewModel.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_NAME_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, businessIntelligenceFieldName),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextBusinessIntelligenceFieldIndex();
                            continue;
                        }

                        importTransactionBusinessIntelligenceFieldHelperList.Add(
                            new ImportTransactionBusinessIntelligenceFieldHelper(
                                businessIntelligenceField,
                                businessIntelligenceFieldTransactionComment
                            )
                        );

                        string[] subfieldArray = businessIntelligenceFieldSubFields.Split('~');

                        List<SCC_BL.BusinessIntelligenceField> tempBusinessIntelligenceFieldHierarchy = new List<SCC_BL.BusinessIntelligenceField>() { businessIntelligenceField };

                        for (int i = 0; i < subfieldArray.Length; i++)
                        {
                            string currentSubfieldName = subfieldArray[i].Trim();

                            if (currentSubfieldName.Length > 1 && currentSubfieldName.ElementAt(0) == '~') currentSubfieldName = currentSubfieldName.Substring(1);
                            if (currentSubfieldName.Length > 1 && currentSubfieldName.ElementAt(0) == '-') currentSubfieldName = currentSubfieldName.Substring(1);

                            if (currentSubfieldName.Equals("~")) currentSubfieldName = string.Empty;
                            if (currentSubfieldName.Equals("-")) currentSubfieldName = string.Empty;

                            if (string.IsNullOrEmpty(currentSubfieldName)) break;

                            using (SCC_BL.BusinessIntelligenceField subfield = new SCC_BL.BusinessIntelligenceField(currentSubfieldName, tempBusinessIntelligenceFieldHierarchy.LastOrDefault().ID))
                            {
                                try
                                {
                                    subfield.SetDataByParentIDAndName();

                                    if (subfield.ID <= 0)
                                    {
                                        _transactionImportErrorList.Add(
                                            new TransactionImportViewModel.Error(
                                                transactionImportErrorElementName,
                                                SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_SUBFIELD_NAME_FOUND
                                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubfieldName),
                                                currentRowCount,
                                                currentCellIndex));

                                        break;
                                    }

                                    importTransactionBusinessIntelligenceFieldHelperList.Add(
                                        new ImportTransactionBusinessIntelligenceFieldHelper(
                                            subfield,
                                            string.Empty
                                        )
                                    );

                                    tempBusinessIntelligenceFieldHierarchy.Add(subfield);
                                }
                                catch (Exception ex)
                                {
                                    _transactionImportErrorList.Add(
                                        new TransactionImportViewModel.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                            currentRowCount,
                                            currentCellIndex + 2));

                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _transactionImportErrorList.Add(
                            new TransactionImportViewModel.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                currentRowCount,
                                currentCellIndex));

                        //Se podría crear si se necesitase

                        _setNextBusinessIntelligenceFieldIndex();
                        continue;
                    }
                }

                _setNextBusinessIntelligenceFieldIndex();
            }

            return importTransactionBusinessIntelligenceFieldHelperList;
        }
    }
}