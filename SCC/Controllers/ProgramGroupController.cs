using SCC.ViewModels;
using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class ProgramGroupController : OverallController
    {
        string _mainControllerName = GetControllerName(typeof(ProgramGroupController));

        public ActionResult Manage(int? programGroupID, bool filterActiveElements = false)
        {
            ProgramGroupManagementViewModel programGroupManagementViewModel = new ProgramGroupManagementViewModel();

            if (programGroupID != null)
            {
                programGroupManagementViewModel.ProgramGroup = new ProgramGroup(programGroupID.Value);
                programGroupManagementViewModel.ProgramGroup.SetDataByID();
            }

            List<Program> allProgramList = new List<Program>();
            List<Program> programList = new List<Program>();

            using (Program program = new Program())
            {
                allProgramList = program.SelectAll();

                programList =
                    allProgramList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
                        .OrderBy(o => o.Name)
                        .ToList();
            }

            ViewData[SCC_BL.Settings.AppValues.ViewData.ProgramGroup.Manage.AllProgramList.NAME] = allProgramList;

            ViewData[SCC_BL.Settings.AppValues.ViewData.ProgramGroup.Manage.ProgramList.NAME] =
                new MultiSelectList(
                    programList,
                    nameof(Program.ID),
                    nameof(Program.Name),
                    programGroupManagementViewModel.ProgramGroup.ProgramList.Select(s => s.ProgramID));

            programGroupManagementViewModel.ProgramGroupList = new ProgramGroup().SelectAll();

            if (filterActiveElements)
                programGroupManagementViewModel.ProgramGroupList =
                    programGroupManagementViewModel.ProgramGroupList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.DISABLED)
                        .ToList();

            return View(programGroupManagementViewModel);
        }

        void UpdateProgramList(ProgramGroup programGroup, int[] programList)
        {
            try
            {
                switch (programGroup.UpdateProgramList(programList != null ? programList : new int[0], GetActualUser().ID))
                {
                    case SCC_BL.Results.ProgramGroup.UpdateProgramList.CODE.SUCCESS:
                        SaveProcessingInformation<SCC_BL.Results.ProgramGroup.UpdateProgramList.Success>(programGroup.ID, programGroup.BasicInfo.StatusID, programGroup);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.ProgramGroup.UpdateProgramList.Error>(programGroup.ID, programGroup.BasicInfo.StatusID, programGroup, ex);
            }

        }

        [HttpPost]
        public ActionResult Edit(ProgramGroupManagementViewModel programGroupManagementViewModel, int[] programList)
        {
            ProgramGroup oldProgramGroup = new ProgramGroup(programGroupManagementViewModel.ProgramGroup.ID);
            oldProgramGroup.SetDataByID();

            ProgramGroup newProgramGroup = new ProgramGroup(programGroupManagementViewModel.ProgramGroup.ID, programGroupManagementViewModel.ProgramGroup.Identifier, programGroupManagementViewModel.ProgramGroup.Name, programGroupManagementViewModel.ProgramGroup.BasicInfoID, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.UPDATED);

            try
            {
                int result = newProgramGroup.Update();

                if (result > 0)
                {
                    UpdateProgramList(oldProgramGroup, programList != null ? programList : new int[0]);

                    SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Update.Success>(newProgramGroup.ID, newProgramGroup.BasicInfo.StatusID, oldProgramGroup);
                }
                else
                {
                    switch ((SCC_BL.Results.ProgramGroup.Update.CODE)result)
                    {
                        case SCC_BL.Results.ProgramGroup.Update.CODE.ALREADY_EXISTS_IDENTIFIER:
                            SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Update.AlreadyExistsIdentifier>(oldProgramGroup.ID, oldProgramGroup.BasicInfo.StatusID, newProgramGroup);
                            break;
                        case SCC_BL.Results.ProgramGroup.Update.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Update.AlreadyExistsName>(oldProgramGroup.ID, oldProgramGroup.BasicInfo.StatusID, newProgramGroup);
                            break;
                        case SCC_BL.Results.ProgramGroup.Update.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Update.Error>(oldProgramGroup.ID, oldProgramGroup.BasicInfo.StatusID, newProgramGroup);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Update.Error>(oldProgramGroup.ID, oldProgramGroup.BasicInfo.StatusID, oldProgramGroup, ex);
            }

            return RedirectToAction(nameof(ProgramGroupController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Create(ProgramGroupManagementViewModel programGroupManagementViewModel, int[] programList)
        {
            ProgramGroup newProgramGroup = new ProgramGroup(programGroupManagementViewModel.ProgramGroup.Identifier, programGroupManagementViewModel.ProgramGroup.Name, GetActualUser().ID, (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.CREATED);

            try
            {
                int result = newProgramGroup.Insert();

                if (result > 0)
                {
                    UpdateProgramList(newProgramGroup, programList != null ? programList : new int[0]);

                    SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Insert.Success>(newProgramGroup.ID, newProgramGroup.BasicInfo.StatusID, newProgramGroup);
                }
                else
                {
                    switch ((SCC_BL.Results.ProgramGroup.Insert.CODE)result)
                    {
                        case SCC_BL.Results.ProgramGroup.Insert.CODE.ALREADY_EXISTS_IDENTIFIER:
                            SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Insert.AlreadyExistsIdentifier>(null, null, newProgramGroup);
                            break;
                        case SCC_BL.Results.ProgramGroup.Insert.CODE.ALREADY_EXISTS_NAME:
                            SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Insert.AlreadyExistsName>(null, null, newProgramGroup);
                            break;
                        case SCC_BL.Results.ProgramGroup.Insert.CODE.ERROR:
                            SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Insert.Error>(null, null, newProgramGroup);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Insert.Error>(null, null, ex);
            }

            return RedirectToAction(nameof(ProgramGroupController.Manage), _mainControllerName);
        }

        [HttpPost]
        public ActionResult Delete(int programGroupID)
        {
            ProgramGroup programGroup = new ProgramGroup(programGroupID);
            programGroup.SetDataByID();

            try
            {
                //programGroup.Delete();

                programGroup.BasicInfo.ModificationUserID = GetActualUser().ID;
                programGroup.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP.DELETED;

                int result = programGroup.BasicInfo.Update();

                if (result > 0)
                {
                    SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Delete.Success>(programGroup.ID, programGroup.BasicInfo.StatusID, programGroup);

                    return RedirectToAction(nameof(ProgramGroupController.Manage), _mainControllerName);
                }
            }
            catch (Exception ex)
            {
                SaveProcessingInformation<SCC_BL.Results.ProgramGroup.Delete.Success>(programGroup.ID, programGroup.BasicInfo.StatusID, programGroup, ex);
            }

            return RedirectToAction(nameof(ProgramGroupController.Manage), _mainControllerName);
        }
    }
}