using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class BIFieldsUploadViewModel
    {
        public List<SCC_BL.UploadedFile> UploadedFileList { get; set; } = new List<SCC_BL.UploadedFile>();
    }
}