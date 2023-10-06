using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
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
    public class UserController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(UserController));

        public ActionResult Manage(int? userID, bool filterActiveElements = false)
        {
            UserManagementViewModel userManagementViewModel = new UserManagementViewModel();

            if (userID != null)
            {
                userManagementViewModel.User = new User(userID.Value);
                userManagementViewModel.User.SetDataByID();

                userManagementViewModel.Person = new Person(userManagementViewModel.User.PersonID);
                userManagementViewModel.Person.SetDataByID();
            }

            List<Catalog> languageList = new List<Catalog>();
            List<Catalog> countryList = new List<Catalog>();
            List<User> supervisorList = new List<User>();
            List<Workspace> workspaceList = new List<Workspace>();
            List<Role> roleList = new List<Role>();
            List<Group> groupList = new List<Group>();
            List<Program> programList = new List<Program>();

            //Starts filling all data

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.USER_LANGUAGE))
                languageList = catalog.SelectByCategoryID();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.PERSON_COUNTRY))
                countryList = catalog.SelectByCategoryID();

            using (User user = new User())
                supervisorList = user.SelectAll(true);

            using (Workspace workspace = new Workspace())
                workspaceList = workspace.SelectAll();

            using (Role role = new Role())
                roleList = role.SelectAll();

            using (Group group = new Group())
                groupList = group.SelectAll();

            using (Program program = new Program())
                programList = program.SelectAll();

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.LanguageCatalog.NAME] = languageList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.CountryCatalog.NAME] = countryList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.Supervisor.NAME] = supervisorList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.Workspace.NAME] = workspaceList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.RoleCatalog.NAME] = roleList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.Group.NAME] = groupList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.AllData.Program.NAME] = programList;

            //Ends filling all data

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.USER_LANGUAGE))
                languageList =
                    languageList
                        .Where(e => e.Active)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.PERSON_COUNTRY))
                countryList =
                    countryList
                        .Where(e => e.Active)
                        .ToList();

            using (User user = new User())
                supervisorList =
                    supervisorList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (Workspace workspace = new Workspace())
                workspaceList =
                    workspaceList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DISABLED)
                        .ToList();

            using (Role role = new Role())
                roleList =
                    roleList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DISABLED)
                        .ToList();

            using (Group group = new Group())
                groupList =
                    groupList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DISABLED)
                        .ToList();

            using (Program program = new Program())
                programList =
                    programList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.LanguageCatalog.NAME] =
                new SelectList(
                    languageList,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.LanguageCatalog.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.LanguageCatalog.SelectList.TEXT,
                    userManagementViewModel.User.LanguageID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.CountryCatalog.NAME] =
                new SelectList(
                    countryList,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.CountryCatalog.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.CountryCatalog.SelectList.TEXT,
                    userManagementViewModel.Person.CountryID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.Supervisor.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    userManagementViewModel.User.SupervisorList.Select(s => s.SupervisorID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.Workspace.NAME] =
                new MultiSelectList(
                    workspaceList,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.Workspace.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.Workspace.SelectList.TEXT,
                    userManagementViewModel.User.UserWorkspaceCatalogList.Select(s => s.WorkspaceID));

            if (
                !GetActualUser().RoleList.Select(e => e.RoleID).Contains((int)SCC_BL.DBValues.Catalog.USER_ROLE.ADMINISTRATOR) &&
                !GetActualUser().RoleList.Select(e => e.RoleID).Contains((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERUSER))
            {
                roleList =
                    roleList
                        .Where(e => e.ID != (int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERUSER)
                        .OrderBy(e => e.Name)
                        .ToList();
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.RoleCatalog.NAME] =
                new MultiSelectList(
                    roleList,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.RoleCatalog.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.RoleCatalog.SelectList.TEXT,
                    userManagementViewModel.User.RoleList.Select(s => s.RoleID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.Group.NAME] =
                new MultiSelectList(
                    groupList,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.Group.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.Group.SelectList.TEXT,
                    userManagementViewModel.User.GroupList.Select(s => s.GroupID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.Edit.Program.NAME] =
                new MultiSelectList(
                    programList,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.Program.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.Edit.Program.SelectList.TEXT,
                    userManagementViewModel.User.ProgramList.Select(s => s.ProgramID));

            userManagementViewModel.UserList = new User().SelectAll();

            if (filterActiveElements)
                userManagementViewModel.UserList = 
                    userManagementViewModel.UserList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            return View(userManagementViewModel);
        }

        public ActionResult AsignRolesAndPermissions(int? userID = null)
        {
            UserRoleAndPermissionManagementViewModel userRoleAndPermissionManagementViewModel = new UserRoleAndPermissionManagementViewModel();

            List<Permission> permissionList = new List<Permission>();
            List<Role> roleList = new List<Role>();
            List<User> userList = new List<User>();

            if (userID != null)
            {
                userRoleAndPermissionManagementViewModel.User = new User(userID.Value);
                userRoleAndPermissionManagementViewModel.User.SetDataByID();
            }

            using (Permission permission = new Permission())
                permissionList =
                    permission.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PERMISSION.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PERMISSION.DISABLED)
                        .OrderBy(e => e.Description)
                        .ToList();

            using (Role role = new Role())
                roleList =
                    role.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DISABLED)
                        .OrderBy(e => e.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll(true)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.AsignRolesAndPermissions.PermissionList.NAME] =
                new MultiSelectList(
                    permissionList,
                    nameof(Permission.ID),
                    nameof(Permission.Description),
                    userRoleAndPermissionManagementViewModel.User.PermissionList.Select(s => s.PermissionID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.AsignRolesAndPermissions.RoleList.NAME] =
                new MultiSelectList(
                    roleList,
                    nameof(Role.ID),
                    nameof(Role.Name),
                    userRoleAndPermissionManagementViewModel.User.RoleList.Select(s => s.RoleID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.AsignRolesAndPermissions.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    userID != null
                        ? userList.Where(e => e.ID == userID.Value).Select(e => e.ID)
                        : null);

            using (User user = new SCC_BL.User())
            {
                userRoleAndPermissionManagementViewModel.UserList =
                    user.SelectAll()
                        .Where(e => e.RoleList.Count > 0 || e.PermissionList.Count > 0)
                        .ToList();
            }

            return View(userRoleAndPermissionManagementViewModel);
        }

        [HttpPost]
        public ActionResult AsignRolesAndPermissions(int[] permissionArray, int[] roleArray, int[] userArray)
        {
            try
            {
                User tempUser = new User();

                List<Permission> permissionList = new List<Permission>();
                List<Role> roleList = new List<Role>();

                if (userArray != null)
                {
                    for (int i = 0; i < userArray.Length; i++)
                    {
                        tempUser = new User(userArray[i]);
                        tempUser.SetDataByID();

                        roleArray =
                            roleArray == null
                                ? new int[0]
                                : roleArray;

                        permissionArray =
                            permissionArray == null
                                ? new int[0]
                                : permissionArray;

                        tempUser.UpdateRoleList(roleArray, GetActualUser().ID);
                        //tempUser.UpdateRoleList(roleArray, GetActualUser().ID, true);

                        if (GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_ASIGN_PERMISSIONS_TO_USERS))
                        {
                            tempUser.UpdatePermissionList(permissionArray, GetActualUser().ID);
                            //tempUser.UpdatePermissionList(permissionArray, GetActualUser().ID, true);
                        }
                        else
                            SaveProcessingInformation<SCC_BL.Results.User.Insert.NotAllowedToChangePermissions>(tempUser.ID, tempUser.BasicInfo.StatusID, tempUser);

                        SaveProcessingInformation<SCC_BL.Results.User.AsignRolesAndPermissions.Success>(null, null, tempUser);
                    }
                }
                else
                {
                    throw new Exception("No se ingresaron usuarios");
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.AsignRolesAndPermissions.Error>(ex);
            }

            return RedirectToAction(nameof(UserController.AsignRolesAndPermissions), _mainControllerName);
        }

        public ActionResult AsignProgramsAndProgramGroups(int? userID = null)
        {
            UserProgramAndProgramGroupManagementViewModel userProgramAndProgramGroupManagementViewModel = new UserProgramAndProgramGroupManagementViewModel();

            List<Program> programList = new List<Program>();
            List<ProgramGroup> programGroupList = new List<ProgramGroup>();
            List<User> userList = new List<User>();

            if (userID != null)
            {
                userProgramAndProgramGroupManagementViewModel.User = new User(userID.Value);
                userProgramAndProgramGroupManagementViewModel.User.SetDataByID();
            }

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(e => e.Name)
                        .ToList();

            using (ProgramGroup programGroup = new ProgramGroup())
                programGroupList =
                    programGroup.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.DISABLED)
                        .OrderBy(e => e.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll(true)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.AsignProgramsAndProgramGroups.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    userProgramAndProgramGroupManagementViewModel.User.ProgramList.Select(s => s.ProgramID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.AsignProgramsAndProgramGroups.ProgramGroupList.NAME] =
                new MultiSelectList(
                    programGroupList,
                    nameof(ProgramGroup.ID),
                    nameof(ProgramGroup.Name),
                    userProgramAndProgramGroupManagementViewModel.User.ProgramGroupList.Select(s => s.ProgramGroupID));

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.AsignProgramsAndProgramGroups.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    userID != null
                        ? userList.Where(e => e.ID == userID.Value).Select(e => e.ID)
                        : null);

            using (User user = new SCC_BL.User())
            {
                userProgramAndProgramGroupManagementViewModel.UserList =
                    user.SelectAll()
                        .Where(e => e.ProgramList.Count > 0 || e.ProgramGroupList.Count > 0)
                        .ToList();
            }

            return View(userProgramAndProgramGroupManagementViewModel);
        }

        [HttpPost]
        public ActionResult AsignProgramsAndProgramGroups(int[] programArray, int[] programGroupArray, int[] userArray)
        {
            try
            {
                User tempUser = new User();

                List<Program> programList = new List<Program>();
                List<ProgramGroup> programGroupList = new List<ProgramGroup>();

                if (userArray != null)
                {
                    for (int i = 0; i < userArray.Length; i++)
                    {
                        tempUser = new User(userArray[i]);
                        tempUser.SetDataByID();

                        programArray =
                            programArray == null
                                ? new int[0]
                                : programArray;

                        programGroupArray =
                            programGroupArray == null
                                ? new int[0]
                                : programGroupArray;

                        tempUser.UpdateProgramList(programArray, GetActualUser().ID);
                        tempUser.UpdateProgramGroupList(programGroupArray, GetActualUser().ID);

                        SaveProcessingInformation<SCC_BL.Results.User.AsignProgramsAndProgramGroups.Success>(null, null, tempUser);
                    }
                }
                else
                {
                    throw new Exception("No se ingresaron usuarios");
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.AsignProgramsAndProgramGroups.Error>(ex);
            }

            return RedirectToAction(nameof(UserController.AsignProgramsAndProgramGroups), _mainControllerName);
        }

        public ActionResult NotificationMatrix()
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CHANGE_NOTIFICATION_ALARMS))
            {
                SaveProcessingInformation<SCC_BL.Results.NotificationMatrix.Manage.NotAllowedToChangeNotificationAlarms>();
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
            }

            NotificationMatrixViewModel notificationMatrixViewModel = new NotificationMatrixViewModel();

            List<NotificationMatrix> notificationMatrixList = new NotificationMatrix().GetAllNotificationMatrixList();

            notificationMatrixViewModel.DirectSupervisorTransactionDispute =
                notificationMatrixList
                    .Where(e => 
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE)
                    .Count() > 0;

            notificationMatrixViewModel.DirectSupervisorTransactionDevolution =
                notificationMatrixList
                    .Where(e => 
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION)
                    .Count() > 0;

            notificationMatrixViewModel.DirectSupervisorTransactionInvalidation =
                notificationMatrixList
                    .Where(e => 
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION)
                    .Count() > 0;

            notificationMatrixViewModel.DirectSupervisorTransactionConfirmation =
                notificationMatrixList
                    .Where(e => 
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION)
                    .Count() > 0;

            notificationMatrixViewModel.DirectSupervisorTeamMonitoringZero =
                notificationMatrixList
                    .Where(e => 
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO)
                    .Count() > 0;

            notificationMatrixViewModel.DirectSupervisorCalibration =
                notificationMatrixList
                    .Where(e => 
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION)
                    .Count() > 0;

            notificationMatrixViewModel.IndirectSupervisorTransactionDispute =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE)
                    .Count() > 0;

            notificationMatrixViewModel.IndirectSupervisorTransactionDevolution =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION)
                    .Count() > 0;

            notificationMatrixViewModel.IndirectSupervisorTransactionInvalidation =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION)
                    .Count() > 0;

            notificationMatrixViewModel.IndirectSupervisorTransactionConfirmation =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION)
                    .Count() > 0;

            notificationMatrixViewModel.IndirectSupervisorTeamMonitoringZero =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO)
                    .Count() > 0;

            notificationMatrixViewModel.IndirectSupervisorCalibration =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoredUserTransactionDispute =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoredUserTransactionDevolution =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoredUserTransactionInvalidation =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoredUserTransactionConfirmation =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoredUserTeamMonitoringZero =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoredUserCalibration =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoringUserTransactionDispute =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoringUserTransactionDevolution =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoringUserTransactionInvalidation =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoringUserTransactionConfirmation =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoringUserTeamMonitoringZero =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO)
                    .Count() > 0;

            notificationMatrixViewModel.MonitoringUserCalibration =
                notificationMatrixList
                    .Where(e =>
                        e.EntityID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER &&
                        e.ActionID == (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION)
                    .Count() > 0;

            return View(notificationMatrixViewModel);
        }

        [HttpPost]
        public ActionResult NotificationMatrix(NotificationMatrixViewModel notificationMatrixViewModel)
        {
            try
            {
                NotificationMatrix notificationMatrix = new NotificationMatrix();

                notificationMatrix.DeleteAll();

                if (notificationMatrixViewModel.DirectSupervisorTransactionDispute) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE).Insert(); }
                if (notificationMatrixViewModel.DirectSupervisorTransactionDevolution) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION).Insert(); }
                if (notificationMatrixViewModel.DirectSupervisorTransactionInvalidation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION).Insert(); }
                if (notificationMatrixViewModel.DirectSupervisorTransactionConfirmation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION).Insert(); }
                if (notificationMatrixViewModel.DirectSupervisorTeamMonitoringZero) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO).Insert(); }
                if (notificationMatrixViewModel.DirectSupervisorCalibration) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION).Insert(); }

                if (notificationMatrixViewModel.IndirectSupervisorTransactionDispute) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE).Insert(); }
                if (notificationMatrixViewModel.IndirectSupervisorTransactionDevolution) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION).Insert(); }
                if (notificationMatrixViewModel.IndirectSupervisorTransactionInvalidation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION).Insert(); }
                if (notificationMatrixViewModel.IndirectSupervisorTransactionConfirmation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION).Insert(); }
                if (notificationMatrixViewModel.IndirectSupervisorTeamMonitoringZero) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO).Insert(); }
                if (notificationMatrixViewModel.IndirectSupervisorCalibration) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION).Insert(); }

                if (notificationMatrixViewModel.MonitoredUserTransactionDispute) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE).Insert(); }
                if (notificationMatrixViewModel.MonitoredUserTransactionDevolution) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION).Insert(); }
                if (notificationMatrixViewModel.MonitoredUserTransactionInvalidation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION).Insert(); }
                if (notificationMatrixViewModel.MonitoredUserTransactionConfirmation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION).Insert(); }
                if (notificationMatrixViewModel.MonitoredUserTeamMonitoringZero) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO).Insert(); }
                if (notificationMatrixViewModel.MonitoredUserCalibration) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION).Insert(); }

                if (notificationMatrixViewModel.MonitoringUserTransactionDispute) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE).Insert(); }
                if (notificationMatrixViewModel.MonitoringUserTransactionDevolution) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION).Insert(); }
                if (notificationMatrixViewModel.MonitoringUserTransactionInvalidation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION).Insert(); }
                if (notificationMatrixViewModel.MonitoringUserTransactionConfirmation) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_CONFIRMATION).Insert(); }
                if (notificationMatrixViewModel.MonitoringUserTeamMonitoringZero) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TEAM_MONITORING_WITH_SCORE_ZERO).Insert(); }
                if (notificationMatrixViewModel.MonitoringUserCalibration) { new NotificationMatrix((int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER, (int)SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION).Insert(); }

                SaveProcessingInformation<SCC_BL.Results.NotificationMatrix.Insert.Success>(null, null, notificationMatrixViewModel);
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.NotificationMatrix.Insert.Error>(null, null, notificationMatrixViewModel, ex);
            }

            return RedirectToAction(nameof(UserController.NotificationMatrix), _mainControllerName);
        }

        public ActionResult Edit(int userID)
        {
            return View();
        }

        void UpdateRoleList(User user, int[] roleList)
        {
            try
            {
                switch (user.UpdateRoleList(roleList != null ? roleList : new int[0], GetActualUser().ID))
                {
                    case SCC_BL.Results.User.UpdateRoleList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.User.UpdateRoleList.Success>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.UpdateRoleList.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

        }

        void UpdateSupervisorList(User user, int[] supervisorList, DateTime startDate)
        {
            try
            {
                switch (user.UpdateSupervisorList(supervisorList != null ? supervisorList : new int[0], startDate, GetActualUser().ID))
                {
                    case SCC_BL.Results.User.UpdateSupervisorList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.User.UpdateSupervisorList.Success>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.UpdateSupervisorList.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

        }

        void UpdateWorkspaceList(User user, int[] workspaceList, DateTime startDate)
        {
            try
            {
                switch (user.UpdateWorkspaceList(workspaceList != null ? workspaceList : new int[0], startDate, GetActualUser().ID))
                {
                    case SCC_BL.Results.User.UpdateWorkspaceList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.User.UpdateWorkspaceList.Success>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.UpdateWorkspaceList.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

        }

        void UpdateGroupList(User user, int[] groupList)
        {
            try
            {
                switch (user.UpdateGroupList(groupList != null ? groupList : new int[0], GetActualUser().ID))
                {
                    case SCC_BL.Results.User.UpdateGroupList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.User.UpdateGroupList.Success>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.UpdateGroupList.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

        }

        void UpdateProgramList(User user, int[] programList)
        {
            try
            {
                switch (user.UpdateProgramList(programList != null ? programList : new int[0], GetActualUser().ID))
                {
                    case SCC_BL.Results.User.UpdateProgramList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.User.UpdateProgramList.Success>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.UpdateProgramList.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

        }

        [HttpPost]
        public ActionResult Edit(UserPersonViewModel userPerson, int? userStatus, int[] supervisorList, DateTime? supervisorStartDate, int[] workspaceList, DateTime? workspaceStartDate, int[] roleList, int[] groupList, int[] programList)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_USERS))
            {
                SaveProcessingInformation<SCC_BL.Results.User.Update.NotAllowedToModifyUsers>();
                return RedirectToAction(nameof(UserController.Manage), GetControllerName(typeof(UserController)));
            }

            if (userPerson.User.StartDate == null) userPerson.User.StartDate = DateTime.Now;
            if (supervisorStartDate == null) supervisorStartDate = DateTime.Now;
            if (workspaceStartDate == null) workspaceStartDate = DateTime.Now;

            Person oldPerson = new Person(userPerson.Person.ID);
            oldPerson.SetDataByID();

            Person newPerson = new Person(
                userPerson.Person.ID, 
                userPerson.Person.Identification, 
                userPerson.Person.FirstName, 
                userPerson.Person.SurName,
                userPerson.Person.CountryID, 
                userPerson.Person.BasicInfoID, 
                GetActualUser().ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_PERSON.UPDATED);
            try
            {
                int result = newPerson.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Person.Update.Success>(newPerson.ID, newPerson.BasicInfo.StatusID, oldPerson);
                }
                else
                {
                    switch ((SCC_BL.Results.Person.Update.CODE)result)
                    {
                        case SCC_BL.Results.Person.Update.CODE.ALREADY_EXISTS:
                            Person foundPerson = new Person(newPerson.Identification);
                            foundPerson.SetDataByIdentification();

                            SaveProcessingInformation<SCC_BL.Results.Person.Update.AlreadyExists>(foundPerson.ID, foundPerson.BasicInfo.StatusID, oldPerson);
                            break;
                        default:
                            break;
                    }
                }

                if (result > 0)
                {
                    User oldUser = new User(userPerson.User.ID);
                    oldUser.SetDataByID();

                    //int newStatusID = userStatus != null ? userStatus.Value : oldUser.BasicInfo.StatusID;
                    int newStatusID = userStatus != null ? userStatus.Value : (int)SCC_BL.DBValues.Catalog.STATUS_USER.UPDATED;

                    /*
                        TEMP: 
                    */
                    userPerson.User.Username = newPerson.Identification;
                    /*
                        TEMP: 
                    */

                    User newUser = new User(userPerson.User.ID, userPerson.User.Username, userPerson.User.Email, userPerson.User.StartDate, userPerson.User.LanguageID, userPerson.User.HasPassPermission, userPerson.User.BasicInfoID, GetActualUser().ID, newStatusID);

                    try
                    {
                        result = newUser.Update();

                        if (result > 0)
                        {
                            UpdateSupervisorList(oldUser, supervisorList != null ? supervisorList : new int[0], supervisorStartDate.Value);
                            
                            UpdateWorkspaceList(oldUser, workspaceList != null ? workspaceList : new int[0], workspaceStartDate.Value);

                            UpdateRoleList(oldUser, roleList != null ? roleList : new int[0]);

                            UpdateGroupList(oldUser, groupList != null ? groupList : new int[0]);

                            UpdateProgramList(oldUser, programList != null ? programList : new int[0]);

                            SaveProcessingInformation<SCC_BL.Results.User.Update.Success>(newUser.ID, oldUser.BasicInfo.StatusID, oldUser);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.User.Update.CODE)result)
                            {
                                case SCC_BL.Results.User.Update.CODE.ALREADY_EXISTS:
                                    /*User foundUser = new User(newUser.Username);
                                    foundUser.SetDataByID();

                                    SaveProcessingInformation<SCC_BL.Results.User.Update.AlreadyExists>(foundUser.ID, foundUser.BasicInfo.StatusID, oldUser);*/

                                    SaveProcessingInformation<SCC_BL.Results.User.Update.AlreadyExists>(newUser.ID, (int)SCC_BL.DBValues.Catalog.STATUS_USER.UPDATED, oldUser);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.Update.Error>(newUser.ID, oldUser.BasicInfo.StatusID, oldUser, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Person.Update.Error>(newPerson.ID, oldPerson.BasicInfo.StatusID, oldPerson, ex);
            }

            return RedirectToAction(nameof(UserController.Manage), _mainControllerName);
        }

        void SendMail(SCC_BL.Settings.AppValues.MailTopic mailTopic, User user, string password = null, string customURL = null)
        {
            //TEMPORARILY DISABLED
            Mail mailHelper = new Mail();

            string url = customURL ?? string.Empty;
            string messageBody = string.Empty;

            switch (mailTopic)
            {
                case SCC_BL.Settings.AppValues.MailTopic.FORGOTTEN_PASSWORD:
                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.User.PasswordRecovery.PATH).Replace("%url%", url);

                    try
                    {
                        mailHelper.SendMail(
                            user.GetEmailByUsername(),
                            messageBody,
                            SCC_BL.Settings.HTML_Content.User.PasswordRecovery.SUBJECT);

                        SaveProcessingInformation<SCC_BL.Results.User.ForgottenPassword.Success>(null, null, user);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.ForgottenPassword.Error>(null, null, ex);
                        throw ex;
                    }
                    break;
                case SCC_BL.Settings.AppValues.MailTopic.USER_CREATION:
                    url = Url.Action(nameof(UserController.LogIn), _mainControllerName, null, Request.Url.Scheme);

                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.User.SignIn.PATH).Replace("%url%", url);
                    messageBody = messageBody.Replace("%username%", user.Username);
                    messageBody = messageBody.Replace("%password%", password);

                    try
                    {
                        mailHelper.SendMail(
                            user.GetEmailByUsername(),
                            messageBody,
                            SCC_BL.Settings.HTML_Content.User.SignIn.SUBJECT);

                        SaveProcessingInformation<SCC_BL.Results.User.SignIn.SendMail.Success>(user.ID, user.BasicInfo.StatusID, user);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.SignIn.SendMail.Error>(null, null, ex);
                        throw ex;
                    }
                    break;
                case SCC_BL.Settings.AppValues.MailTopic.CHANGE_PASSWORD:
                    url = Url.Action(nameof(UserController.LogIn), _mainControllerName, null, Request.Url.Scheme);

                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.User.ChangePassword.PATH).Replace("%url%", url);
                    messageBody = messageBody.Replace("%username%", user.Username);
                    messageBody = messageBody.Replace("%password%", password);

                    try
                    {
                        mailHelper.SendMail(
                            user.GetEmailByUsername(),
                            messageBody,
                            SCC_BL.Settings.HTML_Content.User.ChangePassword.SUBJECT);

                        SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.SendMail.Success>(user.ID, user.BasicInfo.StatusID, user);
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.SendMail.Error>(user.ID, user.BasicInfo.StatusID, ex);
                        throw ex;
                    }
                    break;
                default:
                    break;
            }
        }

        [HttpPost]
        public ActionResult Create(UserPersonViewModel userPerson, int[] supervisorList, DateTime? supervisorStartDate, int[] workspaceList, DateTime? workspaceStartDate, int[] roleList, int[] groupList, int[] programList)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_USERS))
            {
                SaveProcessingInformation<SCC_BL.Results.User.Insert.NotAllowedToCreateUsers>();
                return RedirectToAction(nameof(UserController.Manage), GetControllerName(typeof(UserController)));
            }

            if (userPerson.User.StartDate == null) userPerson.User.StartDate = DateTime.Now;
            if (supervisorStartDate == null) supervisorStartDate = DateTime.Now;
            if (workspaceStartDate == null) workspaceStartDate = DateTime.Now;

            Person newPerson = new Person(
                userPerson.Person.Identification, 
                userPerson.Person.FirstName, 
                userPerson.Person.SurName, 
                userPerson.Person.CountryID, 
                GetActualUser().ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_PERSON.CREATED);

            try
            {
                int result = newPerson.Insert();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Person.Insert.Success>(newPerson.ID, newPerson.BasicInfo.StatusID, newPerson);
                }
                else
                {
                    switch ((SCC_BL.Results.Person.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Person.Insert.CODE.ALREADY_EXISTS:
                            Person foundPerson = new Person(newPerson.Identification);
                            foundPerson.SetDataByIdentification();

                            SaveProcessingInformation<SCC_BL.Results.Person.Insert.AlreadyExists>(foundPerson.ID, foundPerson.BasicInfo.StatusID, newPerson);
                            break;
                        default:
                            break;
                    }
                }

                if (result > 0)
                {
                    //string password = SCC_BL.Settings.Overall.DEFAULT_PASSWORD;
                    string password = GenerateRandomString();

                    byte[] salt = Cryptographic.GenerateSalt();

                    byte[] hashedPassword = Cryptographic.HashPasswordWithSalt(System.Text.Encoding.UTF8.GetBytes(password), salt);

                    /*
                        TEMP: 
                    */
                    userPerson.User.Username = newPerson.Identification;
                    /*
                        TEMP: 
                    */

                    User newUser = new User(newPerson.ID, userPerson.User.Username, hashedPassword, salt, userPerson.User.Email, userPerson.User.StartDate, userPerson.User.LanguageID, userPerson.User.HasPassPermission, DateTime.Now, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_USER.CREATED);

                    try
                    {
                        result = newUser.Insert();

                        if (result > 0)
                        {
                            UpdateSupervisorList(newUser, supervisorList != null ? supervisorList : new int[0], supervisorStartDate.Value);

                            UpdateWorkspaceList(newUser, workspaceList != null ? workspaceList : new int[0], workspaceStartDate.Value);

                            UpdateRoleList(newUser, roleList != null ? roleList : new int[0]);

                            UpdateGroupList(newUser, groupList != null ? groupList : new int[0]);

                            UpdateProgramList(newUser, programList != null ? programList : new int[0]);

                            SaveProcessingInformation<SCC_BL.Results.User.Insert.Success>(newUser.ID, newUser.BasicInfo.StatusID, newUser);

                            SendMail(SCC_BL.Settings.AppValues.MailTopic.USER_CREATION, newUser, password);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.User.Insert.CODE)result)
                            {
                                case SCC_BL.Results.User.Insert.CODE.ALREADY_EXISTS:
                                    User foundUser = new User(newUser.Username);
                                    foundUser.SetDataByUsername();

                                    SaveProcessingInformation<SCC_BL.Results.User.Insert.AlreadyExists>(foundUser.ID, foundUser.BasicInfo.StatusID, newUser);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.Insert.Error>(null, null, newUser, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Person.Insert.Error>(null, null, newPerson, ex);
            }

            return RedirectToAction(nameof(UserController.Manage), _mainControllerName);
        }

        public ActionResult LogIn()
        {
            if (GetActualUser() == null)
                return View();
            else
                return RedirectToAction(nameof(Index), GetControllerName(typeof(HomeController)));
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            try
            {
                User user = new User(username);
                byte[] salt = user.GetSaltByUsername();

                if (salt == null || salt.Length == 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.User.LogIn.WrongUsername>(null, null, user);
                    return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
                }


                byte[] hashedPassword = Cryptographic.HashPasswordWithSalt(System.Text.Encoding.UTF8.GetBytes(password), salt);

                user.Password = hashedPassword;

                int result = user.ValidateLogIn();

                if (result > 0)
                {
                    user = new User(result);
                    user.SetDataByID();

                    user.SetTotalPermissions();

                    SetActualUser(user);

                    user.UpdateLastLogin(GetActualUser().ID);

                    SaveProcessingInformation<SCC_BL.Results.User.LogIn.Success>(user.ID, user.BasicInfo.StatusID, user);

                    SaveCatalogList();
                    SaveTableCategoryList();

                    ExecuteInitialActions();

                    return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
                }
                else
                {
                    switch ((SCC_BL.Results.User.LogIn.CODE)result)
                    {
                        case SCC_BL.Results.User.LogIn.CODE.WRONG_PASSWORD:
                            SaveProcessingInformation<SCC_BL.Results.User.LogIn.WrongPassword>(null, null, user);
                            break;
                        case SCC_BL.Results.User.LogIn.CODE.WRONG_USERNAME:
                            SaveProcessingInformation<SCC_BL.Results.User.LogIn.WrongUsername>(null, null, user);
                            break;
                        case SCC_BL.Results.User.LogIn.CODE.DISABLED:
                            SaveProcessingInformation<SCC_BL.Results.User.LogIn.Disabled>(null, null, user);
                            break;
                        case SCC_BL.Results.User.LogIn.CODE.UNAUTHORIZED:
                            SaveProcessingInformation<SCC_BL.Results.User.LogIn.Unauthorized>(null, null, user);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.LogIn.Error>(ex);
            }

            return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
        }

        public ActionResult SignIn()
        {
            List<Catalog> languageList = new List<Catalog>();
            List<Catalog> countryList = new List<Catalog>();

            using (Catalog catalog = new Catalog() { CategoryID = (int)SCC_BL.DBValues.Catalog.Category.USER_LANGUAGE })
            {
                languageList = catalog.SelectByCategoryID()
                    .ToList();
            }

            using (Catalog catalog = new Catalog() { CategoryID = (int)SCC_BL.DBValues.Catalog.Category.PERSON_COUNTRY })
            {
                countryList = catalog.SelectByCategoryID()
                    .ToList();
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.SignIn.Catalog.LanguageCatalog.NAME] =
                new SelectList(
                    languageList,
                    SCC_BL.Settings.AppValues.ViewData.User.SignIn.Catalog.LanguageCatalog.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.SignIn.Catalog.LanguageCatalog.SelectList.TEXT);

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.SignIn.Catalog.CountryCatalog.NAME] =
                new SelectList(
                    countryList,
                    SCC_BL.Settings.AppValues.ViewData.User.SignIn.Catalog.CountryCatalog.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.User.SignIn.Catalog.CountryCatalog.SelectList.TEXT);

            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserPersonViewModel userPerson, string password, string passwordConfirmation)
        {
            Person person = new Person();
            User user = new User();

            try
            {
                if (!password.Equals(passwordConfirmation))
                {
                    SaveProcessingInformation<SCC_BL.Results.User.SignIn.PasswordsDoNotMatch>(null, null, userPerson);

                    return RedirectToAction(nameof(UserController.SignIn), _mainControllerName);
                }

                person = new Person(userPerson.Person.Identification, userPerson.Person.FirstName, userPerson.Person.SurName, userPerson.Person.CountryID, null, (int)SCC_BL.DBValues.Catalog.STATUS_PERSON.CREATED);

                int result = person.CheckExistence();

                if (result < 0)
                    result = person.Insert();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Person.Insert.Success>(person.ID, person.BasicInfo.StatusID, userPerson);
                }
                else
                {
                    switch ((SCC_BL.Results.Person.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Person.Insert.CODE.ALREADY_EXISTS:
                            Person foundPerson = new Person(person.Identification);
                            foundPerson.SetDataByIdentification();

                            SaveProcessingInformation<SCC_BL.Results.Person.Insert.AlreadyExists>(foundPerson.ID, foundPerson.BasicInfo.StatusID, person);

                            person.BasicInfo.DeleteByID();

                            return RedirectToAction("SignIn", "User");
                        default:
                            break;
                    }
                }

                byte[] salt = Cryptographic.GenerateSalt();

                byte[] hashedPassword = Cryptographic.HashPasswordWithSalt(System.Text.Encoding.UTF8.GetBytes(password), salt);

                /*
                    TEMP: 
                */
                userPerson.User.Username = person.Identification;
                /*
                    TEMP: 
                */

                user = new User(person.ID, userPerson.User.Username, hashedPassword, salt, userPerson.User.Email, DateTime.Now, userPerson.User.LanguageID, false, DateTime.Now, null, (int)SCC_BL.DBValues.Catalog.STATUS_USER.CREATED);

                result = user.Insert();

                if (result > 0)
                {
                    SetActualUser(user);

                    SendMail(SCC_BL.Settings.AppValues.MailTopic.USER_CREATION, user, password);

                    SaveProcessingInformation<SCC_BL.Results.User.SignIn.Success>(user.ID, user.BasicInfo.StatusID, userPerson);

                    return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
                }
                else
                {
                    switch ((SCC_BL.Results.User.SignIn.CODE)result)
                    {
                        case SCC_BL.Results.User.SignIn.CODE.ALREADY_EXISTS:
                            SaveProcessingInformation<SCC_BL.Results.User.SignIn.AlreadyExists>(user.ID, user.BasicInfo.StatusID, userPerson);
                            break;
                        default:
                            break;
                    }

                    try
                    {
                        person.BasicInfo.DeleteByID();
                        user.BasicInfo.DeleteByID();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.SignIn.Error>(user.ID, null, userPerson, ex);
            }

            return RedirectToAction(nameof(UserController.SignIn), _mainControllerName);
        }

        public ActionResult MassiveImport()
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MASSIVELY_IMPORT_USERS))
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));

            List<UploadedFile> uploadedFileList = new List<UploadedFile>();

            using (UploadedFile uploadedFile = new UploadedFile())
            {
                uploadedFileList =
                    uploadedFile.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_USER_IMPORT)
                        .ToList();

            }

            return View(uploadedFileList);
        }

        public ActionResult MassivePasswordChange()
        {
            List<User> userList = new List<User>();

            /*if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_OTHER_USER_PASSWORDS))
            {
                SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.NotAllowedToChangeOtherUsersPasswords>();
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
            }*/
            
            if (GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_OTHER_USER_PASSWORDS))
            {
                using (User user = new User())
                {
                    userList =
                        user.SelectAll(true)
                            .OrderBy(o => o.Person.SurName)
                            .ThenBy(o => o.Person.FirstName)
                            .ToList();
                }
            }
            else
            {
                userList.Add(GetActualUser());
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.MassivePasswordChange.User.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    !GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_OTHER_USER_PASSWORDS)
                        ? new int[] { GetActualUser().ID }
                        : null);
            
            /*Dictionary<int, string> userListDictionary = new Dictionary<int, string>();

            userList
                .ForEach(e => {
                    userListDictionary.Add(e.ID, $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }");
                });

            ViewData[SCC_BL.Settings.AppValues.ViewData.User.MassivePasswordChange.User.NAME] =
                new MultiSelectList(
                    userListDictionary,
                    "Key",
                    "Value");*/

            return View(userList);
        }

        public void ChangePassword(int userID, string password, string passwordConfirmation)
        {
            User user = new User(userID);
            user.SetDataByID();

            try
            {
                if (!passwordConfirmation.Equals(password))
                {
                    SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.PasswordsDoNotMatch>(user.ID, user.BasicInfo.StatusID, user);
                    return;
                }

                switch (user.ProcessPasswordRecovery(password, GetActualUser().ID, user.BasicInfo.StatusID))
                {
                    case SCC_BL.Results.User.PasswordRecovery.CODE.SUCCESS:
                        //SendMail(SCC_BL.Settings.AppValues.MailTopic.CHANGE_PASSWORD, user, password);
                        SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.Success>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    case SCC_BL.Results.User.PasswordRecovery.CODE.ERROR:
                        SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.Error>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }
        }

        [HttpPost]
        public ActionResult MassivePasswordChange(int[] userList, string password, string passwordConfirmation)
        {
            try
            {
                if (userList != null)
                {
                    for (int i = 0; i < userList.Length; i++)
                    {
                        ChangePassword(userList[i], password, passwordConfirmation);

                        if (i == userList.Length - 1)
                            SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.Success>();
                    }
                }
                else
                {
                    throw new Exception("No se ingresaron usuarios");
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.PasswordChange.Error>(ex);
            }

            return RedirectToAction(nameof(UserController.MassivePasswordChange));
        }

        [HttpPost]
        public ActionResult MassiveImport(HttpPostedFileBase file, bool modifyExistingOnes = false)
        {
            string filePath = SaveUploadedFile(file, SCC_BL.Settings.Paths.User.MASSIVE_IMPORT_FOLDER);

            if (!string.IsNullOrEmpty(filePath))
            {
                int uploadedFileID = 0;

                uploadedFileID = SaveFileInDatabase(filePath, (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_USER_IMPORT);

                if (uploadedFileID > 0)
                {
                    using (UploadedFile uploadedFile = new UploadedFile(uploadedFileID))
                    {
                        uploadedFile.SetDataByID();
                        uploadedFile.Data = new byte[0];
                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.Insert.Success>(uploadedFile.ID, uploadedFile.BasicInfo.StatusID, uploadedFile);
                    }

                    SCC_BL.Results.UploadedFile.UserMassiveImport.CODE result = ProcessImportExcel(filePath, modifyExistingOnes);

                    switch (result)
                    {
                        case SCC_BL.Results.UploadedFile.UserMassiveImport.CODE.SUCCESS:
                            SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Success>();
                            break;
                        case SCC_BL.Results.UploadedFile.UserMassiveImport.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>();
            }

            return RedirectToAction(nameof(UserController.MassiveImport));
        }

        public SCC_BL.Results.UploadedFile.UserMassiveImport.CODE ProcessImportExcel(string filePath, bool modifyExistingOnes = false)
        {
            List<User> userList = new List<User>();

            List<int> linesWithErrors = new List<int>();

            using (SCC_BL.Tools.ExcelParser excelParser = new ExcelParser())
            {
                userList = ProcessExcelForMassiveImport(filePath, modifyExistingOnes);
            }

            foreach (User user in userList)
            {
                try
                {
                    if (string.IsNullOrEmpty(user.Username))
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.Insert.UsernameNotSet>(null, null, user);

                        linesWithErrors.Add(user.ExcelRowCount);

                        continue;
                    }

                    int existingPersonID = 0;

                    using (SCC_BL.Person auxPerson = new SCC_BL.Person(user.Person.Identification))
                    {
                        existingPersonID = auxPerson.CheckExistence();
                    }

                    if (existingPersonID <= 0)
                    {
                        int resultPersonInserted = 0;
                        int resultUserInserted = 0;

                        try
                        {
                            resultPersonInserted = user.Person.Insert();
                        }
                        catch (Exception ex)
                        {
                            SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                null, 
                                null, 
                                user.Person, 
                                new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_PERSON_NOT_INSERTED
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO , ex.ToString())));

                            linesWithErrors.Add(user.ExcelRowCount);

                            continue;
                        }

                        if (resultPersonInserted > 0)
                        {
                            user.PersonID = user.Person.ID;

                            try
                            {
                                resultUserInserted = user.Insert();
                            }
                            catch (Exception ex)
                            {
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                    null,
                                    null,
                                    user,
                                    new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_USER_NOT_INSERTED
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, ex.ToString())));

                                user.Person.DeleteByID();

                                linesWithErrors.Add(user.ExcelRowCount);

                                continue;
                            }

                            SendMail(SCC_BL.Settings.AppValues.MailTopic.USER_CREATION, user, user.RawPassword);

                            if (resultUserInserted > 0)
                            {
                                using (User auxUser = new User(user.ID))
                                {
                                    auxUser.SetDataByID();

                                    if (user.SupervisorList.Count > 0)
                                        auxUser.UpdateSupervisorList(
                                            user.SupervisorList.Select(e => e.SupervisorID).ToArray(),
                                            user.SupervisorList.FirstOrDefault().StartDate,
                                            user.BasicInfo.CreationUserID.Value
                                        );

                                    if (user.UserWorkspaceCatalogList.Count > 0)
                                        auxUser.UpdateWorkspaceList(
                                            user.UserWorkspaceCatalogList.Select(e => e.WorkspaceID).ToArray(),
                                            user.UserWorkspaceCatalogList.FirstOrDefault().StartDate,
                                            user.BasicInfo.CreationUserID.Value
                                        );

                                    if (user.RoleList.Count > 0)
                                        auxUser.UpdateRoleList(
                                            user.RoleList.Select(e => e.RoleID).ToArray(),
                                            user.BasicInfo.CreationUserID.Value
                                        );

                                    if (user.GroupList.Count > 0)
                                        auxUser.UpdateGroupList(
                                            user.GroupList.Select(e => e.GroupID).ToArray(),
                                            user.BasicInfo.CreationUserID.Value
                                        );

                                    if (user.ProgramList.Count > 0)
                                        auxUser.UpdateProgramList(
                                            user.ProgramList.Select(e => e.ProgramID).ToArray(),
                                            user.BasicInfo.CreationUserID.Value
                                        );
                                }
                            }
                            else
                            {
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(null, null, user, new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_USER_NOT_INSERTED));

                                user.Person.DeleteByID();

                                linesWithErrors.Add(user.ExcelRowCount);

                                continue;
                            }
                        }
                        else
                        {
                            SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                null,
                                null,
                                user.Person,
                                new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_PERSON_NOT_INSERTED));

                            linesWithErrors.Add(user.ExcelRowCount);

                            continue;
                        }
                    }
                    else
                    {
                        if (!modifyExistingOnes)
                        {
                            Person foundPerson = new Person(user.Person.Identification);
                            foundPerson.SetDataByIdentification();

                            SaveProcessingInformation<SCC_BL.Results.Person.Insert.AlreadyExists>(
                                foundPerson.ID, 
                                foundPerson.BasicInfo.StatusID, 
                                user.Person, 
                                new Exception(
                                    SCC_BL.Results.Person.Insert.AlreadyExists.MESSAGE_CONTENT
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, Serialize(foundPerson))));

                            linesWithErrors.Add(user.ExcelRowCount);

                            continue;
                        }
                        else
                        {
                            int resultPersonUpdated = 0;
                            int resultUserUpdated = 0;

                            try
                            {
                                Person existingPerson = new Person(existingPersonID);
                                existingPerson.SetDataByID();

                                Person newModifiedPerson = new Person(
                                    existingPersonID, 
                                    user.Person.Identification,
                                    user.Person.FirstName,
                                    user.Person.SurName,
                                    user.Person.CountryID,
                                    existingPerson.BasicInfoID,
                                    GetActualUser().ID,
                                    (int)SCC_BL.DBValues.Catalog.STATUS_PERSON.UPDATED);

                                user.Person.ID = existingPersonID;

                                resultPersonUpdated = newModifiedPerson.Update();
                                //resultPersonUpdated = user.Person.Update();
                            }
                            catch (Exception ex)
                            {
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                    null,
                                    null,
                                    user.Person,
                                    new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_PERSON_NOT_UPDATED
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, ex.ToString())));

                                linesWithErrors.Add(user.ExcelRowCount);

                                continue;
                            }

                            if (resultPersonUpdated > 0)
                            {
                                int existingUserID = 0;

                                using (SCC_BL.User auxUser = new SCC_BL.User(user.Username))
                                {
                                    existingUserID = auxUser.CheckExistence();
                                }

                                user.PersonID = user.Person.ID;

                                if (existingUserID <= 0)
                                {
                                    try
                                    {
                                        resultUserUpdated = user.Insert();
                                    }
                                    catch (Exception ex)
                                    {
                                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                            null,
                                            null,
                                            user,
                                            new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_USER_NOT_INSERTED
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, ex.ToString())));

                                        linesWithErrors.Add(user.ExcelRowCount);

                                        continue;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        User existingUser = new User(existingUserID);
                                        existingUser.SetDataByID();

                                        //int id, string username, string email, DateTime startDate, int languageID, bool hasPassPermission, int basicInfoID, int modificationUserID, int statusID
                                        User newModifiedUser = new User(
                                            existingUserID,
                                            user.Username,
                                            user.Email,
                                            user.StartDate,
                                            user.LanguageID,
                                            user.HasPassPermission,
                                            existingUser.BasicInfoID,
                                            GetActualUser().ID,
                                            (int)SCC_BL.DBValues.Catalog.STATUS_USER.UPDATED);

                                        user.ID = existingUserID;

                                        resultUserUpdated = newModifiedUser.Update();
                                        //resultUserUpdated = user.Update();
                                    }
                                    catch (Exception ex)
                                    {
                                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                            null,
                                            null,
                                            user,
                                            new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_USER_NOT_UPDATED
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, ex.ToString())));

                                        linesWithErrors.Add(user.ExcelRowCount);

                                        continue;
                                    }
                                }

                                SendMail(SCC_BL.Settings.AppValues.MailTopic.USER_CREATION, user, user.RawPassword);

                                if (resultUserUpdated > 0)
                                {
                                    using (User auxUser = new User(user.ID))
                                    {
                                        auxUser.SetDataByID();

                                        if (user.SupervisorList.Count > 0)
                                            auxUser.UpdateSupervisorList(
                                                user.SupervisorList.Select(e => e.SupervisorID).ToArray(),
                                                user.SupervisorList.FirstOrDefault().StartDate,
                                                user.BasicInfo.CreationUserID.Value
                                            );

                                        if (user.UserWorkspaceCatalogList.Count > 0)
                                            auxUser.UpdateWorkspaceList(
                                                user.UserWorkspaceCatalogList.Select(e => e.WorkspaceID).ToArray(),
                                                user.UserWorkspaceCatalogList.FirstOrDefault().StartDate,
                                                user.BasicInfo.CreationUserID.Value
                                            );

                                        if (user.RoleList.Count > 0)
                                            auxUser.UpdateRoleList(
                                                user.RoleList.Select(e => e.RoleID).ToArray(),
                                                user.BasicInfo.CreationUserID.Value
                                            );

                                        if (user.GroupList.Count > 0)
                                            auxUser.UpdateGroupList(
                                                user.GroupList.Select(e => e.GroupID).ToArray(),
                                                user.BasicInfo.CreationUserID.Value
                                            );

                                        if (user.ProgramList.Count > 0)
                                            auxUser.UpdateProgramList(
                                                user.ProgramList.Select(e => e.ProgramID).ToArray(),
                                                user.BasicInfo.CreationUserID.Value
                                            );
                                    }
                                }
                                else
                                {
                                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(null, null, user, new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_USER_NOT_UPDATED));

                                    linesWithErrors.Add(user.ExcelRowCount);

                                    continue;
                                }
                            }
                            else
                            {
                                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(
                                    null,
                                    null,
                                    user.Person,
                                    new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_PERSON_NOT_UPDATED));

                                linesWithErrors.Add(user.ExcelRowCount);

                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    linesWithErrors.Add(user.ExcelRowCount);

                    Session[SCC_BL.Settings.AppValues.Session.ERROR_COUNT] = linesWithErrors;
                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow>(null, null, user.ExcelRowCount, ex);

                    continue;
                }
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

                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>(
                    new Exception(
                        SCC_BL.Results.UploadedFile.UserMassiveImport.ErrorSingleRow.CUSTOM_ERROR_EXCEL_LINES
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, lineList)
                    )
                );

                Session[SCC_BL.Settings.AppValues.Session.ERROR_COUNT] = null;
            }

            if (linesWithErrors.Count() > 0)
                return SCC_BL.Results.UploadedFile.UserMassiveImport.CODE.ERROR;
            else
                return SCC_BL.Results.UploadedFile.UserMassiveImport.CODE.SUCCESS;
        }

        public List<User> ProcessExcelForMassiveImport(string filePath, bool modifyExistingOnes = false)
        {
            List<User> elementList = new List<User>();

            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart wbPart = document.WorkbookPart;
                    var workSheet = wbPart.Workbook.Descendants<DocumentFormat.OpenXml.Spreadsheet.Sheet>().FirstOrDefault();
                    if (workSheet != null)
                    {
                        WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(workSheet.Id));
                        IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rows = wsPart.Worksheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();
                        var headersCount = rows.ElementAt(0).Count();

                        int rowCount = 2;

                        foreach (DocumentFormat.OpenXml.Spreadsheet.Row row in rows.Skip(1))
                        {
                            using (SCC_BL.Tools.ExcelParser excelParser = new ExcelParser())
                            {
                                var newRow = excelParser.GetRowCells(row, headersCount).ToArray();

                                User user = new User(newRow, rowCount, GetActualUser().ID);

                                elementList.Add(user);
                            }

                            rowCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>(ex);
            }

            return elementList;
        }

        public ActionResult PasswordRecovery()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            SetActualUser(null);
            return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
        }

        [HttpPost]
        public ActionResult PasswordRecovery(string username, string password, string passwordConfirmation, string token)
        {
            User user = new User(username, token);

            user.SetDataByUsername();

            try
            {
                if (user.ID <= 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.User.PasswordRecovery.WrongUsername>(user.ID, user.BasicInfo.StatusID, user);
                    return RedirectToAction(nameof(UserController.PasswordRecovery), _mainControllerName);
                }

                if (!passwordConfirmation.Equals(password))
                {
                    SaveProcessingInformation<SCC_BL.Results.User.PasswordRecovery.PasswordsDoNotMatch>(user.ID, user.BasicInfo.StatusID, user);
                }

                switch (user.ProcessPasswordRecovery(password, GetActualUser().ID, user.BasicInfo.StatusID))
                {
                    case SCC_BL.Results.User.PasswordRecovery.CODE.SUCCESS:
                        SendMail(SCC_BL.Settings.AppValues.MailTopic.CHANGE_PASSWORD, user, password);
                        SaveProcessingInformation<SCC_BL.Results.User.PasswordRecovery.Success>(user.ID, user.BasicInfo.StatusID, user);

                        return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
                    case SCC_BL.Results.User.PasswordRecovery.CODE.WRONG_TOKEN:
                        SaveProcessingInformation<SCC_BL.Results.User.PasswordRecovery.WrongToken>(user.ID, user.BasicInfo.StatusID, user);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.PasswordRecovery.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

            return RedirectToAction(nameof(UserController.PasswordRecovery), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int userID)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_DELETE_USERS))
            {
                SaveProcessingInformation<SCC_BL.Results.User.Delete.NotAllowedToDeleteUsers>();
                return RedirectToAction(nameof(UserController.Manage), GetControllerName(typeof(UserController)));
            }

            User user = new User(userID);
            user.SetDataByID(true);

            try
            {
                //user.Delete();

                user.BasicInfo.ModificationUserID = GetActualUser().ID;
                user.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED;

                int result = user.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.User.Delete.Success>(user.ID, user.BasicInfo.StatusID, user);

                    return RedirectToAction(nameof(UserController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.Delete.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
            }

            return RedirectToAction(nameof(UserController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Activate(int userID, bool activate)
        {
            User user = new User(userID);
            user.SetDataByID(true);

            try
            {
                if (activate)
                    user.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_USER.ENABLED;
                else
                    user.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED;

                user.BasicInfo.ModificationUserID = GetActualUser().ID;

                int result = user.BasicInfo.Update();

                if (result > 0)
                {
                    if (activate)
                        SaveProcessingInformation<SCC_BL.Results.User.Activate.Success>(user.ID, user.BasicInfo.StatusID, user);
                    else
                        SaveProcessingInformation<SCC_BL.Results.User.Deactivate.Success>(user.ID, user.BasicInfo.StatusID, user);

                    return RedirectToAction(nameof(UserController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                if (activate)
                    SaveProcessingInformation<SCC_BL.Results.User.Activate.Error>(user.ID, user.BasicInfo.StatusID, user, ex);
                else
                    SaveProcessingInformation<SCC_BL.Results.User.Deactivate.Error>(user.ID, user.BasicInfo.StatusID, user);
            }

            return RedirectToAction(nameof(UserController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult ForgottenPassword(string username)
        {
            try
            {
                string token = string.Empty;

                using (User user = new User(username))
                {
                    user.SetDataByUsername();

                    if (user.ID <= 0)
                    {
                        SaveProcessingInformation<SCC_BL.Results.User.ForgottenPassword.WrongUsername>(user.ID, user.BasicInfo.StatusID, user);
                        return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
                    }

                    token = user.GetToken();

                    string url = string.Empty;
                    url = $"{ Url.Action(nameof(UserController.PasswordRecovery), _mainControllerName, null, Request.Url.Scheme) }?token={ token }";

                    SendMail(SCC_BL.Settings.AppValues.MailTopic.FORGOTTEN_PASSWORD, user, null, url);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.User.ForgottenPassword.Error>(ex);
            }

            return RedirectToAction(nameof(UserController.LogIn), _mainControllerName);
        }
    }
}