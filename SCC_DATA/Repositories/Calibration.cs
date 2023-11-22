using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class Calibration : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Calibration.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Calibration.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(DateTime startDate, DateTime endDate, string description, int typeID, int experiencedUserID, bool hasNotificationToBeSent, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.ENDDATE, endDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.TYPEID, typeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.EXPERIENCEDUSERID, experiencedUserID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.HASNOTIFICATIONTOBESENT, hasNotificationToBeSent, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Calibration.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.Calibration.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Calibration.StoredProcedures.SelectByID.NAME,
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
							Queries.Calibration.StoredProcedures.SelectAll.NAME
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
                        db.CreateParameter(Queries.Calibration.StoredProcedures.SelectByProgramID.Parameters.PROGRAM_ID, programID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
							Queries.Calibration.StoredProcedures.SelectByProgramID.NAME, 
							parameters
                        );
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByUserID(int userID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.Calibration.StoredProcedures.SelectByUserID.Parameters.USER_ID, userID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
							Queries.Calibration.StoredProcedures.SelectByUserID.NAME, 
							parameters
                        );
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, DateTime startDate, DateTime endDate, string description, int typeID, int experiencedUserID, bool hasNotificationToBeSent)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.STARTDATE, startDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.ENDDATE, endDate, System.Data.SqlDbType.DateTime),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.TYPEID, typeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.EXPERIENCEDUSERID, experiencedUserID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Calibration.StoredProcedures.Update.Parameters.HASNOTIFICATIONTOBESENT, hasNotificationToBeSent, System.Data.SqlDbType.Bit)
					};

					return
						db.Execute(
							Queries.Calibration.StoredProcedures.Update.NAME,
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