using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class UserSupervisorCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.UserSupervisorCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int userID, int supervisorID, DateTime startDate, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Insert.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Insert.Parameters.SUPERVISORID, supervisorID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Insert.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.UserSupervisorCatalog.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.Parameters.USERID, userID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int userID, int supervisorID, DateTime startDate)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Update.Parameters.USERID, userID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Update.Parameters.SUPERVISORID, supervisorID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.UserSupervisorCatalog.StoredProcedures.Update.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime)
					};

					return
						db.Execute(
							Queries.UserSupervisorCatalog.StoredProcedures.Update.NAME,
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