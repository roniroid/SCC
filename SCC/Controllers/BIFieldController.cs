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
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_BI_QUESTIONS))
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
                GetActualUser().ID, 
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
                        response = newBIField.UpdateBIFieldChildList(childList, GetActualUser().ID);
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
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_BI_QUESTIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.BusinessIntelligenceField.Insert.NotAllowedToCreateBIQuestions>();
                return RedirectToAction(nameof(BIFieldController.Manage), GetControllerName(typeof(BIFieldController)));
            }

            BusinessIntelligenceField newBIField = new BusinessIntelligenceField(
                businessIntelligenceField.Name, 
                businessIntelligenceField.Description, 
                businessIntelligenceField.ParentBIFieldID, 
                businessIntelligenceField.HasForcedComment, 
                GetActualUser().ID, 
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
                        response = newBIField.UpdateBIFieldChildList(childList, GetActualUser().ID);
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
            BusinessIntelligenceField biField = new BusinessIntelligenceField(biFieldID);
            biField.SetDataByID();

            try
            {
                //biField.Delete();

                biField.BasicInfo.ModificationUserID = GetActualUser().ID;
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
    }
}