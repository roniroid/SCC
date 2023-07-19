using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class ProgramGroup : IDisposable
	{
		public int ID { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public int BasicInfoID { get; set; }
		//------------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<ProgramGroupProgramCatalog> ProgramList { get; set; } = new List<ProgramGroupProgramCatalog>();

		//For SelectAll
		public ProgramGroup()
		{
		}

		//For SelectByID and DeleteByID
		public ProgramGroup(int id)
		{
			this.ID = id;
		}

		//For Insert
		public ProgramGroup(string identifier, string name, int creationUserID, int statusID)
		{
			this.Identifier = identifier;
			this.Name = name;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public ProgramGroup(int id, string identifier, string name, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.Name = name;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectAll (RESULT) and SelectByID (RESULT)
		public ProgramGroup(int id, string identifier, string name, int basicInfoID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.Name = name;
			this.BasicInfoID = basicInfoID;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.ProgramGroup repoProgramGroup = new SCC_DATA.Repositories.ProgramGroup())
			{
				int response = repoProgramGroup.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.ProgramGroup repoProgramGroup = new SCC_DATA.Repositories.ProgramGroup())
			{
				this.ID = repoProgramGroup.Insert(this.Identifier, this.Name, this.BasicInfoID);

				return this.ID;
			}
		}

		public List<ProgramGroup> SelectAll()
		{
			List<ProgramGroup> programGroupList = new List<ProgramGroup>();

			using (SCC_DATA.Repositories.ProgramGroup repoProgramGroup = new SCC_DATA.Repositories.ProgramGroup())
			{
				DataTable dt = repoProgramGroup.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					ProgramGroup programGroup = new ProgramGroup(
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectAll.ResultFields.IDENTIFIER]),
						Convert.ToString(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					programGroup.BasicInfo = new BasicInfo(programGroup.BasicInfoID);
					programGroup.BasicInfo.SetDataByID();

					programGroup.ProgramList = ProgramGroupProgramCatalog.ProgramGroupProgramCatalogWithProgramGroupID(programGroup.ID).SelectByProgramGroupID();

					programGroupList.Add(programGroup);
				}
			}

			return programGroupList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.ProgramGroup repoProgramGroup = new SCC_DATA.Repositories.ProgramGroup())
			{
				DataRow dr = repoProgramGroup.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Identifier = Convert.ToString(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectByID.ResultFields.IDENTIFIER]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.ProgramGroup.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.ProgramList = ProgramGroupProgramCatalog.ProgramGroupProgramCatalogWithProgramGroupID(this.ID).SelectByProgramGroupID();
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.ProgramGroup repoProgramGroup = new SCC_DATA.Repositories.ProgramGroup())
			{
				return repoProgramGroup.Update(this.ID, this.Identifier, this.Name);
			}
		}

		public Results.ProgramGroup.UpdateProgramList.CODE UpdateProgramList(int[] programIDList, int creationUserID)
		{
			try
			{
				if (programIDList == null) programIDList = new int[0];

				//Delete old ones
				this.ProgramList
					.ForEach(e => {
						if (!programIDList.Contains(e.ProgramID))
							e.DeleteByID();
					});

				//Create new ones
				foreach (int programID in programIDList)
				{
					if (!this.ProgramList.Select(e => e.ProgramID).Contains(programID))
					{
						ProgramGroupProgramCatalog programGroupProgramCatalog = ProgramGroupProgramCatalog.ProgramGroupProgramCatalogForInsert(this.ID, programID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_GROUP_PROGRAM_CATALOG.CREATED);
						programGroupProgramCatalog.Insert();
					}
				}

				return Results.ProgramGroup.UpdateProgramList.CODE.SUCCESS;
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