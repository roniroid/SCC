using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class RolPermissionCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.RolPermissionCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int rolID, int permissionID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.Insert.Parameters.ROLID, rolID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.Insert.Parameters.PERMISSIONID, permissionID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.RolPermissionCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByRolID(int rolID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.SelectByRolID.Parameters.ROLID, rolID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.RolPermissionCatalog.StoredProcedures.SelectByRolID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int rolID, int permissionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.Update.Parameters.ROLID, rolID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.RolPermissionCatalog.StoredProcedures.Update.Parameters.PERMISSIONID, permissionID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.RolPermissionCatalog.StoredProcedures.Update.NAME,
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