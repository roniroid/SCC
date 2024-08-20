using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
    public class CalibrationCallIdentifierCatalog : IDisposable
    {
        public int DeleteByID(int id)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.CalibrationCallIdentifierCatalog.StoredProcedures.DeleteByID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(int calibrationID, string callIdentifier, int programID, int basicInfoID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Insert.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Insert.Parameters.CALL_IDENTIFIER, callIdentifier, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Insert.Parameters.PROGRAM_ID, programID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
                    };

                    return
                        (int)db.ReadFirstColumn(
                            Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Insert.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable SelectByCalibrationID(int calibrationID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
                            Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCalibrationID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable SelectByCallIdentifier(string callIdentifier)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.Parameters.CALL_IDENTIFIER, callIdentifier, System.Data.SqlDbType.VarChar)
                    };

                    return
                        db.Select(
                            Queries.CalibrationCallIdentifierCatalog.StoredProcedures.SelectByCallIdentifier.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(int id, int calibrationID, string callIdentifier, int programID)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Update.Parameters.CALIBRATIONID, calibrationID, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Update.Parameters.CALL_IDENTIFIER, callIdentifier, System.Data.SqlDbType.VarChar),
                        db.CreateParameter(Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Update.Parameters.PROGRAM_ID, programID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Execute(
                            Queries.CalibrationCallIdentifierCatalog.StoredProcedures.Update.NAME,
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