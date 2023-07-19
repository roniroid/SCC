using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class Catalog : IDisposable
	{
		public int Delete(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Catalog.StoredProcedures.Delete.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Catalog.StoredProcedures.Delete.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int? categoryID, string description, bool active)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Catalog.StoredProcedures.Insert.Parameters.CATEGORYID, categoryID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Catalog.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Catalog.StoredProcedures.Insert.Parameters.ACTIVE, active, System.Data.SqlDbType.Bit)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Catalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow Select(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Catalog.StoredProcedures.Select.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Catalog.StoredProcedures.Select.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByDescription(string description)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Catalog.StoredProcedures.SelectByDescription.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Catalog.StoredProcedures.SelectByDescription.NAME,
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
							Queries.Catalog.StoredProcedures.SelectAll.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByCategoryID(int? categoryID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Catalog.StoredProcedures.SelectByCategoryID.Parameters.CATEGORYID, categoryID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Catalog.StoredProcedures.SelectByCategoryID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int? categoryID, string description, bool active)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Catalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Catalog.StoredProcedures.Update.Parameters.CATEGORYID, categoryID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Catalog.StoredProcedures.Update.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Catalog.StoredProcedures.Update.Parameters.ACTIVE, active, System.Data.SqlDbType.Bit)
					};

					return
						db.Execute(
							Queries.Catalog.StoredProcedures.Update.NAME,
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