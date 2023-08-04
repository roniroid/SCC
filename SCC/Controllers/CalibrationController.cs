using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SCC_BL.Settings.AppValues;

namespace SCC.Controllers
{
    public class CalibrationController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(CalibrationController));

        public ActionResult Manage()
        {
            List<Calibration> calibrationList = new Calibration().SelectAll();

            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_ALL_CALIBRATION_SESSIONS))
            {
                calibrationList = 
                    calibrationList
                        .Where(e =>
                            e.GetUserList()
                                .Select(s => s.ID)
                                .Contains(GetActualUser().ID))
                        .ToList();
            }

            return View(calibrationList);
        }
        public ActionResult CalibrationTypes(int? calibrationTypeID)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_CALIBRATION_OPTIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.CalibrationTypes.NotAllowedToCreateCalibrationOptions>();
                return RedirectToAction(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)));
            }

            CalibrationTypeViewModel calibrationTypeViewModel = new CalibrationTypeViewModel();

            if (calibrationTypeID != null)
            {
                calibrationTypeViewModel.Catalog = new Catalog(calibrationTypeID.Value);
                calibrationTypeViewModel.Catalog.SetDataByID();
            }
            else
            {
                calibrationTypeViewModel.Catalog.Active = true;
                calibrationTypeViewModel.Catalog.CategoryID = (int)SCC_BL.DBValues.Catalog.Category.CALIBRATION_TYPE;
            }

            List<Catalog> catalogCalibrationTypeList = new List<Catalog>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.CALIBRATION_TYPE))
                catalogCalibrationTypeList = catalog.SelectByCategoryID();

            calibrationTypeViewModel.CalibrationTypeList = catalogCalibrationTypeList;

            return View(calibrationTypeViewModel);
        }

        public ActionResult CheckCalibration(int calibratedTransactionID, int calibrationSessionID)
        {
            CalibrationCheckResultsViewModel calibrationCheckResultsViewModel = new CalibrationCheckResultsViewModel();

            calibrationCheckResultsViewModel.CalibratedTransaction = new Transaction(calibratedTransactionID);
            calibrationCheckResultsViewModel.CalibratedTransaction.SetDataByID();

            using (Transaction transaction = Transaction.TransactionWithCalibratedTransactionID(calibratedTransactionID))
            {
                calibrationCheckResultsViewModel.CalibrationList = transaction.SelectByCalibratedTransactionID();
            }

            calibrationCheckResultsViewModel.SelectedCalibration = 
                calibrationCheckResultsViewModel
                    .CalibrationList
                        .Where(e => 
                            e.EvaluatorUserID == GetActualUser().ID)
                        .FirstOrDefault();

            calibrationCheckResultsViewModel.CalibrationSession = new Calibration(calibrationSessionID);
            calibrationCheckResultsViewModel.CalibrationSession.SetDataByID();

            return View(calibrationCheckResultsViewModel);
        }

        public ActionResult CheckGlobalResultsByCalibrator(int calibrationSessionID)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_CALIBRATION_RESULTS))
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.Results.NotAllowedToSeeCalibrationResults>();
                return RedirectToAction(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)));
            }

            Calibration calibrationSession = new Calibration(calibrationSessionID);
            calibrationSession.SetDataByID();

            List<SCC_BL.Transaction> calibrationList = new List<SCC_BL.Transaction>();
            List<SCC_BL.Transaction> calibratedTransactionList = new List<SCC_BL.Transaction>();

            foreach (CalibrationTransactionCatalog calibrationTransactionCatalog in calibrationSession.TransactionList)
            {
                using (Transaction transaction = new Transaction(calibrationTransactionCatalog.TransactionID))
                {
                    if (calibratedTransactionList.Select(e => e.ID).Contains(calibrationTransactionCatalog.TransactionID))
                        continue;

                    transaction.SetDataByID();
                    calibratedTransactionList.Add(transaction);

                    using (Transaction calibration = Transaction.TransactionWithCalibratedTransactionID(transaction.ID))
                        calibrationList.AddRange(calibration.SelectByCalibratedTransactionID());
                }
            }

            CalibrationResultsByCalibratorViewModel calibrationResultsByCalibratorViewModel = 
                new CalibrationResultsByCalibratorViewModel(calibrationSession, calibrationList, calibratedTransactionList);

            return View(calibrationResultsByCalibratorViewModel);
        }

        public ActionResult CheckGlobalResultsByTransaction(int calibrationSessionID)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_CALIBRATION_RESULTS))
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.Results.NotAllowedToSeeCalibrationResults>();
                return RedirectToAction(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)));
            }

            try
            {
                Calibration calibrationSession = new Calibration(calibrationSessionID);
                calibrationSession.SetDataByID();

                List<SCC_BL.Transaction> calibrationList = new List<SCC_BL.Transaction>();
                /*List<SCC_BL.Transaction> calibratedTransactionList = new List<SCC_BL.Transaction>();*/

                foreach (CalibrationTransactionCatalog calibrationTransactionCatalog in calibrationSession.TransactionList)
                {
                    using (Transaction transaction = new Transaction(calibrationTransactionCatalog.TransactionID))
                    {
                        /*if (calibratedTransactionList.Select(e => e.ID).Contains(calibrationTransactionCatalog.TransactionID))
                            continue;

                        transaction.SetDataByID();
                        calibratedTransactionList.Add(transaction);*/

                        calibrationList.AddRange(
                            calibrationSession.CalibrationList
                                .Where(e =>
                                    e.CalibratedTransactionID == transaction.ID));

                        /*using (Transaction calibration = Transaction.TransactionWithCalibratedTransactionID(transaction.ID))
                            calibrationList.AddRange(calibration.SelectByCalibratedTransactionID());*/
                    }
                }

                CalibrationResultsByTransactionViewModel calibrationResultsByTransactionViewModel =
                    new CalibrationResultsByTransactionViewModel(calibrationSession/*, calibrationList*/);

                return View(calibrationResultsByTransactionViewModel);
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.Update.Error>(null, null, null, ex);
            }
            return RedirectToAction(nameof(CalibrationController.Manage), _mainControllerName);
        }

        [HttpGet]
        public ActionResult Edit(string transactionIDList, int? calibrationID = null)
        {
            Calibration calibration = new Calibration();

            if (calibrationID != null)
            {
                calibration = new Calibration(calibrationID.Value);
                calibration.SetDataByID();
            }

            List<User> expertUserList = new List<User>();
            List<User> calibratorUserList = new List<User>();
            List<Group> calibratorUserGroupList = new List<Group>();
            List<Catalog> catalogCalibrationTypeList = new List<Catalog>();
            List<Transaction> transactionList = new List<Transaction>();

            using (User user = new User())
            {
                expertUserList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            !expertUserList.Select(s => s.ID).Contains(e.ID))
                        .ToList());

                expertUserList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERUSER)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            !expertUserList.Select(s => s.ID).Contains(e.ID))
                        .ToList());

                expertUserList =
                    expertUserList
                        .GroupBy(e =>
                            e.ID)
                        .Select(e =>
                            e.First())
                        .ToList();
            }

            using (User user = new User())
            {
                calibratorUserList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.CALIBRATOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            !calibratorUserList.Select(s => s.ID).Contains(e.ID))
                        .ToList());

                calibratorUserList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.FACILITATOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            !calibratorUserList.Select(s => s.ID).Contains(e.ID))
                        .ToList());

                calibratorUserList.AddRange(
                    user.SelectByRoleID((int)SCC_BL.DBValues.Catalog.USER_ROLE.MONITOR)
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            !calibratorUserList.Select(s => s.ID).Contains(e.ID))
                        .ToList());

                calibratorUserList =
                    calibratorUserList
                        .GroupBy(e =>
                            e.ID)
                        .Select(e =>
                            e.First())
                        .ToList();
            }

            using (Group group = new Group())
                calibratorUserGroupList =
                    group.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DISABLED &&
                            e.ApplicableModuleID == (int)SCC_BL.DBValues.Catalog.MODULE.CALIBRATION)
                        .ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.CALIBRATION_TYPE))
                catalogCalibrationTypeList = catalog.SelectByCategoryID();

            if (calibration.ID > 0)
            {
                foreach (CalibrationTransactionCatalog calibrationTransactionCatalog in calibration.TransactionList)
                {
                    using (Transaction transaction = new Transaction(calibrationTransactionCatalog.TransactionID))
                    {
                        transaction.SetDataByID();
                        transactionList.Add(transaction);
                    }
                }
            }
            else
            if (!string.IsNullOrEmpty(transactionIDList))
            {
                for (int i = 0; i < transactionIDList.Split(',').Length; i++)
                {
                    if (!string.IsNullOrEmpty(transactionIDList.Split(',')[i]))
                    {
                        using (Transaction transaction = new Transaction(Convert.ToInt32(transactionIDList.Split(',')[i])))
                        {
                            transaction.SetDataByID();
                            transactionList.Add(transaction);
                        }
                    }
                }
            }

            int? experiencedUserID = null;

            if (calibration.ID > 0)
                experiencedUserID = calibration.ExperiencedUserID;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Edit.ExpertUserList.NAME] =
                new SelectList(
                    expertUserList.Select(e => new { Key = e.ID, Value = $"{ e.Person.SurName } { e.Person.LastName }, { e.Person.FirstName } (id: { e.Person.Identification })" }),
                    "Key",
                    "Value",
                    experiencedUserID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Edit.CalibratorUserList.NAME] =
                new MultiSelectList(
                    calibratorUserList.Select(e => new { Key = e.ID, Value = $"{ e.Person.SurName } { e.Person.LastName }, { e.Person.FirstName } (id: { e.Person.Identification })" }),
                    "Key",
                    "Value",
                    calibration.ID > 0
                        ? calibration.CalibrationUserCatalogList.Select(e => e.UserID)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Edit.CalibratorUserGroupList.NAME] =
                new MultiSelectList(
                    calibratorUserGroupList,
                    nameof(Group.ID),
                    nameof(Group.Name),
                    calibration.ID > 0
                        ? calibration.CalibratorUserGroupList.Select(e => e.GroupID)
                        : null);

            int? calibrationTypeID = null;

            if (calibration.ID > 0)
                calibrationTypeID = calibration.TypeID;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Edit.CalibrationTypeList.NAME] =
                new SelectList(
                    catalogCalibrationTypeList,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    calibrationTypeID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Edit.TransactionList.NAME] = transactionList;

            CalibrationEditViewModel calibrationEditViewModel = new CalibrationEditViewModel();

            calibrationEditViewModel.Calibration = calibration;
            calibrationEditViewModel.TransactionList = transactionList;

            return View(calibrationEditViewModel);
        }

        [HttpPost]
        public ActionResult Edit(Calibration calibration, int[] calibratorUserList, int[] calibratorUserGroupList, string transactionList)
        {
            User currentUser = GetActualUser();

            int[] transactionIDArray = new int[transactionList.Split(',').Length];

            if (!string.IsNullOrEmpty(transactionList))
            {
                for (int i = 0; i < transactionList.Split(',').Length; i++)
                {
                    if (!string.IsNullOrEmpty(transactionList.Split(',')[i]))
                        transactionIDArray[i] = Convert.ToInt32(transactionList.Split(',')[i]);
                }
            }

            Calibration oldCalibration = new Calibration(calibration.ID);

            bool isNew = true;

            if (oldCalibration.ID > 0)
                isNew = false;

            if (!isNew)
                oldCalibration.SetDataByID();

            Calibration newCalibration = new Calibration();

            int result = 0;

            if (isNew)
            {
                newCalibration = 
                    new Calibration(
                        calibration.StartDate,
                        calibration.EndDate,
                        calibration.Description,
                        calibration.TypeID,
                        calibration.ExperiencedUserID,
                        calibration.HasNotificationToBeSent,
                        currentUser.ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION.CREATED
                    );

                result = newCalibration.Insert();

                try
                {

                    if (result > 0)
                    {
                        UpdateCalibratorUserList(newCalibration, calibratorUserList);

                        UpdateCalibratorUserGroupList(newCalibration, calibratorUserGroupList);

                        UpdateTransactionList(newCalibration, transactionIDArray);

                        if (newCalibration.HasNotificationToBeSent)
                        {
                            List<string> transactionIdentifierArray = new List<string>();
                            List<int> usersToNotifyList = new List<int>();

                            for (int i = 0; i < transactionIDArray.Length; i++)
                            {
                                Transaction transaction = new Transaction(transactionIDArray[i]);
                                transaction.SetDataByID(true);

                                transactionIdentifierArray.Add(transaction.Identifier);

                                usersToNotifyList.AddRange(GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION, transactionIDArray[i]));
                            }

                            usersToNotifyList = usersToNotifyList.Distinct().ToList();

                            SendCalibrationNotifications(transactionIdentifierArray.ToArray(), usersToNotifyList.ToArray());

                            string message =
                                SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.Creation.AGENT_CALIBRATION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_CALIBRATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName} {currentUser.Person.LastName}, {currentUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_TRANSACTION_IDENTIFIERS, String.Join(", ", transactionIdentifierArray.Select(e => "\"" + e + "\"")));

                            CalibrationSessionCreatedSendMail(usersToNotifyList.ToArray(), message);
                        }

                        SaveProcessingInformation<SCC_BL.Results.Calibration.Insert.Success>(newCalibration.ID, newCalibration.BasicInfo.StatusID, newCalibration);
                    }
                    else
                    {
                        switch ((SCC_BL.Results.Calibration.Insert.CODE)result)
                        {
                            case SCC_BL.Results.Calibration.Insert.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.Calibration.Insert.Error>(null, null, newCalibration);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SaveProcessingInformation<SCC_BL.Results.Calibration.Insert.Error>(newCalibration.ID, newCalibration.BasicInfo.StatusID, newCalibration, ex);
                }
            }
            else
            {
                newCalibration =
                    new Calibration(
                        calibration.ID,
                        calibration.StartDate,
                        calibration.EndDate,
                        calibration.Description,
                        calibration.TypeID,
                        calibration.ExperiencedUserID,
                        calibration.HasNotificationToBeSent,
                        calibration.BasicInfoID,
                        currentUser.ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION.UPDATED
                    );

                result = newCalibration.Update();

                try
                {
                    if (result > 0)
                    {
                        UpdateCalibratorUserList(oldCalibration, calibratorUserList);

                        UpdateCalibratorUserGroupList(oldCalibration, calibratorUserGroupList);

                        UpdateTransactionList(oldCalibration, transactionIDArray);

                        if (newCalibration.HasNotificationToBeSent)
                        {
                            List<string> transactionIdentifierArray = new List<string>();
                            List<int> usersToNotifyList = new List<int>();

                            for (int i = 0; i < transactionIDArray.Length; i++)
                            {
                                Transaction transaction = new Transaction(transactionIDArray[i]);
                                transaction.SetDataByID(true);

                                transactionIdentifierArray.Add(transaction.Identifier);

                                usersToNotifyList.AddRange(GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION, transactionIDArray[i]));
                            }

                            usersToNotifyList = usersToNotifyList.Distinct().ToList();

                            SendCalibrationNotifications(transactionIdentifierArray.ToArray(), usersToNotifyList.ToArray(), false, oldCalibration);

                            string message =
                                SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.Update.AGENT_CALIBRATION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_CALIBRATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName} {currentUser.Person.LastName}, {currentUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_TRANSACTION_IDENTIFIERS, String.Join(", ", transactionIdentifierArray.Select(e => "\"" + e + "\"")));

                            CalibrationSessionCreatedSendMail(usersToNotifyList.ToArray(), message);
                        }

                        SaveProcessingInformation<SCC_BL.Results.Calibration.Update.Success>(newCalibration.ID, newCalibration.BasicInfo.StatusID, oldCalibration);
                    }
                    else
                    {
                        switch ((SCC_BL.Results.Calibration.Update.CODE)result)
                        {
                            case SCC_BL.Results.Calibration.Update.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.Calibration.Update.Error>(oldCalibration.ID, oldCalibration.BasicInfo.StatusID, newCalibration);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SaveProcessingInformation<SCC_BL.Results.Calibration.Update.Error>(oldCalibration.ID, oldCalibration.BasicInfo.StatusID, oldCalibration, ex);
                }
            }

            return RedirectToAction(nameof(CalibrationController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult EditType(Catalog catalog)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_CALIBRATION_OPTIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.CalibrationTypes.NotAllowedToCreateCalibrationOptions>();
                return RedirectToAction(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)));
            }

            Catalog oldCatalog = new Catalog(catalog.ID);

            bool isNew = true;

            if (oldCatalog.ID > 0)
                isNew = false;

            if (!isNew)
                oldCatalog.SetDataByID();

            Catalog newCatalog = new Catalog();

            int result = 0;

            if (isNew)
            {
                newCatalog = 
                    new Catalog(
                        catalog.CategoryID,
                        catalog.Description,
                        catalog.Active
                    );

                result = newCatalog.Insert();

                try
                {
                    if (result > 0)
                    {
                        SaveProcessingInformation<SCC_BL.Results.Catalog.Insert.Success>(null, null, newCatalog);
                    }
                    else
                    {
                        switch ((SCC_BL.Results.Catalog.Insert.CODE)result)
                        {
                            case SCC_BL.Results.Catalog.Insert.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.Catalog.Insert.Error>(null, null, newCatalog);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SaveProcessingInformation<SCC_BL.Results.Catalog.Insert.Error>(null, null, newCatalog, ex);
                }
            }
            else
            {
                //int id, int? categoryID, string description, bool active
                newCatalog =
                    new Catalog(
                        catalog.ID,
                        catalog.CategoryID,
                        catalog.Description,
                        catalog.Active
                    );

                result = newCatalog.Update();

                try
                {
                    if (result > 0)
                    {
                        SaveProcessingInformation<SCC_BL.Results.Catalog.Update.Success>(null, null, oldCatalog);
                    }
                    else
                    {
                        switch ((SCC_BL.Results.Catalog.Update.CODE)result)
                        {
                            case SCC_BL.Results.Catalog.Update.CODE.ERROR:
                                SaveProcessingInformation<SCC_BL.Results.Catalog.Update.Error>(null, null, newCatalog);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SaveProcessingInformation<SCC_BL.Results.Catalog.Update.Error>(null, null, oldCatalog, ex);
                }
            }

            return RedirectToAction(nameof(CalibrationController.CalibrationTypes), _mainControllerName);
        }

        void UpdateCalibratorUserList(Calibration calibration, int[] calibratorUserIDList)
        {
            try
            {
                switch (calibration.UpdateCalibratorUserList(calibratorUserIDList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Calibration.UpdateCalibratorUserList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Calibration.UpdateCalibratorUserList.Success>(calibration.ID, calibration.BasicInfo.StatusID, calibration);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.UpdateCalibratorUserList.Error>(calibration.ID, calibration.BasicInfo.StatusID, calibration, ex);
            }
        }

        void UpdateCalibratorUserGroupList(Calibration calibration, int[] calibratorUserGroupIDList)
        {
            try
            {
                switch (calibration.UpdateCalibratorUserGroupList(calibratorUserGroupIDList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Calibration.UpdateCalibratorUserGroupList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Calibration.UpdateCalibratorUserGroupList.Success>(calibration.ID, calibration.BasicInfo.StatusID, calibration);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.UpdateCalibratorUserGroupList.Error>(calibration.ID, calibration.BasicInfo.StatusID, calibration, ex);
            }
        }

        void UpdateTransactionList(Calibration calibration, int[] transactionIDList)
        {
            try
            {
                switch (calibration.UpdateTransactionList(transactionIDList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Calibration.UpdateTransactionList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Calibration.UpdateTransactionList.Success>(calibration.ID, calibration.BasicInfo.StatusID, calibration);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.UpdateTransactionList.Error>(calibration.ID, calibration.BasicInfo.StatusID, calibration, ex);
            }
        }

        public ActionResult Search(SCC_BL.Helpers.Transaction.Search.TransactionSearchHelper transactionSearchHelper = null)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CREATE_CALIBRATION_SESSIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.CalibrationSession.NotAllowedToCreateCalibrationSession>();
                return RedirectToAction(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)));
            }

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
                    program.SelectWithForm()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .GroupBy(e =>
                            e.ID)
                        .Select(e =>
                            e.First())
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Search.StringTypeID.NAME] =
                new SelectList(
                    catalogSearchStringType,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Search.TimeUnitTypeID.NAME] =
                new SelectList(
                    catalogSearchTimeUnitType,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Search.UserStatus.NAME] =
                new SelectList(
                    catalogUserStatus,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    transactionSearchHelper.UserStatusID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Search.Workspace.NAME] =
                new MultiSelectList(
                    workspaceList,
                    nameof(Workspace.ID),
                    nameof(Workspace.Name),
                    transactionSearchHelper.WorkspaceIDList != null
                        ? transactionSearchHelper.WorkspaceIDList.Select(e => e)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Search.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    transactionSearchHelper.ProgramIDList != null
                        ? transactionSearchHelper.ProgramIDList.Select(e => e)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Calibration.Search.YesNoQuestion.NAME] =
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
                            Value = ((int)SCC_BL.Settings.AppValues.ViewData.Calibration.Search.YesNoQuestion.Values.YES).ToString()
                        },
                        new SelectListItem()
                        {
                            Text = "No",
                            Value = ((int)SCC_BL.Settings.AppValues.ViewData.Calibration.Search.YesNoQuestion.Values.NO).ToString()
                        }
                    },
                    nameof(SelectListItem.Value),
                    nameof(SelectListItem.Text));

            return View(transactionSearchViewModel);
        }

        [HttpPost]
        public ActionResult Delete(int calibrationID)
        {
            Calibration calibration = new Calibration(calibrationID);
            calibration.SetDataByID();

            try
            {
                //calibration.Delete();

                calibration.BasicInfo.ModificationUserID = GetActualUser().ID;
                calibration.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION.DELETED;

                int result = calibration.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Calibration.Delete.Success>(calibration.ID, calibration.BasicInfo.StatusID, calibration);

                    return RedirectToAction(nameof(CalibrationController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.Delete.Success>(calibration.ID, calibration.BasicInfo.StatusID, calibration, ex);
            }

            return RedirectToAction(nameof(CalibrationController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult DeleteType(int typeID)
        {
            Catalog catalog = new Catalog(typeID);
            catalog.SetDataByID();

            try
            {
                int result = catalog.Delete();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Calibration.Delete.Success>(null, null, catalog);

                    return RedirectToAction(nameof(CalibrationController.CalibrationTypes), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Calibration.Delete.Success>(null, null, catalog, ex);
            }

            return RedirectToAction(nameof(CalibrationController.CalibrationTypes), _mainControllerName);
        }

        void SendCalibrationNotifications(string[] transactionIdentifierArray, int[] userIDArray, bool isNew = true, Calibration oldCalibration = null)
        {
            string url = Url.Action(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)), Request.Url.Scheme);

            List<UserNotificationUrl> userNotificationUrlList = new List<UserNotificationUrl>() {
                new UserNotificationUrl(url, "Ir a la lista de calibraciones")
            };

            for (int i = 0; i < userIDArray.Length; i++)
            {
                if (userIDArray[i] == GetActualUser().ID)
                    SaveNotification(
                        userIDArray[i],
                        (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.CALIBRATION_AGENT,
                        isNew
                            ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.Creation.CALIBRATING_USER_INVALIDATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_TRANSACTION_IDENTIFIERS, String.Join(", ", transactionIdentifierArray.Select(e => "\"" + e + "\"")))
                            : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.Update.CALIBRATING_USER_INVALIDATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_TRANSACTION_IDENTIFIERS, String.Join(", ", transactionIdentifierArray.Select(e => "\"" + e + "\"")))
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_OBJECT_INFO, Serialize(oldCalibration)),
                        userNotificationUrlList);
                else
                {
                    User auxUser = new User(userIDArray[i]);
                    auxUser.SetDataByID();

                    SaveNotification(
                        userIDArray[i],
                        (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.CALIBRATION_OTHERS,
                        isNew
                            ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.Creation.AGENT_CALIBRATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_CALIBRATING_USER, $"{ auxUser.Person.Identification } - {auxUser.Person.SurName} {auxUser.Person.LastName}, {auxUser.Person.FirstName}")
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_TRANSACTION_IDENTIFIERS, String.Join(", ", transactionIdentifierArray.Select(e => "\"" + e + "\"")))
                            : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.Update.AGENT_CALIBRATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_CALIBRATING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName} {auxUser.Person.LastName}, {auxUser.Person.FirstName}")
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_TRANSACTION_IDENTIFIERS, String.Join(", ", transactionIdentifierArray.Select(e => "\"" + e + "\"")))
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.CalibrationSession.REPLACE_OBJECT_INFO, Serialize(oldCalibration)),
                    userNotificationUrlList);
                }
            }
        }

        void CalibrationSessionCreatedSendMail(int[] userIDList, string message = null)
        {
            SCC_BL.Tools.Mail mailHelper = new SCC_BL.Tools.Mail();

            string url = Url.Action(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)), Request.Url.Scheme);

            string messageBody = string.Empty;

            messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.Calibration.CalibrationSessionCreated.PATH);
            messageBody = messageBody.Replace("%url%", url);
            messageBody = messageBody.Replace("%message%", message);

            for (int i = 0; i < userIDList.Length; i++)
            {
                User auxUser = new User(userIDList[i]);
                auxUser.SetDataByID(true);

                string auxEmail = auxUser.Email;

                try
                {
                    mailHelper.SendMail(
                        auxEmail,
                        messageBody,
                        SCC_BL.Settings.HTML_Content.Calibration.CalibrationSessionCreated.SUBJECT);

                    SaveProcessingInformation<SCC_BL.Results.Calibration.CalibrationSession.SendMail.Success>(auxUser.ID, auxUser.BasicInfo.StatusID, auxUser);
                }
                catch (Exception ex)
                {
                    SaveProcessingInformation<SCC_BL.Results.Calibration.CalibrationSession.SendMail.Error>(auxUser.ID, auxUser.BasicInfo.StatusID, ex);
                    throw ex;
                }
            }
        }
    }
}