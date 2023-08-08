using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Repositories
{
	public class Attribute : IDisposable
	{
		public int DeleteByID(int id)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.DeleteByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Attribute.StoredProcedures.DeleteByID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Insert(int formID, string name, string description, int errorTypeID, int? parentAttributeID, int? maxScore, bool topDownScore, bool hasForcedComment, bool isKnown, bool isControllable, bool isScorable, int order, int basicInfoID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.FORMID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.ERRORTYPEID, errorTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.PARENTATTRIBUTEID, parentAttributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.MAXSCORE, maxScore, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.TOPDOWNSCORE, topDownScore, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.HASFORCEDCOMMENT, hasForcedComment, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.ISKNOWN, isKnown, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.ISCONTROLLABLE, isControllable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.ISSCORABLE, isScorable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.ORDER, order, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Insert.Parameters.BASICINFOID, basicInfoID, System.Data.SqlDbType.Int)
					};

					return
						(int)db.ReadFirstColumn(
							Queries.Attribute.StoredProcedures.Insert.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByFormID(int formID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByFormID.Parameters.FORMID, formID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectByFormID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectIDListByFormID(int formID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectIDListByFormID.Parameters.FORMID, formID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectIDListByFormID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByProgramAndErrorTypeID(string programIDArray, string errorTypeIDArray)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.Parameters.PROGRAM_ID_LIST, programIDArray, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.Parameters.ERROR_TYPE_ID_LIST, errorTypeIDArray, System.Data.SqlDbType.VarChar)
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByParentAttributeID(int parentAttributeID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByParentAttributeID.Parameters.PARENTATTRIBUTEID, parentAttributeID, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectByParentAttributeID.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectByLevel(int formID, int level)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByLevel.Parameters.FORM_ID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByLevel.Parameters.LEVEL, level, System.Data.SqlDbType.Int),
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectByLevel.NAME,
							parameters
						);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataTable SelectParentIDListByID(int attributeID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectParentIDListByID.Parameters.ATTRIBUTE_ID, attributeID, System.Data.SqlDbType.Int),
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectParentIDListByID.NAME,
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
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByID.Parameters.ID, id, System.Data.SqlDbType.Int)
					};

					return
						db.Select(
							Queries.Attribute.StoredProcedures.SelectByID.NAME,
							parameters
						).Rows[0];
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public System.Data.DataRow SelectByName(string name, int formID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByName.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectByName.Parameters.FORM_ID, formID, System.Data.SqlDbType.Int),
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.Attribute.StoredProcedures.SelectByName.NAME,
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

		public System.Data.DataRow SelectSubattributeByName(string name, int parentAttributeID)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectSubattributeByName.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.SelectSubattributeByName.Parameters.PARENT_ATTRIBUTE_ID, parentAttributeID, System.Data.SqlDbType.Int),
					};

					System.Data.DataTable response = new System.Data.DataTable();

					response =
						db.Select(
							Queries.Attribute.StoredProcedures.SelectSubattributeByName.NAME,
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

		public int Update(int id, int formID, string name, string description, int errorTypeID, int? parentAttributeID, int? maxScore, bool topDownScore, bool hasForcedComment, bool isKnown, bool isControllable, bool isScorable, int order)
		{
			try
			{
				using (DBDriver db = new DBDriver())
				{
					SqlParameter[] parameters = new SqlParameter[] {
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.ID, id, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.FORMID, formID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.NAME, name, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.DESCRIPTION, description, System.Data.SqlDbType.VarChar),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.ERRORTYPEID, errorTypeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.PARENTATTRIBUTEID, parentAttributeID, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.MAXSCORE, maxScore, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.TOPDOWNSCORE, topDownScore, System.Data.SqlDbType.Int),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.HASFORCEDCOMMENT, hasForcedComment, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.ISKNOWN, isKnown, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.ISCONTROLLABLE, isControllable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.ISSCORABLE, isScorable, System.Data.SqlDbType.Bit),
						db.CreateParameter(Queries.Attribute.StoredProcedures.Update.Parameters.ORDER, order, System.Data.SqlDbType.Int)
					};

					return
						db.Execute(
							Queries.Attribute.StoredProcedures.Update.NAME,
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