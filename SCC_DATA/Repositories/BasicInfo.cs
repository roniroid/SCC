using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class BasicInfo : IDisposable
	{
		public int Delete(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Delete.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.BasicInfo.StoredProcedures.Delete.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int? creationUserID, int statusID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Insert.Parameters.CREATION_USER_ID, creationUserID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Insert.Parameters.STATUS_ID, statusID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.BasicInfo.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Select.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.BasicInfo.StoredProcedures.Select.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int? modificationUserID, int statusID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Update.Parameters.MODIFICATION_USER_ID, modificationUserID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BasicInfo.StoredProcedures.Update.Parameters.STATUS_ID, statusID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.BasicInfo.StoredProcedures.Update.NAME,
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