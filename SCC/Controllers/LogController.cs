using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class LogController : OverallController
    {
        public ActionResult Index(int? categoryID, int? itemID)
        {
            Log log = new Log();

            if (itemID != null && itemID > 0)
            {
                log = new Log(categoryID.Value, itemID.Value);
                //log.SelectByCategoryIDAndItemID();
            }

            List<Catalog> categoryList = new List<Catalog>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.CATEGORY))
                categoryList = categoryList.Concat(catalog.SelectByCategoryID()).ToList();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ELEMENT))
                categoryList = categoryList.Concat(catalog.SelectByCategoryID()).ToList();

            categoryList =
                categoryList
                    .OrderBy(o => o.Description)
                    .ToList();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Log.Index.CategoryCatalog.NAME] =
                new SelectList(
                    categoryList,
                    SCC_BL.Settings.AppValues.ViewData.Log.Index.CategoryCatalog.SelectList.VALUE,
                    SCC_BL.Settings.AppValues.ViewData.Log.Index.CategoryCatalog.SelectList.TEXT);

            return View(log);
        }

        public ActionResult _LogList(int categoryID, int itemID)
        {
            List<Log> logList = new List<Log>();

            logList = new Log(categoryID, itemID).SelectByCategoryIDAndItemID();

            return PartialView(logList);
        }

        public ActionResult LogFiles()
        {
            bool allowedUser =
                GetActualUser().HasRole(SCC_BL.DBValues.Catalog.USER_ROLE.SUPERUSER) ||
                GetActualUser().HasRole(SCC_BL.DBValues.Catalog.USER_ROLE.ADMINISTRATOR);


            if (!allowedUser)
                return RedirectToAction(nameof(HomeController.Index), GetControllerName(typeof(HomeController)));

            string folderPath =
                AppDomain.CurrentDomain.BaseDirectory + 
                System.Web.Configuration.WebConfigurationManager.AppSettings[SCC_BL.Settings.Overall.LOG_PATH];

            List<System.IO.FileInfo> fileInfo = GetFileInfo(folderPath);

            List<ViewModels.LogFileViewModel.LogFileInfo> fileInfoList =
                fileInfo
                    .Select(file =>
                        {
                            string username = file.Name.Split('_')[1];

                            return new SCC.ViewModels.LogFileViewModel.LogFileInfo(
                                file.Name,
                                username,
                                file.LastWriteTime);
                        })
                    .Where(e => 
                        e.CreationDate > DateTime.Now.AddMonths(-3))
                    .ToList();

            ViewModels.LogFileViewModel logFileViewModel = new ViewModels.LogFileViewModel();
            logFileViewModel.LogFileInfoList = fileInfoList;

            return View(logFileViewModel);
        }
                
        public ActionResult DownloadLogFile(string fileName)
        {

            string folderPath =
                AppDomain.CurrentDomain.BaseDirectory +
                System.Web.Configuration.WebConfigurationManager.AppSettings[SCC_BL.Settings.Overall.LOG_PATH] +
                fileName;

            return DownLoadFileFromServer(folderPath, SCC_BL.Settings.AppValues.File.ContentType.TEXT_FILES);
        }
                
        public ActionResult ViewLogFile(string fileName)
        {

            string folderPath =
                AppDomain.CurrentDomain.BaseDirectory +
                System.Web.Configuration.WebConfigurationManager.AppSettings[SCC_BL.Settings.Overall.LOG_PATH] +
                fileName;

            return ViewFile(folderPath, SCC_BL.Settings.AppValues.File.ContentType.TEXT_FILES);
        }
    }
}