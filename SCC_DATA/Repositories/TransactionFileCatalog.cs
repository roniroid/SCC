using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class TransactionFileCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.TransactionFileCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int transactionID, int uploadedFileID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.Insert.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.Insert.Parameters.UPLOADEDFILEID, uploadedFileID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.TransactionFileCatalog.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.SelectByTransactionID.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.TransactionFileCatalog.StoredProcedures.SelectByTransactionID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int transactionID, int uploadedFileID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.Update.Parameters.TRANSACTIONID, transactionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.TransactionFileCatalog.StoredProcedures.Update.Parameters.UPLOADEDFILEID, uploadedFileID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.TransactionFileCatalog.StoredProcedures.Update.NAME,
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