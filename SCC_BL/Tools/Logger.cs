using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Tools
{
    public class Logger
    {
        string _basePath;
        string _fileName;
        string _username;
        int _stackTraceLevel { get; set; } = 2;

        public void SetUsername(string username)
        {
            this._username = username;
        }

        private void CreateDirectory()
        {
            try
            {
                string[] folderArray = _basePath.Split('\\');
                string actualFolder;
                if (folderArray.Length > 1)
                {
                    actualFolder = folderArray[0] + "\\";
                    for (int i = 1; i < folderArray.Length; i++)
                    {
                        actualFolder = System.IO.Path.Combine(actualFolder, folderArray[i]);
                        if (!System.IO.Directory.Exists(actualFolder))
                        {
                            System.IO.Directory.CreateDirectory(actualFolder);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public Logger(string fileName, string basePath)
        {
            try
            {
                _basePath = basePath;
                if (String.IsNullOrEmpty(_basePath)) throw new Exception(Results.LocalLog.Save.Error.LOCAL_LOG);
                if (System.IO.Directory.Exists(_basePath)) throw new Exception(Results.LocalLog.Save.NonExistentLogPath.LOCAL_LOG);

                _fileName = fileName;
                _stackTraceLevel = 1;
                CreateDirectory();
            }
            catch (Exception ex)
            {
                _fileName = "Log";
            }
        }


        public void Write(string localLogLevel, string text)
        {
            string parentFunction = string.Empty;
            string parentFile = string.Empty;
            string parentLine = string.Empty;

            try
            {
                StackFrame stackFrame = (new StackTrace()).GetFrame(_stackTraceLevel);

                parentFunction = stackFrame.GetMethod().Name;
                parentFile = stackFrame.GetFileName();
                parentLine = stackFrame.GetFileLineNumber().ToString();
            }
            catch { }

            try
            {
                string actualUsername =
                    !string.IsNullOrEmpty(_username) ?
                        _username :
                        string.Empty;

                String fileName = _fileName + "_" + actualUsername + DateTime.Today.ToString(Settings.Overall.LOG_DATE_FORMAT) + ".txt";
                fileName = System.IO.Path.Combine(_basePath, fileName);

                String line =
                    SCC_BL.Settings.Notification.LOCAL_LOG_FORMAT
                        .Replace(SCC_BL.Settings.Notification.LOCAL_LOG_DATE_AND_TIME, DateTime.Now.ToString())
                        .Replace(SCC_BL.Settings.Notification.LOCAL_LOG_STACK_LEVEL, "")
                        .Replace(SCC_BL.Settings.Notification.LOCAL_LOG_LEVEL, localLogLevel)
                        .Replace(SCC_BL.Settings.Notification.LOCAL_LOG_METHOD_NAME, parentFile + " " + parentFunction + " " + parentLine)
                        .Replace(SCC_BL.Settings.Notification.LOCAL_LOG_USERNAME, actualUsername)
                        .Replace(SCC_BL.Settings.Notification.LOCAL_LOG_MESSAGE, text);

                int attemptCount = 0;

                while (attemptCount < Settings.Overall.WRITE_LOG_MAX_ATTEMPTS)
                {
                    try
                    {
                        System.IO.File.AppendAllText(fileName, line, System.Text.Encoding.UTF8);
                        attemptCount++;
                        break;
                    }
                    catch (Exception ex) { }
                }

                Debug.WriteLine(line);
            }
            catch (Exception E)
            {
                try
                {
                    EventLog eventLog = new EventLog("Application");
                    eventLog.Source = _fileName;
                    eventLog.WriteEntry(Results.LocalLog.Save.WriteLogError.LOCAL_LOG + E.Message, EventLogEntryType.Warning);
                }
                catch { }
            }
        }
    }
}
