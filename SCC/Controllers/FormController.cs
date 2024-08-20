using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Excel;
using SCC.ViewModels;
using SCC_BL;
using SCC_BL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class FormController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(FormController));

        public ActionResult Manage(bool filterActiveElements = false, bool filterBoundForms = false)
        {
            List<Form> formList = new List<Form>();
            List<Catalog> formTypeList = new List<Catalog>();

            using (Form form = new Form())
            {
                formList = form.SelectAll(true);
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.FORM_TYPE))
                formTypeList = catalog.SelectByCategoryID();

            List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

            using (ProgramFormCatalog programFormCatalog = new ProgramFormCatalog())
            {
                programFormCatalogList = programFormCatalog.SelectAll();
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Manage.AllTypeList.NAME] = formTypeList;
            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Manage.AllProgramFormCatalogList.NAME] = programFormCatalogList;

            if (filterActiveElements)
                formList =
                    formList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DISABLED)
                        .ToList();

            if (filterBoundForms)
                formList =
                    formList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DISABLED &&
                            programFormCatalogList.Select(s => s.FormID).Contains(e.ID))
                        .ToList();

            return View(formList);
        }

        public ActionResult FormBinding(int? formID)
        {
            ProgramFormBindingViewModel programFormBindingViewModel = new ProgramFormBindingViewModel();

            Form form = new Form();

            List<Form> formList = new List<Form>();
            List<Program> allProgramList = new List<Program>();
            List<Program> programList = new List<Program>();
            List<ProgramFormCatalog> allProgramFormCatalogListList = new List<ProgramFormCatalog>();

            DateTime startDate = DateTime.Now;

            using (Form auxForm = new Form())
            {
                formList =
                    auxForm.SelectAll(true)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DISABLED)
                        .ToList();
            }

            if (formID != null)
            {
                if (formList.Select(e => e.ID).Contains(formID.Value))
                {
                    form = formList.Find(e => e.ID == formID.Value);
                }
                else
                {
                    form = new Form(formID.Value);
                    form.SetDataByID(true);
                }

                using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(form.ID))
                    startDate = programFormCatalog.SelectByFormID().LastOrDefault().StartDate;
            }

            /*if (formID != null)
            {
                form = new Form(formID.Value);
                form.SetDataByID(true);

                using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(form.ID))
                    startDate = programFormCatalog.SelectByFormID().LastOrDefault().StartDate;
            }*/

            using (Program program = new Program())
            {
                allProgramList = program.SelectAll();

                programList =
                    allProgramList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .ToList();
            }

            using (ProgramFormCatalog auxProgramFormCatalog = new ProgramFormCatalog())
            {
                allProgramFormCatalogListList = auxProgramFormCatalog.SelectAll();
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.FormBinding.FormList.NAME] =
                new SelectList(
                    formList,
                    nameof(Form.ID),
                    nameof(Form.Name),
                    form.ID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.FormBinding.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    form.ProgramFormCatalogList.Select(s => s.ProgramID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.FormBinding.AllProgramList.NAME] = allProgramList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.FormBinding.AllProgramFormCatalogList.NAME] = allProgramFormCatalogListList;

            programFormBindingViewModel.Form = form;
            programFormBindingViewModel.FormList = formList;
            programFormBindingViewModel.StartDate = startDate;

            return View(programFormBindingViewModel);
        }

        public ActionResult Edit(int? formID)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_TEMPLATE_FORMS))
            {
                SaveProcessingInformation<SCC_BL.Results.Form.Update.NotAllowedToCreateTemplateForms>();
                return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
            }

            Form form = new Form();

            form.TypeID = (int)SCC_BL.DBValues.Catalog.FORM_TYPE.TEMPLATE;

            if (formID != null && formID > 0)
            {
                form = new Form(formID.Value);
                form.SetDataByID();
            }

            List<Catalog> formTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();
            List<BusinessIntelligenceField> biFieldList = new List<BusinessIntelligenceField>();
            List<Catalog> errorTypeList = new List<Catalog>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.FORM_TYPE))
                formTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (CustomControl customControl = new CustomControl())
                customControlList =
                    customControl.SelectAll()
                        .Where(e => 
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DISABLED)
                        .ToList();

            using (BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField())
                biFieldList =
                    businessIntelligenceField.SelectAll()
                        .Where(e =>
                            (e.ParentBIFieldID == null || e.ParentBIFieldID == 0) &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DISABLED)
                        .ToList();

            errorTypeList = SCC_BL.Attribute.GetAttributeErrorType();

            List<AttributeValueCatalog> defineAnswerTypeList = new List<AttributeValueCatalog>();

            foreach (SCC_BL.Attribute attribute in form.AttributeList)
            {
                defineAnswerTypeList.AddRange(form.AttributeList.Where(e => e.ID == attribute.ID).FirstOrDefault().ValueList);
            }

            string jsonDefineAnswerTypeList = OverallController.Serialize(defineAnswerTypeList);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Edit.AttributeErrorTypeList.NAME] = SCC_BL.Attribute.GetAttributeErrorType();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Edit.JsonDefineAnswerTypeList.NAME] = jsonDefineAnswerTypeList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Edit.TypeList.NAME] =
                new SelectList(
                    formTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    form.TypeID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Edit.CustomControlList.NAME] = customControlList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Edit.BIFieldList.NAME] = biFieldList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.Edit.ErrorTypeList.NAME] =
                new SelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            return View(form);
        }

        void UpdateProgramFormCatalogList(Form form, List<SCC_BL.ProgramFormCatalog> programFormCatalogList)
        {
            User currentUser = GetCurrentUser();

            try
            {
                switch (form.UpdateProgramFormCatalogList(programFormCatalogList, currentUser.ID))
                {
                    case SCC_BL.Results.Form.UpdateProgramFormCatalogList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Form.UpdateProgramFormCatalogList.Success>(form.ID, form.BasicInfo.StatusID, form);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.UpdateProgramFormCatalogList.Error>(form.ID, form.BasicInfo.StatusID, form, ex);
            }

        }

        void UpdateProgramFormCatalogList(List<SCC_BL.ProgramFormCatalog> programFormCatalogList)
        {
            User currentUser = GetCurrentUser();

            foreach (int programID in programFormCatalogList.GroupBy(e => e.ProgramID).Select(e => e.First().ProgramID))
            {
                using (Program program = new Program(programID))
                {
                    program.SetDataByID();

                    try
                    {
                        switch (program.UpdateProgramFormCatalogList(programFormCatalogList.Where(e => e.ProgramID == programID).ToList(), currentUser.ID))
                        {
                            case SCC_BL.Results.Program.UpdateProgramFormCatalogList.CODE.SUCCESS:
                                SaveProcessingInformation<SCC_BL.Results.Program.UpdateProgramFormCatalogList.Success>(program.ID, program.BasicInfo.StatusID, program);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.Program.UpdateProgramFormCatalogList.Error>(program.ID, program.BasicInfo.StatusID, program, ex);
                    }
                }
            }
        }

        void UpdateAttributeList(Form form, List<SCC_BL.Attribute> attributeList, List<AttributeValueCatalog> attributeValueCatalogList)
        {
            User currentUser = GetCurrentUser();

            try
            {
                switch (form.UpdateAttributeList(attributeList, attributeValueCatalogList, currentUser.ID))
                {
                    case SCC_BL.Results.Form.UpdateAttributeList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Form.UpdateAttributeList.Success>(form.ID, form.BasicInfo.StatusID, form);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.UpdateAttributeList.Error>(form.ID, form.BasicInfo.StatusID, form, ex);
            }

        }

        void UpdateCustomFieldList(Form form, int[] customControlList)
        {
            User currentUser = GetCurrentUser();

            try
            {
                switch (form.UpdateCustomFieldList(customControlList, currentUser.ID))
                {
                    case SCC_BL.Results.Form.UpdateCustomFieldList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Form.UpdateCustomFieldList.Success>(form.ID, form.BasicInfo.StatusID, form);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.UpdateCustomFieldList.Error>(form.ID, form.BasicInfo.StatusID, form, ex);
            }

        }

        void UpdateBIFieldList(Form form, int[] biFieldList)
        {
            User currentUser = GetCurrentUser();

            try
            {
                switch (form.UpdateBIFieldList(biFieldList, currentUser.ID))
                {
                    case SCC_BL.Results.Form.UpdateBIFieldList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Form.UpdateBIFieldList.Success>(form.ID, form.BasicInfo.StatusID, form);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.UpdateBIFieldList.Error>(form.ID, form.BasicInfo.StatusID, form, ex);
            }

        }

        [HttpPost]
        public ActionResult FormBinding(int[] formIDArray, DateTime startDate, int[] programIDArray = null)
        {
            User currentUser = GetCurrentUser();

            for (int i = 0; i < formIDArray.Length; i++)
            {
                Form form = new Form(formIDArray[i]);
                form.SetDataByID(true);

                List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

                programIDArray =
                    programIDArray != null
                        ? programIDArray
                        : new int[0];

                for (int j = 0; j < programIDArray.Length; j++)
                {
                    programFormCatalogList.Add(new ProgramFormCatalog(programIDArray[j], formIDArray[i], startDate, currentUser.ID, (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.CREATED));
                }

                try
                {
                    bool clearingResult = this.ClearFormBindings(formIDArray[i], programIDArray);

                    if (clearingResult)
                    {
                        //UpdateProgramFormCatalogList(form, programFormCatalogList);
                        UpdateProgramFormCatalogList(programFormCatalogList);

                        SaveProcessingInformation<SCC_BL.Results.Form.UpdateProgramFormCatalogList.Success>(form.ID, form.BasicInfo.StatusID, form);
                    }
                    else
                    {
                        SaveProcessingInformation<SCC_BL.Results.Form.UpdateProgramFormCatalogList.Error>();
                    }
                }
                catch (Exception ex)
                {
                    SaveProcessingInformation<SCC_BL.Results.Form.UpdateProgramFormCatalogList.Error>(null, null, ex);
                }
            }

            //return Json(new { url = Url.Action(nameof(FormController.FormBinding), _mainControllerName) });

            return RedirectToAction(nameof(FormController.FormBinding), _mainControllerName);
        }

        bool ClearFormBindings(int formID, int[] programIDArray)
        {
            User currentUser = GetCurrentUser();

            bool result = true;

            using (ProgramFormCatalog auxProgramFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(formID))
            {
                List<ProgramFormCatalog> programFormCatalogList = auxProgramFormCatalog.SelectByFormID();

                programFormCatalogList
                    .ForEach(async e => {
                        if (!programIDArray.Contains(e.ProgramID))
                        {
                            try
                            {
                                e.Delete();
                            }
                            catch (Exception ex)
                            {
                                e.BasicInfo.ModificationUserID = currentUser.ID;
                                e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.DELETED;
                                e.BasicInfo.Update();
                            }
                        }
                    });
            }

            return result;
        }

        [HttpPost]
        public ActionResult Create(Form form, List<SCC_BL.Attribute> attributeList, List<AttributeValueCatalog> attributeValueCatalogList, int[] customFieldIDArray = null, int[] biFieldArray = null)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_TEMPLATE_FORMS))
            {
                SaveProcessingInformation<SCC_BL.Results.Form.Insert.NotAllowedToCreateTemplateForms>();
                return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
            }

            List<SCC_BL.Attribute> nonCriticalErrorAttributes = attributeList.Where(e => e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE).ToList();

            if (nonCriticalErrorAttributes.Count() > 0)
            {
                SCC_BL.Results.Form.CheckNCEScore.CODE resultCheckNCEScore = SCC_BL.Results.Form.CheckNCEScore.CODE.SUCCESS;

                resultCheckNCEScore = Form.CheckNCEScore(nonCriticalErrorAttributes);

                switch (resultCheckNCEScore)
                {
                    case SCC_BL.Results.Form.CheckNCEScore.CODE.ERROR_LESS_THAN_100:
                        SaveProcessingInformation<SCC_BL.Results.Form.CheckNCEScore.ErrorLessThan100>(null, null, nonCriticalErrorAttributes);
                        return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
                    case SCC_BL.Results.Form.CheckNCEScore.CODE.ERROR_GREATER_THAN_100:
                        SaveProcessingInformation<SCC_BL.Results.Form.CheckNCEScore.ErrorGreaterThan100>(null, null, nonCriticalErrorAttributes);
                        return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
                    default:
                        break;
                }
            }

            Form newForm = new Form(form.Name, form.TypeID, form.Comment, currentUser.ID, (int)SCC_BL.DBValues.Catalog.STATUS_FORM.CREATED);

            try
            {
                int result = newForm.Insert();

                if (result > 0)
                {
                    UpdateCustomFieldList(newForm, customFieldIDArray);

                    UpdateBIFieldList(newForm, biFieldArray);

                    UpdateAttributeList(newForm, attributeList, attributeValueCatalogList);

                    SaveProcessingInformation<SCC_BL.Results.Form.Insert.Success>(newForm.ID, newForm.BasicInfo.StatusID, newForm);
                }
                else
                {
                    switch ((SCC_BL.Results.Form.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Form.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Form.Insert.ALREADY_EXISTS_NAME>(null, null, newForm);
                            break;
                        case SCC_BL.Results.Form.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Form.Insert.Error>(null, null, newForm);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.Insert.Error>(null, null, null, ex);
            }

            return Json(new { url = Url.Action(nameof(FormController.Manage), _mainControllerName) });
        }

        [HttpPost]
        public ActionResult Edit(Form form, List<SCC_BL.Attribute> attributeList, List<AttributeValueCatalog> attributeValueCatalogList, int[] customFieldIDArray = null, int[] biFieldArray = null)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_TEMPLATE_FORMS))
            {
                SaveProcessingInformation<SCC_BL.Results.Form.Insert.NotAllowedToCreateTemplateForms>();
                return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
            }

            List<SCC_BL.Attribute> nonCriticalErrorAttributes = attributeList.Where(e => e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE).ToList();

            if (nonCriticalErrorAttributes.Count() > 0)
            {
                SCC_BL.Results.Form.CheckNCEScore.CODE resultCheckNCEScore = SCC_BL.Results.Form.CheckNCEScore.CODE.SUCCESS;

                resultCheckNCEScore = Form.CheckNCEScore(nonCriticalErrorAttributes);

                switch (resultCheckNCEScore)
                {
                    case SCC_BL.Results.Form.CheckNCEScore.CODE.ERROR_LESS_THAN_100:
                        SaveProcessingInformation<SCC_BL.Results.Form.CheckNCEScore.ErrorLessThan100>(null, null, nonCriticalErrorAttributes);
                        return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
                    case SCC_BL.Results.Form.CheckNCEScore.CODE.ERROR_GREATER_THAN_100:
                        SaveProcessingInformation<SCC_BL.Results.Form.CheckNCEScore.ErrorGreaterThan100>(null, null, nonCriticalErrorAttributes);
                        return RedirectToAction(nameof(FormController.Manage), GetControllerName(typeof(FormController)));
                    default:
                        break;
                }
            }

            Form oldForm = new Form(form.ID);
            oldForm.SetDataByID(true);

            Form newForm = new Form(form.ID, form.Name, form.TypeID, form.Comment, oldForm.BasicInfoID, GetCurrentUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_FORM.UPDATED);

            try
            {
                int result = newForm.Update();

                if (result > 0)
                {
                    newForm.SetDataByID();

                    UpdateCustomFieldList(newForm, customFieldIDArray);

                    UpdateBIFieldList(newForm, biFieldArray);

                    UpdateAttributeList(newForm, attributeList, attributeValueCatalogList);

                    SaveProcessingInformation<SCC_BL.Results.Form.Update.Success>(newForm.ID, newForm.BasicInfo.StatusID, oldForm);
                }
                else
                {
                    switch ((SCC_BL.Results.Form.Update.CODE)result)
                    {
                        case SCC_BL.Results.Form.Update.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Form.Update.ALREADY_EXISTS_NAME>(oldForm.ID, oldForm.BasicInfo.StatusID, newForm);
                            break;
                        case SCC_BL.Results.Form.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Form.Update.Error>(oldForm.ID, oldForm.BasicInfo.StatusID, newForm);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.Update.Error>(oldForm.ID, oldForm.BasicInfo.StatusID, oldForm, ex);
            }

            return Json(new { url = Url.Action(nameof(FormController.Manage), _mainControllerName) });
        }

        public ActionResult UploadForm()
        {
            List<Catalog> formTypeList = new List<Catalog>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.FORM_TYPE))
                formTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Form.UploadForm.TypeList.NAME] =
                new SelectList(
                    formTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            List<UploadedFile> uploadedFileList = new List<UploadedFile>();

            using (UploadedFile uploadedFile = new UploadedFile())
            {
                uploadedFileList =
                    uploadedFile.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_UPLOAD_FORM)
                        .ToList();

            }

            ViewModels.FormUploadViewModel formUploadViewModel = new ViewModels.FormUploadViewModel();
            formUploadViewModel.Form = new Form();
            formUploadViewModel.UploadedFileList = uploadedFileList;

            return View(formUploadViewModel);
        }

        [HttpPost]
        public ActionResult UploadForm(Form form, HttpPostedFileBase file)
        {
            User currentUser = GetCurrentUser();

            Form newForm = new Form(form.Name, form.TypeID, form.Comment, currentUser.ID, (int)SCC_BL.DBValues.Catalog.STATUS_FORM.CREATED);

            string filePath = SaveUploadedFile(file, SCC_BL.Settings.Paths.Form.FORM_UPLOAD_FOLDER);

            if (!string.IsNullOrEmpty(filePath))
            {
                int uploadedFileID = 0;

                uploadedFileID = SaveFileInDatabase(filePath, (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_UPLOAD_FORM);

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
                        SCC_BL.Results.UploadedFile.FormUpload.CODE result = ProcessImportExcel(newForm, filePath);

                        switch (result)
                        {
                            case SCC_BL.Results.UploadedFile.FormUpload.CODE.SUCCESS:
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Success>();
                                break;
                            case SCC_BL.Results.UploadedFile.FormUpload.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(null, null, form);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(null, null, form, ex);
                    }
                }
            }
            else
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>();
            }

            return RedirectToAction(nameof(FormController.UploadForm));
        }

        public SCC_BL.Results.UploadedFile.FormUpload.CODE ProcessImportExcel(Form form, string filePath)
        {
            List<SCC_BL.Attribute> attributeList = new List<SCC_BL.Attribute>();
            List<SCC_BL.AttributeValueCatalog> attributeValueCatalogList = new List<SCC_BL.AttributeValueCatalog>();

            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
            {
                (List<SCC_BL.Attribute>, List<SCC_BL.AttributeValueCatalog>) elements = ProcessExcelForFormUpload(filePath);

                attributeList = elements.Item1;
                attributeValueCatalogList = elements.Item2;

                SCC_BL.Results.Form.CheckNCEScore.CODE resultCheckNCEScore = SCC_BL.Results.Form.CheckNCEScore.CODE.SUCCESS;

                List<SCC_BL.Attribute> nonCriticalErrorAttributes = attributeList.Where(e => e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE).ToList();

                if (nonCriticalErrorAttributes.Count() > 0)
                {
                    resultCheckNCEScore = Form.CheckNCEScore(nonCriticalErrorAttributes);

                    switch (resultCheckNCEScore)
                    {
                        case SCC_BL.Results.Form.CheckNCEScore.CODE.ERROR_LESS_THAN_100:
                            SaveProcessingInformation<SCC_BL.Results.Form.CheckNCEScore.ErrorLessThan100>(null, null, nonCriticalErrorAttributes);

                            return SCC_BL.Results.UploadedFile.FormUpload.CODE.ERROR;
                        case SCC_BL.Results.Form.CheckNCEScore.CODE.ERROR_GREATER_THAN_100:
                            SaveProcessingInformation<SCC_BL.Results.Form.CheckNCEScore.ErrorGreaterThan100>(null, null, nonCriticalErrorAttributes);

                            return SCC_BL.Results.UploadedFile.FormUpload.CODE.ERROR;
                        default:
                            break;
                    }
                }

                int formInsertResultCode = form.Insert();

                if (formInsertResultCode <= 0)
                {
                    switch ((SCC_BL.Results.Form.Insert.CODE)formInsertResultCode)
                    {
                        case SCC_BL.Results.Form.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Form.Insert.ALREADY_EXISTS_NAME>(null, null, form);

                            return SCC_BL.Results.UploadedFile.FormUpload.CODE.ERROR;
                        case SCC_BL.Results.Form.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Form.Insert.Error>(null, null, form);

                            return SCC_BL.Results.UploadedFile.FormUpload.CODE.ERROR;
                        default:
                            break;
                    }
                }

                form.SetDataByID();

                attributeList
                    .ForEach(e => {
                        string newOrder = e.Order.ToString().PadLeft(SCC_BL.Settings.Overall.DEFAULT_ORDER_LENGTH, '0');
                        newOrder = form.ID.ToString() + newOrder;
                        int orderNumber = Convert.ToInt32(newOrder);

                        e.FormID = form.ID;
                        e.Order = orderNumber;
                    });

                UpdateAttributeList(form, attributeList, attributeValueCatalogList);
            }

            return SCC_BL.Results.UploadedFile.FormUpload.CODE.SUCCESS;
        }

        public (List<SCC_BL.Attribute>, List<SCC_BL.AttributeValueCatalog>) ProcessExcelForFormUpload(string filePath)
        {
            User currentUser = GetCurrentUser();

            List<SCC_BL.Attribute> elementList = new List<SCC_BL.Attribute>();
            List<SCC_BL.AttributeValueCatalog> attributeValueCatalogList = new List<SCC_BL.AttributeValueCatalog>();

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

                        SCC_BL.CustomTools.FormUploadInfo formUploadInfo = new SCC_BL.CustomTools.FormUploadInfo();
                        formUploadInfo.FillErrorTypeInfo(rows.Skip(1), headersCount);

                        using (SCC_BL.Attribute attribute = new SCC_BL.Attribute())
                        {
                            elementList = attribute.GetAttributeListFromExcel(rows.Skip(1), formUploadInfo, headersCount, currentUser.ID);
                        }

                        foreach (SCC_BL.Attribute attribute in elementList)
                        {
                            attributeValueCatalogList =
                                attributeValueCatalogList
                                    .Concat(attribute.ValueList)
                                    .ToList();
                        }

                        /*foreach (SCC_BL.Attribute attribute in attributeList)
                        {
                            int result = attribute.Insert();

                            if (result > 0)
                            {
                                foreach (AttributeValueCatalog attributeValueCatalog in attribute.ValueList)
                                {
                                    AttributeValueCatalog newAttributeValueCatalog = new AttributeValueCatalog(attribute.ID, attributeValueCatalog.Name, attributeValueCatalog.Value, attributeValueCatalog.TriggersChildVisualization, attributeValueCatalog.Order, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.CREATED);
                                    newAttributeValueCatalog.Insert();
                                }
                            }
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(ex);
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

                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(
                    new Exception(
                        SCC_BL.Results.UploadedFile.FormUpload.ErrorSingleRow.CUSTOM_ERROR_EXCEL_LINES
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, lineList)
                    )
                );

                Session[SCC_BL.Settings.AppValues.Session.ERROR_COUNT] = null;
            }

            return (elementList, attributeValueCatalogList);
        }

        [HttpPost]
        public ActionResult Delete(int formID)
        {
            User currentUser = GetCurrentUser();

            Form form = new Form(formID);
            form.SetDataByID(true);

            try
            {
                //form.Delete();

                form.BasicInfo.ModificationUserID = currentUser.ID;
                form.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_FORM.DELETED;

                int result = form.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Form.Delete.Success>(form.ID, form.BasicInfo.StatusID, form);

                    return RedirectToAction(nameof(FormController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.Delete.Error>(form.ID, form.BasicInfo.StatusID, form, ex);
            }

            return RedirectToAction(nameof(FormController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult DeleteFormBinding(int formID)
        {
            User currentUser = GetCurrentUser();

            Form form = new Form(formID);
            form.SetDataByID();

            try
            {
                foreach (ProgramFormCatalog programFormCatalog in form.ProgramFormCatalogList)
                {
                    try
                    {
                        programFormCatalog.Delete();

                        SaveProcessingInformation<SCC_BL.Results.Form.DeleteFormBinding.Success>(form.ID, form.BasicInfo.StatusID, form);
                    }
                    catch (Exception ex)
                    {
                        programFormCatalog.BasicInfo.ModificationUserID = currentUser.ID;
                        programFormCatalog.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.DELETED;
                        programFormCatalog.BasicInfo.Update();

                        SaveProcessingInformation<SCC_BL.Results.Form.DeleteFormBinding.Success>(form.ID, form.BasicInfo.StatusID, programFormCatalog, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Form.DeleteFormBinding.Error>(form.ID, form.BasicInfo.StatusID, form, ex);
            }

            return RedirectToAction(nameof(FormController.FormBinding), _mainControllerName);
        }

        [HttpPost]
        public ActionResult ExportFormToExcel(int formID)
        {
            Form currentForm = new Form(formID);
            currentForm.SetDataByID();

            string newFileName = $"Plantilla de formulario - { currentForm.Name } {DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            string filePath =
                AppDomain.CurrentDomain.BaseDirectory +
                SCC_BL.Settings.Paths.Form.FORM_EXPORT_FOLDER.Substring(SCC_BL.Settings.Paths.Form.FORM_EXPORT_FOLDER.IndexOf('/') + 1) +
                newFileName;

            using (ExcelParser excelParser = new ExcelParser())
            {
                excelParser.ExportFormToExcel(currentForm, filePath);
            }

            return DownLoadFileFromServer(filePath, SCC_BL.Settings.AppValues.File.ContentType.EXCEL_FILES_XLSX);
        }
    }
}