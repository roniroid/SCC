using DocumentFormat.OpenXml.Office2016.Excel;
using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class CustomControlController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(CustomControlController));

        public ActionResult Manage(int? customControlID, bool filterActiveElements = false)
        {
            ViewData[SCC_BL.Settings.AppValues.ViewData.CustomControl.Manage.MODEL_ID] = null;

            if (customControlID != null && customControlID > 0)
                ViewData[SCC_BL.Settings.AppValues.ViewData.CustomControl.Manage.MODEL_ID] = customControlID.Value;

            List<CustomControl> customControlList = new List<CustomControl>();

            customControlList = new CustomControl().SelectAll();

            if (filterActiveElements)
                customControlList =
                    customControlList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DISABLED)
                        .ToList();

            return View(customControlList);

            //OLD
            /*CustomControlManagement customControlManagementViewModel = new CustomControlManagement();

            if (customControlID != null)
            {
                customControlManagementViewModel.CustomControl = new CustomControl(customControlID.Value);
                customControlManagementViewModel.CustomControl.SetDataByID();
            }

            customControlManagementViewModel.CustomControlList = new CustomControl().SelectAll();

            if (filterActiveElements)
                customControlManagementViewModel.CustomControlList =
                    customControlManagementViewModel.CustomControlList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DISABLED)
                        .ToList();

            return View(customControlManagementViewModel);*/
        }

        [HttpGet]
        public ActionResult CustomControlTypeView(int customControlTypeID, int? customControlID)
        {
            CustomControl customControl = new CustomControl();
            customControl.ControlTypeID = customControlTypeID;

            int? selectedModuleID = null;

            selectedModuleID = (int)SCC_BL.DBValues.Catalog.MODULE.CALIBRATION;
            customControl.ModuleID = selectedModuleID.Value;

            if (customControlID != null && customControlID > 0)
            {
                customControl = new CustomControl(customControlID.Value);
                customControl.SetDataByID();

                selectedModuleID = customControl.ModuleID;

                if (!string.IsNullOrEmpty(customControl.Mask))
                {
                    int maskID = 0;

                    switch (customControl.Mask)
                    {
                        case SCC_BL.Settings.AppValues.Masks.Alphanumeric1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.ALPHANUMERIC_1;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.Date1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.DATE_1;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.Time1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.TIME_1;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.PhoneNumber1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.PHONE_1;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.PhoneNumber2.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.PHONE_2;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.Name1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.NAME_1;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.LastName1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.LAST_NAME_1;
                            break;
                        case SCC_BL.Settings.AppValues.Masks.Email1.MASK:
                            maskID = (int)SCC_BL.Settings.AppValues.Masks.MaskID.EMAIL_1;
                            break;
                        default:
                            break;
                    }

                    ViewData[SCC_BL.Settings.AppValues.ViewData.CustomControl.CustomControlTypeView.MASK_ID] = maskID;
                }
            }

            List<Catalog> moduleList = new List<Catalog>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.MODULE))
                moduleList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.CustomControl.CustomControlTypeView.Module.NAME] =
                new SelectList(
                    moduleList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    selectedModuleID);

            switch (customControl.ControlTypeID)
            {
                case (int)SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.TEXT_BOX:
                    return PartialView("_TextBox", customControl);
                case (int)SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.TEXT_AREA:
                    return PartialView("_TextArea", customControl);
                case (int)SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.CHECKBOX:
                    return PartialView("_CheckBox", customControl);
                case (int)SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.RADIO_BUTTON:
                    return PartialView("_RadioButton", customControl);
                case (int)SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.SELECT_LIST:
                    return PartialView("_SelectList", customControl);
                default:
                    break;
            }

            return RedirectToAction(nameof(CustomControlController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Edit(CustomControl customControl, List<CustomControlValueCatalog> valueList = null, int maskID = 0)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_CUSTOM_FIELDS))
            {
                SaveProcessingInformation<SCC_BL.Results.CustomControl.Update.NotAllowedToCreateCustomFields>();
                return RedirectToAction(nameof(CustomControlController.Manage), GetControllerName(typeof(CustomControlController)));
            }

            string mask = string.Empty;
            string pattern = string.Empty;

            if (maskID > 0)
            {
                switch (maskID)
                {
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.DATE_1:
                        mask = SCC_BL.Settings.AppValues.Masks.Date1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.Date1.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.TIME_1:
                        mask = SCC_BL.Settings.AppValues.Masks.Time1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.Time1.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.PHONE_1:
                        mask = SCC_BL.Settings.AppValues.Masks.PhoneNumber1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.PhoneNumber1.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.PHONE_2:
                        mask = SCC_BL.Settings.AppValues.Masks.PhoneNumber2.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.PhoneNumber2.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.ALPHANUMERIC_1:
                        mask = SCC_BL.Settings.AppValues.Masks.Alphanumeric1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.Alphanumeric1.PATTERN;
                        break;
                    default:
                        break;
                }
            }

            CustomControl oldCustomControl = new CustomControl(customControl.ID);
            oldCustomControl.SetDataByID();

            CustomControl newCustomControl = new CustomControl(
                customControl.ID, 
                customControl.Label, 
                customControl.ModuleID, 
                customControl.IsRequired, 
                customControl.Description ?? string.Empty, 
                customControl.ControlTypeID,
                customControl.CssClass ?? string.Empty,
                mask,
                pattern,
                customControl.DefaultValue ?? string.Empty,
                customControl.NumberOfRows, 
                customControl.NumberOfColumns, 
                customControl.BasicInfoID, 
                GetActualUser().ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.UPDATED);

            newCustomControl.SetValueList();

            try
            {
                int result = newCustomControl.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.CustomControl.Update.Success>(newCustomControl.ID, newCustomControl.BasicInfo.StatusID, oldCustomControl);

                    SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE response = SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.ERROR;

                    try
                    {
                        response = newCustomControl.UpdateCustomControlValueCatalogList(valueList, GetActualUser().ID);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.Error>(null, null, valueList, ex);
                    }

                    switch (response)
                    {
                        case SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.SUCCESS:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.Success>(newCustomControl.ID, newCustomControl.BasicInfo.StatusID, valueList);
                            break;
                        case SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.Error>(null, null, valueList);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch ((SCC_BL.Results.CustomControl.Update.CODE)result)
                    {
                        case SCC_BL.Results.CustomControl.Update.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.Update.ALREADY_EXISTS_NAME>(oldCustomControl.ID, oldCustomControl.BasicInfo.StatusID, newCustomControl);
                            break;
                        case SCC_BL.Results.CustomControl.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.Update.Error>(oldCustomControl.ID, oldCustomControl.BasicInfo.StatusID, newCustomControl);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.CustomControl.Update.Error>(oldCustomControl.ID, oldCustomControl.BasicInfo.StatusID, oldCustomControl, ex);
            }

            return Json(new { url = Url.Action(nameof(CustomControlController.Manage), _mainControllerName) });

            //return RedirectToAction(nameof(CustomControlController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(CustomControl customControl, List<CustomControlValueCatalog> valueList = null, int maskID = 0)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_CUSTOM_FIELDS))
            {
                SaveProcessingInformation<SCC_BL.Results.CustomControl.Insert.NotAllowedToCreateCustomFields>();
                return RedirectToAction(nameof(CustomControlController.Manage), GetControllerName(typeof(CustomControlController)));
            }

            string mask = string.Empty;
            string pattern = string.Empty;

            if (maskID > 0)
            {
                switch (maskID)
                {
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.DATE_1:
                        mask = SCC_BL.Settings.AppValues.Masks.Date1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.Date1.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.TIME_1:
                        mask = SCC_BL.Settings.AppValues.Masks.Time1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.Time1.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.PHONE_1:
                        mask = SCC_BL.Settings.AppValues.Masks.PhoneNumber1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.PhoneNumber1.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.PHONE_2:
                        mask = SCC_BL.Settings.AppValues.Masks.PhoneNumber2.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.PhoneNumber2.PATTERN;
                        break;
                    case (int)SCC_BL.Settings.AppValues.Masks.MaskID.ALPHANUMERIC_1:
                        mask = SCC_BL.Settings.AppValues.Masks.Alphanumeric1.MASK;
                        pattern = SCC_BL.Settings.AppValues.Masks.Alphanumeric1.PATTERN;
                        break;
                    default:
                        break;
                }
            }

            CustomControl newCustomControl = new CustomControl(
                customControl.Label,
                customControl.ModuleID,
                customControl.IsRequired,
                customControl.Description ?? string.Empty,
                customControl.ControlTypeID,
                customControl.CssClass ?? string.Empty,
                mask,
                pattern,
                customControl.DefaultValue ?? string.Empty,
                customControl.NumberOfRows,
                customControl.NumberOfColumns,
                GetActualUser().ID,
                (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.CREATED);

            try
            {
                int result = newCustomControl.Insert();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.CustomControl.Insert.Success>(newCustomControl.ID, newCustomControl.BasicInfo.StatusID, newCustomControl);

                    SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE response = SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.ERROR;

                    try
                    {
                        response = newCustomControl.UpdateCustomControlValueCatalogList(valueList, GetActualUser().ID);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.Error>(null, null, valueList, ex);
                    }

                    switch (response)
                    {
                        case SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.SUCCESS:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.Success>(newCustomControl.ID, newCustomControl.BasicInfo.StatusID, valueList);
                            break;
                        case SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.UpdateCustomControlValueCatalogList.Error>(null, null, valueList);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch ((SCC_BL.Results.CustomControl.Insert.CODE)result)
                    {
                        case SCC_BL.Results.CustomControl.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.Insert.ALREADY_EXISTS_NAME>(null, null, newCustomControl);
                            break;
                        case SCC_BL.Results.CustomControl.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.CustomControl.Insert.Error>(null, null, newCustomControl);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.CustomControl.Insert.Error>(null, null, ex);
            }

            return Json(new { url = Url.Action(nameof(CustomControlController.Manage), _mainControllerName) });

            //return RedirectToAction(nameof(CustomControlController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int customControlID)
        {
            CustomControl customControl = new CustomControl(customControlID);
            customControl.SetDataByID();

            try
            {
                //customControl.Delete();

                customControl.BasicInfo.ModificationUserID = GetActualUser().ID;
                customControl.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DELETED;

                //int result = customControl.BasicInfo.Update();

                List<CustomControl> customControlList = new List<CustomControl>();

                using (CustomControl auxCustomControl = new CustomControl())
                    customControlList = auxCustomControl.SelectAll();

                int count = 
                    customControlList
                        .Where(e =>
                            (e.Label.Length >= customControl.Label.Length ? e.Label.Substring(0, customControl.Label.Length).Equals(customControl.Label) : false) &&
                            e.ID != customControl.ID)
                        .Count() + 1;

                customControl.Label += 
                    SCC_BL.Settings.AppValues.DELETED_SUFIX
                        .Replace(SCC_BL.Settings.AppValues.DELETED_SUFIX_COUNT, count.ToString());

                int result = customControl.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.CustomControl.Delete.Success>(customControl.ID, customControl.BasicInfo.StatusID, customControl);

                    return RedirectToAction(nameof(CustomControlController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.CustomControl.Delete.Success>(customControl.ID, customControl.BasicInfo.StatusID, customControl, ex);
            }

            return RedirectToAction(nameof(CustomControlController.Manage), _mainControllerName);
        }
    }
}