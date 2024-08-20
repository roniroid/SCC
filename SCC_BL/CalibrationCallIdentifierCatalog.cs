using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
    public class CalibrationCallIdentifierCatalog : IDisposable
    {
        public int ID { get; set; }
        public int CalibrationID { get; set; }
        public string CallIdentifier { get; set; }
        public int ProgramID { get; set; }
        public int BasicInfoID { get; set; }
        //----------------------------------------------------
        public BasicInfo BasicInfo { get; set; }

        public CalibrationCallIdentifierCatalog()
        {
        }

        //For DeleteByID
        public CalibrationCallIdentifierCatalog(int id)
        {
            this.ID = id;
        }

        //For Insert
        public static CalibrationCallIdentifierCatalog CalibrationCallIdentifierCatalogForInsert(int calibrationID, string callIdentifier, int programID, int creationUserID, int statusID)
        {
            CalibrationCallIdentifierCatalog @object = new CalibrationCallIdentifierCatalog();

            @object.CalibrationID = calibrationID;
            @object.CallIdentifier = callIdentifier;
            @object.ProgramID = programID;

            @object.BasicInfo = new BasicInfo(creationUserID, statusID);

            return @object;
        }

        public static CalibrationCallIdentifierCatalog CalibrationCallIdentifierCatalogWithCallIdentifier(string callIdentifier)
        {
            CalibrationCallIdentifierCatalog @object = new CalibrationCallIdentifierCatalog();
            @object.CallIdentifier = callIdentifier;
            return @object;
        }

        public static CalibrationCallIdentifierCatalog CalibrationCallIdentifierCatalogWithCalibrationID(int calibrationID)
        {
            CalibrationCallIdentifierCatalog @object = new CalibrationCallIdentifierCatalog();
            @object.CalibrationID = calibrationID;
            return @object;
        }

        //For Update
        public CalibrationCallIdentifierCatalog(int id, int calibrationID, string callIdentifier, int programID, int basicInfoID, int modificationUserID, int statusID)
        {
            this.ID = id;
            this.CalibrationID = calibrationID;
            this.CallIdentifier = callIdentifier;
            this.ProgramID = programID;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
        }

        //For SelectByCalibrationID (RESULT)
        public CalibrationCallIdentifierCatalog(int id, int calibrationID, string callIdentifier, int programID, int basicInfoID)
        {
            this.ID = id;
            this.CalibrationID = calibrationID;
            this.CallIdentifier = callIdentifier;
            this.ProgramID = programID;
            this.BasicInfoID = basicInfoID;
        }

        public List<CalibrationCallIdentifierCatalog> SelectByCalibrationID()
        {
            List<CalibrationCallIdentifierCatalog> calibrationCallIdentifierCatalogList = new List<CalibrationCallIdentifierCatalog>();

            using (SCC_DATA.Repositories.CalibrationCallIdentifierCatalog repoCalibrationCallIdentifierCatalog = new SCC_DATA.Repositories.CalibrationCallIdentifierCatalog())
            {
                DataTable dt = repoCalibrationCallIdentifierCatalog.SelectByCalibrationID(this.CalibrationID);

                foreach (DataRow dr in dt.Rows)
                {
                    CalibrationCallIdentifierCatalog calibrationCallIdentifierCatalog = new CalibrationCallIdentifierCatalog(
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.CALIBRATIONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.CALL_IDENTIFIER]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.PROGRAM_ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.BASICINFOID])
                    );

                    calibrationCallIdentifierCatalog.BasicInfo = new BasicInfo(calibrationCallIdentifierCatalog.BasicInfoID);
                    calibrationCallIdentifierCatalog.BasicInfo.SetDataByID();

                    calibrationCallIdentifierCatalogList.Add(calibrationCallIdentifierCatalog);
                }
            }

            return calibrationCallIdentifierCatalogList;
        }

        public List<CalibrationCallIdentifierCatalog> SelectByCallIdentifier()
        {
            List<CalibrationCallIdentifierCatalog> calibrationCallIdentifierCatalogList = new List<CalibrationCallIdentifierCatalog>();

            using (SCC_DATA.Repositories.CalibrationCallIdentifierCatalog repoCalibrationCallIdentifierCatalog = new SCC_DATA.Repositories.CalibrationCallIdentifierCatalog())
            {
                DataTable dt = repoCalibrationCallIdentifierCatalog.SelectByCallIdentifier(this.CallIdentifier);

                foreach (DataRow dr in dt.Rows)
                {
                    CalibrationCallIdentifierCatalog calibrationCallIdentifierCatalog = new CalibrationCallIdentifierCatalog(
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.ResultFields.CALIBRATIONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.ResultFields.CALL_IDENTIFIER]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.ResultFields.PROGRAM_ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.ResultFields.BASICINFOID])
                    );

                    calibrationCallIdentifierCatalog.BasicInfo = new BasicInfo(calibrationCallIdentifierCatalog.BasicInfoID);
                    calibrationCallIdentifierCatalog.BasicInfo.SetDataByID();

                    calibrationCallIdentifierCatalogList.Add(calibrationCallIdentifierCatalog);
                }
            }

            return calibrationCallIdentifierCatalogList;
        }

        public int DeleteByID()
        {
            using (SCC_DATA.Repositories.CalibrationCallIdentifierCatalog repoCalibrationCallIdentifierCatalog = new SCC_DATA.Repositories.CalibrationCallIdentifierCatalog())
            {
                int response = repoCalibrationCallIdentifierCatalog.DeleteByID(this.ID);

                this.BasicInfo.DeleteByID();
                return response;
            }
        }

        public int Insert()
        {
            this.BasicInfoID = this.BasicInfo.Insert();

            using (SCC_DATA.Repositories.CalibrationCallIdentifierCatalog repoCalibrationCallIdentifierCatalog = new SCC_DATA.Repositories.CalibrationCallIdentifierCatalog())
            {
                this.ID = repoCalibrationCallIdentifierCatalog.Insert(this.CalibrationID, this.CallIdentifier, this.ProgramID, this.BasicInfoID);

                return this.ID;
            }
        }

        public int Update()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.CalibrationCallIdentifierCatalog repoCalibrationCallIdentifierCatalog = new SCC_DATA.Repositories.CalibrationCallIdentifierCatalog())
            {
                return repoCalibrationCallIdentifierCatalog.Update(this.ID, this.CalibrationID, this.CallIdentifier, this.ProgramID);
            }
        }

        public void Dispose()
        {
        }
    }
}