using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_DATA.Repositories
{
	public class CustomControlValueCatalog : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CustomControlValueCatalog.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int customControlID, string name, string value, bool isDefaultValue, int order, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Insert.Parameters.CUSTOMCONTROLID, customControlID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Insert.Parameters.VALUE, value, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Insert.Parameters.ISDEFAULTVALUE, isDefaultValue, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Insert.Parameters.ORDER, order, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.CustomControlValueCatalog.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByCustomControlID(int customControlID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.Parameters.CUSTOMCONTROLID, customControlID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.NAME,
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
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByCustomControlIDAndValue(int customControlID, string value)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.Parameters.CUSTOM_CONTROL_ID, customControlID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.Parameters.VALUE, value, System.Data.SqlDbType.VarChar)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.NAME,
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

		public int Update(int id, int customControlID, string name, string value, bool isDefaultValue, int order)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Update.Parameters.CUSTOMCONTROLID, customControlID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Update.Parameters.VALUE, value, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Update.Parameters.ISDEFAULTVALUE, isDefaultValue, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.CustomControlValueCatalog.StoredProcedures.Update.Parameters.ORDER, order, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CustomControlValueCatalog.StoredProcedures.Update.NAME,
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