using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class User : IDisposable
	{
		public string GetEmailByUsername(string username)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.GetEmailByUsername.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar)
					};

					return
						(string)db.ReadFirstColumn(
							Queries.User.StoredProcedures.GetEmailByUsername.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public byte[] GetSaltByUsername(string username)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.GetSaltByUsername.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar)
					};

					return
						(byte[])db.ReadFirstColumn(
							Queries.User.StoredProcedures.GetSaltByUsername.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int ValidateLogIn(string username, byte[] password)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.ValidateLogIn.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.User.StoredProcedures.ValidateLogIn.Parameters.PASSWORD, password, System.Data.SqlDbType.VarBinary)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.User.StoredProcedures.ValidateLogIn.NAME,
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
						db.CreateParameter(Queries.User.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.User.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int personID, string username, byte[] password, byte[] salt, string email, DateTime startDate, int languageID, bool hasPassPermission, DateTime lastLoginDate, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.PERSONID, personID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.PASSWORD, password, System.Data.SqlDbType.VarBinary),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.SALT, salt, System.Data.SqlDbType.VarBinary),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.EMAIL, email, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.LANGUAGEID, languageID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.HASPASSPERMISSION, hasPassPermission, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.LASTLOGINDATE, lastLoginDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.User.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.User.StoredProcedures.Insert.NAME,
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
							Queries.User.StoredProcedures.SelectAll.NAME
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
						db.CreateParameter(Queries.User.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.User.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByName(string firstName, string surName)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.SelectByName.Parameters.FIRST_NAME, firstName, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.User.StoredProcedures.SelectByName.Parameters.SUR_NAME, surName, System.Data.SqlDbType.VarChar)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.User.StoredProcedures.SelectByName.NAME,
							parameters
						);

					return
						response.Rows.Count > 0
							? response.Rows[0]
							: null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByRoleID(int roleID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.SelectByRoleID.Parameters.ROLE_ID, roleID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.User.StoredProcedures.SelectByRoleID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        public System.Data.DataTable SelectByProgramID(int programID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.User.StoredProcedures.SelectByProgramID.Parameters.PROGRAM_ID, programID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.User.StoredProcedures.SelectByProgramID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable SelectByPermissionID(int permissionID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.SelectByPermissionID.Parameters.ROLE_ID, permissionID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.User.StoredProcedures.SelectByPermissionID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectEvaluatorList()
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					return
						db.Select(
							Queries.User.StoredProcedures.SelectEvaluatorList.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByUsername(string username)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.SelectByUsername.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.User.StoredProcedures.SelectByUsername.NAME,
							parameters
						);

					return
						response.Rows.Count > 0
							? response.Rows[0]
							: null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, string username, string email, DateTime startDate, int languageID, bool hasPassPermission)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.User.StoredProcedures.Update.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.User.StoredProcedures.Update.Parameters.EMAIL, email, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.User.StoredProcedures.Update.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.User.StoredProcedures.Update.Parameters.LANGUAGEID, languageID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.User.StoredProcedures.Update.Parameters.HASPASSPERMISSION, hasPassPermission, System.Data.SqlDbType.Bit)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.User.StoredProcedures.Update.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int UpdatePassword(int id, byte[] password)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.UpdatePassword.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.User.StoredProcedures.UpdatePassword.Parameters.PASSWORD, password, System.Data.SqlDbType.VarBinary)
					};

					return db.Execute(
					   Queries.User.StoredProcedures.UpdatePassword.NAME,
					   parameters
				   );
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateLastLogin(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.User.StoredProcedures.UpdateLastLogin.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return db.Execute(
					   Queries.User.StoredProcedures.UpdateLastLogin.NAME,
					   parameters
				   );
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
        }

        public int CheckExistence(string username)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.User.StoredProcedures.CheckExistence.Parameters.USERNAME, username, System.Data.SqlDbType.VarChar)
                    };

                    return
                        (int)db.ReadFirstColumn(
                            Queries.User.StoredProcedures.CheckExistence.NAME,
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