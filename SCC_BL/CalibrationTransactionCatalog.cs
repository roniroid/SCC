using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class CalibrationTransactionCatalog : IDisposable
	{
		public int ID { get; set; }
		public int CalibrationID { get; set; }
		public int TransactionID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public CalibrationTransactionCatalog()
		{
		}

		//For DeleteByID
		public CalibrationTransactionCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static CalibrationTransactionCatalog CalibrationTransactionCatalogForInsert(int calibrationID, int transactionID, int creationUserID, int statusID)
		{
			CalibrationTransactionCatalog @object = new CalibrationTransactionCatalog();

			@object.CalibrationID = calibrationID;
			@object.TransactionID = transactionID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		public static CalibrationTransactionCatalog CalibrationTransactionCatalogWithCalibrationID(int calibrationID)
		{
			CalibrationTransactionCatalog @object = new CalibrationTransactionCatalog();
			@object.CalibrationID = calibrationID;
			return @object;
		}

		//For Update
		public CalibrationTransactionCatalog(int id, int calibrationID, int transactionID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.CalibrationID = calibrationID;
			this.TransactionID = transactionID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByCalibrationID (RESULT)
		public CalibrationTransactionCatalog(int id, int calibrationID, int transactionID, int basicInfoID)
		{
			this.ID = id;
			this.CalibrationID = calibrationID;
			this.TransactionID = transactionID;
			this.BasicInfoID = basicInfoID;
		}

		public List<CalibrationTransactionCatalog> SelectByCalibrationID()
		{
			List<CalibrationTransactionCatalog> calibrationTransactionCatalogList = new List<CalibrationTransactionCatalog>();

			using (SCC_DATA.Repositories.CalibrationTransactionCatalog repoCalibrationTransactionCatalog = new SCC_DATA.Repositories.CalibrationTransactionCatalog())
			{
				DataTable dt = repoCalibrationTransactionCatalog.SelectByCalibrationID(this.CalibrationID);

				foreach (DataRow dr in dt.Rows)
				{
					CalibrationTransactionCatalog calibrationTransactionCatalog = new CalibrationTransactionCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationTransactionCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationTransactionCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.CALIBRATIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationTransactionCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.TRANSACTIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CalibrationTransactionCatalog.StoredProcedures.SelectByCalibrationID.ResultFields.BASICINFOID])
					);

					calibrationTransactionCatalog.BasicInfo = new BasicInfo(calibrationTransactionCatalog.BasicInfoID);
					calibrationTransactionCatalog.BasicInfo.SetDataByID();

					calibrationTransactionCatalogList.Add(calibrationTransactionCatalog);
				}
			}

			return calibrationTransactionCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.CalibrationTransactionCatalog repoCalibrationTransactionCatalog = new SCC_DATA.Repositories.CalibrationTransactionCatalog())
			{
				int response = repoCalibrationTransactionCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.CalibrationTransactionCatalog repoCalibrationTransactionCatalog = new SCC_DATA.Repositories.CalibrationTransactionCatalog())
			{
				this.ID = repoCalibrationTransactionCatalog.Insert(this.CalibrationID, this.TransactionID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.CalibrationTransactionCatalog repoCalibrationTransactionCatalog = new SCC_DATA.Repositories.CalibrationTransactionCatalog())
			{
				return repoCalibrationTransactionCatalog.Update(this.ID, this.CalibrationID, this.TransactionID);
			}
		}

		public void Dispose()
		{
		}
	}
}