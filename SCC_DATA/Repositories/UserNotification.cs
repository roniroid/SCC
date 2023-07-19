using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
    public class UserNotification : IDisposable
    {
        public int DeleteByID(int id)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.UserNotification.StoredProcedures.DeleteByID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(int userID, string message, int typeID, int basicInfoID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Insert.Parameters.USERID, userID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Insert.Parameters.MESSAGE, message, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Insert.Parameters.TYPEID, typeID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
                    };

                    return
                        (int)db.ReadFirstColumn(
                            Queries.UserNotification.StoredProcedures.Insert.NAME,
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
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.UserNotification.StoredProcedures.SelectByID.NAME,
                            parameters
                        ).Rows[0];
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
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.SelectByUserID.Parameters.USER_ID, userID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.UserNotification.StoredProcedures.SelectByUserID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(int id, int userID, string message, int typeID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Update.Parameters.USERID, userID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Update.Parameters.MESSAGE, message, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.UserNotification.StoredProcedures.Update.Parameters.TYPEID, typeID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.UserNotification.StoredProcedures.Update.NAME,
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