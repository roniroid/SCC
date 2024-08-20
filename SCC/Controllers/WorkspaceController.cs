using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class WorkspaceController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(WorkspaceController));

        public ActionResult Manage(int? workspaceID, bool filterActiveElements = false)
        {
            WorkspaceManagementViewModel workspaceManagementViewModel = new WorkspaceManagementViewModel();

            if (workspaceID != null)
            {
                workspaceManagementViewModel.Workspace = new Workspace(workspaceID.Value);
                workspaceManagementViewModel.Workspace.SetDataByID();
            }

            workspaceManagementViewModel.WorkspaceList = new Workspace().SelectAll();

            if (filterActiveElements)
                workspaceManagementViewModel.WorkspaceList =
                    workspaceManagementViewModel.WorkspaceList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DISABLED)
                        .ToList();

            return View(workspaceManagementViewModel);
        }

        [HttpPost]
        public ActionResult Edit(WorkspaceManagementViewModel workspaceManagementViewModel)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_WORKSPACES))
            {
                SaveProcessingInformation<SCC_BL.Results.Workspace.Update.NotAllowedToModifyWorkspaces>();
                return RedirectToAction(nameof(WorkspaceController.Manage), GetControllerName(typeof(WorkspaceController)));
            }

            Workspace oldWorkspace = new Workspace(workspaceManagementViewModel.Workspace.ID);
            oldWorkspace.SetDataByID();

            Workspace newWorkspace = new Workspace(workspaceManagementViewModel.Workspace.ID, workspaceManagementViewModel.Workspace.Identifier, workspaceManagementViewModel.Workspace.Name, workspaceManagementViewModel.Workspace.Monitorable, workspaceManagementViewModel.Workspace.BasicInfoID, currentUser.ID, (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.UPDATED);

            try
            {
                int result = newWorkspace.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Workspace.Update.Success>(newWorkspace.ID, newWorkspace.BasicInfo.StatusID, oldWorkspace);
                }
                else
                {
                    switch ((SCC_BL.Results.Workspace.Update.CODE)result)
                    {
                        case SCC_BL.Results.Workspace.Update.CODE.ALREADY_EXISTS_IDENTIFIER:
                            SaveProcessingInformation<SCC_BL.Results.Workspace.Update.ALREADY_EXISTS_IDENTIFIER>(oldWorkspace.ID, oldWorkspace.BasicInfo.StatusID, newWorkspace);
                            break;
                        case SCC_BL.Results.Workspace.Update.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Workspace.Update.ALREADY_EXISTS_NAME>(oldWorkspace.ID, oldWorkspace.BasicInfo.StatusID, newWorkspace);
                            break;
                        case SCC_BL.Results.Workspace.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Workspace.Update.Error>(oldWorkspace.ID, oldWorkspace.BasicInfo.StatusID, newWorkspace);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Workspace.Update.Error>(oldWorkspace.ID, oldWorkspace.BasicInfo.StatusID, oldWorkspace, ex);
            }

            return RedirectToAction(nameof(WorkspaceController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(WorkspaceManagementViewModel workspaceManagementViewModel)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_WORKSPACES))
            {
                SaveProcessingInformation<SCC_BL.Results.Workspace.Insert.NotAllowedToCreateWorkspaces>();
                return RedirectToAction(nameof(WorkspaceController.Manage), GetControllerName(typeof(WorkspaceController)));
            }

            Workspace newWorkspace = new Workspace(workspaceManagementViewModel.Workspace.Identifier, workspaceManagementViewModel.Workspace.Name, workspaceManagementViewModel.Workspace.Monitorable, currentUser.ID, (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.CREATED);

            try
            {
                int result = newWorkspace.Insert();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Workspace.Insert.Success>(newWorkspace.ID, newWorkspace.BasicInfo.StatusID, newWorkspace);
                }
                else
                {
                    switch ((SCC_BL.Results.Workspace.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Workspace.Insert.CODE.ALREADY_EXISTS_IDENTIFIER:
                            SaveProcessingInformation<SCC_BL.Results.Workspace.Insert.ALREADY_EXISTS_IDENTIFIER>(null, null, newWorkspace);
                            break;
                        case SCC_BL.Results.Workspace.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Workspace.Insert.ALREADY_EXISTS_NAME>(null, null, newWorkspace);
                            break;
                        case SCC_BL.Results.Workspace.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Workspace.Insert.Error>(null, null, newWorkspace);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Workspace.Insert.Error>(null, null, null, ex);
            }

            return RedirectToAction(nameof(WorkspaceController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int workspaceID)
        {
            User currentUser = GetCurrentUser();

            if (!currentUser.HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_DELETE_WORKSPACES))
            {
                SaveProcessingInformation<SCC_BL.Results.Workspace.Delete.NotAllowedToDeleteWorkspaces>();
                return RedirectToAction(nameof(WorkspaceController.Manage), GetControllerName(typeof(WorkspaceController)));
            }

            Workspace workspace = new Workspace(workspaceID);
            workspace.SetDataByID();

            try
            {
                //workspace.Delete();

                workspace.BasicInfo.ModificationUserID = currentUser.ID;
                workspace.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DELETED;

                int result = workspace.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Workspace.Delete.Success>(workspace.ID, workspace.BasicInfo.StatusID, workspace);

                    return RedirectToAction(nameof(WorkspaceController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Workspace.Delete.Success>(workspace.ID, workspace.BasicInfo.StatusID, workspace, ex);
            }

            return RedirectToAction(nameof(WorkspaceController.Manage), _mainControllerName);
        }
    }
}