using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class ProgramController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(ProgramController));

        public ActionResult Manage(int? programID, bool filterActiveElements = false)
        {
            ProgramManagementViewModel programManagementViewModel = new ProgramManagementViewModel();

            List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

            using (ProgramFormCatalog programFormCatalog = new ProgramFormCatalog())
                programFormCatalogList = programFormCatalog.SelectAll();

            ViewData[SCC_BL.Settings.AppValues.ViewData.Program.Manage.ProgramFormList.NAME] = programFormCatalogList;

            if (programID != null)
            {
                programManagementViewModel.Program = new Program(programID.Value);
                programManagementViewModel.Program.SetDataByID();
            }

            programManagementViewModel.ProgramList = new Program().SelectAll();

            if (filterActiveElements)
                programManagementViewModel.ProgramList =
                    programManagementViewModel.ProgramList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED &&
                            (DateTime.Now <= e.EndDate ||
                            e.EndDate == null))
                        .ToList();

            return View(programManagementViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProgramManagementViewModel programManagementViewModel)
        {
            Program oldProgram = new Program(programManagementViewModel.Program.ID);
            oldProgram.SetDataByID();

            Program newProgram = new Program(
                programManagementViewModel.Program.ID, 
                programManagementViewModel.Program.Name, 
                programManagementViewModel.Program.StartDate, 
                programManagementViewModel.Program.EndDate, 
                programManagementViewModel.Program.BasicInfoID, 
                GetActualUser().ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.UPDATED);

            try
            {
                int result = newProgram.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Program.Update.Success>(newProgram.ID, newProgram.BasicInfo.StatusID, oldProgram);
                }
                else
                {
                    switch ((SCC_BL.Results.Program.Update.CODE)result)
                    {
                        case SCC_BL.Results.Program.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Program.Update.Error>(oldProgram.ID, oldProgram.BasicInfo.StatusID, newProgram);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Program.Update.Error>(oldProgram.ID, oldProgram.BasicInfo.StatusID, oldProgram, ex);
            }

            return RedirectToAction(nameof(ProgramController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(ProgramManagementViewModel programManagementViewModel)
        {
            Program newProgram = new Program(
                programManagementViewModel.Program.Name, 
                programManagementViewModel.Program.StartDate, 
                programManagementViewModel.Program.EndDate, 
                GetActualUser().ID, 
                (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.CREATED);

            try
            {
                int result = newProgram.Insert();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Program.Insert.Success>(newProgram.ID, newProgram.BasicInfo.StatusID, newProgram);
                }
                else
                {
                    switch ((SCC_BL.Results.Program.Update.CODE)result)
                    {
                        case SCC_BL.Results.Program.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.Program.Update.Error>(null, null, newProgram);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Program.Update.Error>(null, null, ex);
            }

            return RedirectToAction(nameof(ProgramController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int programID)
        {
            Program program = new Program(programID);
            program.SetDataByID();

            try
            {
                //program.Delete();

                program.BasicInfo.ModificationUserID = GetActualUser().ID;
                program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED;

                int result = program.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Program.Delete.Success>(program.ID, program.BasicInfo.StatusID, program);

                    return RedirectToAction(nameof(ProgramController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Program.Delete.Success>(program.ID, program.BasicInfo.StatusID, program, ex);
            }

            return RedirectToAction(nameof(ProgramController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Activate(int programID)
        {
            Program program = new Program(programID);
            program.SetDataByID();

            try
            {
                //program.Delete();

                program.BasicInfo.ModificationUserID = GetActualUser().ID;

                switch ((SCC_BL.DBValues.Catalog.STATUS_PROGRAM)program.BasicInfo.StatusID)
                {
                    case SCC_BL.DBValues.Catalog.STATUS_PROGRAM.CREATED:
                        program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED;
                        break;
                    case SCC_BL.DBValues.Catalog.STATUS_PROGRAM.UPDATED:
                        program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED;
                        break;
                    case SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED:
                        program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.ENABLED;
                        program.EndDate = null;
                        break;
                    case SCC_BL.DBValues.Catalog.STATUS_PROGRAM.ENABLED:
                        program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED;
                        break;
                    case SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED:
                        program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.ENABLED;
                        program.EndDate = null;
                        break;
                    default:
                        break;
                }

                int result = program.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.Program.Reactivate.Success>(program.ID, program.BasicInfo.StatusID, program);

                    return RedirectToAction(nameof(ProgramController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.Program.Reactivate.Success>(program.ID, program.BasicInfo.StatusID, program, ex);
            }

            return RedirectToAction(nameof(ProgramController.Manage), _mainControllerName);
        }
    }
}