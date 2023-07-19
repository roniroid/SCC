using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class Log : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Log.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Log.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteByCategoryIDAndItemID(int categoryID, int itemID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Log.StoredProcedures.DeleteByCategoryIDAndItemID.Parameters.CATEGORYID, categoryID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Log.StoredProcedures.DeleteByCategoryIDAndItemID.Parameters.ITEMID, itemID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Log.StoredProcedures.DeleteByCategoryIDAndItemID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int categoryID, int itemID, string description, int statusID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Log.StoredProcedures.Insert.Parameters.CATEGORYID, categoryID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Log.StoredProcedures.Insert.Parameters.ITEMID, itemID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Log.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Log.StoredProcedures.Insert.Parameters.STATUSID, statusID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Log.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Log.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByCategoryIDAndItemID(int categoryID, int itemID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.Parameters.CATEGORYID, categoryID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.Parameters.ITEMID, itemID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.NAME,
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