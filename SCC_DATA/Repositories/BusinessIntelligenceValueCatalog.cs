using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class BusinessIntelligenceValueCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.BusinessIntelligenceValueCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int bIFieldID, string name, string value, bool triggersChildVisualization, int order, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.Parameters.BIFIELDID, bIFieldID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.Parameters.VALUE, value, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.Parameters.TRIGGERSCHILDVISUALIZATION, triggersChildVisualization, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.Parameters.ORDER, order, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByBIFieldID(int bIFieldID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.Parameters.BIFIELDID, bIFieldID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.NAME,
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
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Update(int id, int bIFieldID, string name, string value, bool triggersChildVisualization, int order)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.Parameters.BIFIELDID, bIFieldID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.Parameters.VALUE, value, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.Parameters.TRIGGERSCHILDVISUALIZATION, triggersChildVisualization, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.Parameters.ORDER, order, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.BusinessIntelligenceValueCatalog.StoredProcedures.Update.NAME,
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