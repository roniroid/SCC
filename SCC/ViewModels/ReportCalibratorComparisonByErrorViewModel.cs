using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCC.ViewModels
{
    public class ReportCalibratorComparisonByErrorViewModel
    {
        public DateTime? CalibrationStartDate { get; set; }
        public DateTime? CalibrationEndDate { get; set; }
        public int[] ProgramIDArray { get; set; }
        public int[] CalibratedUserIDArray { get; set; }
        public int[] CalibratedSupervisorUserIDArray { get; set; }
        public int[] CalibratorUserIDArray { get; set; }
        public int[] CalibrationTypeIDArray { get; set; }
        public int[] ErrorTypeIDArray { get; set; }
        //public ReportResultsCalibratorComparisonViewModel ReportResultsCalibratorComparisonViewModel { get; set; } = null;

        //--------------------------------------------------------------------------------------------------------------
        public string[] ProgramNamesArray { get; set; }
        public string[] CalibratedUserNamesArray { get; set; }
        public string[] CalibratedSupervisorNamesArray { get; set; }
        public string[] CalibratorUserNamesArray { get; set; }
        public string[] CalibrationTypeNamesArray { get; set; }
        public string[] ErrorTypeNamesArray { get; set; }

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

            currentArrayLength = this.CalibratedUserIDArray != null ? this.CalibratedUserIDArray.Length : 0;
            this.CalibratedUserNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (User user = new User(this.CalibratedUserIDArray[i]))
                {
                    user.SetDataByID();
                    this.CalibratedUserNamesArray[i] = $"{user.Person.Identification} - {user.Person.SurName} {user.Person.FirstName}";
                }
            }

            currentArrayLength = this.CalibratedSupervisorUserIDArray != null ? this.CalibratedSupervisorUserIDArray.Length : 0;
            this.CalibratedSupervisorNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (User user = new User(this.CalibratedSupervisorUserIDArray[i]))
                {
                    user.SetDataByID();
                    this.CalibratedSupervisorNamesArray[i] = $"{user.Person.Identification} - {user.Person.SurName} {user.Person.FirstName}";
                }
            }

            currentArrayLength = this.CalibratorUserIDArray != null ? this.CalibratorUserIDArray.Length : 0;
            this.CalibratorUserNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (User user = new User(this.CalibratorUserIDArray[i]))
                {
                    user.SetDataByID();
                    this.CalibratorUserNamesArray[i] = $"{user.Person.Identification} - {user.Person.SurName} {user.Person.FirstName}";
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

            currentArrayLength = this.CalibrationTypeIDArray != null ? this.CalibrationTypeIDArray.Length : 0;
            this.CalibrationTypeNamesArray = new string[currentArrayLength];

            for (int i = 0; i < currentArrayLength; i++)
            {
                using (Catalog catalog = new Catalog(this.CalibrationTypeIDArray[i]))
                {
                    catalog.SetDataByID();
                    this.CalibrationTypeNamesArray[i] = catalog.Description;
                }
            }
        }
    }
}