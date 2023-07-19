using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class CalibrationGroupCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CalibrationGroupCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int calibrationID, int groupID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.Insert.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.Insert.Parameters.GROUPID, groupID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.CalibrationGroupCatalog.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.SelectByCalibrationID.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.CalibrationGroupCatalog.StoredProcedures.SelectByCalibrationID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int calibrationID, int groupID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.Update.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationGroupCatalog.StoredProcedures.Update.Parameters.GROUPID, groupID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CalibrationGroupCatalog.StoredProcedures.Update.NAME,
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