using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class BusinessIntelligenceField : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.BusinessIntelligenceField.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(string name, string description, int? parentBIFieldID, bool hasForcedComment, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Insert.Parameters.PARENTBIFIELDID, parentBIFieldID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Insert.Parameters.HASFORCEDCOMMENT, hasForcedComment, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.BusinessIntelligenceField.StoredProcedures.Insert.NAME,
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
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByParentIDAndName(string name, int? parentID = null)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.Parameters.PARENT_ID, parentID, System.Data.SqlDbType.Int)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.NAME,
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

		public System.Data.DataTable SelectAll()
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					return
						db.Select(
							Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.NAME
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectChildren(int parentBIFieldID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.Parameters.PARENTBIFIELDID, parentBIFieldID, System.Data.SqlDbType.Int)
                    };

                    return
                        db.Select(
							Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.NAME,
                            parameters
                        );
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public System.Data.DataTable SelectByProgramID(string programIDArray)
        {
            try
            {
                using (DBDriver db = new DBDriver())
                {
                    SqlParameter[] parameters = new SqlParameter[] {
                        db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.SelectByProgramID.Parameters.PROGRAM_ID_LIST, programIDArray, System.Data.SqlDbType.VarChar)
                    };

                    return
                        db.Select(
                            Queries.BusinessIntelligenceField.StoredProcedures.SelectByProgramID.NAME,
                            parameters
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(int id, string name, string description, int? parentBIFieldID, bool hasForcedComment, int order)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Update.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Update.Parameters.PARENTBIFIELDID, parentBIFieldID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Update.Parameters.HASFORCEDCOMMENT, hasForcedComment, System.Data.SqlDbType.Bit),
                        db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.Update.Parameters.ORDER, order, System.Data.SqlDbType.Int)
                    };

					return
						db.Execute(
							Queries.BusinessIntelligenceField.StoredProcedures.Update.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateOrder(int id, int order)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.UpdateOrder.Parameters.ID, id, System.Data.SqlDbType.Int),
                        db.CreateParameter(Queries.BusinessIntelligenceField.StoredProcedures.UpdateOrder.Parameters.ORDER, order, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.BusinessIntelligenceField.StoredProcedures.UpdateOrder.NAME,
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