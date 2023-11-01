using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using SCC.ViewModels;
using SCC_BL;
using SCC_BL.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SCC_BL.Settings.AppValues.ViewData.Transaction.FormView;

namespace SCC.Controllers
{
    public class TransactionController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(TransactionController));

        public ActionResult Edit(int? transactionID, int? calibratedTransactionID = null, int typeID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.EVALUATION)
        {
            if (calibratedTransactionID != null)
            {
                if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_CALIBRATE_IN_CALIBRATION_SESSIONS))
                {
                    SaveProcessingInformation<SCC_BL.Results.Calibration.Insert.NotAllowedToCalibrate>();
                    return RedirectToAction(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)));
                }
            }

            int? transactionProgram = null;
            Transaction transaction = new Transaction((SCC_BL.DBValues.Catalog.TRANSACTION_TYPE)typeID, calibratedTransactionID);
            transaction.EvaluatorUserID = GetActualUser().ID;
            transaction.SetIdentifier();

            if (transactionID != null && transactionID > 0)
            {
                transaction = new Transaction(transactionID.Value);
                transaction.SetDataByID();

                if (GetActualUser().ID != transaction.UserToEvaluateID)
                {
                    if (GetActualUser().ID == transaction.EvaluatorUserID)
                    {
                        if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_OWN_MONITORING))
                        {
                            SaveProcessingInformation<SCC_BL.Results.Transaction.Update.NotAllowedToSeeOwnMonitorings>();
                            return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
                        }
                    }
                    else
                    {
                        if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MONITOR))
                        {
                            SaveProcessingInformation<SCC_BL.Results.Transaction.Update.NotAllowedToMonitorTransactions>();
                            return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
                        }
                    }
                }

                using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(transaction.FormID))
                {
                    transactionProgram = programFormCatalog.SelectByFormID().LastOrDefault().ProgramID;
                }
            }

            List<Program> programList = new List<Program>();

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

            programList =
                programList
                    .Where(e =>
                        e.StartDate <= DateTime.Now)
                    .ToList();

            programList =
                programList
                    .Where(e =>
                        e.EndDate == null ||
                        e.EndDate >= DateTime.Now)
                    .ToList();

            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_ALL_PROGRAMS))
            {
                programList =
                    programList
                        .Where(e => GetActualUser().CurrentProgramList.Select(s => s.ID).Contains(e.ID))
                        .ToList();
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Edit.ProgramList.NAME] =
                new SelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    transactionProgram);

            List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

            using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithFormID(transaction.FormID))
                programFormCatalogList = programFormCatalog.SelectByFormID();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Edit.ProgramFormList.NAME] = programFormCatalogList;

            return View(transaction);
        }

        [HttpGet]
        public ActionResult _FormView(int? programID = null, int? formID = null, int? transactionID = null, bool hasDisputation = false, bool hasInvalidation = false, bool hasDevolution = false, int? calibratedTransactionID = null, int typeID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.EVALUATION)
        {
            try
            {
                List<User> userList = new List<User>();

                using (User user = new User())
                    userList =
                        user.SelectAll()
                            .Where(e =>
                                e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                                e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                                e.WorkspaceList
                                    .Where(w => w.Monitorable)
                                    .Count() > 0)
                            .OrderBy(o => o.Person.SurName)
                            .ThenBy(o => o.Person.FirstName)
                            .ToList();

                /*userList =
                    userList
                        .Where(e => e.ID != GetActualUser().ID)
                        .ToList();*/

                TransactionFormViewModel transactionFormViewModel = new TransactionFormViewModel();
                transactionFormViewModel.Transaction.SetIdentifier();
                transactionFormViewModel.Transaction.EvaluatorUserID = GetActualUser().ID;
                transactionFormViewModel.Transaction.CalibratedTransactionID = calibratedTransactionID;
                transactionFormViewModel.Transaction.TypeID = typeID;

                if (calibratedTransactionID != null && calibratedTransactionID > 0)
                {
                    using (Transaction currentCalibratedTransaction = new Transaction(calibratedTransactionID.Value))
                    {
                        currentCalibratedTransaction.SetDataByID(true);
                        transactionFormViewModel.Transaction.UserToEvaluateID = currentCalibratedTransaction.UserToEvaluateID;
                    }
                }

                if (transactionID != null)
                {
                    transactionFormViewModel.Transaction = new Transaction(transactionID.Value);
                    transactionFormViewModel.Transaction.SetDataByID();

                    transactionFormViewModel.Form = new Form(transactionFormViewModel.Transaction.FormID);
                    transactionFormViewModel.Form.SetDataByID();
                }

                int? userToEvaluateID = null;

                if (transactionFormViewModel.Transaction.UserToEvaluateID > 0)
                    userToEvaluateID = transactionFormViewModel.Transaction.UserToEvaluateID;

                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.UserList.NAME] =
                    new SelectList(
                        userList.Select(e => new { Key = e.ID, Value = $"{ e.Person.Identification } - { e.Person.SurName } { e.Person.FirstName }" }),
                        "Key",
                        "Value",
                        userToEvaluateID);

                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.HasDisputation.NAME] = hasDisputation;
                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.HasInvalidation.NAME] = hasInvalidation;
                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.HasDevolution.NAME] = hasDevolution;

                if (transactionID != null)
                    return PartialView(transactionFormViewModel);

                if (formID != null)
                {
                    transactionFormViewModel.Form = new Form(formID.Value);
                    transactionFormViewModel.Form.SetDataByID();
                    return PartialView(transactionFormViewModel);
                }
                else
                if (programID != null)
                {
                    using (ProgramFormCatalog programFormCatalog = ProgramFormCatalog.ProgramFormCatalogWithProgramID(programID.Value))
                    {
                        transactionFormViewModel.Form.ID = programFormCatalog.SelectByProgramID().LastOrDefault().FormID;
                        transactionFormViewModel.Form.SetDataByID();
                        return PartialView(transactionFormViewModel);
                    }
                }

                SaveProcessingInformation<SCC_BL.Results.Transaction.FormView.Error>();
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.FormView.Error>(null, null, null, ex);
            }

            return Json(new { error_url = Url.Action(nameof(TransactionController.Edit), _mainControllerName) }, JsonRequestBehavior.AllowGet);

            //return RedirectToAction();
        }

        void SendDisputeMail(int transactionID, string transactionIdentifier, bool isUpdate = false, string oldCommentary = null)
        {
            User currentUser = GetActualUser();

            string message = string.Empty;

            if (!isUpdate)
            {
                message =
                    SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.Creation.AGENT_DISPUTATION
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_DISPUTATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_TRANSACTION_IDENTIFIER, transactionIdentifier);
            }
            else
            {
                message =
                    SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.Update.AGENT_DISPUTATION
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_DISPUTATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_TRANSACTION_IDENTIFIER, transactionIdentifier)
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_OBJECT_INFO, oldCommentary);
            }


            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE, transactionID);

            SendMail(SCC_BL.Settings.AppValues.MailTopic.DISPUTE, usersToNotify, message, transactionID);
        }

        void SendInvalidationMail(int transactionID, string transactionIdentifier, bool isUpdate = false, string oldCommentary = null)
        {
            User currentUser = GetActualUser();

            string message = string.Empty;

            if (!isUpdate)
            {
                message =
                    SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.Creation.AGENT_INVALIDATION
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_INVALIDATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_TRANSACTION_IDENTIFIER, transactionIdentifier);
            }
            else
            {
                message =
                    SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.Update.AGENT_INVALIDATION
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_INVALIDATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_TRANSACTION_IDENTIFIER, transactionIdentifier)
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_OBJECT_INFO, oldCommentary);
            }


            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION, transactionID);

            SendMail(SCC_BL.Settings.AppValues.MailTopic.INVALIDATION, usersToNotify, message, transactionID);
        }

        void SendDevolutionMail(int transactionID, string transactionIdentifier, bool isUpdate = false, string oldCommentary = null)
        {
            User currentUser = GetActualUser();

            string message = string.Empty;

            if (!isUpdate)
            {
                message =
                    SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.Creation.AGENT_DEVOLUTION
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_RETURNING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_TRANSACTION_IDENTIFIER, transactionIdentifier);
            }
            else
            {
                message =
                    SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.Update.AGENT_DEVOLUTION
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_RETURNING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_TRANSACTION_IDENTIFIER, transactionIdentifier)
                        .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_OBJECT_INFO, oldCommentary);
            }


            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION, transactionID);

            SendMail(SCC_BL.Settings.AppValues.MailTopic.DEVOLUTION, usersToNotify, message, transactionID);
        }

        [HttpPost]
        public ActionResult SaveDisputation(List<TransactionCommentary> commentaryList)
        {
            int? transactionID = null;

            if (commentaryList.Count() > 0) transactionID = commentaryList.FirstOrDefault().TransactionID;

            foreach (TransactionCommentary transactionCommentary in commentaryList)
            {
                if (transactionCommentary.ID > 0)
                {
                    TransactionCommentary newTransactionCommentary = new TransactionCommentary(
                        transactionCommentary.ID,
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE,
                        transactionCommentary.TransactionID,
                        transactionCommentary.Comment,
                        transactionCommentary.BasicInfoID,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

                    try
                    {
                        transactionCommentary.SetDataByID();

                        int result = newTransactionCommentary.Update();

                        if (result > 0)
                        {
                            newTransactionCommentary.SetDataByID();

                            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE, transactionID.Value);

                            Transaction transaction = new Transaction(transactionID.Value);
                            transaction.SetDataByID(true);

                            SendDisputeNotifications(transactionID.Value, transaction.Identifier, usersToNotify, false, transactionCommentary.Comment);

                            SendDisputeMail(transactionID.Value, transaction.Identifier, true, transactionCommentary.Comment);

                            SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Success>(newTransactionCommentary.ID, newTransactionCommentary.BasicInfo.StatusID, transactionCommentary);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.TransactionCommentary.Update.CODE)result)
                            {
                                case SCC_BL.Results.TransactionCommentary.Update.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Error>(transactionCommentary.ID, transactionCommentary.BasicInfo.StatusID, newTransactionCommentary);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Error>(transactionCommentary.ID, transactionCommentary.BasicInfo.StatusID, transactionCommentary, ex);
                    }
                }
                else
                {
                    TransactionCommentary newTransactionCommentary = new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE,
                        transactionCommentary.TransactionID,
                        transactionCommentary.Comment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

                    try
                    {
                        int result = newTransactionCommentary.Insert();

                        if (result > 0)
                        {
                            newTransactionCommentary.SetDataByID();

                            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DISPUTE, transactionID.Value);

                            Transaction transaction = new Transaction(transactionID.Value);
                            transaction.SetDataByID(true);

                            SendDisputeNotifications(transactionID.Value, transaction.Identifier, usersToNotify);

                            SendDisputeMail(transactionID.Value, transaction.Identifier);

                            SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Success>(newTransactionCommentary.ID, newTransactionCommentary.BasicInfo.StatusID, newTransactionCommentary);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.TransactionCommentary.Insert.CODE)result)
                            {
                                case SCC_BL.Results.TransactionCommentary.Insert.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Error>(null, null, newTransactionCommentary);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Error>(null, null, null, ex);
                        //return Json(new { message = "Ha ocurrido un error al guardar las disputas: " + ex.ToString() });
                    }
                }
            }

            return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName, new { transactionID = transactionID, hasDisputation = "true" }) });
        }

        void SendDisputeNotifications(int transactionID, string transactionIDentifier, int[] userIDList, bool isNew = true, string oldCommentary = null)
        {
            string url = Url.Action(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transactionID, hasDisputation = "true" }, Request.Url.Scheme);

            List<UserNotificationUrl> userNotificationUrlList = new List<UserNotificationUrl>() {
                    new UserNotificationUrl(url, "Ir a disputa")
                };

            for (int i = 0; i < userIDList.Length; i++)
            {
                if (userIDList[i] == GetActualUser().ID)
                    SaveNotification(
                        userIDList[i],
                        (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.DISPUTE_AGENT,
                        isNew
                            ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.Creation.SELF_AGENT_DISPUTATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                            : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.Update.SELF_AGENT_DISPUTATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_OBJECT_INFO, oldCommentary),
                        userNotificationUrlList);
                else
                {
                    User auxUser = new User(userIDList[i]);
                    auxUser.SetDataByID(true);

                    SaveNotification(
                            userIDList[i],
                            (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.DISPUTE_OTHERS,
                            isNew
                                ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.Creation.AGENT_DISPUTATION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_DISPUTATING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName}, {auxUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.Update.AGENT_DISPUTATION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_DISPUTATING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName}, {auxUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Disputation.REPLACE_OBJECT_INFO, oldCommentary),
                            userNotificationUrlList);
                }
            }
        }

        void SendInvalidationNotifications(int transactionID, string transactionIDentifier, int[] userIDList, bool isNew = true, string oldCommentary = null)
        {
            string url = Url.Action(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transactionID, hasInvalidation = "true" }, Request.Url.Scheme);

            List<UserNotificationUrl> userNotificationUrlList = new List<UserNotificationUrl>() {
                    new UserNotificationUrl(url, "Ir a invalidación")
                };

            for (int i = 0; i < userIDList.Length; i++)
            {
                if (userIDList[i] == GetActualUser().ID)
                    SaveNotification(
                        userIDList[i],
                        (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.INVALIDATION_AGENT,
                        isNew
                            ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.Creation.INVALIDATING_USER_INVALIDATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                            : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.Update.INVALIDATING_USER_INVALIDATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_OBJECT_INFO, oldCommentary),
                        userNotificationUrlList);
                else
                {
                    User auxUser = new User(userIDList[i]);
                    auxUser.SetDataByID(true);

                    SaveNotification(
                            userIDList[i],
                            (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.INVALIDATION_OTHERS,
                            isNew
                                ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.Creation.AGENT_INVALIDATION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_INVALIDATING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName}, {auxUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.Update.AGENT_INVALIDATION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_INVALIDATING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName}, {auxUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Invalidation.REPLACE_OBJECT_INFO, oldCommentary),
                            userNotificationUrlList);
                }
            }
        }

        void SendDevolutionNotifications(int transactionID, string transactionIDentifier, int[] userIDList, bool isNew = true, string oldCommentary = null)
        {
            string url = Url.Action(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transactionID, hasDevolution = "true" }, Request.Url.Scheme);

            List<UserNotificationUrl> userNotificationUrlList = new List<UserNotificationUrl>() {
                    new UserNotificationUrl(url, "Ir a devolución")
                };

            for (int i = 0; i < userIDList.Length; i++)
            {
                if (userIDList[i] == GetActualUser().ID)
                    SaveNotification(
                        userIDList[i],
                        (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.DEVOLUTION_AGENT,
                        isNew
                            ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.Creation.RETURNING_USER_DEVOLUTION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                            : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.Update.RETURNING_USER_DEVOLUTION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_OBJECT_INFO, oldCommentary),
                        userNotificationUrlList);
                else
                {
                    User auxUser = new User(userIDList[i]);
                    auxUser.SetDataByID(true);

                    SaveNotification(
                            userIDList[i],
                            (int)SCC_BL.DBValues.Catalog.NOTIFICATION_TYPE.DEVOLUTION_OTHERS,
                            isNew
                                ? SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.Creation.AGENT_DEVOLUTION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_RETURNING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName}, {auxUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                : SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.Update.AGENT_DEVOLUTION
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_RETURNING_USER, $"{auxUser.Person.Identification} - {auxUser.Person.SurName}, {auxUser.Person.FirstName}")
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_TRANSACTION_IDENTIFIER, transactionIDentifier)
                                    .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Devolution.REPLACE_OBJECT_INFO, oldCommentary),
                            userNotificationUrlList);
                }
            }
        }

        [HttpPost]
        public ActionResult SaveInvalidation(List<TransactionCommentary> commentaryList)
        {
            int? transactionID = null;

            if (commentaryList.Count() > 0) transactionID = commentaryList.FirstOrDefault().TransactionID;

            foreach (TransactionCommentary transactionCommentary in commentaryList)
            {
                if (transactionCommentary.ID > 0)
                {
                    TransactionCommentary newTransactionCommentary = new TransactionCommentary(
                        transactionCommentary.ID,
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION,
                        transactionCommentary.TransactionID,
                        transactionCommentary.Comment,
                        transactionCommentary.BasicInfoID,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

                    try
                    {
                        transactionCommentary.SetDataByID();

                        int result = newTransactionCommentary.Update();

                        if (result > 0)
                        {
                            newTransactionCommentary.SetDataByID();

                            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION, transactionID.Value);

                            Transaction transaction = new Transaction(transactionID.Value);
                            transaction.SetDataByID(true);

                            SendInvalidationNotifications(transactionID.Value, transaction.Identifier, usersToNotify, false, transactionCommentary.Comment);

                            SendInvalidationMail(transactionID.Value, transaction.Identifier, true, transactionCommentary.Comment);

                            SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Success>(newTransactionCommentary.ID, newTransactionCommentary.BasicInfo.StatusID, transactionCommentary);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.TransactionCommentary.Update.CODE)result)
                            {
                                case SCC_BL.Results.TransactionCommentary.Update.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Error>(transactionCommentary.ID, transactionCommentary.BasicInfo.StatusID, newTransactionCommentary);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Error>(transactionCommentary.ID, transactionCommentary.BasicInfo.StatusID, transactionCommentary, ex);
                    }
                }
                else
                {
                    TransactionCommentary newTransactionCommentary = new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION,
                        transactionCommentary.TransactionID,
                        transactionCommentary.Comment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

                    try
                    {
                        int result = newTransactionCommentary.Insert();

                        if (result > 0)
                        {
                            newTransactionCommentary.SetDataByID();

                            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_INVALIDATION, transactionID.Value);

                            Transaction transaction = new Transaction(transactionID.Value);
                            transaction.SetDataByID(true);

                            SendInvalidationNotifications(transactionID.Value, transaction.Identifier, usersToNotify);

                            SendInvalidationMail(transactionID.Value, transaction.Identifier);

                            SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Success>(newTransactionCommentary.ID, newTransactionCommentary.BasicInfo.StatusID, newTransactionCommentary);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.TransactionCommentary.Insert.CODE)result)
                            {
                                case SCC_BL.Results.TransactionCommentary.Insert.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Error>(null, null, newTransactionCommentary);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Error>(null, null, null, ex);
                        //return Json(new { message = "Ha ocurrido un error al guardar las invalidaciones: " + ex.ToString() });
                    }
                }
            }

            return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName, new { transactionID = transactionID, hasInvalidation = "true" }) });
        }

        [HttpPost]
        public ActionResult SaveDevolution(List<TransactionCommentary> commentaryList)
        {
            int? transactionID = null;

            if (commentaryList.Count() > 0) transactionID = commentaryList.FirstOrDefault().TransactionID;

            foreach (TransactionCommentary transactionCommentary in commentaryList)
            {
                if (transactionCommentary.ID > 0)
                {
                    TransactionCommentary newTransactionCommentary = new TransactionCommentary(
                        transactionCommentary.ID,
                        transactionCommentary.TypeID,
                        transactionCommentary.TransactionID,
                        transactionCommentary.Comment,
                        transactionCommentary.BasicInfoID,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.UPDATED);

                    try
                    {
                        transactionCommentary.SetDataByID();

                        int result = newTransactionCommentary.Update();

                        if (result > 0)
                        {
                            newTransactionCommentary.SetDataByID();

                            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION, transactionID.Value);

                            Transaction transaction = new Transaction(transactionID.Value);
                            transaction.SetDataByID(true);

                            SendDevolutionNotifications(transactionID.Value, transaction.Identifier, usersToNotify, false, transactionCommentary.Comment);

                            SendDevolutionMail(transactionID.Value, transaction.Identifier, true, transactionCommentary.Comment);

                            SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Success>(newTransactionCommentary.ID, newTransactionCommentary.BasicInfo.StatusID, transactionCommentary);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.TransactionCommentary.Update.CODE)result)
                            {
                                case SCC_BL.Results.TransactionCommentary.Update.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Error>(transactionCommentary.ID, transactionCommentary.BasicInfo.StatusID, newTransactionCommentary);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Update.Error>(transactionCommentary.ID, transactionCommentary.BasicInfo.StatusID, transactionCommentary, ex);
                    }
                }
                else
                {
                    TransactionCommentary newTransactionCommentary = new TransactionCommentary(
                        transactionCommentary.TypeID,
                        transactionCommentary.TransactionID,
                        transactionCommentary.Comment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED);

                    try
                    {
                        int result = newTransactionCommentary.Insert();

                        if (result > 0)
                        {
                            newTransactionCommentary.SetDataByID();

                            int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.TRANSACTION_DEVOLUTION, transactionID.Value);

                            Transaction transaction = new Transaction(transactionID.Value);
                            transaction.SetDataByID(true);

                            SendDevolutionNotifications(transactionID.Value, transaction.Identifier, usersToNotify);

                            SendDevolutionMail(transactionID.Value, transaction.Identifier);

                            SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Success>(newTransactionCommentary.ID, newTransactionCommentary.BasicInfo.StatusID, newTransactionCommentary);
                        }
                        else
                        {
                            switch ((SCC_BL.Results.TransactionCommentary.Insert.CODE)result)
                            {
                                case SCC_BL.Results.TransactionCommentary.Insert.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Error>(null, null, newTransactionCommentary);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.TransactionCommentary.Insert.Error>(null, null, null, ex);
                        //return Json(new { message = "Ha ocurrido un error al guardar las invalidaciones: " + ex.ToString() });
                    }
                }
            }

            return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName, new { transactionID = transactionID, hasDevolution = "true" }) });
        }

        [HttpPost]
        public ActionResult Create(
            Transaction transaction,
            List<SCC_BL.TransactionAttributeCatalog> transactionAttributeList,
            List<SCC_BL.TransactionCustomFieldCatalog> transactionCustomFieldIDList,
            List<SCC_BL.TransactionBIFieldCatalog> transactionBIFieldList,
            List<SCC_BL.TransactionCommentary> transactionCommentaryList,
            string[] transactionLabelArray)
        {
            Transaction newTransaction = new Transaction(
                transaction.Identifier,
                transaction.UserToEvaluateID,
                transaction.EvaluatorUserID,
                transaction.EvaluationDate,
                transaction.TransactionDate,
                DateTime.Now,
                transaction.FormID,
                transaction.Comment,
                transaction.GeneralResultID,
                transaction.GeneralFinalUserCriticalErrorResultID,
                transaction.GeneralBusinessCriticalErrorResultID,
                transaction.GeneralFulfillmentCriticalErrorResultID,
                transaction.GeneralNonCriticalErrorResult,
                transaction.AccurateResultID,
                transaction.AccurateFinalUserCriticalErrorResultID,
                transaction.AccurateBusinessCriticalErrorResultID,
                transaction.AccurateFulfillmentCriticalErrorResultID,
                transaction.AccurateNonCriticalErrorResult,
                transaction.ControllableResultID,
                transaction.ControllableFinalUserCriticalErrorResultID,
                transaction.ControllableBusinessCriticalErrorResultID,
                transaction.ControllableFulfillmentCriticalErrorResultID,
                transaction.ControllableNonCriticalErrorResult,
                transaction.TimeElapsed,
                GetActualUser().ID,
                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION.CREATED,
                transaction.TypeID,
                transaction.CalibratedTransactionID);

            try
            {
                int result = newTransaction.Insert();

                if (result > 0)
                {
                    newTransaction.SetDataByID();

                    UpdateAttributeList(newTransaction, transactionAttributeList);

                    UpdateTransactionCustomFieldCatalogList(newTransaction, transactionCustomFieldIDList);

                    UpdateTransactionBIFieldCatalogList(newTransaction, transactionBIFieldList);

                    /*UpdateDisputeCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE)
                            .ToList());

                    UpdateInvalidationCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION)
                            .ToList());

                    UpdateDevolutionCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
                            .ToList());*/

                    UpdateTransactionLabelList(newTransaction, transactionLabelArray);

                    if (newTransaction.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.CALIBRATION)
                    {
                        User currentUser = new User(newTransaction.EvaluatorUserID);
                        currentUser.SetDataByID(true);

                        string message =
                            SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.Creation.AGENT_CALIBRATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.REPLACE_CALIBRATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.REPLACE_TRANSACTION_IDENTIFIER, newTransaction.Identifier);

                        int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION, newTransaction.ID);

                        SendMail(SCC_BL.Settings.AppValues.MailTopic.CALIBRATION_CREATED, usersToNotify, message, newTransaction.ID);
                    }

                    SaveProcessingInformation<SCC_BL.Results.Transaction.Insert.Success>(newTransaction.ID, newTransaction.BasicInfo.StatusID, newTransaction);
                }
                else
                {
                    switch ((SCC_BL.Results.Transaction.Insert.CODE)result)
                    {
                        case SCC_BL.Results.Transaction.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Transaction.Insert.Error>(null, null, newTransaction);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.Insert.Error>(null, null, null, ex);
            }

            if (transaction.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.CALIBRATION)
                return Json(new { url = Url.Action(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController))) });
            else
                return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName) });
        }

        void UpdateAttributeList(Transaction transaction, List<SCC_BL.TransactionAttributeCatalog> transactionAttributeList)
        {
            try
            {
                switch (transaction.UpdateAttributeList(transactionAttributeList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateAttributeList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateAttributeList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateAttributeList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateTransactionCustomFieldCatalogList(Transaction transaction, List<SCC_BL.TransactionCustomFieldCatalog> transactionCustomFieldCatalog)
        {
            try
            {
                switch (transaction.UpdateCustomFieldList(transactionCustomFieldCatalog, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateCustomFieldList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateCustomFieldList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateCustomFieldList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateTransactionBIFieldCatalogList(Transaction transaction, List<SCC_BL.TransactionBIFieldCatalog> transactionBIFieldCatalog)
        {
            try
            {
                switch (transaction.UpdateBIFieldList(transactionBIFieldCatalog, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateBIFieldList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateBIFieldList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateBIFieldList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateDisputeCommentList(Transaction transaction, List<SCC_BL.TransactionCommentary> transactionCommentaryList)
        {
            try
            {
                switch (transaction.UpdateDisputeCommentList(transactionCommentaryList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateDisputeCommentList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDisputeCommentList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDisputeCommentList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateInvalidationCommentList(Transaction transaction, List<SCC_BL.TransactionCommentary> transactionCommentaryList)
        {
            try
            {
                switch (transaction.UpdateInvalidationCommentList(transactionCommentaryList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateInvalidationCommentList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateInvalidationCommentList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateInvalidationCommentList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateDevolutionCommentList(Transaction transaction, List<SCC_BL.TransactionCommentary> transactionCommentaryList)
        {
            try
            {
                switch (transaction.UpdateDevolutionCommentList(transactionCommentaryList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateDevolutionCommentList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDevolutionCommentList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDevolutionCommentList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        void UpdateTransactionLabelList(Transaction transaction, string[] labelList)
        {
            try
            {
                switch (transaction.UpdateTransactionLabelList(labelList, GetActualUser().ID))
                {
                    case SCC_BL.Results.Transaction.UpdateTransactionLabelList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateTransactionLabelList.Success>(transaction.ID, transaction.BasicInfo.StatusID, transaction);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateTransactionLabelList.Error>(transaction.ID, transaction.BasicInfo.StatusID, transaction, ex);
            }
        }

        [HttpPost]
        public ActionResult Edit(
            Transaction transaction,
            List<SCC_BL.TransactionAttributeCatalog> transactionAttributeList,
            List<SCC_BL.TransactionCustomFieldCatalog> transactionCustomFieldIDList,
            List<SCC_BL.TransactionBIFieldCatalog> transactionBIFieldList,
            List<SCC_BL.TransactionCommentary> transactionCommentaryList,
            string[] transactionLabelArray)
        {
            if (GetActualUser().ID == transaction.EvaluatorUserID)
            {
                if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_TRANSACTIONS))
                {
                    SaveProcessingInformation<SCC_BL.Results.Transaction.Update.NotAllowedToModifyTransactions>();
                    return RedirectToAction(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transaction.ID });
                }
            }
            else
            {
                if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_MODIFY_USER_TRANSACTION))
                {
                    SaveProcessingInformation<SCC_BL.Results.Transaction.Update.NotAllowedToModifyOtherUsersTransactions>();
                    return RedirectToAction(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transaction.ID });
                }
            }

            Transaction oldTransaction = new Transaction(transaction.ID);
            oldTransaction.SetDataByID();

            Transaction newTransaction = new Transaction(
                transaction.ID,
                transaction.Identifier,
                transaction.UserToEvaluateID,
                transaction.EvaluatorUserID,
                transaction.EvaluationDate,
                transaction.TransactionDate,
                transaction.LoadDate,
                transaction.FormID,
                transaction.Comment,
                transaction.GeneralResultID,
                transaction.GeneralFinalUserCriticalErrorResultID,
                transaction.GeneralBusinessCriticalErrorResultID,
                transaction.GeneralFulfillmentCriticalErrorResultID,
                transaction.GeneralNonCriticalErrorResult,
                transaction.AccurateResultID,
                transaction.AccurateFinalUserCriticalErrorResultID,
                transaction.AccurateBusinessCriticalErrorResultID,
                transaction.AccurateFulfillmentCriticalErrorResultID,
                transaction.AccurateNonCriticalErrorResult,
                transaction.ControllableResultID,
                transaction.ControllableFinalUserCriticalErrorResultID,
                transaction.ControllableBusinessCriticalErrorResultID,
                transaction.ControllableFulfillmentCriticalErrorResultID,
                transaction.ControllableNonCriticalErrorResult,
                transaction.TimeElapsed,
                transaction.BasicInfoID,
                GetActualUser().ID,
                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION.UPDATED);

            try
            {
                int result = newTransaction.Update();

                if (result > 0)
                {
                    newTransaction.SetDataByID();

                    UpdateAttributeList(newTransaction, transactionAttributeList);

                    UpdateTransactionCustomFieldCatalogList(newTransaction, transactionCustomFieldIDList);

                    UpdateTransactionBIFieldCatalogList(newTransaction, transactionBIFieldList);

                    /*UpdateDisputeCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE)
                            .ToList());

                    UpdateInvalidationCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION)
                            .ToList());

                    UpdateDevolutionCommentList(
                        newTransaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
                            .ToList());*/

                    UpdateTransactionLabelList(newTransaction, transactionLabelArray); 

                    if (newTransaction.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.CALIBRATION)
                    {
                        User currentUser = new User(newTransaction.EvaluatorUserID);
                        currentUser.SetDataByID(true);

                        string message =
                            SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.Update.AGENT_CALIBRATION
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.REPLACE_CALIBRATING_USER, $"{currentUser.Person.Identification} - {currentUser.Person.SurName}, {currentUser.Person.FirstName}")
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.REPLACE_TRANSACTION_IDENTIFIER, oldTransaction.Identifier)
                                .Replace(SCC_BL.Results.NotificationMatrix.UserNotification.Transaction.Calibration.REPLACE_OBJECT_INFO, Serialize(oldTransaction));

                        int[] usersToNotify = GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION.CALIBRATION, newTransaction.ID);

                        SendMail(SCC_BL.Settings.AppValues.MailTopic.CALIBRATION_CREATED, usersToNotify, message, newTransaction.ID);
                    }

                    SaveProcessingInformation<SCC_BL.Results.Transaction.Update.Success>(newTransaction.ID, newTransaction.BasicInfo.StatusID, oldTransaction);
                }
                else
                {
                    switch ((SCC_BL.Results.Transaction.Update.CODE)result)
                    {
                        case SCC_BL.Results.Transaction.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Transaction.Update.Error>(oldTransaction.ID, oldTransaction.BasicInfo.StatusID, newTransaction);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.Update.Error>(oldTransaction.ID, oldTransaction.BasicInfo.StatusID, oldTransaction, ex);
            }

            if (transaction.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.CALIBRATION)
                return Json(new { url = Url.Action(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController))) });
            else
                return Json(new { url = Url.Action(nameof(TransactionController.Edit), _mainControllerName) });
        }

        public ActionResult Search(SCC_BL.Helpers.Transaction.Search.TransactionSearchHelper transactionSearchHelper = null)
        {
            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEARCH_TRANSACTIONS))
            {
                SaveProcessingInformation<SCC_BL.Results.Transaction.Search.NotAllowedToSearchTransactions>();
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));
            }

            TransactionSearchViewModel transactionSearchViewModel = new TransactionSearchViewModel();

            bool hasHelper = false;

            foreach (System.Reflection.PropertyInfo propertyInfo in transactionSearchHelper.GetType().GetProperties())
            {
                if (propertyInfo.Name.Equals("TransactionIdentifier"))
                {

                }
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

                List<Catalog> allResultList = new List<Catalog>();
                List<User> allUserList = new List<User>();

                //Starts filling all data

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_GENERAL_RESULT_FINAL))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_CONTROLLABLE_RESULT_FINAL))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_ACCURATE_RESULT_FINAL))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR))
                    allResultList.AddRange(catalog.SelectByCategoryID());

                using (User user = new User())
                    allUserList = user.SelectAll(true);

                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.AllData.ResultCatalog.NAME] = allResultList;

                ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.AllData.User.NAME] = allUserList;

                //Ends filling all data
            }

            List<Catalog> catalogSearchStringType = new List<Catalog>();
            List<Catalog> catalogSearchTimeUnitType = new List<Catalog>();
            List<Catalog> catalogUserStatus = new List<Catalog>();
            List<Workspace> workspaceList = new List<Workspace>();
            List<Program> programList = new List<Program>();
            List<User> userList = new List<User>();

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
                    workspace.SelectAll()
                        .Where(e => 
                            e.Monitorable &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DISABLED)
                        .ToList();

            using (Program program = new Program())
                programList =
                    program.SelectWithForm()
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED &&
                            (DateTime.Now < e.EndDate ||
                            e.EndDate == null))
                        .GroupBy(e =>
                            e.ID)
                        .Select(e =>
                            e.First())
                        .ToList();

            if (!GetActualUser().HasPermission(SCC_BL.DBValues.Catalog.Permission.CAN_SEE_ALL_PROGRAMS))
            {
                programList =
                    programList
                        .Where(e => GetActualUser().CurrentProgramList.Select(s => s.ID).Contains(e.ID))
                        .ToList();
            }

            using (User user = new User())
                userList =
                    user.SelectAll(true)
                        .OrderBy(e => e.Person.SurName)
                        .ThenBy(e => e.Person.FirstName)
                        .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.StringTypeID.NAME] =
                new SelectList(
                    catalogSearchStringType,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.TimeUnitTypeID.NAME] =
                new SelectList(
                    catalogSearchTimeUnitType,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description));

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.UserStatus.NAME] =
                new SelectList(
                    catalogUserStatus,
                    nameof(Catalog.ID),
                    nameof(Catalog.Description),
                    transactionSearchHelper.UserStatusID);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.Workspace.NAME] =
                new MultiSelectList(
                    workspaceList,
                    nameof(Workspace.ID),
                    nameof(Workspace.Name),
                    transactionSearchHelper.WorkspaceIDList != null
                        ? transactionSearchHelper.WorkspaceIDList.Select(e => e)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    transactionSearchHelper.ProgramIDList != null
                        ? transactionSearchHelper.ProgramIDList.Select(e => e)
                        : null);

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.UserList.NAME] = userList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.Transaction.Search.YesNoQuestion.NAME] =
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
                            Value = ((int)SCC_BL.Settings.AppValues.ViewData.Transaction.Search.YesNoQuestion.Values.YES).ToString()
                        },
                        new SelectListItem()
                        {
                            Text = "No",
                            Value = ((int)SCC_BL.Settings.AppValues.ViewData.Transaction.Search.YesNoQuestion.Values.NO).ToString()
                        }
                    },
                    nameof(SelectListItem.Value),
                    nameof(SelectListItem.Text));

            return View(transactionSearchViewModel);
        }

        /*public static object GetValueFromTransaction(int valueID, SCC_BL.DBValues.Catalog.ELEMENT category)
        {
            switch (category)
            {
                case SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_ATTRIBUTE:
                    using (AttributeValueCatalog attributeValueCatalog = new AttributeValueCatalog(valueID))
                    {
                        attributeValueCatalog.SetDataByID();
                        return attributeValueCatalog;
                    }
                case SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_BUSINESSINTELLIGENCEFIELD:
                    using (BusinessIntelligenceValueCatalog businessIntelligenceValueCatalog = new BusinessIntelligenceValueCatalog(valueID))
                    {
                        businessIntelligenceValueCatalog.SetDataByID();
                        return businessIntelligenceValueCatalog;
                    }
                case SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_CUSTOMCONTROL:
                    using (CustomControlValueCatalog customControlValueCatalog = new CustomControlValueCatalog(valueID))
                    {
                        customControlValueCatalog.SetDataByID();
                        return customControlValueCatalog;
                    }
                default:
                    return null;
            }
        }*/

        List<SCC_BL.Helpers.Transaction.Import.Error> _transactionImportErrorList { get; set; } = new List<SCC_BL.Helpers.Transaction.Import.Error>();
        List<SCC_BL.Helpers.Transaction.Import.Success> _transactionImportSuccessList { get; set; } = new List<SCC_BL.Helpers.Transaction.Import.Success>();

        public ActionResult ImportData(int programID)
        {
            List<UploadedFile> uploadedFileList = new List<UploadedFile>();

            using (UploadedFile uploadedFile = new UploadedFile())
            {
                uploadedFileList =
                    uploadedFile.SelectAll()
                        .Where(e =>
                            e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_DATA_IMPORT)
                        .ToList();

            }

            ViewModels.TransactionImportViewModel transactionImportViewModel = new ViewModels.TransactionImportViewModel();
            transactionImportViewModel.UploadedFileList = uploadedFileList;
            //transactionImportViewModel.ErrorList = _transactionImportErrorList;

            return View(transactionImportViewModel);
        }

        public ActionResult ErrorList(TransactionImportResults transactionImportResults)
        {
            return View(transactionImportResults);
        }

        [HttpPost]
        public ActionResult ImportDataAction(HttpPostedFileBase file, int programID)
        {
            try
            {
                _transactionImportErrorList = new List<SCC_BL.Helpers.Transaction.Import.Error>();
                _transactionImportSuccessList = new List<SCC_BL.Helpers.Transaction.Import.Success>();

                string filePath = SaveUploadedFile(file, SCC_BL.Settings.Paths.Transaction.TRANSACTION_IMPORT_FOLDER);

                System.Data.DataTable dt = new System.Data.DataTable();

                if (!string.IsNullOrEmpty(filePath))
                {
                    int uploadedFileID = 0;

                    uploadedFileID = SaveFileInDatabase(filePath, (int)SCC_BL.DBValues.Catalog.STATUS_UPLOADED_FILE.LOADED_FILE_DATA_IMPORT);

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
                            SCC_BL.Results.UploadedFile.TransactionImport.CODE result = SCC_BL.Results.UploadedFile.TransactionImport.CODE.ERROR;

                            using (SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser())
                            {
                                dt = excelParser.ReadAsDataTableForTransactionImport(filePath);
                                result = ProcessImportExcel(dt, programID);
                            }

                            switch (result)
                            {
                                case SCC_BL.Results.UploadedFile.TransactionImport.CODE.SUCCESS:
                                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Success>();
                                    break;
                                case SCC_BL.Results.UploadedFile.TransactionImport.CODE.ERROR:
                                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>(null, null, null, new Exception("Ha ocurrido un error interno al procesar las transacciones"));
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>(null, null, null, ex);
                        }
                    }
                }
                else
                {
                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>(null, null, null, new Exception("No se ha encontrado la ruta de carga del archivo"));
                }

                //if (_transactionImportErrorList.Count() > 0)
                if (true)
                {
                    if (_transactionImportErrorList != null)
                    {
                        try
                        {
                            if (_transactionImportErrorList.Where(e => e.Type == SCC_BL.Settings.Notification.Type.INFO).Count() > 0)
                            {
                                _transactionImportErrorList =
                                    _transactionImportErrorList
                                        .Where(e =>
                                            e.Type != SCC_BL.Settings.Notification.Type.INFO)
                                        .ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (_transactionImportErrorList != null)
                    {
                        try
                        {
                            _transactionImportErrorList =
                                _transactionImportErrorList
                                    .OrderBy(e => e.RowNumber)
                                    .ThenBy(e => e.ColumnNumber)
                                    .ToList();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                _transactionImportErrorList =
                                    _transactionImportErrorList
                                        .OrderBy(e => e.ColumnNumber)
                                        .ToList();
                            }
                            catch (Exception ex1)
                            {
                            }
                        }
                    }

                    if (_transactionImportSuccessList != null)
                    {
                        try
                        {
                            _transactionImportSuccessList =
                                _transactionImportSuccessList
                                    .OrderBy(e => e.RowIndex)
                                    .ToList();
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    System.Data.DataTable nonProcessedData = new System.Data.DataTable();
                    nonProcessedData = dt.Clone();

                    foreach (System.Data.DataRow oldRow in dt.Rows)
                    {
                        nonProcessedData.ImportRow(oldRow);
                    }

                    /*nonProcessedData.Rows.Clear();
                    dt.Rows
                        .Cast<System.Data.DataRow>()
                        .Where(e => 
                            !_transactionImportSuccessList
                                .Select(f => f.OldIdentifier)
                                .Contains(e.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER].ToString().Trim()))
                        .ToList()
                        .ForEach(e => {
                            nonProcessedData.Rows.Add(e);
                        });*/

                    nonProcessedData.Rows
                        .Cast<System.Data.DataRow>()
                        .Where(e =>
                            _transactionImportSuccessList
                                .Select(f => f.OldIdentifier)
                                .Contains(e.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER].ToString().Trim()))
                        .ToList()
                        .ForEach(e => {
                            nonProcessedData.Rows.Remove(e);
                        });

                    int[] dateIndexArray =
                        new List<int>()
                        {
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.EVALUATION_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.LOAD_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.MODIFICATION_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.ActionPlan.ACTION_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Tracing.TRACING_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Coaching.SENDING_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Coaching.READING_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Disputation.CREATION_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Invalidation.CREATION_DATE,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE
                        }.ToArray();

                    foreach (System.Data.DataRow dr in nonProcessedData.Rows)
                    {
                        for (int j = 0; j < dr.ItemArray.Length; j++)
                        {
                            if (!dateIndexArray.Contains(j)) continue;

                            string dateValue = dr[j].ToString();

                            if (dateValue.Length <= 1) continue;

                            using (ExcelParser excelParser = new ExcelParser())
                            {
                                DateTime? dateTimeValue = null;

                                /*try
                                {
                                    dateTimeValue = excelParser.FormatDateTime(dateValue, ExcelParser.DoubleSeparator.DOT);
                                }
                                catch (Exception ex0)
                                {
                                    try
                                    {
                                        dateTimeValue = excelParser.FormatDateTime(dateValue, ExcelParser.DoubleSeparator.COMMA);
                                    }
                                    catch (Exception ex1)
                                    {
                                        dateTimeValue = excelParser.FormatDateTime(dateValue, ExcelParser.DoubleSeparator.NONE);
                                    }
                                }*/

                                dateTimeValue = excelParser.FormatDateTime(dateValue);

                                dateValue =
                                    dateTimeValue != null
                                        ? dateTimeValue.Value.ToString("g", System.Globalization.CultureInfo.GetCultureInfo("es-ES"))
                                        : string.Empty;
                            }

                            dr[j] = dateValue;
                        }
                    }

                    /*for (int i = 0; i < nonProcessedData.Rows.Count; i++)
                    {
                        for (int j = 0; j < nonProcessedData.Rows[i].ItemArray.Length; j++)
                        {
                            if (!dateIndexArray.Contains(j)) continue;

                            string dateValue = nonProcessedData.Rows[i].ItemArray[j].ToString();

                            if (dateValue.Length <= 1) continue;

                            using (ExcelParser excelParser = new ExcelParser())
                            {
                                DateTime? dateTimeValue = excelParser.FormatDateTime(dateValue);
                                dateValue = 
                                    dateTimeValue != null
                                        ? dateTimeValue.Value.ToString("dd/MM/yyyy hh:mm")
                                        : string.Empty;

                                nonProcessedData.Rows[i].

                                nonProcessedData.Rows[i].ItemArray[j] = dateTimeValue;
                            }
                        }
                    }*/

                    string generatedFilePath =
                        SaveFileFromDataTable(
                            nonProcessedData,
                            SCC_BL.Settings.Paths.Transaction.TRANSACTION_GENERATED_IMPORT_RESULTS_FOLDER,
                            SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.FILE_NAME_PREFIX + SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.DEFAULT_EXTENSION,
                            SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.DEFAULT_EXTENSION);

                    Session[SCC_BL.Settings.AppValues.Session.Transaction.LAST_GENERATED_IMPORT_RESULTS_FILE] = generatedFilePath;

                    TransactionImportResults transactionImportResults = new TransactionImportResults(_transactionImportErrorList, _transactionImportSuccessList, generatedFilePath);

                    return View(nameof(ErrorList), transactionImportResults);
                }
                else
                    return RedirectToAction(nameof(TransactionController.ImportData), new { programID = programID });
            }
            catch (Exception ex1)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.TransactionImport.Error>(null, null, null, ex1);
                throw;
            }
        }

        [HttpGet]
        public FileResult DownLoadTransactionImportResultsFile()
        {
            string currentFileName = Session[SCC_BL.Settings.AppValues.Session.Transaction.LAST_GENERATED_IMPORT_RESULTS_FILE].ToString();

            Session[SCC_BL.Settings.AppValues.Session.Transaction.LAST_GENERATED_IMPORT_RESULTS_FILE] = null;

            return DownLoadFileFromServer(
                currentFileName, 
                SCC_BL.Settings.AppValues.File.ContentType.EXCEL_FILES_XLSX);
        }

        //Start data for transaction import

        List<User> _transactionImportUserList = null;
        Program _transactionImportProgram = null;
        ProgramFormCatalog _transactionImportProgramFormCatalog = null;
        Form _transactionImportForm = null;

        //End data for transaction import

        void StartPersistentDataForTransactionImport(int programID, string formName, int rowCount)
        {
            if (_transactionImportUserList == null)
            {
                using (User user = new SCC_BL.User())
                {
                    this._transactionImportUserList = user.SelectAll(true);
                }
            }

            if (_transactionImportProgram != null)
            {
                if (_transactionImportProgram.ID != programID)
                {
                    _transactionImportProgram = null;
                    _transactionImportProgramFormCatalog = null;
                    _transactionImportForm = null;
                }
            }

            if (_transactionImportProgram == null)
            {
                using (Program program = new SCC_BL.Program(programID))
                {
                    program.SetDataByID();
                    this._transactionImportProgram = program;
                }
            }

            if (_transactionImportProgramFormCatalog == null)
            {
                _transactionImportProgramFormCatalog = GetProgramFormCatalogByProgramID(programID, rowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME);
            }

            if (_transactionImportForm == null)
            {
                if (_transactionImportProgramFormCatalog != null)
                    _transactionImportForm = GetFormByID(_transactionImportProgramFormCatalog.FormID);
                else
                    _transactionImportForm = GetFormByName(formName, rowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Form.ExcelFields.NAME);
            }
        }

        public SCC_BL.Results.UploadedFile.TransactionImport.CODE ProcessImportExcel(System.Data.DataTable dt, int programID)
        {
            try
            {
                DateTime? minDate = GetMinDate(dt);
                DateTime? maxDate = GetMaxDate(dt);

                List<string> headerList = dt.Columns.Cast<System.Data.DataColumn>().Select(e => e.ColumnName).ToList();

                int headersCount = headerList.Count();

                int firstAttributeIndex = 0;
                int firstAttributeCommentaryIndex = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.CONSTANT_END_INDEX + 1;

                int lastAttributeIndex = 0;

                for (int i = firstAttributeCommentaryIndex; i < headerList.Count(); i++)
                {
                    string currentValue = headerList[i].Trim();

                    if (currentValue.Equals("Comentarios_1"))
                    {
                        firstAttributeIndex = i - 1;
                        //break;
                    }

                    if (currentValue.Length >= 13)
                    {
                        if (currentValue.Substring(0, 13).Equals("Subatributos_"))
                            lastAttributeIndex = i;
                    }
                }

                int customControlStartIndex = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.CONSTANT_END_INDEX + 1;
                int customControlEndIndex = firstAttributeIndex - 1;

                int biFieldStartIndex = lastAttributeIndex + 1;
                int biFieldEndIndex = headersCount - 1;

                /*foreach (System.Data.DataRow row in dt.Rows)
                {
                    ProcessRow(headerList, row, dt.Rows.Cast<System.Data.DataRow>().ToList().IndexOf(row), minDate, maxDate, programID, firstAttributeIndex, lastAttributeIndex, customControlStartIndex, customControlEndIndex, biFieldStartIndex, biFieldEndIndex);
                }*/

                StartPersistentDataForTransactionImport(
                    programID,
                    dt.Rows[0].ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Form.ExcelFields.NAME].ToString().Trim(),
                    0);

                Parallel.ForEach<System.Data.DataRow>(dt.Rows.Cast<System.Data.DataRow>(), (row) => {
                    ProcessRow(headerList, row, dt.Rows.Cast<System.Data.DataRow>().ToList().IndexOf(row), minDate, maxDate, programID, firstAttributeIndex, lastAttributeIndex, customControlStartIndex, customControlEndIndex, biFieldStartIndex, biFieldEndIndex);
                });
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(ex);
            }

            bool hasNonInfoErrors = 
                _transactionImportErrorList != null 
                    ? _transactionImportErrorList.Any(e => e.Type != SCC_BL.Settings.Notification.Type.INFO)
                    : false;

            if (hasNonInfoErrors)
            {
                string lineList = string.Empty;

                /*_transactionImportErrorList
                    .ForEach(e => {
                        if (!string.IsNullOrEmpty(lineList))
                            lineList += ", ";

                        lineList += e.RowNumber.ToString();
                    });*/

                SaveProcessingInformation<SCC_BL.Results.UploadedFile.FormUpload.Error>(
                    new Exception(
                        SCC_BL.Results.UploadedFile.FormUpload.ErrorSingleRow.CUSTOM_ERROR_EXCEL_LINES
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, lineList)
                    )
                );

                return SCC_BL.Results.UploadedFile.TransactionImport.CODE.ERROR;
            }

            return SCC_BL.Results.UploadedFile.TransactionImport.CODE.SUCCESS;
        }

        void ProcessRow(List<string> headerList, System.Data.DataRow row, int currentRowCount, DateTime? minDate, DateTime? maxDate, int programID, int firstAttributeIndex, int lastAttributeIndex, int customControlStartIndex, int customControlEndIndex, int biFieldStartIndex, int biFieldEndIndex)
        {
            bool hasError = false;

            User agentUser = new User();
            User supervisorUser = new User();
            User evaluatorUser = new User();
            Program program = new Program();
            ProgramFormCatalog programFormCatalog = new ProgramFormCatalog();
            Form form = new Form();

            List<ImportTransactionAttributeHelper> importTransactionAttributeHelperList = new List<ImportTransactionAttributeHelper>();
            List<ImportTransactionCustomControlHelper> importTransactionCustomControlHelperList = new List<ImportTransactionCustomControlHelper>();
            List<ImportTransactionBusinessIntelligenceFieldHelper> importTransactionBusinessIntelligenceFieldHelperList = new List<ImportTransactionBusinessIntelligenceFieldHelper>();

            Transaction transaction = new Transaction();
            List<string> labelList = new List<string>();
            List<TransactionLabelCatalog> transactionLabelCatalogList = new List<TransactionLabelCatalog>();
            List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

            //BEGIN TO ENCAPSULATE OBJECTS

            string agentUserIdentification = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.AGENT_IDENTIFICATION].ToString().Trim();
            string supervisorUserName = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.SUPERVISOR_NAME].ToString().Trim();
            string evaluatorUserName = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.EVALUATOR_NAME].ToString().Trim();
            string programName = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME].ToString().Trim();
            string formName = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Form.ExcelFields.NAME].ToString().Trim();
            string transactionOldIdentifier = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER].ToString().Trim();
            string labelExcelField = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.LABELS].ToString().Trim();

            StartPersistentDataForTransactionImport(
                programID,
                formName,
                currentRowCount);

            //Starts looking for data in database

            agentUser = GetAgentUserByIdentification(agentUserIdentification, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.AGENT_IDENTIFICATION);

            supervisorUser = GetSupervisorUserByName(supervisorUserName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.SUPERVISOR_NAME);

            evaluatorUser = GetEvaluatorUserByName(evaluatorUserName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.User.ExcelFields.EVALUATOR_NAME);

            //program = GetProgramByName(programName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME, minDate, maxDate);
            program = GetProgramByID(programID);

            //programFormCatalog = GetProgramFormCatalogByProgramID(program.ID, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Program.ExcelFields.NAME);

            programFormCatalog = _transactionImportProgramFormCatalog;

            /*if (programFormCatalog != null)
                form = GetFormByID(programFormCatalog.FormID);
            else
                form = GetFormByName(formName, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Form.ExcelFields.NAME);*/

            form = _transactionImportForm;

            //Finishes looking for data in database

            importTransactionAttributeHelperList = GetAttributesFromRow(headerList, row, firstAttributeIndex, lastAttributeIndex, form.ID, currentRowCount, form);

            importTransactionCustomControlHelperList = GetCustomControlsFromRow(headerList, row, customControlStartIndex, customControlEndIndex, currentRowCount, form);

            importTransactionBusinessIntelligenceFieldHelperList = GetBusinessIntelligenceFieldsFromRow(headerList, row, biFieldStartIndex, biFieldEndIndex, currentRowCount, form);

            string transactionNewIdentifier = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.IDENTIFIER_PREFIX + transactionOldIdentifier;
            transaction = GetTransactionByIdentifier(transactionNewIdentifier, currentRowCount, (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER);

            labelList.Add(labelExcelField);

            transactionCommentaryList = GetTransactionCommentaryListFromRow(row);

            //CHECK IF EVERYTHING IS OK

            if (agentUser == null) hasError = true;
            if (supervisorUser == null) hasError = true;
            if (evaluatorUser == null) hasError = true;
            if (program == null) hasError = true;
            if (form == null) hasError = true;
            //if (programFormCatalog == null) hasError = true;
            if (importTransactionAttributeHelperList == null) hasError = true;
            if (importTransactionCustomControlHelperList == null) hasError = true;
            if (importTransactionBusinessIntelligenceFieldHelperList == null) hasError = true;

            if (hasError) return;

            if (transaction == null)
                transaction = CreateTransactionFromRow(row, agentUser.ID, supervisorUser.ID, evaluatorUser.ID, form.ID, currentRowCount);

            if (transaction == null) hasError = true;

            if (!hasError)
            {
                if (transaction.ID <= 0)
                    transaction.Insert();

                if (transaction.ID > 0)
                {
                    //PENDIENTES BI y CAMPOS PERSONALIZADOS

                    List<TransactionCustomFieldCatalog> transactionCustomFieldCatalogList = new List<TransactionCustomFieldCatalog>();

                    foreach (ImportTransactionCustomControlHelper importTransactionCustomControlHelper in importTransactionCustomControlHelperList)
                    {
                        int? tempCustomControlValueCatalogID = null;

                        if (importTransactionCustomControlHelper.CustomControlValueCatalog != null)
                            tempCustomControlValueCatalogID = importTransactionCustomControlHelper.CustomControlValueCatalog.ID;

                        TransactionCustomFieldCatalog transactionCustomFieldCatalog =
                            new TransactionCustomFieldCatalog(
                                transaction.ID,
                                importTransactionCustomControlHelper.CustomField.ID,
                                importTransactionCustomControlHelper.CustomControlComment,
                                tempCustomControlValueCatalogID,
                                GetActualUser().ID,
                                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_CUSTOM_FIELD_CATALOG.CREATED);

                        transactionCustomFieldCatalogList.Add(transactionCustomFieldCatalog);
                    }

                    UpdateTransactionCustomFieldCatalogList(transaction, transactionCustomFieldCatalogList);

                    List<TransactionBIFieldCatalog> transactionBIFieldCatalogList = new List<TransactionBIFieldCatalog>();

                    foreach (ImportTransactionBusinessIntelligenceFieldHelper importTransactionBusinessIntelligenceFieldHelper in importTransactionBusinessIntelligenceFieldHelperList)
                    {
                        TransactionBIFieldCatalog transactionBIFieldCatalog =
                            new TransactionBIFieldCatalog(
                                transaction.ID,
                                importTransactionBusinessIntelligenceFieldHelper.BusinessIntelligenceField.ID,
                                importTransactionBusinessIntelligenceFieldHelper.BusinessIntelligenceValueTransactionComment,
                                true,
                                GetActualUser().ID,
                                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_BI_FIELD_CATALOG.CREATED);

                        transactionBIFieldCatalogList.Add(transactionBIFieldCatalog);
                    }

                    UpdateTransactionBIFieldCatalogList(transaction, transactionBIFieldCatalogList);

                    for (int i = 0; i < transactionCommentaryList.Count(); i++)
                    {
                        transactionCommentaryList[i].TransactionID = transaction.ID;
                    }

                    List<TransactionAttributeCatalog> transactionAttributeCatalogList = new List<TransactionAttributeCatalog>();

                    foreach (ImportTransactionAttributeHelper importTransactionAttributeHelper in importTransactionAttributeHelperList)
                    {
                        int? tempAttributeValueCatalogID = null;

                        if (importTransactionAttributeHelper.AttributeValueCatalog != null)
                            tempAttributeValueCatalogID = importTransactionAttributeHelper.AttributeValueCatalog.ID;

                        TransactionAttributeCatalog transactionAttributeCatalog = 
                            new TransactionAttributeCatalog(
                                transaction.ID, 
                                importTransactionAttributeHelper.Attribute.ID, 
                                importTransactionAttributeHelper.AttributeTransactionComment,
                                tempAttributeValueCatalogID,
                                0,
                                true,
                                GetActualUser().ID,
                                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_ATTRIBUTE_CATALOG.CREATED);

                        transactionAttributeCatalogList.Add(transactionAttributeCatalog);
                    }

                    UpdateAttributeList(transaction, transactionAttributeCatalogList);

                    for (int i = 0; i < transactionCommentaryList.Count(); i++)
                    {
                        transactionCommentaryList[i].TransactionID = transaction.ID;
                    }

                    UpdateDisputeCommentList(
                        transaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE)
                            .ToList());

                    UpdateInvalidationCommentList(
                        transaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION)
                            .ToList());

                    UpdateDevolutionCommentList(
                        transaction,
                        transactionCommentaryList ?? new List<SCC_BL.TransactionCommentary>()
                            .Where(e =>
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS ||
                                e.TypeID == (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS)
                            .ToList());

                    transactionLabelCatalogList = TransactionLabelCatalog.TransactionLabelCatalogWithTransactionID(transaction.ID).SelectByTransactionID();

                    foreach (TransactionLabelCatalog transactionLabelCatalog in transactionLabelCatalogList)
                    {
                        using (TransactionLabel transactionLabel = new TransactionLabel(transactionLabelCatalog.LabelID))
                        {
                            transactionLabel.SetDataByID();
                            labelList.Add(transactionLabel.Description);
                        }
                    }

                    UpdateTransactionLabelList(transaction, labelList.ToArray());

                    _transactionImportSuccessList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Success(
                            transactionOldIdentifier,
                            transactionNewIdentifier,
                            currentRowCount));
                }
            }
        }

        List<TransactionCommentary> GetTransactionCommentaryListFromRow(System.Data.DataRow row)
        {
            SCC_BL.Tools.ExcelParser excelParser = new SCC_BL.Tools.ExcelParser();

            List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

            string disputationComment = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Disputation.COMMENT].ToString().Trim();
            string invalidationComment = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Invalidation.COMMENT].ToString().Trim();
            string devolutionGeneralComment = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.COMMENT].ToString().Trim();
            string devolutionImprovementStepsComment = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.ImprovementSteps.COMMENT].ToString().Trim();
            string devolutionUserStrengthsComment = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.UserStrengths.COMMENT].ToString().Trim();

            if (!disputationComment.Equals("-") && disputationComment.Length > 1)
            {
                string disputationCreationDateExcelField = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Disputation.CREATION_DATE].ToString().Trim();
                DateTime? disputationCreationDate = null;
                if (!string.IsNullOrEmpty(disputationCreationDateExcelField))
                    disputationCreationDate = Convert.ToDateTime(excelParser.FormatDate(disputationCreationDateExcelField));

                TransactionCommentary disputationCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DISPUTE,
                        0,
                        disputationComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                disputationCommentary.BasicInfo.CreationDate = disputationCreationDate != null ? disputationCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(disputationCommentary);
            }

            if (!invalidationComment.Equals("-") && invalidationComment.Length > 1)
            {
                string invalidationCreationDateExcelField = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Invalidation.CREATION_DATE].ToString().Trim();
                DateTime? invalidationCreationDate = null;
                if (!string.IsNullOrEmpty(invalidationCreationDateExcelField))
                    invalidationCreationDate = Convert.ToDateTime(excelParser.FormatDate(invalidationCreationDateExcelField));

                TransactionCommentary invalidationCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.INVALIDATION,
                        0,
                        invalidationComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                invalidationCommentary.BasicInfo.CreationDate = invalidationCreationDate != null ? invalidationCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(invalidationCommentary);
            }

            if (!devolutionGeneralComment.Equals("-") && devolutionGeneralComment.Length > 1)
            {
                string devolutionGeneralCreationDateExcelField = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE].ToString().Trim();
                DateTime? devolutionGeneralCreationDate = null;
                if (!string.IsNullOrEmpty(devolutionGeneralCreationDateExcelField))
                    devolutionGeneralCreationDate = Convert.ToDateTime(excelParser.FormatDate(devolutionGeneralCreationDateExcelField));

                TransactionCommentary devolutionGeneralCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_COMMENTARIES,
                        0,
                        devolutionGeneralComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                devolutionGeneralCommentary.BasicInfo.CreationDate = devolutionGeneralCreationDate != null ? devolutionGeneralCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(devolutionGeneralCommentary);
            }

            if (!devolutionImprovementStepsComment.Equals("-") && devolutionImprovementStepsComment.Length > 1)
            {
                string devolutionImprovementStepsCreationDateExcelField = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE].ToString().Trim();
                DateTime? devolutionImprovementStepsCreationDate = null;
                if (!string.IsNullOrEmpty(devolutionImprovementStepsCreationDateExcelField))
                    devolutionImprovementStepsCreationDate = Convert.ToDateTime(excelParser.FormatDate(devolutionImprovementStepsCreationDateExcelField));

                TransactionCommentary devolutionImprovementStepsCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_IMPROVEMENT_STEPS,
                        0,
                        devolutionImprovementStepsComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                devolutionImprovementStepsCommentary.BasicInfo.CreationDate = devolutionImprovementStepsCreationDate != null ? devolutionImprovementStepsCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(devolutionImprovementStepsCommentary);
            }

            if (!devolutionUserStrengthsComment.Equals("-") && devolutionUserStrengthsComment.Length > 1)
            {
                string devolutionUserStrengthsCreationDateExcelField = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Commentaries.Devolution.General.CREATION_DATE].ToString().Trim();
                DateTime? devolutionUserStrengthsCreationDate = null;
                if (!string.IsNullOrEmpty(devolutionUserStrengthsCreationDateExcelField))
                    devolutionUserStrengthsCreationDate = Convert.ToDateTime(excelParser.FormatDate(devolutionUserStrengthsCreationDateExcelField));

                TransactionCommentary devolutionUserStrengthsCommentary =
                    new TransactionCommentary(
                        (int)SCC_BL.DBValues.Catalog.TRANSACTION_COMMENT_TYPE.DEVOLUTION_USER_STRENGTHS,
                        0,
                        devolutionUserStrengthsComment,
                        GetActualUser().ID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION_COMMENTARY.CREATED
                    );

                devolutionUserStrengthsCommentary.BasicInfo.CreationDate = devolutionUserStrengthsCreationDate != null ? devolutionUserStrengthsCreationDate.Value : DateTime.Now;

                transactionCommentaryList.Add(devolutionUserStrengthsCommentary);
            }

            return transactionCommentaryList;
        }

        DateTime? GetMinDate(System.Data.DataTable dt)
        {
            DateTime? minDate = null;

            int headersCount = dt.Columns.Count;

            Parallel.ForEach<System.Data.DataRow>(dt.Rows.Cast<System.Data.DataRow>(), (row) => {
                string excelFieldStartDate = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE].ToString().Trim();

                DateTime? currentDate = null;

                if (!string.IsNullOrEmpty(excelFieldStartDate))
                {
                    using (ExcelParser excelParser = new ExcelParser())
                        currentDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldStartDate));
                }

                if (currentDate != null)
                {
                    if (minDate == null)
                        minDate = currentDate;
                    else
                        minDate = currentDate < minDate ? currentDate : minDate;
                }
            });

            return minDate;
        }

        DateTime? GetMaxDate(System.Data.DataTable dt)
        {
            DateTime? minDate = null;

            int headersCount = dt.Columns.Count;

            Parallel.ForEach<System.Data.DataRow>(dt.Rows.Cast<System.Data.DataRow>(), (row) => {
                string excelFieldStartDate = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE].ToString().Trim();

                DateTime? currentDate = null;

                if (!string.IsNullOrEmpty(excelFieldStartDate))
                {
                    using (ExcelParser excelParser = new ExcelParser())
                        currentDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldStartDate));
                }

                if (currentDate != null)
                {
                    if (minDate == null)
                        minDate = currentDate;
                    else
                        minDate = currentDate > minDate ? currentDate : minDate;
                }
            });

            return minDate;
        }

        User GetAgentUserByIdentification(string userIdentification, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "User";

            if (string.IsNullOrEmpty(userIdentification))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.NO_IDENTIFICATION_ENTERED,
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            User user = new SCC_BL.User(userIdentification);

            try
            {
                //user.SetDataByUsername();
                user = _transactionImportUserList.Where(e => e.Username.ToUpper().Trim().Equals(user.Username.ToUpper().Trim())).FirstOrDefault();

                if (user == null)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.NO_IDENTIFICATION_FOUND
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userIdentification),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }
                else
                if (user.ID <= 0)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.NO_IDENTIFICATION_FOUND
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userIdentification),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }
            }
            catch (Exception ex)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Agent.UNKNOWN
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            return user;
        }

        User GetSupervisorUserByName(string userName, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "User";

            if (string.IsNullOrEmpty(userName))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            if (!userName.Contains(','))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_VALID_FORMAT
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            User user = new SCC_BL.User();

            try
            {
                string surname = userName.Split(',')[0].Trim();
                string firstName = userName.Split(',')[1].Trim();

                //user.SetDataByName(firstName, surname);
                user = 
                    _transactionImportUserList
                        .Where(e =>
                            e.Person.SurName.ToUpper().Trim().Equals(surname.ToUpper().Trim()) &&
                            e.Person.FirstName.ToUpper().Trim().Equals(firstName.ToUpper().Trim()))
                        .FirstOrDefault();

                if (user == null)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_NAME_FOUND
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }
                else
                if (user.ID <= 0)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.NO_NAME_FOUND
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }
            }
            catch (Exception ex)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Supervisor.UNKNOWN
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            return user;
        }

        User GetEvaluatorUserByName(string userName, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "User";

            if (string.IsNullOrEmpty(userName))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            if (!userName.Contains(','))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_VALID_FORMAT
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            User user = new SCC_BL.User();

            try
            {
                string surname = userName.Split(',')[0].Trim();
                string firstName = userName.Split(',')[1].Trim();

                //user.SetDataByName(firstName, surname);
                user =
                    _transactionImportUserList
                        .Where(e =>
                            e.Person.SurName.ToUpper().Trim().Equals(surname.ToUpper().Trim()) &&
                            e.Person.FirstName.ToUpper().Trim().Equals(firstName.ToUpper().Trim()))
                        .FirstOrDefault();

                if (user == null)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_NAME_FOUND
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }
                else
                if (user.ID <= 0)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.NO_NAME_FOUND
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, userName),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }
            }
            catch (Exception ex)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.UNKNOWN
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            return user;
        }

        ProgramFormCatalog GetProgramFormCatalogByProgramID(int programID, int currentRowCount, int currentColumnCount)
        {
            ProgramFormCatalog programFormCatalogResult = null;

            string transactionImportErrorElementName = "ProgramFormCatalog";

            using (ProgramFormCatalog programFormCatalog = SCC_BL.ProgramFormCatalog.ProgramFormCatalogWithProgramID(programID))
            {
                try
                {
                    List<ProgramFormCatalog> programFormCatalogList = programFormCatalog.SelectByProgramID();

                    if (programFormCatalogList.Count() <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.ProgramFormCatalog.NO_PROGRAM_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, programID.ToString()),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }

                    programFormCatalogResult = programFormCatalogList.LastOrDefault();
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.User.Evaluator.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    return null;
                }

                return programFormCatalogResult;
            }
        }

        Program GetProgramByName(string programName, int currentRowCount, int currentColumnCount, DateTime? minDate, DateTime? maxDate)
        {
            string transactionImportErrorElementName = "Program";

            if (string.IsNullOrEmpty(programName))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Program.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));
                return null;
            }

            using (Program program = new SCC_BL.Program(programName))
            {
                try
                {
                    program.SetDataByName();

                    if (program.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Program.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, programName),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName, 
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Program.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    //Se podría crear si se necesitase

                    return null;
                }

                return program;
            }
        }

        Program GetProgramByID(int programID)
        {
            string transactionImportErrorElementName = "Program";

            Program program = new SCC_BL.Program(programID);

            try
            {
                //program.SetDataByID();
                program = _transactionImportProgram;
            }
            catch (Exception ex)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Program.UNKNOWN
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                        0,
                        0));

                //Se podría crear si se necesitase

                return null;
            }

            return program;
        }

        Form GetFormByName(string formName, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "Form";

            if (string.IsNullOrEmpty(formName))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Form.NO_NAME_ENTERED,
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            using (Form form = new SCC_BL.Form(formName))
            {
                try
                {
                    form.SetDataByName();

                    if (form.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Form.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, formName),
                                currentRowCount,
                                currentColumnCount));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Form.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    //Se podría crear si se necesitase

                    return null;
                }

                return form;
            }
        }

        Form GetFormByID(int formID)
        {
            string transactionImportErrorElementName = "Form";

            using (Form form = new SCC_BL.Form(formID))
            {
                try
                {
                    form.SetDataByID();
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Form.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            0,
                            0));

                    //Se podría crear si se necesitase

                    return null;
                }

                return form;
            }
        }

        Transaction CreateTransactionFromRow(System.Data.DataRow row, int userToEvaluateID, int supervisorUserID, int evaluatorUserID, int formID, int currentRowCount)
        {
            bool hasError = false;

            string transactionImportErrorElementName = "Transaction";

            string rawIdentifier = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER].ToString().Trim();

            if (string.IsNullOrEmpty(rawIdentifier))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_IDENTIFIER_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.OLD_IDENTIFIER));

                hasError = true;
            }

            string identifier = SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.IDENTIFIER_PREFIX + rawIdentifier;

            string excelFieldEvaluationDate = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.EVALUATION_DATE].ToString().Trim();
            string excelFieldTransactionDate = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE].ToString().Trim();
            string excelFieldLoadDate = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.LOAD_DATE].ToString().Trim();

            if (string.IsNullOrEmpty(excelFieldEvaluationDate))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_EVALUATION_DATE_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.EVALUATION_DATE));

                hasError = true;
            }

            if (string.IsNullOrEmpty(excelFieldTransactionDate))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_TRANSACTION_DATE_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TRANSACTION_DATE));

                hasError = true;
            }

            if (string.IsNullOrEmpty(excelFieldLoadDate))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_LOAD_DATE_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.LOAD_DATE));

                hasError = true;
            }

            DateTime evaluationDate = DateTime.Now;
            DateTime transactionDate = DateTime.Now;
            DateTime loadDate = DateTime.Now;

            if (!string.IsNullOrEmpty(excelFieldEvaluationDate))
            {
                using (ExcelParser excelParser = new ExcelParser())
                    evaluationDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldEvaluationDate));
            }

            if (!string.IsNullOrEmpty(excelFieldTransactionDate))
            {
                using (ExcelParser excelParser = new ExcelParser())
                    transactionDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldTransactionDate));
            }

            if (!string.IsNullOrEmpty(excelFieldLoadDate))
            {
                using (ExcelParser excelParser = new ExcelParser())
                    loadDate = Convert.ToDateTime(excelParser.FormatDate(excelFieldLoadDate));
            }

            string comment = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.COMMENT].ToString().Trim();

            /*if (string.IsNullOrEmpty(comment))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_COMMENT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.COMMENT));

                hasError = true;
            }*/

            //GENERAL RESULTS

            string generalResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.GENERAL_RESULT].ToString().Trim();

            if (string.IsNullOrEmpty(generalResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_GENERAL_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.GENERAL_RESULT));

                hasError = true;
            }

            int generalResultID = 0;

            switch (generalResult)
            {
                case "Pasó":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS;
                    break;
                case "Falló":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL;
                    break;
                case "100%":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.SUCCESS;
                    break;
                case "0%":
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL;
                    break;
                default:
                    generalResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL.FAIL;
                    break;
            }

            string generalFUCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FINAL_USER_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(generalFUCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_FUCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FINAL_USER_CRITICAL_ERROR));

                hasError = true;
            }

            int generalFUCEResultID = 0;

            switch (generalFUCEResult)
            {
                case "Pasó":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    generalFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
            }

            string generalBCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.BUSINESS_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(generalBCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_BCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.BUSINESS_CRITICAL_ERROR));

                hasError = true;
            }

            int generalBCEResultID = 0;

            switch (generalBCEResult)
            {
                case "Pasó":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    generalBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
            }

            string generalFCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FULFILMENT_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(generalFCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_FCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.FULFILMENT_CRITICAL_ERROR));

                hasError = true;
            }

            int generalFCEResultID = 0;

            switch (generalFCEResult)
            {
                case "Pasó":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    generalFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_GENERAL_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
            }

            string generalNCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.NON_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(generalNCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.General.NO_NCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.General.NON_CRITICAL_ERROR));

                hasError = true;
            }

            int generalNCEResultValue = 0;

            if (!SCC_BL.Settings.Overall.NEUTRAL_VALUES.Contains(generalNCEResult))
            {
                try
                {
                    generalNCEResultValue = Convert.ToInt32(generalNCEResult.ToUpper().Replace("%", "").Replace("PTS", ""));
                }
                catch (Exception ex)
                {
                }
            }

            //ACCURATE RESULTS

            int accurateResultID = generalResultID;

            string accurateFUCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FINAL_USER_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(accurateFUCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Accurate.NO_FUCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FINAL_USER_CRITICAL_ERROR));

                hasError = true;
            }

            int accurateFUCEResultID = 0;

            switch (accurateFUCEResult)
            {
                case "Pasó":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    accurateFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
            }

            string accurateBCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.BUSINESS_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(accurateBCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Accurate.NO_BCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.BUSINESS_CRITICAL_ERROR));

                hasError = true;
            }

            int accurateBCEResultID = 0;

            switch (accurateBCEResult)
            {
                case "Pasó":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    accurateBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
            }

            string accurateFCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FULFILMENT_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(accurateFCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Accurate.NO_FCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Accurate.FULFILMENT_CRITICAL_ERROR));

                hasError = true;
            }

            int accurateFCEResultID = 0;

            switch (accurateFCEResult)
            {
                case "Pasó":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    accurateFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_ACCURATE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
            }

            int accurateNCEResultValue = generalNCEResultValue;

            //CONTROLLABLE RESULTS

            int controllableResultID = generalResultID;

            string controllableFUCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FINAL_USER_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(controllableFUCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Controllable.NO_FUCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FINAL_USER_CRITICAL_ERROR));

                hasError = true;
            }

            int controllableFUCEResultID = 0;

            switch (controllableFUCEResult)
            {
                case "Pasó":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    controllableFUCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FINAL_USER_CRITICAL_ERROR.FAIL;
                    break;
            }

            string controllableBCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.BUSINESS_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(controllableBCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Controllable.NO_BCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.BUSINESS_CRITICAL_ERROR));

                hasError = true;
            }

            int controllableBCEResultID = 0;

            switch (controllableBCEResult)
            {
                case "Pasó":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    controllableBCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_BUSINESS_CRITICAL_ERROR.FAIL;
                    break;
            }

            string controllableFCEResult = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FULFILMENT_CRITICAL_ERROR].ToString().Trim();

            if (string.IsNullOrEmpty(controllableFCEResult))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.Results.Controllable.NO_FCE_RESULT_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.Results.Controllable.FULFILMENT_CRITICAL_ERROR));

                hasError = true;
            }

            int controllableFCEResultID = 0;

            switch (controllableFCEResult)
            {
                case "Pasó":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "Falló":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                case "100%":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.SUCCESS;
                    break;
                case "0%":
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
                default:
                    controllableFCEResultID = (int)SCC_BL.DBValues.Catalog.TRANSACTION_CONTROLLABLE_RESULT_FULFILLMENT_CRITICAL_ERROR.FAIL;
                    break;
            }

            int controllableNCEResultValue = generalNCEResultValue;

            string timeElapsed = row.ItemArray[(int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TIME_ELAPSED].ToString().Trim();

            if (string.IsNullOrEmpty(timeElapsed))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_TIME_ELAPSED_ENTERED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TIME_ELAPSED));

                hasError = true;
            }

            TimeSpan timeElapsedValue = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            try
            {
                if (!string.IsNullOrEmpty(timeElapsed))
                {
                    int hours = 0;
                    int minutes = 0;
                    int seconds = 0;

                    if (timeElapsed.Contains("day"))
                    {
                        hours = 23;
                        minutes = 59;
                        seconds = 59;
                    }
                    else
                    {

                        hours = Convert.ToInt32(timeElapsed.Split(':')[0]);
                        minutes = Convert.ToInt32(timeElapsed.Split(':')[1]);
                        seconds = Convert.ToInt32(timeElapsed.Split(':')[2]);

                        if (hours > 23)
                        {
                            hours = 23;
                            minutes = 59;
                            seconds = 59;
                        }
                    }

                    timeElapsedValue = new TimeSpan(hours, minutes, seconds);
                }
                else
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.BAD_FORMAT_TIME_ELAPSED,
                            currentRowCount,
                            (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TIME_ELAPSED));

                    hasError = true;
                }
            }
            catch (Exception ex)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.BAD_FORMAT_TIME_ELAPSED,
                        currentRowCount,
                        (int)SCC_BL.Settings.AppValues.ExcelTasks.Transaction.ImportData.Transaction.ExcelFields.BaseInfo.TIME_ELAPSED));

                hasError = true;
            }

            Transaction transaction = new Transaction(
                identifier, 
                userToEvaluateID, 
                evaluatorUserID, 
                evaluationDate, 
                transactionDate, 
                loadDate, 
                formID, 
                comment, 
                generalResultID, 
                generalFUCEResultID, 
                generalBCEResultID, 
                generalFCEResultID, 
                generalNCEResultValue, 
                accurateResultID, 
                accurateFUCEResultID, 
                accurateBCEResultID, 
                accurateFCEResultID, 
                accurateNCEResultValue, 
                controllableResultID, 
                controllableFUCEResultID, 
                controllableBCEResultID, 
                controllableFCEResultID,
                controllableNCEResultValue, 
                timeElapsedValue, 
                GetActualUser().ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_TRANSACTION.CREATED, 
                (int)SCC_BL.DBValues.Catalog.TRANSACTION_TYPE.EVALUATION, 
                null);

            return hasError ? null : transaction;
        }

        Transaction GetTransactionByIdentifier(string transactionIdentifier, int currentRowCount, int currentColumnCount)
        {
            string transactionImportErrorElementName = "Transaction";

            if (string.IsNullOrEmpty(transactionIdentifier))
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_IDENTIFIER_ENTERED,
                        currentRowCount,
                        currentColumnCount));

                return null;
            }

            using (Transaction transaction = new SCC_BL.Transaction(transactionIdentifier))
            {
                try
                {
                    transaction.SetDataByIdentifier();

                    if (transaction.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.NO_IDENTIFIER_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, transactionIdentifier),
                                currentRowCount,
                                currentColumnCount,
                                SCC_BL.Settings.Notification.Type.INFO));

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Transaction.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentColumnCount));

                    //Se podría crear si se necesitase

                    return null;
                }

                return transaction;
            }
        }

        string Replace(string currentValue, string[] oldValueArray, string newValueArray)
        {
            for (int i = 0; i < oldValueArray.Length; i++)
            {
                currentValue = currentValue.Replace(oldValueArray[i], newValueArray);
            }

            return currentValue;
        }

        class ImportTransactionAttributeHelper
        {
            public SCC_BL.Attribute Attribute { get; set; }
            public SCC_BL.AttributeValueCatalog AttributeValueCatalog { get; set; }
            public string AttributeTransactionComment { get; set; }

            public ImportTransactionAttributeHelper(SCC_BL.Attribute attribute, SCC_BL.AttributeValueCatalog attributeValueCatalog, string attributeTransactionComment)
            {
                if (attributeTransactionComment.Length <= 1) attributeTransactionComment = string.Empty;

                this.Attribute = attribute;
                this.AttributeValueCatalog = attributeValueCatalog;
                this.AttributeTransactionComment = attributeTransactionComment;
            }
        }

        List<ImportTransactionAttributeHelper> GetAttributesFromRow(List<string> headerList, System.Data.DataRow row, int firstAttributeIndex, int lastAttributeIndex, int formID, int currentRowCount, Form form)
        {
            List<ImportTransactionAttributeHelper> importTransactionAttributeHelperList = new List<ImportTransactionAttributeHelper>();

            string transactionImportErrorElementName = "Attribute";

            if (lastAttributeIndex - firstAttributeIndex < 3)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName, 
                        SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_COLUMNS, 
                        currentRowCount,
                        firstAttributeIndex));

                return null;
            }

            int currentCellIndex = firstAttributeIndex;

            while (currentCellIndex < lastAttributeIndex)
            {
                void _setNextAttributeIndex()
                {
                    currentCellIndex += 3;
                }

                bool isRepeated = false;
                int repeatedIndex = 0;

                string attributeName = headerList[(int)currentCellIndex].ToString().Trim();

                isRepeated = attributeName.Contains(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN);

                if (isRepeated)
                    repeatedIndex = Convert.ToInt32(attributeName.Substring(attributeName.LastIndexOf('_') + 1));

                attributeName =
                    isRepeated
                        ? attributeName.Substring(
                            0,
                            attributeName.IndexOf(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN))
                        : attributeName;

                string attributeValue = row.ItemArray[(int)currentCellIndex].ToString().Trim();
                string attributeTransactionComment = row.ItemArray[(int)currentCellIndex + 1].ToString().Trim();
                string attributeSubattributes = row.ItemArray[(int)currentCellIndex + 2].ToString().Trim();

                attributeName = Replace(attributeName, SCC_BL.Settings.Overall.ErrorType.List.Select(e => "(" + e + ")").ToArray(), "");
                attributeName = attributeName.Trim();

                if (attributeValue.Equals("-")) attributeValue = string.Empty;
                attributeValue = attributeValue.Trim();

                if (string.IsNullOrEmpty(attributeValue))
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_VALUE_ENTERED,
                            currentRowCount,
                            currentCellIndex));

                    _setNextAttributeIndex();
                    continue;
                }

                if (attributeSubattributes.Equals("~")) attributeSubattributes = string.Empty;

                if (attributeSubattributes.Length > 1 && attributeSubattributes.ElementAt(0) == '~') attributeSubattributes = attributeSubattributes.Substring(1);

                try
                {
                    SCC_BL.Attribute attribute = null;

                    if (isRepeated)
                    {
                        int repeatedCounter = 0;

                        List<SCC_BL.Attribute> auxList = 
                            form.AttributeList
                                .Where(e =>
                                    e.Name.Trim().ToUpper().Equals(attributeName.Trim().ToUpper()) &&
                                    (e.ParentAttributeID == 0 || e.ParentAttributeID == null))
                                .ToList();

                        foreach (SCC_BL.Attribute auxAttribute in auxList)
                        {
                            if (repeatedCounter == repeatedIndex)
                            {
                                attribute = auxAttribute;
                                break;
                            }

                            repeatedCounter++;
                        }
                    }
                    else
                    {
                        try
                        {
                            attribute =
                                form.AttributeList
                                    .Where(e =>
                                        e.Name.Trim().ToUpper().Equals(attributeName.Trim().ToUpper()) &&
                                        (e.ParentAttributeID == 0 || e.ParentAttributeID == null))
                                    .FirstOrDefault();
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (attribute == null)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextAttributeIndex();
                        continue;
                    }
                    else
                    if (attribute.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextAttributeIndex();
                        continue;
                    }

                    SCC_BL.AttributeValueCatalog attributeValueCatalog = new SCC_BL.AttributeValueCatalog();

                    attributeValueCatalog =
                        attribute.ValueList
                            .Where(e =>
                                e.AttributeID == attribute.ID &&
                                e.Value.Trim().ToUpper().Equals(attributeValue.Trim().ToUpper()))
                            .FirstOrDefault();

                    if (attributeValueCatalog == null)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_VALUE_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeValue),
                                currentRowCount,
                                currentCellIndex));

                        _setNextAttributeIndex();
                        continue;
                    }
                    else
                    if (attributeValueCatalog.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_VALUE_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeValue),
                                currentRowCount,
                                currentCellIndex));

                        _setNextAttributeIndex();
                        continue;
                    }

                    importTransactionAttributeHelperList.Add(
                        new ImportTransactionAttributeHelper(
                            attribute,
                            attributeValueCatalog,
                            attributeTransactionComment
                        )
                    );

                    string[] subattributeArray = attributeSubattributes.Split('~');

                    List<SCC_BL.Attribute> tempAttributeHierarchy = new List<SCC_BL.Attribute>() { attribute };

                    for (int i = 0; i < subattributeArray.Length; i++)
                    {
                        string currentSubattributeName = subattributeArray[i].Trim();

                        if (string.IsNullOrEmpty(currentSubattributeName)) continue;

                        if (currentSubattributeName.Length > 1 && currentSubattributeName.ElementAt(0) == '~') currentSubattributeName = currentSubattributeName.Substring(1);

                        int indexTempAttributeHierarchy = tempAttributeHierarchy.Count() - 1;
                        bool foundTempAttribute = false;

                        while (indexTempAttributeHierarchy >= 0)
                        {
                            using (SCC_BL.Attribute subattribute = SCC_BL.Attribute.AttributeWithParentAttributeIDAndName(tempAttributeHierarchy[indexTempAttributeHierarchy].ID, currentSubattributeName))
                            {
                                try
                                {
                                    subattribute.SetSubattributeDataByName();

                                    if (subattribute.ID > 0 && subattribute.ID != null)
                                    {
                                        foundTempAttribute = true;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _transactionImportErrorList.Add(
                                        new SCC_BL.Helpers.Transaction.Import.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                            currentRowCount,
                                            currentCellIndex + 2));
                                }
                            }

                            indexTempAttributeHierarchy--;
                        }

                        if (!foundTempAttribute) continue;

                        using (SCC_BL.Attribute subattribute = SCC_BL.Attribute.AttributeWithParentAttributeIDAndName(tempAttributeHierarchy[indexTempAttributeHierarchy].ID, currentSubattributeName))
                        {
                            try
                            {
                                subattribute.SetSubattributeDataByName();

                                if (subattribute == null)
                                {
                                    _transactionImportErrorList.Add(
                                        new SCC_BL.Helpers.Transaction.Import.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_SUBATTRIBUTE_NAME_FOUND
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubattributeName),
                                            currentRowCount,
                                            currentCellIndex + 2));

                                    break;
                                }
                                else
                                if (subattribute.ID <= 0)
                                {
                                    _transactionImportErrorList.Add(
                                        new SCC_BL.Helpers.Transaction.Import.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_SUBATTRIBUTE_NAME_FOUND
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubattributeName),
                                            currentRowCount,
                                            currentCellIndex + 2));

                                    break;
                                }

                                importTransactionAttributeHelperList.Add(
                                    new ImportTransactionAttributeHelper(
                                        subattribute,
                                        null,
                                        string.Empty
                                    )
                                );

                                tempAttributeHierarchy.Add(subattribute);
                            }
                            catch (Exception ex)
                            {
                                _transactionImportErrorList.Add(
                                    new SCC_BL.Helpers.Transaction.Import.Error(
                                        transactionImportErrorElementName,
                                        SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                            .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                        currentRowCount,
                                        currentCellIndex + 2));

                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentCellIndex));

                    //Se podría crear si se necesitase

                    _setNextAttributeIndex();
                    continue;
                }

                /*using (SCC_BL.Attribute attribute = SCC_BL.Attribute.AttributeWithFormIDAndName(formID, attributeName))
                {
                    try
                    {
                        attribute.SetDataByName();

                        if (attribute.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_NAME_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeName),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextAttributeIndex();
                            continue;
                        }

                        SCC_BL.AttributeValueCatalog attributeValueCatalog = new SCC_BL.AttributeValueCatalog(attribute.ID, attributeValue);
                        attributeValueCatalog.SetDataByAttributeIDAndValue();

                        if (attributeValueCatalog.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_VALUE_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, attributeValue),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextAttributeIndex();
                            continue;
                        }

                        importTransactionAttributeHelperList.Add(
                            new ImportTransactionAttributeHelper(
                                attribute,
                                attributeValueCatalog,
                                attributeTransactionComment
                            )
                        );

                        string[] subattributeArray = attributeSubattributes.Split('~');

                        List<SCC_BL.Attribute> tempAttributeHierarchy = new List<SCC_BL.Attribute>() { attribute };

                        for (int i = 0; i < subattributeArray.Length; i++)
                        {
                            string currentSubattributeName = subattributeArray[i].Trim();

                            if (string.IsNullOrEmpty(currentSubattributeName)) continue;

                            if (currentSubattributeName.Length > 1 && currentSubattributeName.ElementAt(0) == '~') currentSubattributeName = currentSubattributeName.Substring(1);

                            using (SCC_BL.Attribute subattribute = SCC_BL.Attribute.AttributeWithParentAttributeIDAndName(tempAttributeHierarchy.LastOrDefault().ID, currentSubattributeName))
                            {
                                try
                                {
                                    subattribute.SetSubattributeDataByName();

                                    if (subattribute.ID <= 0)
                                    {
                                        _transactionImportErrorList.Add(
                                            new SCC_BL.Helpers.Transaction.Import.Error(
                                                transactionImportErrorElementName,
                                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.NO_SUBATTRIBUTE_NAME_FOUND
                                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubattributeName),
                                                currentRowCount,
                                                currentCellIndex + 2));

                                        break;
                                    }

                                    importTransactionAttributeHelperList.Add(
                                        new ImportTransactionAttributeHelper(
                                            subattribute,
                                            null,
                                            string.Empty
                                        )
                                    );

                                    tempAttributeHierarchy.Add(subattribute);
                                }
                                catch (Exception ex)
                                {
                                    _transactionImportErrorList.Add(
                                        new SCC_BL.Helpers.Transaction.Import.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                            currentRowCount,
                                            currentCellIndex + 2));

                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.Attribute.UNKNOWN
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                currentRowCount,
                                currentCellIndex));

                        //Se podría crear si se necesitase

                        _setNextAttributeIndex();
                        continue;
                    }
                }*/

                _setNextAttributeIndex();
            }

            return importTransactionAttributeHelperList;
        }

        class ImportTransactionCustomControlHelper
        {
            public SCC_BL.CustomField CustomField { get; set; }
            public SCC_BL.CustomControlValueCatalog CustomControlValueCatalog { get; set; }
            public string CustomControlComment { get; set; } = string.Empty;

            public ImportTransactionCustomControlHelper(SCC_BL.CustomField customField, SCC_BL.CustomControlValueCatalog customControlValueCatalog, string customControlComment)
            {
                this.CustomField = customField;
                this.CustomControlValueCatalog = customControlValueCatalog;
                this.CustomControlComment = customControlComment;
            }
        }

        List<ImportTransactionCustomControlHelper> GetCustomControlsFromRow(List<string> headerList, System.Data.DataRow row, int customControlStartIndex, int customControlEndIndex, int currentRowCount, Form form)
        {
            List<ImportTransactionCustomControlHelper> importTransactionCustomControlHelperList = new List<ImportTransactionCustomControlHelper>();

            string transactionImportErrorElementName = "CustomControl";

            if (customControlEndIndex - customControlStartIndex < 1)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName, 
                        SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_COLUMNS, 
                        currentRowCount,
                        customControlStartIndex));

                return null;
            }

            int currentCellIndex = customControlStartIndex;

            while (currentCellIndex < customControlEndIndex)
            {
                void _setNextCustomControlIndex()
                {
                    currentCellIndex += 1;
                }

                bool isRepeated = false;
                int repeatedIndex = 0;

                string customControlName = headerList[(int)currentCellIndex].Trim();

                isRepeated = customControlName.Contains(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN);

                if (isRepeated)
                    repeatedIndex = Convert.ToInt32(customControlName.Substring(customControlName.LastIndexOf('_') + 1));

                customControlName =
                    isRepeated
                        ? customControlName.Substring(
                            0, 
                            customControlName.IndexOf(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN))
                        : customControlName;

                string customControlValue = row.ItemArray[(int)currentCellIndex].ToString().Trim();
                string customControlComment = row.ItemArray[(int)currentCellIndex].ToString().Trim();

                if (customControlValue.Equals("-")) customControlValue = string.Empty;
                customControlValue = customControlValue.Trim();

                /*if (string.IsNullOrEmpty(customControlValue))
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_VALUE_ENTERED,
                            currentRowCount,
                            currentCellIndex));

                    _setNextCustomControlIndex();
                    continue;
                }*/

                try
                {
                    CustomControl customControl = null;

                    if (isRepeated)
                    {
                        int repeatedCounter = 0;

                        List<CustomControl> auxList = form.CustomControlList.Where(e => e.Label.Trim().ToUpper().Equals(customControlName.Trim().ToUpper())).ToList();

                        foreach (CustomControl auxCustomControl in auxList)
                        {
                            if (repeatedCounter == repeatedIndex)
                            {
                                customControl = auxCustomControl;
                                break;
                            }

                            repeatedCounter++;
                        }
                    }
                    else
                    {
                        try
                        {
                            customControl =
                                form.CustomControlList
                                    .Where(e =>
                                        e.Label.Trim().ToUpper().Equals(customControlName.Trim().ToUpper()))
                                    .FirstOrDefault();
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (customControl == null)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextCustomControlIndex();
                        continue;
                    }
                    else
                    if (customControl.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextCustomControlIndex();
                        continue;
                    }

                    SCC_BL.CustomControlValueCatalog customControlValueCatalog = null;

                    SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE customControlType = (SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE)customControl.ControlTypeID;

                    if (
                        customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.TEXT_AREA || 
                        customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.TEXT_BOX)
                    {
                        switch (customControl.Mask)
                        {
                            case SCC_BL.Settings.AppValues.Masks.Alphanumeric1.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.Date1.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.Email1.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.SurName1.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.Name1.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.PhoneNumber1.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.PhoneNumber2.MASK:
                                break;
                            case SCC_BL.Settings.AppValues.Masks.Time1.MASK:
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    if (
                        customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.CHECKBOX || 
                        customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.RADIO_BUTTON || 
                        customControlType == SCC_BL.DBValues.Catalog.CUSTOM_CONTROL_TYPE.SELECT_LIST)
                    {
                        if (string.IsNullOrEmpty(customControlValue))
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.EMPTY_VALUE_FOUND,
                                    currentRowCount,
                                    currentCellIndex,
                                    SCC_BL.Settings.Notification.Type.INFO));

                            _setNextCustomControlIndex();
                            continue;
                        }

                        customControlValueCatalog =
                            customControl.ValueList
                                .Where(e =>
                                    e.Value.Trim().ToUpper().Equals(customControlValue.Trim().ToUpper()))
                                .FirstOrDefault();

                        if (customControlValueCatalog == null)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_VALUE_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT_2, customControlName)
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlValue),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextCustomControlIndex();
                            continue;
                        }
                        else
                        if (customControlValueCatalog.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_VALUE_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT_2, customControlName)
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlValue),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextCustomControlIndex();
                            continue;
                        }
                    }

                    SCC_BL.CustomField customField = new SCC_BL.CustomField();

                    customField =
                        form.CustomFieldList
                            .Where(e =>
                                e.CustomControlID.Equals(customControl.ID))
                            .FirstOrDefault();

                    if (customField.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_CUSTOM_FIELD_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextCustomControlIndex();
                        continue;
                    }

                    importTransactionCustomControlHelperList.Add(
                        new ImportTransactionCustomControlHelper(
                            customField,
                            customControlValueCatalog,
                            customControlComment
                        )
                    );
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentCellIndex));

                    //Se podría crear si se necesitase

                    _setNextCustomControlIndex();
                    continue;
                }

                /*using (SCC_BL.CustomControl customControl = new SCC_BL.CustomControl(customControlName))
                {
                    try
                    {
                        customControl.SetDataByLabel();

                        if (customControl.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_NAME_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlName),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextCustomControlIndex();
                            continue;
                        }

                        SCC_BL.CustomControlValueCatalog customControlValueCatalog = new SCC_BL.CustomControlValueCatalog(customControl.ID, customControlValue);
                        customControlValueCatalog.SetDataByCustomControlIDAndValue();

                        if (customControlValueCatalog.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.NO_VALUE_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, customControlValue),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextCustomControlIndex();
                            continue;
                        }

                        importTransactionCustomControlHelperList.Add(
                            new ImportTransactionCustomControlHelper(
                                customControl,
                                customControlValueCatalog
                            )
                        );
                    }
                    catch (Exception ex)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.CustomControl.UNKNOWN
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                currentRowCount,
                                currentCellIndex));

                        //Se podría crear si se necesitase

                        _setNextCustomControlIndex();
                        continue;
                    }
                }*/

                _setNextCustomControlIndex();
            }

            return importTransactionCustomControlHelperList;
        }

        class ImportTransactionBusinessIntelligenceFieldHelper
        {
            public SCC_BL.BusinessIntelligenceField BusinessIntelligenceField { get; set; }
            public string BusinessIntelligenceValueTransactionComment { get; set; }

            public ImportTransactionBusinessIntelligenceFieldHelper(SCC_BL.BusinessIntelligenceField businessIntelligenceField, string businessIntelligenceValueTransactionComment)
            {
                if (businessIntelligenceValueTransactionComment.Length <= 1) businessIntelligenceValueTransactionComment = string.Empty;

                this.BusinessIntelligenceField = businessIntelligenceField;
                this.BusinessIntelligenceValueTransactionComment = businessIntelligenceValueTransactionComment;
            }
        }

        List<ImportTransactionBusinessIntelligenceFieldHelper> GetBusinessIntelligenceFieldsFromRow(List<string> headerList, System.Data.DataRow row, int biFieldStartIndex, int biFieldEndIndex, int currentRowCount, Form form)
        {
            List<ImportTransactionBusinessIntelligenceFieldHelper> importTransactionBusinessIntelligenceFieldHelperList = new List<ImportTransactionBusinessIntelligenceFieldHelper>();

            string transactionImportErrorElementName = "BusinessIntelligenceField";

            if (biFieldEndIndex - biFieldStartIndex < 2)
            {
                _transactionImportErrorList.Add(
                    new SCC_BL.Helpers.Transaction.Import.Error(
                        transactionImportErrorElementName,
                        SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_COLUMNS,
                        currentRowCount,
                        biFieldStartIndex));

                return null;
            }

            int currentCellIndex = biFieldStartIndex;

            while (currentCellIndex < biFieldEndIndex)
            {
                void _setNextBusinessIntelligenceFieldIndex()
                {
                    currentCellIndex += 2;
                }

                bool isRepeated = false;
                int repeatedIndex = 0;

                string businessIntelligenceFieldName = headerList[(int)currentCellIndex].ToString().Trim();

                isRepeated = businessIntelligenceFieldName.Contains(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN);

                if (isRepeated)
                    repeatedIndex = Convert.ToInt32(businessIntelligenceFieldName.Substring(businessIntelligenceFieldName.LastIndexOf('_') + 1));

                businessIntelligenceFieldName =
                    isRepeated
                        ? businessIntelligenceFieldName.Substring(
                            0,
                            businessIntelligenceFieldName.IndexOf(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN))
                        : businessIntelligenceFieldName;

                /*businessIntelligenceFieldName =
                    businessIntelligenceFieldName.Contains(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN)
                        ? businessIntelligenceFieldName.Substring(
                            0,
                            businessIntelligenceFieldName.IndexOf(SCC_BL.Settings.Overall.ImportTasks.Transaction.REPEATED_COLUMN))
                        : businessIntelligenceFieldName;*/

                string businessIntelligenceFieldSubFields = row.ItemArray[(int)currentCellIndex].ToString().Trim();
                string businessIntelligenceFieldTransactionComment = row.ItemArray[(int)currentCellIndex + 1].ToString().Trim();

                if (businessIntelligenceFieldName.Substring(0, 4).Equals("BI: "))
                    businessIntelligenceFieldName = businessIntelligenceFieldName.Substring(4);

                businessIntelligenceFieldName = businessIntelligenceFieldName.Trim();

                if (businessIntelligenceFieldSubFields.Equals("-")) businessIntelligenceFieldSubFields = string.Empty;

                try
                {
                    SCC_BL.BusinessIntelligenceField businessIntelligenceField = null;

                    if (isRepeated)
                    {
                        int repeatedCounter = 0;

                        List<SCC_BL.BusinessIntelligenceField> auxList = 
                            form.BusinessIntelligenceFieldList
                                .Where(e =>
                                    e.ParentBIFieldID == null &&
                                    e.Name
                                        .Trim()
                                        .ToUpper()
                                        .Equals(
                                            businessIntelligenceFieldName
                                                .Trim()
                                                .ToUpper()))
                                .ToList();

                        foreach (SCC_BL.BusinessIntelligenceField auxBusinessIntelligenceField in auxList)
                        {
                            if (repeatedCounter == repeatedIndex)
                            {
                                businessIntelligenceField = auxBusinessIntelligenceField;
                                break;
                            }

                            repeatedCounter++;
                        }
                    }
                    else
                    {
                        businessIntelligenceField =
                            form.BusinessIntelligenceFieldList
                                .Where(e =>
                                    e.ParentBIFieldID == null &&
                                    e.Name
                                        .Trim()
                                        .ToUpper()
                                        .Equals(
                                            businessIntelligenceFieldName
                                                .Trim()
                                                .ToUpper()))
                                .FirstOrDefault();
                    }

                    if (businessIntelligenceField == null)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, businessIntelligenceFieldName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextBusinessIntelligenceFieldIndex();
                        continue;
                    }

                    if (businessIntelligenceField.ID <= 0)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_NAME_FOUND
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, businessIntelligenceFieldName),
                                currentRowCount,
                                currentCellIndex));

                        _setNextBusinessIntelligenceFieldIndex();
                        continue;
                    }

                    importTransactionBusinessIntelligenceFieldHelperList.Add(
                        new ImportTransactionBusinessIntelligenceFieldHelper(
                            businessIntelligenceField,
                            businessIntelligenceFieldTransactionComment
                        )
                    );

                    string[] subfieldArray = businessIntelligenceFieldSubFields.Split('~');

                    List<SCC_BL.BusinessIntelligenceField> tempBusinessIntelligenceFieldHierarchy = new List<SCC_BL.BusinessIntelligenceField>() { businessIntelligenceField };

                    for (int i = 0; i < subfieldArray.Length; i++)
                    {
                        string currentSubfieldName = subfieldArray[i].Trim();

                        if (currentSubfieldName.Length > 1 && currentSubfieldName.ElementAt(0) == '~') currentSubfieldName = currentSubfieldName.Substring(1);
                        if (currentSubfieldName.Length > 1 && currentSubfieldName.ElementAt(0) == '-') currentSubfieldName = currentSubfieldName.Substring(1);

                        if (currentSubfieldName.Equals("~")) currentSubfieldName = string.Empty;
                        if (currentSubfieldName.Equals("-")) currentSubfieldName = string.Empty;

                        if (string.IsNullOrEmpty(currentSubfieldName)) break;

                        SCC_BL.BusinessIntelligenceField subfield = new SCC_BL.BusinessIntelligenceField();

                        int indexTempBusinessIntelligenceFieldHierarchy = tempBusinessIntelligenceFieldHierarchy.Count() - 1;
                        bool foundTempBusinessIntelligenceField = false;

                        while (indexTempBusinessIntelligenceFieldHierarchy >= 0)
                        {
                            using (SCC_BL.BusinessIntelligenceField subBusinessIntelligenceField = SCC_BL.BusinessIntelligenceField.BusinessIntelligenceFieldWithParentIDAndName(tempBusinessIntelligenceFieldHierarchy[indexTempBusinessIntelligenceFieldHierarchy].ID, currentSubfieldName))
                            {
                                try
                                {
                                    subBusinessIntelligenceField.SetDataByParentIDAndName();

                                    if (subBusinessIntelligenceField.ID > 0 && subBusinessIntelligenceField.ID != null)
                                    {
                                        foundTempBusinessIntelligenceField = true;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _transactionImportErrorList.Add(
                                        new SCC_BL.Helpers.Transaction.Import.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                            currentRowCount,
                                            currentCellIndex));
                                }
                            }

                            indexTempBusinessIntelligenceFieldHierarchy--;
                        }

                        if (!foundTempBusinessIntelligenceField) continue;

                        try
                        {
                            subfield =
                                form.BusinessIntelligenceFieldList
                                    .Where(e =>
                                        e.ParentBIFieldID == tempBusinessIntelligenceFieldHierarchy[indexTempBusinessIntelligenceFieldHierarchy].ID &&
                                        e.Name.Trim().ToUpper().Equals(currentSubfieldName.Trim().ToUpper()))
                                    .FirstOrDefault();

                            if (subfield == null)
                            {
                                _transactionImportErrorList.Add(
                                    new SCC_BL.Helpers.Transaction.Import.Error(
                                        transactionImportErrorElementName,
                                        SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_SUBFIELD_NAME_FOUND
                                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubfieldName),
                                        currentRowCount,
                                        currentCellIndex));

                                break;
                            }
                            else
                            if (subfield.ID <= 0)
                            {
                                _transactionImportErrorList.Add(
                                    new SCC_BL.Helpers.Transaction.Import.Error(
                                        transactionImportErrorElementName,
                                        SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_SUBFIELD_NAME_FOUND
                                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubfieldName),
                                        currentRowCount,
                                        currentCellIndex));

                                break;
                            }

                            importTransactionBusinessIntelligenceFieldHelperList.Add(
                                new ImportTransactionBusinessIntelligenceFieldHelper(
                                    subfield,
                                    string.Empty
                                )
                            );

                            tempBusinessIntelligenceFieldHierarchy.Add(subfield);
                        }
                        catch (Exception ex)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                    currentRowCount,
                                    currentCellIndex + 2));

                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _transactionImportErrorList.Add(
                        new SCC_BL.Helpers.Transaction.Import.Error(
                            transactionImportErrorElementName,
                            SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                            currentRowCount,
                            currentCellIndex));

                    //Se podría crear si se necesitase

                    _setNextBusinessIntelligenceFieldIndex();
                    continue;
                }

                /*using (SCC_BL.BusinessIntelligenceField businessIntelligenceField = new SCC_BL.BusinessIntelligenceField(businessIntelligenceFieldName))
                {
                    try
                    {
                        businessIntelligenceField.SetDataByParentIDAndName();

                        if (businessIntelligenceField.ID <= 0)
                        {
                            _transactionImportErrorList.Add(
                                new SCC_BL.Helpers.Transaction.Import.Error(
                                    transactionImportErrorElementName,
                                    SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_NAME_FOUND
                                        .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, businessIntelligenceFieldName),
                                    currentRowCount,
                                    currentCellIndex));

                            _setNextBusinessIntelligenceFieldIndex();
                            continue;
                        }

                        importTransactionBusinessIntelligenceFieldHelperList.Add(
                            new ImportTransactionBusinessIntelligenceFieldHelper(
                                businessIntelligenceField,
                                businessIntelligenceFieldTransactionComment
                            )
                        );

                        string[] subfieldArray = businessIntelligenceFieldSubFields.Split('~');

                        List<SCC_BL.BusinessIntelligenceField> tempBusinessIntelligenceFieldHierarchy = new List<SCC_BL.BusinessIntelligenceField>() { businessIntelligenceField };

                        for (int i = 0; i < subfieldArray.Length; i++)
                        {
                            string currentSubfieldName = subfieldArray[i].Trim();

                            if (currentSubfieldName.Length > 1 && currentSubfieldName.ElementAt(0) == '~') currentSubfieldName = currentSubfieldName.Substring(1);
                            if (currentSubfieldName.Length > 1 && currentSubfieldName.ElementAt(0) == '-') currentSubfieldName = currentSubfieldName.Substring(1);

                            if (currentSubfieldName.Equals("~")) currentSubfieldName = string.Empty;
                            if (currentSubfieldName.Equals("-")) currentSubfieldName = string.Empty;

                            if (string.IsNullOrEmpty(currentSubfieldName)) break;

                            using (SCC_BL.BusinessIntelligenceField subfield = new SCC_BL.BusinessIntelligenceField(currentSubfieldName, tempBusinessIntelligenceFieldHierarchy.LastOrDefault().ID))
                            {
                                try
                                {
                                    subfield.SetDataByParentIDAndName();

                                    if (subfield.ID <= 0)
                                    {
                                        _transactionImportErrorList.Add(
                                            new SCC_BL.Helpers.Transaction.Import.Error(
                                                transactionImportErrorElementName,
                                                SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.NO_SUBFIELD_NAME_FOUND
                                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_CUSTOM_CONTENT, currentSubfieldName),
                                                currentRowCount,
                                                currentCellIndex));

                                        break;
                                    }

                                    importTransactionBusinessIntelligenceFieldHelperList.Add(
                                        new ImportTransactionBusinessIntelligenceFieldHelper(
                                            subfield,
                                            string.Empty
                                        )
                                    );

                                    tempBusinessIntelligenceFieldHierarchy.Add(subfield);
                                }
                                catch (Exception ex)
                                {
                                    _transactionImportErrorList.Add(
                                        new SCC_BL.Helpers.Transaction.Import.Error(
                                            transactionImportErrorElementName,
                                            SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                            currentRowCount,
                                            currentCellIndex + 2));

                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _transactionImportErrorList.Add(
                            new SCC_BL.Helpers.Transaction.Import.Error(
                                transactionImportErrorElementName,
                                SCC_BL.Results.Transaction.ImportData.ErrorList.BusinessIntelligenceField.UNKNOWN
                                    .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.ToString()),
                                currentRowCount,
                                currentCellIndex));

                        //Se podría crear si se necesitase

                        _setNextBusinessIntelligenceFieldIndex();
                        continue;
                    }
                }*/

                _setNextBusinessIntelligenceFieldIndex();
            }

            return importTransactionBusinessIntelligenceFieldHelperList;
        }

        void SendMail(SCC_BL.Settings.AppValues.MailTopic mailTopic, int[] userIDList, string message = null, int? transactionID = null)
        {
            SCC_BL.Tools.Mail mailHelper = new SCC_BL.Tools.Mail();

            string url = string.Empty;
            string messageBody = string.Empty;

            switch (mailTopic)
            {
                case SCC_BL.Settings.AppValues.MailTopic.CALIBRATION_CREATED:
                    url = Url.Action(nameof(CalibrationController.Manage), GetControllerName(typeof(CalibrationController)), null, Request.Url.Scheme);

                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.Calibration.CalibrationCreated.PATH);
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
                                SCC_BL.Settings.HTML_Content.Calibration.CalibrationCreated.SUBJECT);

                            SaveProcessingInformation<SCC_BL.Results.Calibration.Insert.SendMail.Success>(auxUser.ID, auxUser.BasicInfo.StatusID, auxUser);
                        }
                        catch (Exception ex)
                        {
                            SaveProcessingInformation<SCC_BL.Results.Calibration.Insert.SendMail.Error>(auxUser.ID, auxUser.BasicInfo.StatusID, ex);
                            throw ex;
                        }
                    }
                    break;
                case SCC_BL.Settings.AppValues.MailTopic.DISPUTE:
                    url = Url.Action(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transactionID.Value, hasDisputation = "true" }, Request.Url.Scheme);

                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.Transaction.Dispute.PATH);
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
                                SCC_BL.Settings.HTML_Content.Transaction.Dispute.SUBJECT);

                            SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDisputeCommentList.SendMail.Success>(auxUser.ID, auxUser.BasicInfo.StatusID, auxUser);
                        }
                        catch (Exception ex)
                        {
                            SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDisputeCommentList.SendMail.Error>(auxUser.ID, auxUser.BasicInfo.StatusID, ex);
                            throw ex;
                        }
                    }
                    break;
                case SCC_BL.Settings.AppValues.MailTopic.INVALIDATION:
                    url = Url.Action(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transactionID.Value, hasInvalidation = "true" }, Request.Url.Scheme);

                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.Transaction.Invalidation.PATH);
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
                                SCC_BL.Settings.HTML_Content.Transaction.Invalidation.SUBJECT);

                            SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateInvalidationCommentList.SendMail.Success>(auxUser.ID, auxUser.BasicInfo.StatusID, auxUser);
                        }
                        catch (Exception ex)
                        {
                            SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateInvalidationCommentList.SendMail.Error>(auxUser.ID, auxUser.BasicInfo.StatusID, ex);
                            throw ex;
                        }
                    }
                    break;
                case SCC_BL.Settings.AppValues.MailTopic.DEVOLUTION:
                    url = Url.Action(nameof(TransactionController.Edit), GetControllerName(typeof(TransactionController)), new { transactionID = transactionID.Value, hasDevolution = "true" }, Request.Url.Scheme);

                    messageBody = LoadHTMLBody(SCC_BL.Settings.HTML_Content.Transaction.Devolution.PATH);
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
                                SCC_BL.Settings.HTML_Content.Transaction.Devolution.SUBJECT);

                            SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDevolutionCommentList.SendMail.Success>(auxUser.ID, auxUser.BasicInfo.StatusID, auxUser);
                        }
                        catch (Exception ex)
                        {
                            SaveProcessingInformation<SCC_BL.Results.Transaction.UpdateDevolutionCommentList.SendMail.Error>(auxUser.ID, auxUser.BasicInfo.StatusID, ex);
                            throw ex;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        [HttpPost]
        public ActionResult ExportTransactionListToExcel(string transactionIDArray)
        {
            string newFileName = $"Transacciones exportadas { DateTime.Now.ToString("yyyyMMddHHmmss") }.xlsx";

            string filePath =
                AppDomain.CurrentDomain.BaseDirectory +
                SCC_BL.Settings.Paths.Transaction.TRANSACTION_IMPORT_FOLDER.Substring(SCC_BL.Settings.Paths.Transaction.TRANSACTION_IMPORT_FOLDER.IndexOf('/') + 1) +
                newFileName;

            List<Transaction> transactionList = new List<Transaction>();

            for (int i = 0; i < transactionIDArray.Split(',').Length; i++)
            {
                using (Transaction transaction = new Transaction(Convert.ToInt32(transactionIDArray.Split(',')[i])))
                {
                    transaction.SetDataByID();
                    transactionList.Add(transaction);
                }
            }

            using (ExcelParser excelParser = new ExcelParser())
            {
                excelParser.ExportTransactionListToExcel(transactionList, filePath);
            }

            return DownLoadFileFromServer(filePath, SCC_BL.Settings.AppValues.File.ContentType.EXCEL_FILES_XLSX);
        }
    }
}