using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class UploadedFile : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UploadedFile.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(string fileName, string extension, byte[] data, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Insert.Parameters.FILENAME, fileName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Insert.Parameters.EXTENSION, extension, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Insert.Parameters.DATA, data, System.Data.SqlDbType.VarBinary),
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.UploadedFile.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.UploadedFile.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectAll()
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					return
						db.Select(
							Queries.UploadedFile.StoredProcedures.SelectAll.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, string fileName, string extension, byte[] data)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Update.Parameters.FILENAME, fileName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Update.Parameters.EXTENSION, extension, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.UploadedFile.StoredProcedures.Update.Parameters.DATA, data, System.Data.SqlDbType.VarBinary)
					};

					return
						db.Execute(
							Queries.UploadedFile.StoredProcedures.Update.NAME,
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