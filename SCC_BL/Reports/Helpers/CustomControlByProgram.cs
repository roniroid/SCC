using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SCC_BL.Settings.AppValues.ViewData.Calibration.Edit;

namespace SCC_BL.Reports.Helpers
{
    public class CustomControlByProgram
    {
        public int CustomControlID { get; set; }
        public int ProgramID { get; set; }
        public List<CustomControlByProgram> CustomControlByProgramList { get; set; } = new List<CustomControlByProgram>();

        public CustomControlByProgram()
        {
            this.SetData();
        }

        public CustomControlByProgram(int customControlID, int programID)
        {
            this.CustomControlID = customControlID;
            this.ProgramID = programID;
        }

        public void SetData()
        {
            List<ProgramFormCatalog> allProgramFormCatalogList = new List<ProgramFormCatalog>();
            List<CustomField> allCustomFieldList = new List<CustomField>();

            using (ProgramFormCatalog programFormCatalog = new ProgramFormCatalog())
                allProgramFormCatalogList = programFormCatalog.SelectAll();

            using (CustomField customField = new CustomField())
                allCustomFieldList = customField.SelectAll();

            foreach (ProgramFormCatalog programFormCatalog in allProgramFormCatalogList)
            {
                this.CustomControlByProgramList.AddRange(
                    allCustomFieldList
                        .Where(e =>
                            e.FormID == programFormCatalog.FormID)
                        .Select(e =>
                            new CustomControlByProgram(
                                e.CustomControlID,
                                programFormCatalog.ProgramID)));
            }

            this.CustomControlByProgramList =
                this.CustomControlByProgramList
                    .GroupBy(e =>
                        new
                        {
                            e.CustomControlID,
                            e.ProgramID,
                        })
                    .Select(e =>
                        e.First())
                    .ToList();
        }
    }
}


/*OLD*/
/*
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static SCC_BL.Settings.AppValues.ViewData.Calibration.Edit;

namespace SCC_BL.Reports.Helpers
{
    public class CustomControlByProgram
    {
        public int CustomControlID { get; set; }
        public int ProgramID { get; set; }
        public List<CustomControlByProgram> CustomControlByProgramList { get; set; } = new List<CustomControlByProgram>();

        public CustomControlByProgram()
        {
            this.SetData();
        }

        public CustomControlByProgram(int customControlID, int programID) {
            this.CustomControlID = customControlID;
            this.ProgramID = programID;
        }

        public void SetData()
        {
            List<ProgramFormCatalog> allProgramFormCatalogList = new List<ProgramFormCatalog>();
            List<CustomField> allCustomFieldList = new List<CustomField>();

            using (ProgramFormCatalog programFormCatalog = new ProgramFormCatalog())
                allProgramFormCatalogList = programFormCatalog.SelectAll();

            using (CustomField customField = new CustomField())
                allCustomFieldList = customField.SelectAll();

            foreach (ProgramFormCatalog programFormCatalog in allProgramFormCatalogList)
            {
                this.CustomControlByProgramList.AddRange(
                    allCustomFieldList
                        .Where(e => 
                            e.FormID == programFormCatalog.FormID)
                        .Select(e => 
                            new CustomControlByProgram(
                                e.CustomControlID, 
                                programFormCatalog.ProgramID)));
            }

            this.CustomControlByProgramList =
                this.CustomControlByProgramList
                    .GroupBy(e =>
                        new
                        {
                            e.CustomControlID,
                            e.ProgramID,
                        })
                    .Select(e =>
                        e.First())
                    .ToList();
        }
    }
}
*/