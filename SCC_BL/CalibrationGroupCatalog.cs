using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class CalibrationGroupCatalog : IDisposable
	{
		public int ID { get; set; }
		public int CalibrationID { get; set; }
		public int GroupID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public CalibrationGroupCatalog()
		{
		}

		//For DeleteByID
		public CalibrationGroupCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static CalibrationGroupCatalog CalibrationGroupCatalogForInsert(int calibrationID, int groupID, int creationUserID, int statusID)
		{
			CalibrationGroupCatalog @object = new CalibrationGroupCatalog();

			@object.CalibrationID = calibrationID;
			@object.GroupID = groupID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		public static CalibrationGroupCatalog CalibrationGroupCatalogWithCalibrationID(int calibrationID)
		{
			CalibrationGroupCatalog @object = new CalibrationGroupCatalog();
			@object.CalibrationID = calibrationID;
			return @object;
		}

		//For Update
		public CalibrationGroupCatalog(int id, int calibrationID, int groupID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.CalibrationID = calibrationID;
			this.GroupID = groupID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByCalibrationID (RESULT)
		public CalibrationGroupCatalog(int id, int calibrationID, int groupID, int basicInfoID)
		{
			this.ID = id;
			this.CalibrationID = calibrationID;
			this.GroupID = groupID;
			this.BasicInfoID = basicInfoID;
		}

		public List<CalibrationGroupCatalog> SelectByCalibrationID()
		{
			List<CalibrationGroupCatalog> calibrationGroupCatalogList = new List<CalibrationGroupCatalog>();

			using (SCC_DATA.Repositories.CalibrationGroupCatalog repoCalibrationGroupCatalog = new SCC_DATA.Repositories.CalibrationGroupCatalog())
			{
				DataTable dt = repoCalibrationGroupCatalog.SelectByCalibrationID(this.CalibrationID);

				foreach (DataRow dr in dt.Rows)
				{
					CalibrationGroupCatalog calibrationGroupCatalog = new CalibrationGroupCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationGroupCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationGroupCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.CALIBRATIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationGroupCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.GROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationGroupCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.BASICINFOID])
					);

					calibrationGroupCatalog.BasicInfo = new BasicInfo(calibrationGroupCatalog.BasicInfoID);
					calibrationGroupCatalog.BasicInfo.SetDataByID();

					calibrationGroupCatalogList.Add(calibrationGroupCatalog);
				}
			}

			return calibrationGroupCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.CalibrationGroupCatalog repoCalibrationGroupCatalog = new SCC_DATA.Repositories.CalibrationGroupCatalog())
			{
				int response = repoCalibrationGroupCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.CalibrationGroupCatalog repoCalibrationGroupCatalog = new SCC_DATA.Repositories.CalibrationGroupCatalog())
			{
				this.ID = repoCalibrationGroupCatalog.Insert(this.CalibrationID, this.GroupID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.CalibrationGroupCatalog repoCalibrationGroupCatalog = new SCC_DATA.Repositories.CalibrationGroupCatalog())
			{
				return repoCalibrationGroupCatalog.Update(this.ID, this.CalibrationID, this.GroupID);
			}
		}

		public void Dispose()
		{
		}
	}
}