using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Reports.Helpers
{
    public class BusinessIntelligenceFieldByProgram
    {
        public int BusinessIntelligenceFieldID { get; set; }
        public int ProgramID { get; set; }
        public List<BusinessIntelligenceFieldByProgram> BusinessIntelligenceFieldByProgramList { get; set; } = new List<BusinessIntelligenceFieldByProgram>();

        public BusinessIntelligenceFieldByProgram()
        {
            this.SetData();
        }

        public BusinessIntelligenceFieldByProgram(int businessIntelligenceFieldID, int programID)
        {
            this.BusinessIntelligenceFieldID = businessIntelligenceFieldID;
            this.ProgramID = programID;
        }

        public void SetData()
        {
            List<ProgramFormCatalog> allProgramFormCatalogList = new List<ProgramFormCatalog>();
            List<FormBIFieldCatalog> allFormBIFieldCatalogList = new List<FormBIFieldCatalog>();

            using (ProgramFormCatalog programFormCatalog = new ProgramFormCatalog())
                allProgramFormCatalogList = programFormCatalog.SelectAll();

            using (FormBIFieldCatalog formBIFieldCatalog = new FormBIFieldCatalog())
                allFormBIFieldCatalogList = formBIFieldCatalog.SelectAll();

            foreach (ProgramFormCatalog programFormCatalog in allProgramFormCatalogList)
            {
                this.BusinessIntelligenceFieldByProgramList.AddRange(
                    allFormBIFieldCatalogList
                        .Where(e =>
                            e.FormID == programFormCatalog.FormID)
                        .Select(e =>
                            new BusinessIntelligenceFieldByProgram(
                                e.BIFieldID,
                                programFormCatalog.ProgramID)));
            }

            this.BusinessIntelligenceFieldByProgramList =
                this.BusinessIntelligenceFieldByProgramList
                    .GroupBy(e =>
                        new
                        {
                            e.BusinessIntelligenceFieldID,
                            e.ProgramID,
                        })
                    .Select(e =>
                        e.First())
                    .ToList();
        }
    }
}
