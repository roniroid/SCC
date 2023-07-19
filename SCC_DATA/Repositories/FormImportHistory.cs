using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class FormImportHistory : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.FormImportHistory.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int formID, int uploadedFileID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.Insert.Parameters.FORMID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.Insert.Parameters.UPLOADEDFILEID, uploadedFileID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.FormImportHistory.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByFormID(int formID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.SelectByFormID.Parameters.FORMID, formID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.FormImportHistory.StoredProcedures.SelectByFormID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int formID, int uploadedFileID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.Update.Parameters.FORMID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.FormImportHistory.StoredProcedures.Update.Parameters.UPLOADEDFILEID, uploadedFileID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.FormImportHistory.StoredProcedures.Update.NAME,
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