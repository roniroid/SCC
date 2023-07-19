using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class ReportController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(ReportController));
        bool debugging = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["DEBUGGING"]);

        public ActionResult Index()
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_REPORTS))
            {
                SaveProcessingInformation<SCC_BL.Results.Report.Index.NotAllowedToSeeReports>();
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
            }

            return View();
        }

        public ActionResult _OverallAccuracy(ViewModels.ReportOverallAccuracyViewModel reportOverallAccuracyViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportOverallAccuracyViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportOverallAccuracyViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<Catalog> errorTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
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

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._OverallAccuracy.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportOverallAccuracyViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._OverallAccuracy.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportOverallAccuracyViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._OverallAccuracy.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportOverallAccuracyViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._OverallAccuracy.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportOverallAccuracyViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._OverallAccuracy.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportOverallAccuracyViewModel.ErrorTypeIDArray
                            : null);

            if (!hasModel)
                reportOverallAccuracyViewModel.AttributeNoConstraint = true;

            reportOverallAccuracyViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_OverallAccuracy), reportOverallAccuracyViewModel);
        }

        public ActionResult _ParetoBI(ViewModels.ReportParetoBIViewModel reportParetoBIViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportParetoBIViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportParetoBIViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<BusinessIntelligenceField> businessIntelligenceFieldList = new List<BusinessIntelligenceField>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            /*using (User user = new User())
                supervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField())
                businessIntelligenceFieldList =
                    businessIntelligenceField.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DISABLED &&
                            (e.ParentBIFieldID == null || e.ParentBIFieldID == 0))
                        .ToList();

            using (CustomControl customControl = new CustomControl())
                customControlList =
                    customControl.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DISABLED)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ParetoBI.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportParetoBIViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ParetoBI.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportParetoBIViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ParetoBI.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportParetoBIViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ParetoBI.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportParetoBIViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ParetoBI.BusinessIntelligenceFieldList.NAME] =
                new MultiSelectList(
                    businessIntelligenceFieldList,
                    nameof(BusinessIntelligenceField.ID),
                    nameof(BusinessIntelligenceField.Name),
                    debugging
                        ? businessIntelligenceFieldList.Select(e => e.ID)
                        : hasModel
                            ? reportParetoBIViewModel.BIFieldIDArray
                            : null);

            reportParetoBIViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_ParetoBI), reportParetoBIViewModel);
        }

        public ActionResult _ComparativeByUser(ViewModels.ReportComparativeByUserViewModel reportComparativeByUserViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportComparativeByUserViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportComparativeByUserViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<Catalog> errorTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            /*using (User user = new User())
                supervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
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

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByUser.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByUserViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByUser.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByUserViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByUser.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByUserViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByUser.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByUserViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByUser.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByUserViewModel.ErrorTypeIDArray
                            : null);

            if (!hasModel)
                reportComparativeByUserViewModel.AttributeNoConstraint = true;

            reportComparativeByUserViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_ComparativeByUser), reportComparativeByUserViewModel);
        }        

        public ActionResult _ComparativeByProgram(ViewModels.ReportComparativeByProgramViewModel reportComparativeByProgramViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportComparativeByProgramViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportComparativeByProgramViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<Catalog> errorTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            /*using (User user = new User())
                supervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
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

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByProgram.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByProgramViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByProgram.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByProgramViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByProgram.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByProgramViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByProgram.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByProgramViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._ComparativeByProgram.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportComparativeByProgramViewModel.ErrorTypeIDArray
                            : null);

            if (!hasModel)
                reportComparativeByProgramViewModel.AttributeNoConstraint = true;

            reportComparativeByProgramViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_ComparativeByProgram), reportComparativeByProgramViewModel);
        }

        public ActionResult _AccuracyByAttribute(ViewModels.ReportAccuracyByAttributeViewModel reportAttributeAccuracyViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportAttributeAccuracyViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportAttributeAccuracyViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<Catalog> errorTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            /*using (User user = new User())
                supervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
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

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyByAttribute.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportAttributeAccuracyViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyByAttribute.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportAttributeAccuracyViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyByAttribute.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportAttributeAccuracyViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyByAttribute.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportAttributeAccuracyViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyByAttribute.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportAttributeAccuracyViewModel.ErrorTypeIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyByAttribute.AttributeList.NAME] =
                new MultiSelectList(
                    new List<object>());

            if (!hasModel)
                reportAttributeAccuracyViewModel.AttributeNoConstraint = true;

            reportAttributeAccuracyViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_AccuracyByAttribute), reportAttributeAccuracyViewModel);
        }

        public ActionResult _CalibratorComparison(ViewModels.ReportCalibratorComparisonViewModel reportCalibratorComparisonViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportCalibratorComparisonViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportCalibratorComparisonViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> calibratedUserList = new List<User>();
            List<User> calibratedSupervisorList = new List<User>();
            List<User> calibratorUserList = new List<User>();
            List<Catalog> calibrationTypeList = new List<Catalog>();
            List<Catalog> errorTypeList = new List<Catalog>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                calibratedUserList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            using (User user = new User())
                calibratedSupervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                calibratorUserList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.CALIBRATOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.CALIBRATION_TYPE))
                calibrationTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._CalibratorComparison.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportCalibratorComparisonViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._CalibratorComparison.CalibratedUserList.NAME] =
                new MultiSelectList(
                    calibratedUserList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? calibratedUserList.Select(e => e.ID)
                        : hasModel
                            ? reportCalibratorComparisonViewModel.CalibratedUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._CalibratorComparison.CalibratedSupervisorList.NAME] =
                new MultiSelectList(
                    calibratedSupervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? calibratedSupervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportCalibratorComparisonViewModel.CalibratedSupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._CalibratorComparison.CalibratorUserList.NAME] =
                new MultiSelectList(
                    calibratorUserList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? calibratorUserList.Select(e => e.ID)
                        : hasModel
                            ? reportCalibratorComparisonViewModel.CalibratorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._CalibratorComparison.CalibrationTypeList.NAME] =
                new MultiSelectList(
                    calibrationTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? calibrationTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportCalibratorComparisonViewModel.CalibrationTypeIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._CalibratorComparison.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportCalibratorComparisonViewModel.ErrorTypeIDArray
                            : null);

            return PartialView(nameof(_CalibratorComparison), reportCalibratorComparisonViewModel);
        }

        public ActionResult _AccuracyTrend(ViewModels.ReportAccuracyTrendViewModel reportAccuracyTrendViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportAccuracyTrendViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportAccuracyTrendViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<Catalog> errorTypeList = new List<Catalog>();
            List<Catalog> intervalTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            /*using (User user = new User())
                supervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TIME_INTERVAL))
                intervalTypeList =
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

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrend.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrend.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrend.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrend.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrend.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendViewModel.ErrorTypeIDArray
                            : null);

            int? selectedInterval = null;

            if (hasModel) selectedInterval = reportAccuracyTrendViewModel.IntervalTypeID;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrend.IntervalTypeList.NAME] =
                new SelectList(
                    intervalTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    selectedInterval);

            if (!hasModel)
                reportAccuracyTrendViewModel.AttributeNoConstraint = true;

            reportAccuracyTrendViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_AccuracyTrend), reportAccuracyTrendViewModel);
        }

        public ActionResult _AccuracyTrendByAttribute(ViewModels.ReportAccuracyTrendByAttributeViewModel reportAccuracyTrendByAttributeViewModel = null)
        {
            bool hasModel = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in reportAccuracyTrendByAttributeViewModel.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(reportAccuracyTrendByAttributeViewModel) != null)
                {
                    hasModel = true;
                    break;
                }
            }

            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();
            List<User> supervisorList = new List<User>();
            List<User> evaluatorList = new List<User>();
            List<Catalog> errorTypeList = new List<Catalog>();
            List<Catalog> intervalTypeList = new List<Catalog>();
            List<CustomControl> customControlList = new List<CustomControl>();

            using (Program program = new Program())
                programList =
                    program.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();

            using (User user = new User())
                userList =
                    user.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            e.WorkspaceList.Select(s => s.Monitorable).Count() > 0)
                        .OrderBy(o => o.Person.SurName)
                        .ThenBy(o => o.Person.LastName)
                        .ThenBy(o => o.Person.FirstName)
                        .ToList();

            /*using (User user = new User())
                supervisorList =
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();

            using (User user = new User())
                evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

            using (User user = new User())
            {
                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                supervisorList =
                    supervisorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERVISOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                evaluatorList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList());

                /*evaluatorList =
                    user.SelectEvaluatorList()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED)
                        .ToList();*/

                evaluatorList =
                    evaluatorList
                        .GroupBy(e => e.ID)
                        .Select(e => e.First())
                        .ToList();
            }

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TIME_INTERVAL))
                intervalTypeList =
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

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    debugging
                        ? programList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendByAttributeViewModel.ProgramIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.UserList.NAME] =
                new MultiSelectList(
                    userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? userList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendByAttributeViewModel.UserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.SupervisorList.NAME] =
                new MultiSelectList(
                    supervisorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? supervisorList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendByAttributeViewModel.SupervisorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.EvaluatorList.NAME] =
                new MultiSelectList(
                    evaluatorList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.LastName } { e.Person.FirstName }" }),
                    "Key",
                    "Value",
                    debugging
                        ? evaluatorList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendByAttributeViewModel.EvaluatorUserIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.ErrorTypeList.NAME] =
                new MultiSelectList(
                    errorTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    debugging
                        ? errorTypeList.Select(e => e.ID)
                        : hasModel
                            ? reportAccuracyTrendByAttributeViewModel.ErrorTypeIDArray
                            : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.AttributeList.NAME] =
                new MultiSelectList(
                    new List<object>());

            int? selectedInterval = null;

            if (hasModel) selectedInterval = reportAccuracyTrendByAttributeViewModel.IntervalTypeID;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Report._AccuracyTrendByAttribute.IntervalTypeList.NAME] =
                new SelectList(
                    intervalTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    selectedInterval);

            if (!hasModel)
                reportAccuracyTrendByAttributeViewModel.AttributeNoConstraint = true;

            reportAccuracyTrendByAttributeViewModel.CustomControlList = customControlList;

            return PartialView(nameof(_AccuracyTrendByAttribute), reportAccuracyTrendByAttributeViewModel);
        }

        [HttpGet]
        public ActionResult ReportTypeView(int reportTypeID)
        {
            switch (reportTypeID)
            {
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.OVERALL_ACCURACY:
                    return RedirectToAction(nameof(_OverallAccuracy));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.ACCURACY_TREND:
                    return RedirectToAction(nameof(_AccuracyTrend));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.CALIBRATOR_COMPARISON:
                    return RedirectToAction(nameof(_CalibratorComparison));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.ACCURACY_BY_ATTRIBUTE:
                    return RedirectToAction(nameof(_AccuracyByAttribute));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.ACCURACY_TREND_BY_ATTRIBUTE:
                    return RedirectToAction(nameof(_AccuracyTrendByAttribute));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.COMPARATIVE_BY_USER:
                    return RedirectToAction(nameof(_ComparativeByUser));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.COMPARATIVE_BY_PROGRAM:
                    return RedirectToAction(nameof(_ComparativeByProgram));
                case (int)SCC_BL.DBValues.Catalog.REPORT_TYPE.PARETO_BI:
                    return RedirectToAction(nameof(_ParetoBI));
                default:
                    break;
            }

            return RedirectToAction(nameof(CustomControlController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult OverallAccuracy(ViewModels.ReportOverallAccuracyViewModel reportOverallAccuracyViewModel)
        {
            List<SCC_BL.Reports.Results.OverallAccuracy> resultOverallAccuracy = new List<SCC_BL.Reports.Results.OverallAccuracy>();
            ViewModels.ReportResultsOverallAccuracyViewModel reportResultsOverallAccuracyViewModel = new ViewModels.ReportResultsOverallAccuracyViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportOverallAccuracyViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportOverallAccuracyViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportOverallAccuracyViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportOverallAccuracyViewModel.TransactionEndDate).Second == 0)
                        {
                            reportOverallAccuracyViewModel.TransactionEndDate = ((DateTime)reportOverallAccuracyViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportOverallAccuracyViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportOverallAccuracyViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportOverallAccuracyViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportOverallAccuracyViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportOverallAccuracyViewModel.EvaluationEndDate = ((DateTime)reportOverallAccuracyViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultOverallAccuracy = report.OverallAccuracy(
                        reportOverallAccuracyViewModel.TransactionStartDate,
                        reportOverallAccuracyViewModel.TransactionEndDate,
                        reportOverallAccuracyViewModel.EvaluationStartDate,
                        reportOverallAccuracyViewModel.EvaluationEndDate,
                        reportOverallAccuracyViewModel.ProgramIDArray != null
                            ? String.Join(",", reportOverallAccuracyViewModel.ProgramIDArray)
                            : string.Empty,
                        reportOverallAccuracyViewModel.UserIDArray != null
                            ? String.Join(",", reportOverallAccuracyViewModel.UserIDArray)
                            : string.Empty,
                        reportOverallAccuracyViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportOverallAccuracyViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportOverallAccuracyViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportOverallAccuracyViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        reportOverallAccuracyViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportOverallAccuracyViewModel.ErrorTypeIDArray)
                            : string.Empty,
                        reportOverallAccuracyViewModel.AttributeNoConstraint != null
                            ? !reportOverallAccuracyViewModel.AttributeNoConstraint.Value
                                ? reportOverallAccuracyViewModel.AttributeControllable
                                : null
                            : null,
                        reportOverallAccuracyViewModel.AttributeNoConstraint != null
                            ? !reportOverallAccuracyViewModel.AttributeNoConstraint.Value
                                ? reportOverallAccuracyViewModel.AttributeKnown
                                : null
                            : null,
                        reportOverallAccuracyViewModel.TransactionCustomFieldCatalog);

                    reportResultsOverallAccuracyViewModel = new ViewModels.ReportResultsOverallAccuracyViewModel(resultOverallAccuracy);
                    /*reportOverallAccuracyViewModel.ReportResultsOverallAccuracyViewModel = reportResultsOverallAccuracyViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.OverallAccuracy.Error>(null, null, reportOverallAccuracyViewModel, ex);
            }

            reportResultsOverallAccuracyViewModel.RequestObject = reportOverallAccuracyViewModel;
            reportResultsOverallAccuracyViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.OverallAccuracyResults), reportResultsOverallAccuracyViewModel);
        }

        [HttpPost]
        public ActionResult ComparativeByUser(ViewModels.ReportComparativeByUserViewModel reportComparativeByUserViewModel)
        {
            List<SCC_BL.Reports.Results.ComparativeByUser> resultComparativeByUser = new List<SCC_BL.Reports.Results.ComparativeByUser>();
            ViewModels.ReportResultsComparativeByUserViewModel reportResultsComparativeByUserViewModel = new ViewModels.ReportResultsComparativeByUserViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportComparativeByUserViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportComparativeByUserViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportComparativeByUserViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportComparativeByUserViewModel.TransactionEndDate).Second == 0)
                        {
                            reportComparativeByUserViewModel.TransactionEndDate = ((DateTime)reportComparativeByUserViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportComparativeByUserViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportComparativeByUserViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportComparativeByUserViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportComparativeByUserViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportComparativeByUserViewModel.EvaluationEndDate = ((DateTime)reportComparativeByUserViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultComparativeByUser = report.ComparativeByUser(
                        reportComparativeByUserViewModel.TransactionStartDate,
                        reportComparativeByUserViewModel.TransactionEndDate,
                        reportComparativeByUserViewModel.EvaluationStartDate,
                        reportComparativeByUserViewModel.EvaluationEndDate,
                        reportComparativeByUserViewModel.ProgramIDArray != null
                            ? String.Join(",", reportComparativeByUserViewModel.ProgramIDArray)
                            : string.Empty,
                        reportComparativeByUserViewModel.UserIDArray != null
                            ? String.Join(",", reportComparativeByUserViewModel.UserIDArray)
                            : string.Empty,
                        reportComparativeByUserViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportComparativeByUserViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportComparativeByUserViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportComparativeByUserViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        reportComparativeByUserViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportComparativeByUserViewModel.ErrorTypeIDArray)
                            : string.Empty,
                        reportComparativeByUserViewModel.AttributeNoConstraint != null
                            ? !reportComparativeByUserViewModel.AttributeNoConstraint.Value
                                ? reportComparativeByUserViewModel.AttributeControllable
                                : null
                            : null,
                        reportComparativeByUserViewModel.AttributeNoConstraint != null
                            ? !reportComparativeByUserViewModel.AttributeNoConstraint.Value
                                ? reportComparativeByUserViewModel.AttributeKnown
                                : null
                            : null,
                        reportComparativeByUserViewModel.TransactionCustomFieldCatalog);

                    reportResultsComparativeByUserViewModel = new ViewModels.ReportResultsComparativeByUserViewModel(resultComparativeByUser);
                    /*reportComparativeByUserViewModel.ReportResultsComparativeByUserViewModel = reportResultsComparativeByUserViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.ComparativeByUser.Error>(null, null, reportComparativeByUserViewModel, ex);
            }

            reportResultsComparativeByUserViewModel.RequestObject = reportComparativeByUserViewModel;
            reportResultsComparativeByUserViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.ComparativeByUserResults), reportResultsComparativeByUserViewModel);
        }

        [HttpPost]
        public ActionResult ComparativeByProgram(ViewModels.ReportComparativeByProgramViewModel reportComparativeByProgramViewModel)
        {
            List<SCC_BL.Reports.Results.ComparativeByProgram> resultComparativeByProgram = new List<SCC_BL.Reports.Results.ComparativeByProgram>();
            ViewModels.ReportResultsComparativeByProgramViewModel reportResultsComparativeByProgramViewModel = new ViewModels.ReportResultsComparativeByProgramViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportComparativeByProgramViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportComparativeByProgramViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportComparativeByProgramViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportComparativeByProgramViewModel.TransactionEndDate).Second == 0)
                        {
                            reportComparativeByProgramViewModel.TransactionEndDate = ((DateTime)reportComparativeByProgramViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportComparativeByProgramViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportComparativeByProgramViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportComparativeByProgramViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportComparativeByProgramViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportComparativeByProgramViewModel.EvaluationEndDate = ((DateTime)reportComparativeByProgramViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultComparativeByProgram = report.ComparativeByProgram(
                        reportComparativeByProgramViewModel.TransactionStartDate,
                        reportComparativeByProgramViewModel.TransactionEndDate,
                        reportComparativeByProgramViewModel.EvaluationStartDate,
                        reportComparativeByProgramViewModel.EvaluationEndDate,
                        reportComparativeByProgramViewModel.ProgramIDArray != null
                            ? String.Join(",", reportComparativeByProgramViewModel.ProgramIDArray)
                            : string.Empty,
                        reportComparativeByProgramViewModel.UserIDArray != null
                            ? String.Join(",", reportComparativeByProgramViewModel.UserIDArray)
                            : string.Empty,
                        reportComparativeByProgramViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportComparativeByProgramViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportComparativeByProgramViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportComparativeByProgramViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        reportComparativeByProgramViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportComparativeByProgramViewModel.ErrorTypeIDArray)
                            : string.Empty,
                        reportComparativeByProgramViewModel.AttributeNoConstraint != null
                            ? !reportComparativeByProgramViewModel.AttributeNoConstraint.Value
                                ? reportComparativeByProgramViewModel.AttributeControllable
                                : null
                            : null,
                        reportComparativeByProgramViewModel.AttributeNoConstraint != null
                            ? !reportComparativeByProgramViewModel.AttributeNoConstraint.Value
                                ? reportComparativeByProgramViewModel.AttributeKnown
                                : null
                            : null,
                        reportComparativeByProgramViewModel.TransactionCustomFieldCatalog);

                    reportResultsComparativeByProgramViewModel = new ViewModels.ReportResultsComparativeByProgramViewModel(resultComparativeByProgram);
                    /*reportComparativeByProgramViewModel.ReportResultsComparativeByProgramViewModel = reportResultsComparativeByProgramViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.ComparativeByProgram.Error>(null, null, reportComparativeByProgramViewModel, ex);
            }

            reportResultsComparativeByProgramViewModel.RequestObject = reportComparativeByProgramViewModel;
            reportResultsComparativeByProgramViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.ComparativeByProgramResults), reportResultsComparativeByProgramViewModel);
        }

        [HttpPost]
        public ActionResult CalibratorComparison(ViewModels.ReportCalibratorComparisonViewModel reportCalibratorComparisonViewModel)
        {
            List<SCC_BL.Reports.Results.CalibratorComparison> resultCalibratorComparison = new List<SCC_BL.Reports.Results.CalibratorComparison>();
            ViewModels.ReportResultsCalibratorComparisonViewModel reportResultsCalibratorComparisonViewModel = new ViewModels.ReportResultsCalibratorComparisonViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportCalibratorComparisonViewModel.CalibrationEndDate != null)
                    {
                        if (
                            ((DateTime)reportCalibratorComparisonViewModel.CalibrationEndDate).Hour == 0 &&
                            ((DateTime)reportCalibratorComparisonViewModel.CalibrationEndDate).Minute == 0 &&
                            ((DateTime)reportCalibratorComparisonViewModel.CalibrationEndDate).Second == 0)
                        {
                            reportCalibratorComparisonViewModel.CalibrationEndDate = ((DateTime)reportCalibratorComparisonViewModel.CalibrationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultCalibratorComparison = report.CalibratorComparison(
                        reportCalibratorComparisonViewModel.CalibrationStartDate,
                        reportCalibratorComparisonViewModel.CalibrationEndDate,
                        reportCalibratorComparisonViewModel.ProgramIDArray != null
                            ? String.Join(",", reportCalibratorComparisonViewModel.ProgramIDArray)
                            : string.Empty,
                        reportCalibratorComparisonViewModel.CalibratedUserIDArray != null
                            ? String.Join(",", reportCalibratorComparisonViewModel.CalibratedUserIDArray)
                            : string.Empty,
                        reportCalibratorComparisonViewModel.CalibratedSupervisorUserIDArray != null
                            ? String.Join(",", reportCalibratorComparisonViewModel.CalibratedSupervisorUserIDArray)
                            : string.Empty,
                        reportCalibratorComparisonViewModel.CalibratorUserIDArray != null
                            ? String.Join(",", reportCalibratorComparisonViewModel.CalibratorUserIDArray)
                            : string.Empty,
                        reportCalibratorComparisonViewModel.CalibrationTypeIDArray != null
                            ? String.Join(",", reportCalibratorComparisonViewModel.CalibrationTypeIDArray)
                            : string.Empty,
                        reportCalibratorComparisonViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportCalibratorComparisonViewModel.ErrorTypeIDArray)
                            : string.Empty);

                    reportResultsCalibratorComparisonViewModel = new ViewModels.ReportResultsCalibratorComparisonViewModel(resultCalibratorComparison);
                    /*reportCalibratorComparisonViewModel.ReportResultsCalibratorComparisonViewModel = reportResultsCalibratorComparisonViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.CalibratorComparison.Error>(null, null, reportCalibratorComparisonViewModel, ex);
            }

            reportResultsCalibratorComparisonViewModel.RequestObject = reportCalibratorComparisonViewModel;
            reportResultsCalibratorComparisonViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.CalibratorComparisonResults), reportResultsCalibratorComparisonViewModel);
        }

        [HttpPost]
        public ActionResult AccuracyTrend(ViewModels.ReportAccuracyTrendViewModel reportAccuracyTrendViewModel)
        {
            List<SCC_BL.Reports.Results.AccuracyTrend> resultAccuracyTrendResultList = new List<SCC_BL.Reports.Results.AccuracyTrend>();
            ViewModels.ReportResultsAccuracyTrendViewModel reportResultsAccuracyTrendViewModel = new ViewModels.ReportResultsAccuracyTrendViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportAccuracyTrendViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportAccuracyTrendViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportAccuracyTrendViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportAccuracyTrendViewModel.TransactionEndDate).Second == 0)
                        {
                            reportAccuracyTrendViewModel.TransactionEndDate = ((DateTime)reportAccuracyTrendViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportAccuracyTrendViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportAccuracyTrendViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportAccuracyTrendViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportAccuracyTrendViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportAccuracyTrendViewModel.EvaluationEndDate = ((DateTime)reportAccuracyTrendViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultAccuracyTrendResultList = report.AccuracyTrend(
                        reportAccuracyTrendViewModel.TransactionStartDate,
                        reportAccuracyTrendViewModel.TransactionEndDate,
                        reportAccuracyTrendViewModel.EvaluationStartDate,
                        reportAccuracyTrendViewModel.EvaluationEndDate,
                        reportAccuracyTrendViewModel.ProgramIDArray != null
                            ? String.Join(",", reportAccuracyTrendViewModel.ProgramIDArray)
                            : string.Empty,
                        reportAccuracyTrendViewModel.UserIDArray != null
                            ? String.Join(",", reportAccuracyTrendViewModel.UserIDArray)
                            : string.Empty,
                        reportAccuracyTrendViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportAccuracyTrendViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportAccuracyTrendViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportAccuracyTrendViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        reportAccuracyTrendViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportAccuracyTrendViewModel.ErrorTypeIDArray)
                            : string.Empty,
                        reportAccuracyTrendViewModel.AttributeNoConstraint != null
                            ? !reportAccuracyTrendViewModel.AttributeNoConstraint.Value
                                ? reportAccuracyTrendViewModel.AttributeControllable
                                : null
                            : null,
                        reportAccuracyTrendViewModel.AttributeNoConstraint != null
                            ? !reportAccuracyTrendViewModel.AttributeNoConstraint.Value
                                ? reportAccuracyTrendViewModel.AttributeKnown
                                : null
                            : null,
                        reportAccuracyTrendViewModel.TransactionCustomFieldCatalog);

                    reportResultsAccuracyTrendViewModel = new ViewModels.ReportResultsAccuracyTrendViewModel(resultAccuracyTrendResultList, (SCC_BL.DBValues.Catalog.TIME_INTERVAL)reportAccuracyTrendViewModel.IntervalTypeID);
                    /*reportResultsAccuracyTrendViewModel.ReportResultsAccuracyTrendViewModel = reportResultsAccuracyTrendViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.AccuracyTrend.Error>(null, null, reportAccuracyTrendViewModel, ex);
            }

            reportResultsAccuracyTrendViewModel.RequestObject = reportAccuracyTrendViewModel;
            reportResultsAccuracyTrendViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.AccuracyTrendResults), reportResultsAccuracyTrendViewModel);
        }

        [HttpPost]
        public ActionResult AccuracyTrendByAttribute(ViewModels.ReportAccuracyTrendByAttributeViewModel reportAccuracyTrendByAttributeViewModel)
        {
            List<SCC_BL.Reports.Results.AccuracyTrend> resultAccuracyTrendResultList = new List<SCC_BL.Reports.Results.AccuracyTrend>();
            ViewModels.ReportResultsAccuracyTrendViewModel reportResultsAccuracyTrendViewModel = new ViewModels.ReportResultsAccuracyTrendViewModel();

            List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> resultAccuracyTrendByAttribute = new List<SCC_BL.Reports.Results.AccuracyTrendByAttribute>();
            ViewModels.ReportResultsAccuracyTrendByAttributeViewModel reportResultsAccuracyTrendByAttributeViewModel = new ViewModels.ReportResultsAccuracyTrendByAttributeViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportAccuracyTrendByAttributeViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportAccuracyTrendByAttributeViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportAccuracyTrendByAttributeViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportAccuracyTrendByAttributeViewModel.TransactionEndDate).Second == 0)
                        {
                            reportAccuracyTrendByAttributeViewModel.TransactionEndDate = ((DateTime)reportAccuracyTrendByAttributeViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportAccuracyTrendByAttributeViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportAccuracyTrendByAttributeViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportAccuracyTrendByAttributeViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportAccuracyTrendByAttributeViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportAccuracyTrendByAttributeViewModel.EvaluationEndDate = ((DateTime)reportAccuracyTrendByAttributeViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultAccuracyTrendResultList = report.AccuracyTrend(
                        reportAccuracyTrendByAttributeViewModel.TransactionStartDate,
                        reportAccuracyTrendByAttributeViewModel.TransactionEndDate,
                        reportAccuracyTrendByAttributeViewModel.EvaluationStartDate,
                        reportAccuracyTrendByAttributeViewModel.EvaluationEndDate,
                        reportAccuracyTrendByAttributeViewModel.ProgramIDArray != null
                            ? String.Join(",", reportAccuracyTrendByAttributeViewModel.ProgramIDArray)
                            : string.Empty,
                        reportAccuracyTrendByAttributeViewModel.UserIDArray != null
                            ? String.Join(",", reportAccuracyTrendByAttributeViewModel.UserIDArray)
                            : string.Empty,
                        reportAccuracyTrendByAttributeViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportAccuracyTrendByAttributeViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportAccuracyTrendByAttributeViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportAccuracyTrendByAttributeViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        reportAccuracyTrendByAttributeViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportAccuracyTrendByAttributeViewModel.ErrorTypeIDArray)
                            : string.Empty,
                        reportAccuracyTrendByAttributeViewModel.AttributeNoConstraint != null
                            ? !reportAccuracyTrendByAttributeViewModel.AttributeNoConstraint.Value
                                ? reportAccuracyTrendByAttributeViewModel.AttributeControllable
                                : null
                            : null,
                        reportAccuracyTrendByAttributeViewModel.AttributeNoConstraint != null
                            ? !reportAccuracyTrendByAttributeViewModel.AttributeNoConstraint.Value
                                ? reportAccuracyTrendByAttributeViewModel.AttributeKnown
                                : null
                            : null,
                        reportAccuracyTrendByAttributeViewModel.TransactionCustomFieldCatalog);

                    reportResultsAccuracyTrendViewModel = new ViewModels.ReportResultsAccuracyTrendViewModel(resultAccuracyTrendResultList, (SCC_BL.DBValues.Catalog.TIME_INTERVAL)reportAccuracyTrendByAttributeViewModel.IntervalTypeID);
                    /*reportResultsAccuracyTrendViewModel.ReportResultsAccuracyTrendViewModel = reportResultsAccuracyTrendViewModel;*/

                    resultAccuracyTrendByAttribute = report.AccuracyTrendByAttribute(
                        String.Join(",", resultAccuracyTrendResultList.Select(e => e.TransactionID)),
                        String.Join(",", reportAccuracyTrendByAttributeViewModel.ErrorTypeIDArray),
                        String.Join(",", reportAccuracyTrendByAttributeViewModel.AttributeIDArray));

                    reportResultsAccuracyTrendByAttributeViewModel = 
                        new ViewModels.ReportResultsAccuracyTrendByAttributeViewModel(
                            resultAccuracyTrendByAttribute, 
                            (SCC_BL.DBValues.Catalog.TIME_INTERVAL)reportAccuracyTrendByAttributeViewModel.IntervalTypeID);
                    /*reportAccuracyByAttributeViewModel.ReportResultsAccuracyByAttributeViewModel = reportResultsAccuracyByAttributeViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.AccuracyTrendByAttribute.Error>(null, null, reportAccuracyTrendByAttributeViewModel, ex);
            }

            reportResultsAccuracyTrendByAttributeViewModel.RequestObject = reportAccuracyTrendByAttributeViewModel;
            reportResultsAccuracyTrendByAttributeViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.AccuracyTrendByAttributeResults), reportResultsAccuracyTrendByAttributeViewModel);
        }

        [HttpPost]
        public ActionResult AccuracyByAttributeWithOverallAcurracy(string transactionIDList, int errorTypeID, int totalTransactions)
        {
            List<SCC_BL.Reports.Results.AccuracyByAttribute> resultAccuracyByAttribute = new List<SCC_BL.Reports.Results.AccuracyByAttribute>();
            ViewModels.ReportResultsAccuracyByAttributeViewModel reportResultsAccuracyByAttributeViewModel = new ViewModels.ReportResultsAccuracyByAttributeViewModel();

            try
            {
                using (Report report = new Report())
                {
                    resultAccuracyByAttribute = report.AccuracyByAttribute(
                        transactionIDList,
                        errorTypeID.ToString());

                    reportResultsAccuracyByAttributeViewModel = new ViewModels.ReportResultsAccuracyByAttributeViewModel(resultAccuracyByAttribute, totalTransactions);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.AccuracyByAttribute.Error>(null, null, reportResultsAccuracyByAttributeViewModel, ex);
            }

            return View(nameof(ReportController.AccuracyByAttributeResults), reportResultsAccuracyByAttributeViewModel);
        }

        [HttpPost]
        public ActionResult AccuracyTrendByAttributeWithAcurracyTrend(string transactionIDList, int errorTypeID, int intervalTypeID)
        {
            List<SCC_BL.Reports.Results.AccuracyTrendByAttribute> resultAccuracyTrendByAttribute = new List<SCC_BL.Reports.Results.AccuracyTrendByAttribute>();
            ViewModels.ReportResultsAccuracyTrendByAttributeViewModel reportResultsAccuracyTrendByAttributeViewModel = new ViewModels.ReportResultsAccuracyTrendByAttributeViewModel();

            try
            {
                using (Report report = new Report())
                {
                    resultAccuracyTrendByAttribute = report.AccuracyTrendByAttribute(
                        transactionIDList,
                        errorTypeID.ToString());

                    reportResultsAccuracyTrendByAttributeViewModel = 
                        new ViewModels.ReportResultsAccuracyTrendByAttributeViewModel(
                            resultAccuracyTrendByAttribute,
                            (SCC_BL.DBValues.Catalog.TIME_INTERVAL)intervalTypeID);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.AccuracyByAttribute.Error>(null, null, reportResultsAccuracyTrendByAttributeViewModel, ex);
            }

            return View(nameof(ReportController.AccuracyByAttributeResults), reportResultsAccuracyTrendByAttributeViewModel);
        }

        [HttpPost]
        public ActionResult AccuracyByAttribute(ViewModels.ReportAccuracyByAttributeViewModel reportAccuracyByAttributeViewModel)
        {
            List<SCC_BL.Reports.Results.OverallAccuracy> resultOverallAccuracy = new List<SCC_BL.Reports.Results.OverallAccuracy>();
            ViewModels.ReportResultsOverallAccuracyViewModel reportResultsOverallAccuracyViewModel = new ViewModels.ReportResultsOverallAccuracyViewModel();

            List<SCC_BL.Reports.Results.AccuracyByAttribute> resultAccuracyByAttribute = new List<SCC_BL.Reports.Results.AccuracyByAttribute>();
            ViewModels.ReportResultsAccuracyByAttributeViewModel reportResultsAccuracyByAttributeViewModel = new ViewModels.ReportResultsAccuracyByAttributeViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportAccuracyByAttributeViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportAccuracyByAttributeViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportAccuracyByAttributeViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportAccuracyByAttributeViewModel.TransactionEndDate).Second == 0)
                        {
                            reportAccuracyByAttributeViewModel.TransactionEndDate = ((DateTime)reportAccuracyByAttributeViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportAccuracyByAttributeViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportAccuracyByAttributeViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportAccuracyByAttributeViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportAccuracyByAttributeViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportAccuracyByAttributeViewModel.EvaluationEndDate = ((DateTime)reportAccuracyByAttributeViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultOverallAccuracy = report.OverallAccuracy(
                        reportAccuracyByAttributeViewModel.TransactionStartDate,
                        reportAccuracyByAttributeViewModel.TransactionEndDate,
                        reportAccuracyByAttributeViewModel.EvaluationStartDate,
                        reportAccuracyByAttributeViewModel.EvaluationEndDate,
                        reportAccuracyByAttributeViewModel.ProgramIDArray != null
                            ? String.Join(",", reportAccuracyByAttributeViewModel.ProgramIDArray)
                            : string.Empty,
                        reportAccuracyByAttributeViewModel.UserIDArray != null
                            ? String.Join(",", reportAccuracyByAttributeViewModel.UserIDArray)
                            : string.Empty,
                        reportAccuracyByAttributeViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportAccuracyByAttributeViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportAccuracyByAttributeViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportAccuracyByAttributeViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        reportAccuracyByAttributeViewModel.ErrorTypeIDArray != null
                            ? String.Join(",", reportAccuracyByAttributeViewModel.ErrorTypeIDArray)
                            : string.Empty,
                        reportAccuracyByAttributeViewModel.AttributeNoConstraint != null
                            ? !reportAccuracyByAttributeViewModel.AttributeNoConstraint.Value
                                ? reportAccuracyByAttributeViewModel.AttributeControllable
                                : null
                            : null,
                        reportAccuracyByAttributeViewModel.AttributeNoConstraint != null
                            ? !reportAccuracyByAttributeViewModel.AttributeNoConstraint.Value
                                ? reportAccuracyByAttributeViewModel.AttributeKnown
                                : null
                            : null,
                        reportAccuracyByAttributeViewModel.TransactionCustomFieldCatalog);

                    reportResultsOverallAccuracyViewModel = new ViewModels.ReportResultsOverallAccuracyViewModel(resultOverallAccuracy);
                    /*reportOverallAccuracyViewModel.ReportResultsOverallAccuracyViewModel = reportResultsOverallAccuracyViewModel;*/

                    resultAccuracyByAttribute = report.AccuracyByAttribute(
                        String.Join(",", resultOverallAccuracy.Select(e => e.TransactionID)),
                        String.Join(",", reportAccuracyByAttributeViewModel.ErrorTypeIDArray),
                        String.Join(",", reportAccuracyByAttributeViewModel.AttributeIDArray));

                    reportResultsAccuracyByAttributeViewModel = new ViewModels.ReportResultsAccuracyByAttributeViewModel(resultAccuracyByAttribute, reportResultsOverallAccuracyViewModel.TotalTransactions);
                    /*reportAccuracyByAttributeViewModel.ReportResultsAccuracyByAttributeViewModel = reportResultsAccuracyByAttributeViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.AccuracyByAttribute.Error>(null, null, reportAccuracyByAttributeViewModel, ex);
            }

            reportResultsAccuracyByAttributeViewModel.RequestObject = reportAccuracyByAttributeViewModel;
            reportResultsAccuracyByAttributeViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.AccuracyByAttributeResults), reportResultsAccuracyByAttributeViewModel);
        }

        [HttpPost]
        public ActionResult ParetoBI(ViewModels.ReportParetoBIViewModel reportParetoBIViewModel)
        {
            List<SCC_BL.Reports.Results.OverallAccuracy> resultOverallAccuracy = new List<SCC_BL.Reports.Results.OverallAccuracy>();
            ViewModels.ReportResultsOverallAccuracyViewModel reportResultsOverallAccuracyViewModel = new ViewModels.ReportResultsOverallAccuracyViewModel();

            List<SCC_BL.Reports.Results.ParetoBI> resultParetoBI = new List<SCC_BL.Reports.Results.ParetoBI>();
            ViewModels.ReportResultsParetoBIViewModel reportResultsParetoBIViewModel = new ViewModels.ReportResultsParetoBIViewModel();

            try
            {
                using (Report report = new Report())
                {
                    if (reportParetoBIViewModel.TransactionEndDate != null)
                    {
                        if (
                            ((DateTime)reportParetoBIViewModel.TransactionEndDate).Hour == 0 &&
                            ((DateTime)reportParetoBIViewModel.TransactionEndDate).Minute == 0 &&
                            ((DateTime)reportParetoBIViewModel.TransactionEndDate).Second == 0)
                        {
                            reportParetoBIViewModel.TransactionEndDate = ((DateTime)reportParetoBIViewModel.TransactionEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    if (reportParetoBIViewModel.EvaluationEndDate != null)
                    {
                        if (
                            ((DateTime)reportParetoBIViewModel.EvaluationEndDate).Hour == 0 &&
                            ((DateTime)reportParetoBIViewModel.EvaluationEndDate).Minute == 0 &&
                            ((DateTime)reportParetoBIViewModel.EvaluationEndDate).Second == 0)
                        {
                            reportParetoBIViewModel.EvaluationEndDate = ((DateTime)reportParetoBIViewModel.EvaluationEndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                        }
                    }

                    resultOverallAccuracy = report.OverallAccuracy(
                        reportParetoBIViewModel.TransactionStartDate,
                        reportParetoBIViewModel.TransactionEndDate,
                        reportParetoBIViewModel.EvaluationStartDate,
                        reportParetoBIViewModel.EvaluationEndDate,
                        reportParetoBIViewModel.ProgramIDArray != null
                            ? String.Join(",", reportParetoBIViewModel.ProgramIDArray)
                            : string.Empty,
                        reportParetoBIViewModel.UserIDArray != null
                            ? String.Join(",", reportParetoBIViewModel.UserIDArray)
                            : string.Empty,
                        reportParetoBIViewModel.SupervisorUserIDArray != null
                            ? String.Join(",", reportParetoBIViewModel.SupervisorUserIDArray)
                            : string.Empty,
                        reportParetoBIViewModel.EvaluatorUserIDArray != null
                            ? String.Join(",", reportParetoBIViewModel.EvaluatorUserIDArray)
                            : string.Empty,
                        string.Empty,
                        null,
                        null,
                        reportParetoBIViewModel.TransactionCustomFieldCatalog);

                    reportResultsOverallAccuracyViewModel = new ViewModels.ReportResultsOverallAccuracyViewModel(resultOverallAccuracy);
                    /*reportOverallAccuracyViewModel.ReportResultsOverallAccuracyViewModel = reportResultsOverallAccuracyViewModel;*/

                    resultParetoBI = report.ParetoBI(
                        String.Join(",", resultOverallAccuracy.Select(e => e.TransactionID)),
                        String.Join(",", reportParetoBIViewModel.BIFieldIDArray));

                    reportResultsParetoBIViewModel = new ViewModels.ReportResultsParetoBIViewModel(resultParetoBI, reportResultsOverallAccuracyViewModel.TotalTransactions);
                    /*reportParetoBIViewModel.ReportResultsParetoBIViewModel = reportResultsParetoBIViewModel;*/
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.ParetoBI.Error>(null, null, reportParetoBIViewModel, ex);
            }

            reportResultsParetoBIViewModel.RequestObject = reportParetoBIViewModel;
            reportResultsParetoBIViewModel.RequestObject.SetDescriptiveData();

            return View(nameof(ReportController.ParetoBIResults), reportResultsParetoBIViewModel);
        }

        [HttpPost]
        public ActionResult AccuracyBySubattribute(string transactionIDList, int totalTransactions)
        {
            List<SCC_BL.Reports.Results.AccuracyBySubattribute> resultAccuracyBySubattribute = new List<SCC_BL.Reports.Results.AccuracyBySubattribute>();
            ViewModels.ReportResultsAccuracyBySubattributeViewModel reportResultsAccuracyBySubattributeViewModel = new ViewModels.ReportResultsAccuracyBySubattributeViewModel();

            try
            {
                using (Report report = new Report())
                {
                    resultAccuracyBySubattribute = report.AccuracyBySubattribute(
                        transactionIDList);

                    reportResultsAccuracyBySubattributeViewModel = new ViewModels.ReportResultsAccuracyBySubattributeViewModel(resultAccuracyBySubattribute, totalTransactions);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Report.AccuracyBySubattribute.Error>(null, null, reportResultsAccuracyBySubattributeViewModel, ex);
            }

            return View(nameof(ReportController.AccuracyBySubattributeResults), reportResultsAccuracyBySubattributeViewModel);
        }

        public ActionResult OverallAccuracyResults(ViewModels.ReportResultsOverallAccuracyViewModel reportResultsOverallAccuracyViewModel)
        {
            return View(reportResultsOverallAccuracyViewModel);
        }

        public ActionResult ComparativeByUserResults(ViewModels.ReportResultsComparativeByUserViewModel reportResultsComparativeByUserViewModel)
        {
            return View(reportResultsComparativeByUserViewModel);
        }

        public ActionResult ComparativeByProgramResults(ViewModels.ReportResultsComparativeByProgramViewModel reportResultsComparativeByProgramViewModel)
        {
            return View(reportResultsComparativeByProgramViewModel);
        }

        public ActionResult CalibratorComparisonResults(ViewModels.ReportResultsCalibratorComparisonViewModel reportResultsCalibratorComparisonViewModel)
        {
            return View(reportResultsCalibratorComparisonViewModel);
        }

        public ActionResult AccuracyTrendResults(ViewModels.ReportResultsAccuracyTrendViewModel reportResultsAccuracyTrendViewModel)
        {
            return View(reportResultsAccuracyTrendViewModel);
        }

        public ActionResult AccuracyTrendByAttributeResults(ViewModels.ReportResultsAccuracyTrendByAttributeViewModel reportResultsAccuracyTrendByAttributeViewModel)
        {
            return View(reportResultsAccuracyTrendByAttributeViewModel);
        }

        public ActionResult AccuracyByAttributeResults(ViewModels.ReportResultsAccuracyByAttributeViewModel reportResultsAccuracyByAttributeViewModel)
        {
            return View(reportResultsAccuracyByAttributeViewModel);
        }

        public ActionResult AccuracyBySubattributeResults(ViewModels.ReportResultsAccuracyBySubattributeViewModel reportResultsAccuracyBySubattributeViewModel)
        {
            return View(reportResultsAccuracyBySubattributeViewModel);
        }

        public ActionResult ParetoBIResults(ViewModels.ReportResultsParetoBIViewModel reportResultsParetoBIViewModel)
        {
            return View(reportResultsParetoBIViewModel);
        }

        public string AttributesByProgramAndErrorID(int[] programIDArray, int[] errorTypeIDArray)
        {
            if (programIDArray == null) return string.Empty;
            if (errorTypeIDArray == null) return string.Empty;

            List<SCC_BL.Attribute> attributeList = new List<SCC_BL.Attribute>();

            using (SCC_BL.Attribute attribute = new SCC_BL.Attribute())
            {
                attributeList = 
                    attribute.SelectByProgramAndErrorTypeID(
                        String.Join(",", programIDArray),
                        String.Join(",", errorTypeIDArray));
            }

            return Serialize(attributeList);
        }
    }
}