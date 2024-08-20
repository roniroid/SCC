using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class UserWorkspaceCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UserWorkspaceCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int userID, int workspaceID, DateTime startDate, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Insert.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Insert.Parameters.WORKSPACEID, workspaceID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Insert.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.UserWorkspaceCatalog.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.Parameters.USERID, userID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int userID, int workspaceID, DateTime startDate)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Update.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Update.Parameters.WORKSPACEID, workspaceID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserWorkspaceCatalog.StoredProcedures.Update.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime)
					};

					return
						db.Execute(
							Queries.UserWorkspaceCatalog.StoredProcedures.Update.NAME,
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