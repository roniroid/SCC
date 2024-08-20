using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class TransactionAttributeCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.TransactionAttributeCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int transactionID, int attributeID, string comment, int? valueID, int scoreValue, bool @checked, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.ATTRIBUTEID, attributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.VALUEID, valueID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.SCORE_VALUE, scoreValue, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.CHECKED, @checked, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.TransactionAttributeCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByTransactionID(int transactionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByTransactionIDAndAttributeID(int transactionID, int attributeID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.Parameters.ATTRIBUTE_ID, attributeID, System.Data.SqlDbType.Int),
					};

					return
						db.Select(
							Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectAttributeIDListByTransactionID(int transactionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.SelectAttributeIDListByTransactionID.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionAttributeCatalog.StoredProcedures.SelectAttributeIDListByTransactionID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int transactionID, int attributeID, string comment, int? valueID, int scoreValue, bool @checked)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.ATTRIBUTEID, attributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.VALUEID, valueID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.SCORE_VALUE, scoreValue, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionAttributeCatalog.StoredProcedures.Update.Parameters.CHECKED, @checked, System.Data.SqlDbType.Bit)
					};

					return
						db.Execute(
							Queries.TransactionAttributeCatalog.StoredProcedures.Update.NAME,
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