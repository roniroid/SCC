
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class Person : IDisposable
	{
		public int CheckExistence(string identification)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Person.StoredProcedures.CheckExistence.Parameters.IDENTIFICATION, identification, System.Data.SqlDbType.VarChar)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Person.StoredProcedures.CheckExistence.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Person.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Person.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(string identification, string firstName, string surName, int countryID, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Person.StoredProcedures.Insert.Parameters.IDENTIFICATION, identification, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Person.StoredProcedures.Insert.Parameters.FIRSTNAME, firstName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Person.StoredProcedures.Insert.Parameters.SURNAME, surName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Person.StoredProcedures.Insert.Parameters.COUNTRY_ID, countryID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Person.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Person.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.Person.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Person.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByIdentification(string identification)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Person.StoredProcedures.SelectByIdentification.Parameters.IDENTIFICATION, identification, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Person.StoredProcedures.SelectByIdentification.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, string identification, string firstName, string surName, int countryID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Person.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Person.StoredProcedures.Update.Parameters.IDENTIFICATION, identification, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Person.StoredProcedures.Update.Parameters.FIRSTNAME, firstName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Person.StoredProcedures.Update.Parameters.SURNAME, surName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Person.StoredProcedures.Update.Parameters.COUNTRY_ID, countryID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Person.StoredProcedures.Update.NAME,
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