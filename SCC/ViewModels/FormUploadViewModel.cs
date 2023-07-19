using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class FormUploadViewModel
    {
        public SCC_BL.Form Form { get; set; } = new SCC_BL.Form();
        public List<SCC_BL.UploadedFile> UploadedFileList { get; set; } = new List<SCC_BL.UploadedFile>();
    }
}