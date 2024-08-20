using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class ProgramGroupProgramCatalog : IDisposable
	{
		public int ID { get; set; }
		public int ProgramGroupID { get; set; }
		public int ProgramID { get; set; }
		public int BasicInfoID { get; set; }
		//------------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		//For SelectAll
		public ProgramGroupProgramCatalog()
		{
		}

		//For SelectByID and DeleteByID
		public ProgramGroupProgramCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static ProgramGroupProgramCatalog ProgramGroupProgramCatalogForInsert(int programGroupID, int programID, int creationUserID, int statusID)
		{
			ProgramGroupProgramCatalog @object = new ProgramGroupProgramCatalog();

			@object.ProgramGroupID = programGroupID;
			@object.ProgramID = programID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For Update
		public ProgramGroupProgramCatalog(int id, int programGroupID, int programID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.ProgramGroupID = programGroupID;
			this.ProgramID = programID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT) and  SelectAll (RESULT)
		public ProgramGroupProgramCatalog(int id, int programGroupID, int programID, int basicInfoID)
		{
			this.ID = id;
			this.ProgramGroupID = programGroupID;
			this.ProgramID = programID;
			this.BasicInfoID = basicInfoID;
		}

		//For SelectByProgramGroupID
		public static ProgramGroupProgramCatalog ProgramGroupProgramCatalogWithProgramGroupID(int programGroupID)
		{
			ProgramGroupProgramCatalog @object = new ProgramGroupProgramCatalog();
			@object.ProgramGroupID = programGroupID;
			return @object;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.ProgramGroupProgramCatalog repoProgramGroupProgramCatalog = new SCC_DATA.Repositories.ProgramGroupProgramCatalog())
			{
				int response = repoProgramGroupProgramCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.ProgramGroupProgramCatalog repoProgramGroupProgramCatalog = new SCC_DATA.Repositories.ProgramGroupProgramCatalog())
			{
				this.ID = repoProgramGroupProgramCatalog.Insert(this.ProgramGroupID, this.ProgramID, this.BasicInfoID);

				return this.ID;
			}
		}

		public List<ProgramGroupProgramCatalog> SelectAllProgramGroupProgramCatalogList()
		{
			List<ProgramGroupProgramCatalog> programGroupProgramCatalogList = new List<ProgramGroupProgramCatalog>();

			using (SCC_DATA.Repositories.ProgramGroupProgramCatalog repoProgramGroupProgramCatalog = new SCC_DATA.Repositories.ProgramGroupProgramCatalog())
			{
				DataTable dt = repoProgramGroupProgramCatalog.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					ProgramGroupProgramCatalog programGroupProgramCatalog = new ProgramGroupProgramCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectAll.ResultFields.PROGRAMGROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectAll.ResultFields.PROGRAMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					programGroupProgramCatalog.BasicInfo = new BasicInfo(programGroupProgramCatalog.BasicInfoID);
					programGroupProgramCatalog.BasicInfo.SetDataByID();

					programGroupProgramCatalogList.Add(programGroupProgramCatalog);
				}
			}

			return programGroupProgramCatalogList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.ProgramGroupProgramCatalog repoProgramGroupProgramCatalog = new SCC_DATA.Repositories.ProgramGroupProgramCatalog())
			{
				DataRow dr = repoProgramGroupProgramCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.ProgramGroupID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByID.ResultFields.PROGRAMGROUPID]);
				this.ProgramID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByID.ResultFields.PROGRAMID]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<ProgramGroupProgramCatalog> SelectByProgramGroupID()
		{
			List<ProgramGroupProgramCatalog> programGroupProgramCatalogList = new List<ProgramGroupProgramCatalog>();

			using (SCC_DATA.Repositories.ProgramGroupProgramCatalog repoProgramGroupProgramCatalog = new SCC_DATA.Repositories.ProgramGroupProgramCatalog())
			{
				DataTable dt = repoProgramGroupProgramCatalog.SelectByProgramGroupID(this.ProgramGroupID);

				foreach (DataRow dr in dt.Rows)
				{
					ProgramGroupProgramCatalog programGroupProgramCatalog = new ProgramGroupProgramCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByProgramGroupID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByProgramGroupID.ResultFields.PROGRAMGROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByProgramGroupID.ResultFields.PROGRAMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroupProgramCatalog.StoredProcedures.SelectByProgramGroupID.ResultFields.BASICINFOID])
					);

					programGroupProgramCatalog.BasicInfo = new BasicInfo(programGroupProgramCatalog.BasicInfoID);
					programGroupProgramCatalog.BasicInfo.SetDataByID();

					programGroupProgramCatalogList.Add(programGroupProgramCatalog);
				}
			}

			return programGroupProgramCatalogList;
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.ProgramGroupProgramCatalog repoProgramGroupProgramCatalog = new SCC_DATA.Repositories.ProgramGroupProgramCatalog())
			{
				return repoProgramGroupProgramCatalog.Update(this.ID, this.ProgramGroupID, this.ProgramID);
			}
		}

		public void Dispose()
		{
		}
	}
}