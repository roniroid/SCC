using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class AttributeValueCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.AttributeValueCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int attributeID, string name, string value, bool triggersChildVisualization, int order, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Insert.Parameters.ATTRIBUTEID, attributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Insert.Parameters.VALUE, value, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Insert.Parameters.TRIGGERSCHILDVISUALIZATION, triggersChildVisualization, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Insert.Parameters.ORDER, order, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.AttributeValueCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByAttributeID(int attributeID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.Parameters.ATTRIBUTEID, attributeID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.NAME,
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
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.AttributeValueCatalog.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByAttributeIDAndValue(int attributeID, string value)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.Parameters.ATTRIBUTE_ID, attributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.Parameters.VALUE, value, System.Data.SqlDbType.VarChar)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.NAME,
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

		public int Update(int id, int attributeID, string name, string value, bool triggersChildVisualization, int order)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Update.Parameters.ATTRIBUTEID, attributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Update.Parameters.VALUE, value, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Update.Parameters.TRIGGERSCHILDVISUALIZATION, triggersChildVisualization, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.AttributeValueCatalog.StoredProcedures.Update.Parameters.ORDER, order, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.AttributeValueCatalog.StoredProcedures.Update.NAME,
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