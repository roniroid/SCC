using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
    public class UserNotificationUrl : IDisposable
    {
        public int DeleteByID(int id)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.UserNotificationUrl.StoredProcedures.DeleteByID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(string content, string description, int basicInfoID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.Insert.Parameters.CONTENT, content, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
                    };

                    return
                        (int)db.ReadFirstColumn(
                            Queries.UserNotificationUrl.StoredProcedures.Insert.NAME,
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
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.UserNotificationUrl.StoredProcedures.SelectByID.NAME,
                            parameters
                        ).Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(int id, string content, string description)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.Update.Parameters.CONTENT, content, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.UserNotificationUrl.StoredProcedures.Update.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar)
                    };

                    return
                        db.Execute(
                            Queries.UserNotificationUrl.StoredProcedures.Update.NAME,
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