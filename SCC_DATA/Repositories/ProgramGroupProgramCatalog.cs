using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class ProgramGroupProgramCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.ProgramGroupProgramCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int programGroupID, int programID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.Insert.Parameters.PROGRAMGROUPID, programGroupID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.Insert.Parameters.PROGRAMID, programID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.ProgramGroupProgramCatalog.StoredProcedures.Insert.NAME,
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
							Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectAll.NAME
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
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByProgramGroupID(int programGroupID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByProgramGroupID.Parameters.PROGRAMGROUPID, programGroupID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByProgramGroupID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int programGroupID, int programID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.Update.Parameters.PROGRAMGROUPID, programGroupID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.ProgramGroupProgramCatalog.StoredProcedures.Update.Parameters.PROGRAMID, programID, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.ProgramGroupProgramCatalog.StoredProcedures.Update.NAME,
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