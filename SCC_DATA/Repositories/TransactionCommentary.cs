using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class TransactionCommentary : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.TransactionCommentary.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int typeID, int transactionID, string comment, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Insert.Parameters.TYPEID, typeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Insert.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Insert.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.TransactionCommentary.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionCommentary.StoredProcedures.SelectByID.NAME,
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
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int typeID, int transactionID, string comment)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Update.Parameters.TYPEID, typeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Update.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionCommentary.StoredProcedures.Update.Parameters.COMMENT, comment, System.Data.SqlDbType.VarChar)
					};

					return
						db.Execute(
							Queries.TransactionCommentary.StoredProcedures.Update.NAME,
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