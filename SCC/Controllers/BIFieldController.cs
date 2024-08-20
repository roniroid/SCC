using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Presentation;
using SCC.ViewModels;
using SCC_BL;

namespace SCC.Controllers
{
    public class BIFieldController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(BIFieldController));

        public ActionResult Manage(int? biFieldID = null, int? parentID = null, bool filterActiveElements = false)
        {
            BIFieldManagementViewModel biFieldManagementViewModel = new BIFieldManagementViewModel();

            if (biFieldID != null)
            {
                biFieldManagementViewModel.BusinessIntelligenceField = new BusinessIntelligenceField(biFieldID.Value);
                biFieldManagementViewModel.BusinessIntelligenceField.SetDataByID();
            }

            if (parentID != null && (biFieldManagementViewModel.BusinessIntelligenceField.ParentBIFieldID == null || biFieldManagementViewModel.BusinessIntelligenceField.ParentBIFieldID <= 0))
            {
                biFieldManagementViewModel.BusinessIntelligenceField.ParentBIFieldID = parentID;
            }

            biFieldManagementViewModel.BIFieldList = new BusinessIntelligenceField().SelectAll();

            if (filterActiveElements)
                biFieldManagementViewModel.BIFieldList =
                    biFieldManagementViewModel.BIFieldList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DISABLED)
                        .ToList();

            biFieldManagementViewModel.BIFieldList =
                biFieldManagementViewModel.BIFieldList
                    .OrderBy(e => e.Order)
                    .ToList();

            return View(biFieldManagementViewModel);
        }

        [HttpPost]
        public ActionResult Edit(BusinessIntelligenceField businessIntelligenceField, List<BusinessIntelligenceField> childList = null/*, List<BusinessIntelligenceValueCatalog> valueList = null*/)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_BI_QUESTIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Update.NotAllowedToCreateBIQuestions>();
                return RedirectToAction(nameof(BIFieldController.Manage), GetControllerName(typeof(BIFieldController)));
            }

            BusinessIntelligenceField oldBIField = new BusinessIntelligenceField(businessIntelligenceField.ID);
            oldBIField.SetDataByID();

            BusinessIntelligenceField newBIField = new BusinessIntelligenceField(
                businessIntelligenceField.ID, 
                businessIntelligenceField.Name, 
                businessIntelligenceField.Description, 
                businessIntelligenceField.ParentBIFieldID, 
                businessIntelligenceField.HasForcedComment,
                businessIntelligenceField.Order,
                businessIntelligenceField.BasicInfoID,
                currentUser.ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.UPDATED);

            try
            {
                int result = newBIField.Update();

                if (result > 0)
                {
                    newBIField.SetDataByID();

                    SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Update.Success>(newBIField.ID, newBIField.BasicInfo.StatusID, oldBIField);

                    SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldChildList.CODE response = SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldChildList.CODE.ERROR;

                    /*
                    SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.CODE response = SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.CODE.ERROR;
                    
                    try
                    {
                        response = newBIField.UpdateBIFieldValueCatalogList(valueList, GetActualUser().ID);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.Error>(null, null, valueList, ex);
                    }*/

                    try
                    {
                        response = newBIField.UpdateBIFieldChildList(childList, newBIField, currentUser.ID);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldChildList.Error>(null, null, childList, ex);
                    }
                }
                else
                {
                    switch ((SCC_BL.Results.BusinessIntelligenceField.Update.CODE)result)
                    {
                        case SCC_BL.Results.BusinessIntelligenceField.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Update.Error>(oldBIField.ID, oldBIField.BasicInfo.StatusID, newBIField);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Update.Error>(oldBIField.ID, oldBIField.BasicInfo.StatusID, oldBIField, ex);
            }

            return Json(new { url = Url.Action(nameof(BIFieldController.Manage), _mainControllerName) });

            //return RedirectToAction(nameof(BIFieldController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(BusinessIntelligenceField businessIntelligenceField, List<BusinessIntelligenceField> childList = null/*, List<BusinessIntelligenceValueCatalog> valueList = null*/)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_BI_QUESTIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Insert.NotAllowedToCreateBIQuestions>();
                return RedirectToAction(nameof(BIFieldController.Manage), GetControllerName(typeof(BIFieldController)));
            }

            BusinessIntelligenceField newBIField = new BusinessIntelligenceField(
                businessIntelligenceField.Name, 
                businessIntelligenceField.Description, 
                businessIntelligenceField.ParentBIFieldID, 
                businessIntelligenceField.HasForcedComment, 
                businessIntelligenceField.ID,
                currentUser.ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.CREATED);

            try
            {
                int result = newBIField.Insert();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Insert.Success>(newBIField.ID, newBIField.BasicInfo.StatusID, newBIField);

                    SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldChildList.CODE response = SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldChildList.CODE.ERROR;

                    /*try
                    {
                        response = newBIField.UpdateBIFieldValueCatalogList(valueList, GetActualUser().ID);
                        SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.Success>(null, null, newBIField);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.Error>(null, null, valueList, ex);
                    }*/

                    try
                    {
                        response = newBIField.UpdateBIFieldChildList(childList, newBIField, currentUser.ID);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.UpdateBIFieldChildList.Error>(null, null, childList, ex);
                    }
                }
                else
                {
                    switch ((SCC_BL.Results.BusinessIntelligenceField.Update.CODE)result)
                    {
                        case SCC_BL.Results.BusinessIntelligenceField.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Update.Error>(null, null, newBIField);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Update.Error>(null, null, ex);
            }

            return Json(new { url = Url.Action(nameof(BIFieldController.Manage), _mainControllerName) });

            //return RedirectToAction(nameof(BIFieldController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int biFieldID)
        {
            User currentUser = GetCurrentUser();

            BusinessIntelligenceField biField = new BusinessIntelligenceField(biFieldID);
            biField.SetDataByID();

            try
            {
                //biField.Delete();

                biField.BasicInfo.ModificationUserID = currentUser.ID;
                biField.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DELETED;

                int result = biField.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Delete.Success>(biField.ID, biField.BasicInfo.StatusID, biField);

                    return RedirectToAction(nameof(BIFieldController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Delete.Success>(biField.ID, biField.BasicInfo.StatusID, biField, ex);
            }

            return RedirectToAction(nameof(BIFieldController.Manage), _mainControllerName);
        }

        public ActionResult UploadBIFields()
        {
            List<UploadedFile> uploadedFileList = new List<UploadedFile>();

            using (UploadedFile uploadedFile = new UploadedFile())
            {
                uploadedFileList =
                    uploadedFile.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_BI_FIELD_IMPORT)
                        .ToList();

            }

            ViewModels.BIFieldsUploadViewModel biFieldsUploadViewModel = new ViewModels.BIFieldsUploadViewModel();
            biFieldsUploadViewModel.UploadedFileList = uploadedFileList;

            return View(biFieldsUploadViewModel);
        }

        [HttpPost]
        public ActionResult UploadBIFields(HttpPostedFileBase file)
        {
            User currentUser = GetCurrentUser();
            string filePath = SaveUploadedFile(file, SCC_BL.Settings.Paths.BusinessIntelligenceField.BI_FIELD_UPLOAD_FOLDER);

            if (!string.IsNullOrEmpty(filePath))
            {
                int uploadedFileID = 0;

                uploadedFileID = SaveFileInDatabase(filePath, (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_BI_FIELD_IMPORT);

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
                        SCC_BL.Results.BusinessIntelligenceField.MassiveImport.CODE result = ProcessImportExcel(filePath, currentUser.ID);

                        switch (result)
                        {
                            case SCC_BL.Results.BusinessIntelligenceField.MassiveImport.CODE.SUCCESS:
                                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.MassiveImport.Success>();
                                break;
                            case SCC_BL.Results.BusinessIntelligenceField.MassiveImport.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.MassiveImport.Error>();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.MassiveImport.Error>(null, null, null, ex);
                    }
                }
            }
            else
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.MassiveImport.Error>();
            }

            return RedirectToAction(nameof(BIFieldController.UploadBIFields));
        }

        public SCC_BL.Results.BusinessIntelligenceField.MassiveImport.CODE ProcessImportExcel(string filePath, int creationUserID)
        {
            List<SCC_BL.BusinessIntelligenceField> businessIntelligenceFieldList = new List<SCC_BL.BusinessIntelligenceField>();

            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
            {
                businessIntelligenceFieldList = ProcessExcelForUpload(filePath);

                businessIntelligenceFieldList
                    .ForEach(e => {
                        DateTime currentDate = DateTime.Now;

                        string newOrder = e.Order.ToString().PadLeft(SCC_BL.Settings.Overall.DEFAULT_BI_ORDER_LENGTH, '0');
                        newOrder = currentDate.ToString("yyMMdd") + newOrder;
                        int orderNumber = Convert.ToInt32(newOrder);

                        e.Order = orderNumber;
                    });

                businessIntelligenceFieldList = businessIntelligenceFieldList
                    .GroupBy(b => new { b.ParentBIFieldGhostID, b.Name })
                    .Select(group => group.First())
                    .ToList();

                UpdateBIFieldList(businessIntelligenceFieldList, creationUserID);
            }

            return SCC_BL.Results.BusinessIntelligenceField.MassiveImport.CODE.SUCCESS;
        }

        public SCC_BL.Results.BusinessIntelligenceField.MassiveUpdate.CODE UpdateBIFieldList(List<BusinessIntelligenceField> biFieldList, int creationUserID)
        {
            try
            {
                /*List<BusinessIntelligenceField> allBusinessIntelligenceField = new List<BusinessIntelligenceField>();

                using (BusinessIntelligenceField auxBusinessIntelligenceField = new BusinessIntelligenceField())
                {
                    allBusinessIntelligenceField = auxBusinessIntelligenceField.SelectAll();
                }*/

                if (biFieldList == null) biFieldList = new List<BusinessIntelligenceField>();

                //Update existing ones
                List<BusinessIntelligenceField> updatedBusinessIntelligenceFieldList = new List<BusinessIntelligenceField>();

                foreach (BusinessIntelligenceField currentBusinessIntelligenceField in biFieldList.Where(e => e.ID > 0))
                {
                    int currentBasicInfoID = 0;

                    using (BusinessIntelligenceField auxBusinessIntelligenceField = new BusinessIntelligenceField(currentBusinessIntelligenceField.ID))
                    {
                        auxBusinessIntelligenceField.SetDataByID();
                        currentBasicInfoID = auxBusinessIntelligenceField.BasicInfoID;
                    }

                    BusinessIntelligenceField newBusinessIntelligenceField = new BusinessIntelligenceField(
                        currentBusinessIntelligenceField.ID,
                        currentBusinessIntelligenceField.Name,
                        currentBusinessIntelligenceField.Description,
                        currentBusinessIntelligenceField.ParentBIFieldID,
                        currentBusinessIntelligenceField.HasForcedComment,
                        currentBusinessIntelligenceField.Order,
                        currentBasicInfoID,
                        creationUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.UPDATED);


                    int result = newBusinessIntelligenceField.Update();

                    if (result > 0)
                    {
                        updatedBusinessIntelligenceFieldList.Add(currentBusinessIntelligenceField);
                    }
                }

                //Create new ones
                List<BusinessIntelligenceField> insertedBusinessIntelligenceFieldList = new List<BusinessIntelligenceField>();

                foreach (BusinessIntelligenceField currentBusinessIntelligenceField in biFieldList.Where(e => e.ID <= 0 || e.ID == null))
                {
                    int? parentID = currentBusinessIntelligenceField.ParentBIFieldID;

                    if (currentBusinessIntelligenceField.ParentBIFieldGhostID >= SCC_BL.Settings.Overall.MIN_EXISTING_BI_FIELD_GHOST_ID)
                    {
                        parentID = biFieldList.Where(e => e.BIFieldGhostID == currentBusinessIntelligenceField.ParentBIFieldGhostID).First().ID;
                    }
                    else
                    if (currentBusinessIntelligenceField.ParentBIFieldGhostID >= SCC_BL.Settings.Overall.MIN_NON_EXISTING_BI_FIELD_GHOST_ID)
                    {
                        parentID = insertedBusinessIntelligenceFieldList.Where(e => e.BIFieldGhostID == currentBusinessIntelligenceField.ParentBIFieldGhostID).First().ID;
                    }

                    parentID = parentID == 0 ? null : parentID;

                    BusinessIntelligenceField newBusinessIntelligenceField = new BusinessIntelligenceField(
                        currentBusinessIntelligenceField.Name,
                        currentBusinessIntelligenceField.Description,
                        parentID,
                        currentBusinessIntelligenceField.HasForcedComment,
                        currentBusinessIntelligenceField.Order,
                        creationUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.CREATED);

                    int result = newBusinessIntelligenceField.Insert();

                    if (result > 0)
                    {
                        currentBusinessIntelligenceField.ID = result;

                        insertedBusinessIntelligenceFieldList.Add(currentBusinessIntelligenceField);
                    }
                    else
                    {
                        switch ((SCC_BL.Results.BusinessIntelligenceField.Insert.CODE)result)
                        {
                            case SCC_BL.Results.BusinessIntelligenceField.Insert.CODE.ERROR:
                                break;
                            default:
                                return SCC_BL.Results.BusinessIntelligenceField.MassiveUpdate.CODE.ERROR;
                        }
                    }
                }

                return SCC_BL.Results.BusinessIntelligenceField.MassiveUpdate.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SCC_BL.BusinessIntelligenceField> ProcessExcelForUpload(string filePath)
        {
            User currentUser = GetCurrentUser();

            List<SCC_BL.BusinessIntelligenceField> elementList = new List<SCC_BL.BusinessIntelligenceField>();

            List<int> linesWithErrors = new List<int>();

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

                        SCC_BL.CustomTools.BIFieldUploadInfo biFieldUploadInfo = new SCC_BL.CustomTools.BIFieldUploadInfo();
                        biFieldUploadInfo.FillUploadInfo(rows.Skip(1), headersCount);

                        using (SCC_BL.BusinessIntelligenceField businessIntelligenceField = new SCC_BL.BusinessIntelligenceField())
                        {
                            elementList = businessIntelligenceField.GetBIFieldListFromExcel(rows.Skip(1), biFieldUploadInfo, headersCount, currentUser.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.MassiveImport.Error>(ex);
            }

            if (linesWithErrors.Count() > 0)
            {
                string lineList = string.Empty;

                linesWithErrors
                    .ForEach(e => {
                        if (!string.IsNullOrEmpty(lineList))
                            lineList += ", ";

                        lineList += e.ToString();
                    });

                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.MassiveImport.Error>(
                    new Exception(
                        SCC_BL.Results.BusinessIntelligenceField.MassiveImport.ErrorSingleRow.CUSTOM_ERROR_EXCEL_LINES
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, lineList)
                    )
                );

                Session[SCC_BL.Settings.AppValues.Session.ERROR_COUNT] = null;
            }

            return elementList;
        }
    }
}