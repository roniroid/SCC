using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class RoleController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(RoleController));

        public ActionResult Manage(int? roleID, bool filterActiveElements = false)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_ROLES) && !GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_ROLES))
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Manage.NotAllowedToSeeOrCreateRoles>();
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
            }
            
            RoleManagementViewModel roleManagementViewModel = new RoleManagementViewModel();

            if (roleID != null)
            {
                roleManagementViewModel.Role = new Role(roleID.Value);
                roleManagementViewModel.Role.SetDataByID();
            }

            List<Permission> permissionList = new List<Permission>();

            using (Permission permission = new Permission())
                permissionList =
                    permission.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PERMISSION.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PERMISSION.DISABLED)
                        .OrderBy(e => e.Description)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Role.Manage.PermissionList.NAME] =
                new MultiSelectList(
                    permissionList,
                    SCC_BL.Settings.AppValues.ViewData.Role.Manage.PermissionList.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.Role.Manage.PermissionList.SelectList.TEXT,
                    roleManagementViewModel.Role.PermissionList.Select(s => s.PermissionID));

            roleManagementViewModel.RoleList = new Role().SelectAll();

            if (filterActiveElements)
                roleManagementViewModel.RoleList =
                    roleManagementViewModel.RoleList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DISABLED)
                        .ToList();

            return View(roleManagementViewModel);
        }

        void UpdatePermissionList(Role role, int[] permissionList)
        {
            try
            {
                switch (role.UpdatePermissionList(permissionList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Role.UpdatePermissionList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Role.UpdatePermissionList.Success>(role.ID, role.BasicInfo.StatusID, role);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Role.UpdatePermissionList.Error>(role.ID, role.BasicInfo.StatusID, role, ex);
            }

        }

        [HttpPost]
        public ActionResult Edit(RoleManagementViewModel roleManagementViewModel, int[] permissionList)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_ROLES))
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Update.NotAllowedToModifyRoles>();
                return RedirectToAction(nameof(RoleController.Manage), GetControllerName(typeof(RoleController)));
            }

            Role oldRole = new Role(roleManagementViewModel.Role.ID);
            oldRole.SetDataByID();

            Role newRole = new Role(roleManagementViewModel.Role.ID, roleManagementViewModel.Role.Identifier, roleManagementViewModel.Role.Name, roleManagementViewModel.Role.BasicInfoID, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.UPDATED);
            try
            {
                int result = newRole.Update();

                if (result > 0)
                {
                    if (GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_ASIGN_PERMISSIONS_TO_ROLES))
                    {
                        if (permissionList == null)
                            permissionList = new int[0];

                        UpdatePermissionList(oldRole, permissionList);
                    }
                    else
                        SaveProcessingInformation<SCC_BL.Results.Role.Update.NotAllowedToChangePermissions>(newRole.ID, newRole.BasicInfo.StatusID, oldRole);

                    SaveProcessingInformation<SCC_BL.Results.Role.Update.Success>(newRole.ID, newRole.BasicInfo.StatusID, oldRole);
                }
                else
                {
                    switch ((SCC_BL.Results.Role.Update.CODE)result)
                    {
                        case SCC_BL.Results.Role.Update.CODE.ALREADY_EXISTS_IDENTIFIER:
                            SaveProcessingInformation<SCC_BL.Results.Role.Update.ALREADY_EXISTS_IDENTIFIER>(oldRole.ID, oldRole.BasicInfo.StatusID, newRole);
                            break;
                        case SCC_BL.Results.Role.Update.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Role.Update.ALREADY_EXISTS_NAME>(oldRole.ID, oldRole.BasicInfo.StatusID, newRole);
                            break;
                        case SCC_BL.Results.Role.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Role.Update.Error>(oldRole.ID, oldRole.BasicInfo.StatusID, newRole);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Update.Error>(oldRole.ID, oldRole.BasicInfo.StatusID, oldRole, ex);
            }

            return RedirectToAction(nameof(RoleController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(RoleManagementViewModel roleManagementViewModel, int[] permissionList)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_ROLES))
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Insert.NotAllowedToCreateRoles>();
                return RedirectToAction(nameof(RoleController.Manage), GetControllerName(typeof(RoleController)));
            }

            Role newRole = new Role(roleManagementViewModel.Role.Identifier, roleManagementViewModel.Role.Name, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.CREATED);
            try
            {
                int result = newRole.Insert();

                if (result > 0)
                {
                    if (GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_ASIGN_PERMISSIONS_TO_ROLES))
                    {
                        if (permissionList == null)
                            permissionList = new int[0];

                        UpdatePermissionList(newRole, permissionList);
                    }
                    else
                        SaveProcessingInformation<SCC_BL.Results.Role.Insert.NotAllowedToChangePermissions>(newRole.ID, newRole.BasicInfo.StatusID, newRole);

                    SaveProcessingInformation<SCC_BL.Results.Role.Insert.Success>(newRole.ID, newRole.BasicInfo.StatusID, newRole);
                }
                else
                {
                    switch ((SCC_BL.Results.Role.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Role.Insert.CODE.ALREADY_EXISTS_IDENTIFIER:
                            SaveProcessingInformation<SCC_BL.Results.Role.Insert.ALREADY_EXISTS_IDENTIFIER>(null, null, newRole);
                            break;
                        case SCC_BL.Results.Role.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Role.Insert.ALREADY_EXISTS_NAME>(null, null, newRole);
                            break;
                        case SCC_BL.Results.Role.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Role.Insert.Error>(null, null, newRole);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Insert.Error>(null, null, newRole, ex);
            }

            return RedirectToAction(nameof(RoleController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int roleID)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_DELETE_ROLES))
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Delete.NotAllowedToDeleteRoles>();
                return RedirectToAction(nameof(RoleController.Manage), GetControllerName(typeof(RoleController)));
            }

            Role role = new Role(roleID);
            role.SetDataByID();

            try
            {
                //role.Delete();

                role.BasicInfo.ModificationUserID = GetActualUser().ID;
                role.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DELETED;

                int result = role.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Role.Delete.Success>(role.ID, role.BasicInfo.StatusID, role);

                    return RedirectToAction(nameof(RoleController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Role.Delete.Success>(role.ID, role.BasicInfo.StatusID, role, ex);
            }

            return RedirectToAction(nameof(RoleController.Manage), _mainControllerName);
        }
    }
}