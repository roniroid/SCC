using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class UserProgramCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UserProgramCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int userID, int programID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.Insert.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.Insert.Parameters.PROGRAMID, programID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.UserProgramCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByUserID(int userID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.SelectByUserID.Parameters.USERID, userID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.UserProgramCatalog.StoredProcedures.SelectByUserID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int userID, int programID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.Update.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserProgramCatalog.StoredProcedures.Update.Parameters.PROGRAMID, programID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UserProgramCatalog.StoredProcedures.Update.NAME,
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