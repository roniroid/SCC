using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class CalibrationUserCatalog : IDisposable
	{
		public int ID { get; set; }
		public int CalibrationID { get; set; }
		public int UserID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public CalibrationUserCatalog()
		{
		}

		//For DeleteByID
		public CalibrationUserCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static CalibrationUserCatalog CalibrationUserCatalogForInsert(int calibrationID, int userID, int creationUserID, int statusID)
		{
			CalibrationUserCatalog @object = new CalibrationUserCatalog();

			@object.CalibrationID = calibrationID;
			@object.UserID = userID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByCalibrationID
		public static CalibrationUserCatalog CalibrationUserCatalogWithCalibrationID(int calibrationID)
		{
			CalibrationUserCatalog @object = new CalibrationUserCatalog();
			@object.CalibrationID = calibrationID;
			return @object;
		}

		//For Update
		public CalibrationUserCatalog(int id, int calibrationID, int userID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.CalibrationID = calibrationID;
			this.UserID = userID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByCalibrationID (RESULT)
		public CalibrationUserCatalog(int id, int calibrationID, int userID, int basicInfoID)
		{
			this.ID = id;
			this.CalibrationID = calibrationID;
			this.UserID = userID;
			this.BasicInfoID = basicInfoID;
		}

		public List<CalibrationUserCatalog> SelectByCalibrationID()
		{
			List<CalibrationUserCatalog> calibrationUserCatalogList = new List<CalibrationUserCatalog>();

			using (SCC_DATA.Repositories.CalibrationUserCatalog repoCalibrationUserCatalog = new SCC_DATA.Repositories.CalibrationUserCatalog())
			{
				DataTable dt = repoCalibrationUserCatalog.SelectByCalibrationID(this.CalibrationID);

				foreach (DataRow dr in dt.Rows)
				{
					CalibrationUserCatalog calibrationUserCatalog = new CalibrationUserCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationUserCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationUserCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.CALIBRATIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationUserCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationUserCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.BASICINFOID])
					);

					calibrationUserCatalog.BasicInfo = new BasicInfo(calibrationUserCatalog.BasicInfoID);
					calibrationUserCatalog.BasicInfo.SetDataByID();

					calibrationUserCatalogList.Add(calibrationUserCatalog);
				}
			}

			return calibrationUserCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.CalibrationUserCatalog repoCalibrationUserCatalog = new SCC_DATA.Repositories.CalibrationUserCatalog())
			{
				int response = repoCalibrationUserCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.CalibrationUserCatalog repoCalibrationUserCatalog = new SCC_DATA.Repositories.CalibrationUserCatalog())
			{
				this.ID = repoCalibrationUserCatalog.Insert(this.CalibrationID, this.UserID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.CalibrationUserCatalog repoCalibrationUserCatalog = new SCC_DATA.Repositories.CalibrationUserCatalog())
			{
				return repoCalibrationUserCatalog.Update(this.ID, this.CalibrationID, this.UserID);
			}
		}

		public void Dispose()
		{
		}
	}
}