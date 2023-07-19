using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class CustomControl : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControl.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.CustomControl.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(string label, int moduleID, bool isRequired, string description, int controlTypeID, string cssClass, string mask, string pattern, string defaultValue, int numberOfRows, int numberOfColumns, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.LABEL, label, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.MODULEID, moduleID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.ISREQUIRED, isRequired, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.CONTROLTYPEID, controlTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.CSSCLASS, cssClass, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.MASK, mask, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.PATTERN, pattern, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.DEFAULTVALUE, defaultValue, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.NUMBEROFROWS, numberOfRows, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.NUMBEROFCOLUMNS, numberOfColumns, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.CustomControl.StoredProcedures.Insert.NAME,
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
							Queries.CustomControl.StoredProcedures.SelectAll.NAME
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
						db.CreateParameter(Queries.CustomControl.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.CustomControl.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByLabel(string label)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControl.StoredProcedures.SelectByLabel.Parameters.LABEL, label, System.Data.SqlDbType.VarChar)
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.CustomControl.StoredProcedures.SelectByLabel.NAME,
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

		public int Update(int id, string label, int moduleID, bool isRequired, string description, int controlTypeID, string cssClass, string mask, string pattern, string defaultValue, int numberOfRows, int numberOfColumns)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.LABEL, label, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.MODULEID, moduleID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.ISREQUIRED, isRequired, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.CONTROLTYPEID, controlTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.CSSCLASS, cssClass, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.MASK, mask, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.PATTERN, pattern, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.DEFAULTVALUE, defaultValue, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.NUMBEROFROWS, numberOfRows, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.CustomControl.StoredProcedures.Update.Parameters.NUMBEROFCOLUMNS, numberOfColumns, System.Data.SqlDbType.Int)
					};

					return
                        (int)db.ReadFirstColumn(
                            Queries.CustomControl.StoredProcedures.Update.NAME,
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