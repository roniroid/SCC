using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class ProgramFormCatalog : IDisposable
	{
		public int ID { get; set; }
		public int ProgramID { get; set; }
		public int FormID { get; set; }
		public DateTime StartDate { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public ProgramFormCatalog()
		{
		}

		//For DeleteByID
		public ProgramFormCatalog(int id)
		{
			this.ID = id;
		}

		public static ProgramFormCatalog ProgramFormCatalogWithFormID(int formID)
		{
			ProgramFormCatalog @object = new ProgramFormCatalog();
			@object.FormID = formID;
			return @object;
		}

		//For Insert
		public ProgramFormCatalog(int programID, int formID, DateTime startDate, int creationUserID, int statusID)
		{
			this.ProgramID = programID;
			this.FormID = formID;
			this.StartDate = startDate;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		public static ProgramFormCatalog ProgramFormCatalogWithProgramID(int programID)
		{
			ProgramFormCatalog @object = new ProgramFormCatalog();
			@object.ProgramID = programID;
			return @object;
		}

		//For Update
		public ProgramFormCatalog(int id, int programID, int formID, DateTime startDate, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.ProgramID = programID;
			this.FormID = formID;
			this.StartDate = startDate;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByProgramID (RESULT)
		public ProgramFormCatalog(int id, int programID, int formID, DateTime startDate, int basicInfoID)
		{
			this.ID = id;
			this.ProgramID = programID;
			this.FormID = formID;
			this.StartDate = startDate;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				DataRow dr = repoProgramFormCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.ID]);
				this.ProgramID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.PROGRAMID]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.FORMID]);
				this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.STARTDATE]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<ProgramFormCatalog> SelectByProgramID()
		{
			List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				DataTable dt = repoProgramFormCatalog.SelectByProgramID(this.ProgramID);

				foreach (DataRow dr in dt.Rows)
				{
					ProgramFormCatalog programFormCatalog = new ProgramFormCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.PROGRAMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.FORMID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.STARTDATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByProgramID.ResultFields.BASICINFOID])
					);

					programFormCatalog.BasicInfo = new BasicInfo(programFormCatalog.BasicInfoID);
					programFormCatalog.BasicInfo.SetDataByID();

					programFormCatalogList.Add(programFormCatalog);
				}
			}

			return programFormCatalogList;
		}

		public List<ProgramFormCatalog> SelectAll()
		{
			List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				DataTable dt = repoProgramFormCatalog.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					ProgramFormCatalog programFormCatalog = new ProgramFormCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectAll.ResultFields.PROGRAMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectAll.ResultFields.FORMID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectAll.ResultFields.STARTDATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					programFormCatalog.BasicInfo = new BasicInfo(programFormCatalog.BasicInfoID);
					programFormCatalog.BasicInfo.SetDataByID();

					programFormCatalogList.Add(programFormCatalog);
				}
			}

			return programFormCatalogList;
		}

		public List<ProgramFormCatalog> SelectByFormID()
		{
			List<ProgramFormCatalog> programFormCatalogList = new List<ProgramFormCatalog>();

			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				DataTable dt = repoProgramFormCatalog.SelectByFormID(this.FormID);

				foreach (DataRow dr in dt.Rows)
				{
					ProgramFormCatalog programFormCatalog = new ProgramFormCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByFormID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByFormID.ResultFields.PROGRAMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByFormID.ResultFields.FORMID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByFormID.ResultFields.STARTDATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramFormCatalog.StoredProcedures.SelectByFormID.ResultFields.BASICINFOID])
					);

					programFormCatalog.BasicInfo = new BasicInfo(programFormCatalog.BasicInfoID);
					programFormCatalog.BasicInfo.SetDataByID();

					programFormCatalogList.Add(programFormCatalog);
				}
			}

			return programFormCatalogList;
		}

		public int Delete()
		{
			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				int response = repoProgramFormCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				this.ID = repoProgramFormCatalog.Insert(this.ProgramID, this.FormID, this.StartDate, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.ProgramFormCatalog repoProgramFormCatalog = new SCC_DATA.Repositories.ProgramFormCatalog())
			{
				return repoProgramFormCatalog.Update(this.ID, this.ProgramID, this.FormID, this.StartDate);
			}
		}

		public void Dispose()
		{
		}
	}
}