using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class CalibrationUserCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CalibrationUserCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int calibrationID, int userID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.Insert.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.Insert.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.CalibrationUserCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByCalibrationID(int calibrationID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.SelectByCalibrationID.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.CalibrationUserCatalog.StoredProcedures.SelectByCalibrationID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int calibrationID, int userID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.Update.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationUserCatalog.StoredProcedures.Update.Parameters.USERID, userID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CalibrationUserCatalog.StoredProcedures.Update.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Dispose()
		{
		}
	}
}