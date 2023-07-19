using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class NotificationMatrix : IDisposable
	{
		public int DeleteAll()
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					return
						db.Execute(
							Queries.NotificationMatrix.StoredProcedures.DeleteAll.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int entityID, int actionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.NotificationMatrix.StoredProcedures.Insert.Parameters.ENTITYID, entityID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.NotificationMatrix.StoredProcedures.Insert.Parameters.ACTIONID, actionID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.NotificationMatrix.StoredProcedures.Insert.NAME,
							parameters
						);
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
							Queries.NotificationMatrix.StoredProcedures.SelectAll.NAME
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