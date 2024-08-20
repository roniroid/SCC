using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
    public class UserNotificationUrlCatalog : IDisposable
    {
        public int DeleteByID(int id)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.UserNotificationUrlCatalog.StoredProcedures.DeleteByID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(int userNotificationID, int userNotificationUrlID, int basicInfoID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.Insert.Parameters.USERNOTIFICATIONID, userNotificationID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.Insert.Parameters.USERNOTIFICATIONURLID, userNotificationUrlID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
                    };

                    return
                        (int)db.ReadFirstColumn(
                            Queries.UserNotificationUrlCatalog.StoredProcedures.Insert.NAME,
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
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByID.NAME,
                            parameters
                        ).Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable SelectByUserNotificationID(int userNotificationID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByUserNotificationID.Parameters.USER_NOTIFICATION_ID, userNotificationID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByUserNotificationID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(int id, int userNotificationID, int userNotificationUrlID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.Update.Parameters.USERNOTIFICATIONID, userNotificationID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotificationUrlCatalog.StoredProcedures.Update.Parameters.USERNOTIFICATIONURLID, userNotificationUrlID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.UserNotificationUrlCatalog.StoredProcedures.Update.NAME,
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