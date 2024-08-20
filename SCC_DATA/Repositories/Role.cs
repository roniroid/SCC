using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class Role : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Role.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Role.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(string identifier, string name, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Role.StoredProcedures.Insert.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Role.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Role.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Role.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.Role.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Role.StoredProcedures.SelectByID.NAME,
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
							Queries.Role.StoredProcedures.SelectAll.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, string identifier, string name)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Role.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Role.StoredProcedures.Update.Parameters.IDENTIFIER, identifier, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Role.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Role.StoredProcedures.Update.NAME,
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