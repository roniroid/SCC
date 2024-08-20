using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class UserRoleCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UserRoleCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int userID, int roleID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.Insert.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.Insert.Parameters.ROLEID, roleID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.UserRoleCatalog.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.SelectByUserID.Parameters.USERID, userID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.UserRoleCatalog.StoredProcedures.SelectByUserID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int userID, int roleID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.Update.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserRoleCatalog.StoredProcedures.Update.Parameters.ROLEID, roleID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UserRoleCatalog.StoredProcedures.Update.NAME,
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