using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class Workspace : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Workspace.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Workspace.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(string identifier, string name, bool monitorable, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Workspace.StoredProcedures.Insert.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Workspace.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Workspace.StoredProcedures.Insert.Parameters.MONITORABLE, monitorable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Workspace.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Workspace.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.Workspace.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Workspace.StoredProcedures.SelectByID.NAME,
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
							Queries.Workspace.StoredProcedures.SelectAll.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, string identifier, string name, bool monitorable)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Workspace.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Workspace.StoredProcedures.Update.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Workspace.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Workspace.StoredProcedures.Update.Parameters.MONITORABLE, monitorable, System.Data.SqlDbType.Bit)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Workspace.StoredProcedures.Update.NAME,
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