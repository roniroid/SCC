using SCC_BL;
using SCC_BL.Settings;
using SCC_BL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class OverallController : Controller
    {
        Logger _logger = new Logger(Overall.DEFAULT_LOG_NAME, AppDomain.CurrentDomain.BaseDirectory + System.Web.Configuration.WebConfigurationManager.AppSettings[Overall.LOG_PATH]);

        User _actualUser
        {
            get
            {
                return (User)Session[AppValues.Session.GLOBAL_ACTUAL_USER];
            }
            set
            {
                Session[AppValues.Session.GLOBAL_ACTUAL_USER] = value;
            }
        }

        public static Object GetScriptInformation<T>(String name)
        {
            dynamic obj = new Object();

            try
            {
                obj = (T)Activator.CreateInstance(typeof(T));
            }
            catch (Exception ex)
            {
            }

            foreach (String part in name.Split('.'))
            {
                Type type = obj.GetType();
                System.Reflection.FieldInfo info = type.GetField(part);
                if (info == null) return null;

                obj = info.GetValue(obj);
            }

            return obj;
        }

        public void SaveProcessingInformation<T>(Exception ex)
        {
            SaveProcessingInformation<T>(null, null, null, ex);
        }

        public void SaveProcessingInformation<T>(int? objectID = null, int? statusID = null, object @object = null, Exception ex = null)
        {
            Notification.Type type = Notification.Type.INFO;

            SCC_BL.DBValues.Catalog.ELEMENT elementCategory = SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_NO_ELEMENT;

            string databaseLogMessage = string.Empty;

            string localLogLevel = string.Empty;
            string localLogMessage = string.Empty;

            string tempMessageTitle = string.Empty;
            string tempMessageContent = string.Empty;

            string jsonInfoString = string.Empty;
            string actualUsername = string.Empty;
            string exceptionMessage = string.Empty;

            jsonInfoString = @object != null ? Serialize(@object) : "NULL";

            actualUsername = GetActualUser() != null ? GetActualUser().Username : "NULL";

            exceptionMessage = ex != null ? ex.ToString() : "NULL";

            type =
                GetScriptInformation<T>("TYPE") != null ?
                    (Notification.Type)GetScriptInformation<T>("TYPE") :
                    Notification.Type.INFO;

            elementCategory =
                GetScriptInformation<T>("METHOD_ELEMENT_CATEGORY") != null ?
                    (SCC_BL.DBValues.Catalog.ELEMENT)GetScriptInformation<T>("METHOD_ELEMENT_CATEGORY") :
                    SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_NO_ELEMENT;

            databaseLogMessage =
                GetScriptInformation<T>("DATABASE_LOG") != null ?
                    GetScriptInformation<T>("DATABASE_LOG").ToString() :
                    string.Empty;

            localLogMessage =
                GetScriptInformation<T>("LOCAL_LOG") != null ?
                    GetScriptInformation<T>("LOCAL_LOG").ToString() :
                    string.Empty;

            localLogLevel =
                GetScriptInformation<T>("LOCAL_LOG_LEVEL") != null ?
                    GetScriptInformation<T>("LOCAL_LOG_LEVEL").ToString() :
                    "NULL";

            tempMessageTitle =
                GetScriptInformation<T>("MESSAGE_TITLE") != null ?
                    GetScriptInformation<T>("MESSAGE_TITLE").ToString() :
                    string.Empty;

            tempMessageContent =
                GetScriptInformation<T>("MESSAGE_CONTENT") != null ?
                    GetScriptInformation<T>("MESSAGE_CONTENT").ToString() :
                    string.Empty;

            databaseLogMessage = databaseLogMessage
                .Replace(SCC_BL.Results.CommonElements.REPLACE_USERNAME, actualUsername)
                .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, jsonInfoString)
                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, exceptionMessage);

            localLogMessage = localLogMessage
                .Replace(SCC_BL.Results.CommonElements.REPLACE_USERNAME, actualUsername)
                .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, jsonInfoString)
                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, exceptionMessage);

            tempMessageContent = tempMessageContent
                .Replace(SCC_BL.Results.CommonElements.REPLACE_USERNAME, actualUsername)
                .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, jsonInfoString)
                .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, exceptionMessage);

            try
            {
                if (objectID != null)
                {
                    if (objectID > 0)
                        if (!string.IsNullOrEmpty(databaseLogMessage))
                            SaveDatabaseLog(
                                elementCategory,
                                objectID.Value,
                                statusID.Value,
                                localLogMessage);
                }
            }
            catch (Exception ex1)
            {
            }

            try
            {
                if (!string.IsNullOrEmpty(localLogMessage))
                    SaveApplicationLog(actualUsername, localLogLevel, localLogMessage);
            }
            catch (Exception ex1)
            {
            }

            try
            {
                if (!string.IsNullOrEmpty(tempMessageContent))
                    SetTempMessage(
                        type,
                        tempMessageContent,
                        tempMessageTitle);
            }
            catch (Exception ex1)
            {
            }
        }

        protected bool IsThereAnyMessage()
        {
            return !string.IsNullOrEmpty(Session[AppValues.Session.GLOBAL_TEMP_MESSAGE].ToString());
        }

        public void SaveApplicationLog(string username, string localLogLevel, string text)
        {
            _logger.SetUsername(username);

            try
            {
                _logger.Write(localLogLevel, text);
            }
            catch (Exception ex)
            {
            }
        }

        public void SaveDatabaseLog(
            SCC_BL.DBValues.Catalog.ELEMENT elementCategory,
            int itemID,
            int statusID,
            string description)
        {
            try
            {
                int? actualUserID = null;

                if (GetActualUser() != null) actualUserID = GetActualUser().ID;

                Log log = new Log(
                    (int)elementCategory,
                    itemID,
                    description,
                    statusID,
                    actualUserID,
                    (int)SCC_BL.DBValues.Catalog.STATUS_LOG.CREATED);

                try
                {
                    if (log.Insert() > 0)
                    {
                        /*SaveApplicationLog(
                            SCC_BL.Results.LocalLog.Save.Success.LOCAL_LOG
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_JSON_INFO, GetActualUser().Username)
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_CATEGORY_NAME, GetCategoryName(log.CategoryID))
                                .Replace(SCC_BL.Results.CommonElements.REPLACE_ITEM_ID, itemID.ToString()));*/
                    }
                }
                catch (Exception ex)
                {
                    /*SaveApplicationLog(
                        SCC_BL.Results.LocalLog.Save.Error.LOCAL_LOG
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_USERNAME, GetActualUser().Username)
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_CATEGORY_NAME, GetCategoryName(log.CategoryID))
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_ITEM_ID, itemID.ToString())
                            .Replace(SCC_BL.Results.CommonElements.REPLACE_INNER_EXCEPTION, ex.ToString()));*/
                }
            }
            catch (Exception ex)
            {
            }
        }

        string GetCategoryName(int id)
        {
            using (Catalog catalog = new Catalog(id))
            {
                catalog.SelectByCategoryID();

                return catalog.Description;
            }
        }

        public User GetActualUser()
        {
            return _actualUser;
        }

        public void SetActualUser(User user)
        {
            _actualUser = user;
        }

        //public T GetSessionValue<T>(string name)
        //{
        //    return (T)Session[name];
        //}

        //public void SetSessionValue(string name, object @object)
        //{
        //    Session[name] = @object;
        //}

        public JsonResult GetJsonFormatMessage(Notification notification)
        {
            return null;
            /*return new JsonResult()
            {
                Data = new
                {
                    type = Enum.GetName(typeof(Notification.Type), notification.MessageType),
                    title = notification.Title,
                    content = notification.Content,
                    //footerContent = notification.FooterContent,
                    //responseURL = notification.Response.ResponseURL,
                    //responseValue = notification.Response.Value
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };*/
        }

        public string LoadHTMLBody(string file_path)
        {
            string result = string.Empty;
            try
            {
                string full_path = AppDomain.CurrentDomain.BaseDirectory + file_path;
                if (System.IO.File.Exists(full_path))
                {
                    result = System.IO.File.ReadAllText(full_path, System.Text.Encoding.UTF8);
                }
            }
            catch (Exception ex) { }
            return result;
        }

        public void DeleteGeneralInformation(int generalInformationID)
        {
            using (BasicInfo basicInfo = new BasicInfo(generalInformationID))
                basicInfo.DeleteByID();
        }

        public string CheckEmptyValue(string value)
        {
            return !Overall.NEUTRAL_VALUES.Contains(value) ? value : null;
        }

        public List<Catalog> GetCatalogList()
        {
            return (List<Catalog>)Session[AppValues.Session.GLOBAL_CATALOG_LIST];
        }

        public void SaveCatalogList()
        {
            using (Catalog catalog = new Catalog())
            {
                Session[AppValues.Session.GLOBAL_CATALOG_LIST] = catalog.GetAllCatalogList();
            }
        }

        public void SaveTableCategoryList()
        {
            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.CATEGORY))
            {
                Session[AppValues.Session.GLOBAL_CATEGORY_LIST] = catalog.SelectByCategoryID();
            }
        }

        public List<Catalog> GetTableCategoryList()
        {
            return (List<Catalog>)Session[AppValues.Session.GLOBAL_CATEGORY_LIST];
        }

        public void SetTempMessage(Notification.Type type, string message, string title = "")
        {
            Session[AppValues.Session.GLOBAL_TEMP_MESSAGE] += SCC_BL.Settings.HTML_Content.Message.GetBody(type, message);
        }

        public static string GetControllerName(Type controllerType)
        {
            Type baseType = typeof(Controller);
            if (baseType.IsAssignableFrom(controllerType))
            {
                int lastControllerIndex = controllerType.Name.LastIndexOf("Controller");
                if (lastControllerIndex > 0)
                {
                    return controllerType.Name.Substring(0, lastControllerIndex);
                }
            }

            return controllerType.Name;
        }

        public static string Serialize(Object @object)
        {
            string serializedObject = "";

            try
            {
                serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(
                    @object,
                    Newtonsoft.Json.Formatting.None,
                    new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None,
                        MaxDepth = 1,
                        ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Reuse
                    }
                );
            }
            catch (Exception ex)
            {
                serializedObject =
                    SCC_BL.Results.LocalLog.Save.SerializationLogError.LOCAL_LOG
                        .Replace(SCC_BL.Results.CommonElements.REPLACE_LOCAL_LOG_MESSAGE, ex.Message)
                        .Replace(SCC_BL.Results.CommonElements.REPLACE_LOCAL_LOG_STACK_TRACE, ex.StackTrace)
                        .Replace(SCC_BL.Results.CommonElements.REPLACE_EXCEPTION_MESSAGE, ex.InnerException?.Message);
            }

            return serializedObject;
        }

        public static T Deserialize<T>(string @object)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(@object);
        }

        public FileResult DownLoadFile(byte[] data, string contentType, string fileName)
        {
            return File(data, contentType, fileName);
        }

        [HttpGet]
        public FileResult DownLoadFileFromDatabase(int uploadedFileID, string contentType)
        {
            using (UploadedFile uploadedFile = new UploadedFile(uploadedFileID))
            {
                uploadedFile.SetDataByID();

                return DownLoadFile(uploadedFile.Data, contentType, uploadedFile.FileName);
            }
        }

        [HttpGet]
        public FileResult DownLoadFileFromServer(string filePath, string contentType)
        {
            using (System.IO.Stream stream = System.IO.File.OpenRead(filePath))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                string name = new System.IO.FileInfo(filePath).Name;
                string extension = new System.IO.FileInfo(filePath).Extension;

                return DownLoadFile(buffer, contentType, name);
            }
        }

        public string SaveFileFromDataTable(System.Data.DataTable dt, string containerPath, string fileName, string extension)
        {
            string newFilePath = GetValidPath(containerPath, fileName, extension);

            using (ExcelParser excelParser = new ExcelParser())
                excelParser.ExportDataTableToExcelBytedFile(dt, newFilePath);

            return newFilePath;
        }

        public string SaveUploadedFile(HttpPostedFileBase file, string uploadsPath)
        {
            string finalPath = string.Empty;

            if (file == null)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>(new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.NO_FILE_FOUND));
            }
            else if (file.ContentLength > 0)
            {
                int maxContentLength = 1024 * 1024 * SCC_BL.Settings.Overall.MAX_FILE_SIZE_MB;
                string[] allowedFileExtensions = SCC_BL.Settings.Overall.ALLOWED_FILE_EXTENSIONS;

                string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.'));

                if (!allowedFileExtensions.Contains(fileExtension))
                {
                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>(new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.WRONG_FILE_EXTENSION));
                }

                else if (file.ContentLength > maxContentLength)
                {
                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>(new Exception(SCC_BL.Results.UploadedFile.UserMassiveImport.MAX_SIZE_EXCEEDED.Replace(SCC_BL.Results.UploadedFile.UserMassiveImport.REPLACE_MAX_SIZE, SCC_BL.Settings.Overall.MAX_FILE_SIZE_MB.ToString())));
                }
                else
                {
                    string fileName = System.IO.Path.GetFileName(file.FileName);

                    string path = System.IO.Path.Combine(Server.MapPath(uploadsPath), fileName);

                    path = GetValidPath(uploadsPath, file.FileName, fileExtension);

                    try
                    {
                        file.SaveAs(path);

                        finalPath = path;

                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Success>();
                    }
                    catch (Exception ex)
                    {
                        SaveProcessingInformation<SCC_BL.Results.UploadedFile.UserMassiveImport.Error>(ex);
                    }

                }
            }

            return finalPath;
        }

        string GetValidPath(string containerPath, string fileName, string fileExtension)
        {
            int count = 1;

            string path = System.IO.Path.Combine(Server.MapPath(containerPath), fileName);

            string[] existingFileNames = System.IO.Directory.GetFiles(Server.MapPath(containerPath));

            string tempFileName = fileName;

            while (existingFileNames.Contains(path))
            {
                string pattern = @"\(\d+\)";

                if (System.Text.RegularExpressions.Regex.IsMatch(tempFileName, pattern))
                {
                    int indexLastOpeningParenthesis = path.LastIndexOf('(');
                    int indexLastClosingParenthesis = path.LastIndexOf(')');

                    path =
                        path.Substring(0, indexLastOpeningParenthesis + 1) +
                        count.ToString() +
                        path.Substring(indexLastClosingParenthesis);
                }
                else
                {
                    path = (path.Replace(fileExtension, string.Empty) + " (%count%)" + fileExtension)
                        .Replace("%count%", count.ToString());

                    tempFileName = (tempFileName.Replace(fileExtension, string.Empty) + " (%count%)" + fileExtension)
                        .Replace("%count%", count.ToString());
                }

                count++;
            }

            return path;
        }

        public int SaveFileInDatabase(string filePath, int statusID)
        {
            int result = (int)SCC_BL.Results.UploadedFile.Insert.CODE.ERROR;

            try
            {
                using (System.IO.Stream stream = System.IO.File.OpenRead(filePath))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    string name = new System.IO.FileInfo(filePath).Name;
                    string extension = new System.IO.FileInfo(filePath).Extension;
                    //string fileName, string extension, byte[] data, int creationUserID, int statusID

                    using (UploadedFile uploadedFile = new UploadedFile(name, extension, buffer, GetActualUser().ID, statusID))
                    {
                        result = uploadedFile.Insert();
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.Insert.Error>(ex);
                throw ex;
            }

            return result;
        }

        public string GetFileFromDatabase(int uploadedFileID)
        {
            try
            {
                string filePath = "";

                using (UploadedFile uploadedFile = new UploadedFile(uploadedFileID))
                {
                    uploadedFile.SetDataByID();

                    string newFilePath = SCC_BL.Settings.Paths.User.DB_PROCESSED_FILES_FOLDER + uploadedFile.FileName + uploadedFile.Extension;

                    System.IO.File.WriteAllBytes(newFilePath, uploadedFile.Data);

                    filePath = newFilePath;

                    SaveProcessingInformation<SCC_BL.Results.UploadedFile.Obtaining.Success>(uploadedFile.ID, uploadedFile.BasicInfo.StatusID, uploadedFile);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.UploadedFile.Obtaining.Error>(ex);
                throw ex;
            }
        }

        public ActionResult ViewFile(string filePath, string contentType = SCC_BL.Settings.AppValues.File.ContentType.OCTET_STREAM)
        {
            return File(filePath, contentType);
        }

        public List<System.IO.FileInfo> GetFileInfo(string folderPath)
        {
            string[] filePaths = System.IO.Directory.GetFiles(folderPath);

            return filePaths
                .Select(path =>
                    new System.IO.FileInfo(path))
                .ToList();
        }

        protected SCC_BL.Results.NotificationMatrix.UserNotification.CODE SaveNotification(int userID, int typeID, string message, List<UserNotificationUrl> userNotificationUrlList = null)
        {
            SCC_BL.Results.NotificationMatrix.UserNotification.CODE result = SCC_BL.Results.NotificationMatrix.UserNotification.CODE.ERROR;

            try
            {
                using (UserNotification userNotification = new UserNotification(userID, message, typeID, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_NOTIFICATION.CREATED))
                {
                    int auxResult = userNotification.Insert();

                    if (auxResult > 0)
                    {
                        if (userNotificationUrlList != null)
                        {
                            foreach (UserNotificationUrl userNotificationUrl in userNotificationUrlList)
                            {
                                using (UserNotificationUrl newUserNotificationUrl = new UserNotificationUrl(userNotificationUrl.Content, userNotificationUrl.Description, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_NOTIFICATION_URL.CREATED))
                                {
                                    auxResult = newUserNotificationUrl.Insert();

                                    if (auxResult > 0)
                                    {
                                        using (UserNotificationUrlCatalog userNotificationUrlCatalog = UserNotificationUrlCatalog.UserNotificationUrlCatalogForInsert(userNotification.ID, newUserNotificationUrl.ID, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_NOTIFICATION_URL_CATALOG.CREATED))
                                        {
                                            auxResult = userNotificationUrlCatalog.Insert();
                                        }
                                    }
                                }
                            }
                        }

                        result = SCC_BL.Results.NotificationMatrix.UserNotification.CODE.SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.NotificationMatrix.UserNotification.Error>(ex);
            }

            return result;
        }

        protected int[] GetUsersToNotify(SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ACTION action, int tranasctionID)
        {

            List<int> usersToNotify = new List<int>();

            using (NotificationMatrix notificationMatrix = new NotificationMatrix())
            {
                List<NotificationMatrix> notificationMatrixList = notificationMatrix.GetAllNotificationMatrixList();

                notificationMatrixList =
                    notificationMatrixList
                        .Where(e =>
                            e.ActionID == (int)action)
                        .ToList();

                if (notificationMatrixList.Count == 0)
                    return usersToNotify.ToArray();

                Transaction transaction = new Transaction();

                transaction = new Transaction(tranasctionID);
                transaction.SetDataByID(true);

                User userToEvaluate = new User(transaction.UserToEvaluateID);
                userToEvaluate.SetDataByID();

                foreach (NotificationMatrix auxNotificationMatrix in notificationMatrixList)
                {
                    switch ((SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY)auxNotificationMatrix.EntityID)
                    {
                        case SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.DIRECT_SUPERVISOR:
                            usersToNotify.Add(userToEvaluate.SupervisorList.FirstOrDefault().SupervisorID);
                            break;
                        case SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.INDIRECT_SUPERVISOR:
                            usersToNotify.AddRange(userToEvaluate.SupervisorList.Select(e => e.SupervisorID));
                            break;
                        case SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORED_USER:
                            usersToNotify.Add(transaction.UserToEvaluateID);
                            break;
                        case SCC_BL.DBValues.Catalog.NOTIFICATION_MATRIX_ENTITY.MONITORING_USER:
                            usersToNotify.Add(transaction.EvaluatorUserID);
                            break;
                        default:
                            break;
                    }
                }
            }

            usersToNotify = usersToNotify.Distinct().ToList();

            return usersToNotify.ToArray();
        }

        public static string GenerateRandomString()
        {
            return SCC_BL.Tools.Utils.GenerateRandomString();
        }
    }
}