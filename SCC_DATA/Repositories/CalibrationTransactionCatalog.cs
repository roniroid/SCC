using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class CalibrationTransactionCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CalibrationTransactionCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int calibrationID, int transactionID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.Insert.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.Insert.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.CalibrationTransactionCatalog.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.SelectByCalibrationID.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.CalibrationTransactionCatalog.StoredProcedures.SelectByCalibrationID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int calibrationID, int transactionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.Update.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CalibrationTransactionCatalog.StoredProcedures.Update.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CalibrationTransactionCatalog.StoredProcedures.Update.NAME,
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