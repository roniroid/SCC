using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class LogFileViewModel
    {
        public List<LogFileInfo> LogFileInfoList { get; set; }

        public class LogFileInfo
        {
            public string FileName { get; set; }
            public string Username { get; set; }
            public DateTime CreationDate { get; set; }

            public LogFileInfo(string fileName, string username, DateTime creationDate)
            {
                this.FileName = fileName;
                this.Username = username;
                this.CreationDate = creationDate;
            }
        }
    }
}