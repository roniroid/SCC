using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Reports.Helpers
{
    public class CustomControlByProgram
    {
        public int CustomControlID { get; set; }
        public int ProgramID { get; set; }
        public List<CustomControlByProgram> CustomControlByProgramList { get; set; }

        public CustomControlByProgram() { 
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
                    );
            }
        }
    }
}
