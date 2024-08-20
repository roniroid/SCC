using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class TransactionCustomFieldCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.TransactionCustomFieldCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int transactionID, int customFieldID, string comment, int? valueID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Insert.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Insert.Parameters.CUSTOMFIELDID, customFieldID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Insert.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Insert.Parameters.VALUEID, valueID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.TransactionCustomFieldCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
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
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.NAME,
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
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int transactionID, int customFieldID, string comment, int? valueID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Update.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Update.Parameters.CUSTOMFIELDID, customFieldID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Update.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.TransactionCustomFieldCatalog.StoredProcedures.Update.Parameters.VALUEID, valueID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.TransactionCustomFieldCatalog.StoredProcedures.Update.NAME,
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