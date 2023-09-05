using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportAccuracyByAttributeViewModel
    {
        public DateTime? TransactionStartDate { get; set; }
        public DateTime? TransactionEndDate { get; set; }
        public DateTime? EvaluationStartDate { get; set; }
        public DateTime? EvaluationEndDate { get; set; }
        public int[] ProgramIDArray { get; set; }
        public int[] UserIDArray { get; set; }
        public int[] SupervisorUserIDArray { get; set; }
        public int[] EvaluatorUserIDArray { get; set; }
        public int[] ErrorTypeIDArray { get; set; }
        public bool? AttributeControllable { get; set; }
        public bool? AttributeKnown { get; set; }
        public bool? AttributeNoConstraint { get; set; }
        public int[] AttributeIDArray { get; set; }
        public List<CustomControl> CustomControlList { get; set; } = null;
        public string TransactionCustomFieldCatalog { get; set; } = null;
        //public ReportResultsOverallAccuracyViewModel ReportResultsOverallAccuracyViewModel { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------------
        public string[] ProgramNamesArray { get; set; }
        public string[] UserNamesArray { get; set; }
        public string[] SupervisorNamesArray { get; set; }
        public string[] EvaluatorUserNamesArray { get; set; }
        public string[] ErrorTypeNamesArray { get; set; }
        public string[] AttributeNamesArray { get; set; }
        public Dictionary<string, string> TransactionCustomFieldCatalogNamesAndValues { get; set; } = null;

        public class CustomControlHelper
        {
            public int CustomControlID { get; set; }
            public int[] CustomControlValueIDArray { get; set; }
            public string CustomControlTextValue { get; set; }
        }

        public void SetDescriptiveData()
        {
            int currentArrayLength = 0;

            currentArrayLength = this.ProgramIDArray != null ? this.ProgramIDArray.Length : 0;
            this.ProgramNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (Program program = new Program(this.ProgramIDArray[i]))
                {
                    program.SetDataByID();
                    this.ProgramNamesArray[i] = program.Name;
                }
            }

            currentArrayLength = this.UserIDArray != null ? this.UserIDArray.Length : 0;
            this.UserNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (User user = new User(this.UserIDArray[i]))
                {
                    user.SetDataByID();
                    this.UserNamesArray[i] = $"{user.Person.Identification} - {user.Person.SurName} {user.Person.FirstName}";
                }
            }

            currentArrayLength = this.SupervisorUserIDArray != null ? this.SupervisorUserIDArray.Length : 0;
            this.SupervisorNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (User user = new User(this.SupervisorUserIDArray[i]))
                {
                    user.SetDataByID();
                    this.SupervisorNamesArray[i] = $"{user.Person.Identification} - {user.Person.SurName} {user.Person.FirstName}";
                }
            }

            currentArrayLength = this.EvaluatorUserIDArray != null ? this.EvaluatorUserIDArray.Length : 0;
            this.EvaluatorUserNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (User user = new User(this.EvaluatorUserIDArray[i]))
                {
                    user.SetDataByID();
                    this.EvaluatorUserNamesArray[i] = $"{user.Person.Identification} - {user.Person.SurName} {user.Person.FirstName}";
                }
            }

            currentArrayLength = this.ErrorTypeIDArray != null ? this.ErrorTypeIDArray.Length : 0;
            this.ErrorTypeNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (Catalog catalog = new Catalog(this.ErrorTypeIDArray[i]))
                {
                    catalog.SetDataByID();
                    this.ErrorTypeNamesArray[i] = catalog.Description;
                }
            }

            currentArrayLength = this.AttributeIDArray != null ? this.AttributeIDArray.Length : 0;
            this.AttributeNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (SCC_BL.Attribute attribute = new SCC_BL.Attribute(this.AttributeIDArray[i]))
                {
                    attribute.SetDataByID();
                    this.AttributeNamesArray[i] = attribute.Name;
                }
            }

            SetTransactionCustomFieldCatalogList();
        }

        void SetTransactionCustomFieldCatalogList()
        {
            this.TransactionCustomFieldCatalogNamesAndValues = new Dictionary<string, string>();

            try
            {
                List<TransactionCustomFieldCatalog> currentTransactionCustomFieldCatalogList = Controllers.OverallController.Deserialize<List<TransactionCustomFieldCatalog>>(this.TransactionCustomFieldCatalog);

                foreach (TransactionCustomFieldCatalog currentTransactionCustomFieldCatalog in currentTransactionCustomFieldCatalogList)
                {
                    if (currentTransactionCustomFieldCatalog.ValueID == null && string.IsNullOrEmpty(currentTransactionCustomFieldCatalog.Comment))
                        continue;

                    using (CustomControl currentCustomControl = new CustomControl(currentTransactionCustomFieldCatalog.CustomFieldID))
                    {
                        currentCustomControl.SetDataByID();

                        if (currentTransactionCustomFieldCatalog.ValueID != null)
                        {
                            using (CustomControlValueCatalog currentCustomControlValueCatalog = new CustomControlValueCatalog(currentTransactionCustomFieldCatalog.ValueID.Value))
                            {
                                currentCustomControlValueCatalog.SetDataByID();

                                string newKey = $"{currentCustomControl.ID} - {currentCustomControl.Label}";

                                if (this.TransactionCustomFieldCatalogNamesAndValues.Keys.Contains(newKey))
                                {
                                    this.TransactionCustomFieldCatalogNamesAndValues[newKey] = this.TransactionCustomFieldCatalogNamesAndValues[newKey] + $", {currentCustomControlValueCatalog.Value}";
                                }
                                else
                                {
                                    this.TransactionCustomFieldCatalogNamesAndValues.Add(newKey, currentCustomControlValueCatalog.Value);
                                }
                            }
                        }
                        else
                        {
                            this.TransactionCustomFieldCatalogNamesAndValues.Add($"{currentCustomControl.ID} - {currentCustomControl.Label}", currentTransactionCustomFieldCatalog.Comment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}