using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class GroupController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(GroupController));

        public ActionResult Manage(int? groupID, bool filterActiveElements = false)
        {
            GroupManagementViewModel groupManagementViewModel = new GroupManagementViewModel();

            if (groupID != null)
            {
                groupManagementViewModel.Group = new Group(groupID.Value);
                groupManagementViewModel.Group.SetDataByID();
            }

            List<User> userList = new List<User>();
            List<Catalog> moduleList = new List<Catalog>();

            using (User user = new User())
                userList =
                    user.SelectAll(true)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.MODULE))
                moduleList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Group.Manage.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    groupManagementViewModel.Group.UserList.Select(s => s.UserID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Group.Manage.Module.NAME] =
                new SelectList(
                    moduleList,
                    SCC_BL.Settings.AppValues.ViewData.Group.Manage.Module.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.Group.Manage.Module.SelectList.TEXT);

            groupManagementViewModel.GroupList = new Group().SelectAll();

            if (filterActiveElements)
                groupManagementViewModel.GroupList =
                    groupManagementViewModel.GroupList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DISABLED)
                        .ToList();

            return View(groupManagementViewModel);
        }

        void UpdateUserList(Group group, int[] userList)
        {
            try
            {
                switch (group.UpdateUserList(userList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Group.UpdateUserList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Group.UpdateUserList.Success>(group.ID, group.BasicInfo.StatusID, group);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Group.UpdateUserList.Error>(group.ID, group.BasicInfo.StatusID, group, ex);
            }
        }

        [HttpPost]
        public ActionResult Edit(GroupManagementViewModel groupManagementViewModel, int[] userList)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_GROUPS))
            {
                SaveProcessingInformation<SCC_BL.Results.Group.Update.NotAllowedToCreateGroups>();
                return RedirectToAction(nameof(GroupController.Manage), GetControllerName(typeof(GroupController)));
            }

            Group oldGroup = new Group(groupManagementViewModel.Group.ID);
            oldGroup.SetDataByID();

            Group newGroup = new Group(groupManagementViewModel.Group.ID, groupManagementViewModel.Group.Name, groupManagementViewModel.Group.ApplicableModuleID, groupManagementViewModel.Group.BasicInfoID, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.UPDATED);

            try
            {
                int result = newGroup.Update();

                if (result > 0)
                {
                    UpdateUserList(oldGroup, userList);

                    SaveProcessingInformation<SCC_BL.Results.Group.Update.Success>(newGroup.ID, newGroup.BasicInfo.StatusID, oldGroup);
                }
                else
                {
                    switch ((SCC_BL.Results.Group.Update.CODE)result)
                    {
                        case SCC_BL.Results.Group.Update.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Group.Update.ALREADY_EXISTS_NAME>(oldGroup.ID, oldGroup.BasicInfo.StatusID, newGroup);
                            break;
                        case SCC_BL.Results.Group.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Group.Update.Error>(oldGroup.ID, oldGroup.BasicInfo.StatusID, newGroup);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Group.Update.Error>(oldGroup.ID, oldGroup.BasicInfo.StatusID, oldGroup, ex);
            }

            return RedirectToAction(nameof(GroupController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(GroupManagementViewModel groupManagementViewModel, int[] userList)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_GROUPS))
            {
                SaveProcessingInformation<SCC_BL.Results.Group.Insert.NotAllowedToCreateGroups>();
                return RedirectToAction(nameof(GroupController.Manage), GetControllerName(typeof(GroupController)));
            }

            Group newGroup = new Group(groupManagementViewModel.Group.Name, groupManagementViewModel.Group.ApplicableModuleID, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.CREATED);

            try
            {
                int result = newGroup.Insert();

                if (result > 0)
                {
                    UpdateUserList(newGroup, userList);

                    SaveProcessingInformation<SCC_BL.Results.Group.Insert.Success>(newGroup.ID, newGroup.BasicInfo.StatusID, newGroup);
                }
                else
                {
                    switch ((SCC_BL.Results.Group.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Group.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.Group.Insert.ALREADY_EXISTS_NAME>(null, null, newGroup);
                            break;
                        case SCC_BL.Results.Group.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Group.Insert.Error>(null, null, newGroup);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Group.Insert.Error>(null, null, newGroup, ex);
            }

            return RedirectToAction(nameof(GroupController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int groupID)
        {
            Group group = new Group(groupID);
            group.SetDataByID();

            try
            {
                //group.Delete();

                group.BasicInfo.ModificationUserID = GetActualUser().ID;
                group.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DELETED;

                int result = group.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Group.Delete.Success>(group.ID, group.BasicInfo.StatusID, group);

                    return RedirectToAction(nameof(GroupController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Group.Delete.Success>(group.ID, group.BasicInfo.StatusID, group, ex);
            }

            return RedirectToAction(nameof(GroupController.Manage), _mainControllerName);
        }
    }
}