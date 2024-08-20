using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class Program : IDisposable
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; } = null;
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
        public List<ProgramFormCatalog> ProgramFormCatalogList { get; set; } = new List<ProgramFormCatalog>();

        public Program()
		{
		}

		//For SelectByID and DeleteByID
		public Program(int id)
		{
			this.ID = id;
		}

		//For SelectByName
		public Program(string name)
		{
			this.Name = name;
		}

		//For Insert
		public Program(string name, DateTime startDate, DateTime? endDate, int creationUserID, int statusID)
		{
			this.Name = name;
			this.StartDate = startDate;
			this.EndDate = endDate;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Program(int id, string name, DateTime startDate, DateTime? endDate, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Name = name;
			this.StartDate = startDate;
			this.EndDate = endDate;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT) and SelectAll
		public Program(int id, string name, DateTime startDate, DateTime? endDate, int basicInfoID)
		{
			this.ID = id;
			this.Name = name;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				DataRow dr = repoProgram.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByID.ResultFields.STARTDATE]);
				try { this.EndDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByID.ResultFields.ENDDATE]); } catch (Exception) { }
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.SetProgramFormCatalogList();
            }
		}

		public void SetDataByName()
		{
			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				DataRow dr = repoProgram.SelectByName(this.Name);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByName.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByName.ResultFields.NAME]);
				this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByName.ResultFields.STARTDATE]);
				try { this.EndDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByName.ResultFields.ENDDATE]); } catch (Exception) { }
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectByName.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

                this.SetProgramFormCatalogList();
            }
		}

		public List<Program> SelectAll()
		{
			List<Program> programList = new List<Program>();

			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				DataTable dt = repoProgram.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					DateTime? endDate = null;

					try 
					{
						endDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectAll.ResultFields.ENDDATE]); 
					} 
					catch (Exception) 
					{ 
					}

					Program program = new Program(
						Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectAll.ResultFields.STARTDATE]),
						endDate,
						Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					program.BasicInfo = new BasicInfo(program.BasicInfoID);
					program.BasicInfo.SetDataByID();

                    program.SetProgramFormCatalogList();

                    programList.Add(program);
				}
			}

			UpdateProgramListStatus(programList);

			return programList
				.OrderBy(o => o.Name)
				.ToList();
		}

		void UpdateProgramListStatus(List<Program> programList)
		{
			foreach (Program program in programList)
			{
				if (DateTime.Now > program.EndDate && program.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED)
				{
					program.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED;
					program.BasicInfo.Update();
                }
			}
		}

		public List<Program> SelectWithForm()
		{
			List<Program> programList = new List<Program>();

			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				DataTable dt = repoProgram.SelectWithForm();

				foreach (DataRow dr in dt.Rows)
				{
					DateTime? endDate = null;

					try 
					{
						endDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectWithForm.ResultFields.ENDDATE]); 
					} 
					catch (Exception) 
					{ 
					}

					Program program = new Program(
						Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectWithForm.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectWithForm.ResultFields.NAME]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectWithForm.ResultFields.STARTDATE]),
						endDate,
						Convert.ToInt32(dr[SCC_DATA.Queries.Program.StoredProcedures.SelectWithForm.ResultFields.BASICINFOID])
					);

					program.BasicInfo = new BasicInfo(program.BasicInfoID);
					program.BasicInfo.SetDataByID();

                    program.SetProgramFormCatalogList();

                    programList.Add(program);
				}
            }

            UpdateProgramListStatus(programList);

            return programList
				.OrderBy(o => o.Name)
				.ToList();
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				int response = repoProgram.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				this.ID = repoProgram.Insert(this.Name, this.StartDate, this.EndDate, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Program repoProgram = new SCC_DATA.Repositories.Program())
			{
				return repoProgram.Update(this.ID, this.Name, this.StartDate, this.EndDate);
			}
        }

        void SetProgramFormCatalogList()
        {
            this.ProgramFormCatalogList = new List<ProgramFormCatalog>();
            this.ProgramFormCatalogList = ProgramFormCatalog.ProgramFormCatalogWithProgramID(this.ID).SelectByProgramID();
        }

        public Results.Program.UpdateProgramFormCatalogList.CODE UpdateProgramFormCatalogList(List<ProgramFormCatalog> programFormCatalogList, int creationUserID)
        {
            try
            {
                if (programFormCatalogList == null) programFormCatalogList = new List<ProgramFormCatalog>();

                //Delete old ones
                /*this.ProgramFormCatalogList
                    .ForEach(e => {
                        if (programFormCatalogList
                            .Where(w =>
                                w.FormID == e.FormID &&
                                w.ProgramID == e.ProgramID)
							.Count() == 0)
                            try
                            {
                                e.Delete();
                            }
                            catch (Exception ex)
                            {
                                e.BasicInfo.ModificationUserID = creationUserID;
                                e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.DELETED;
                                e.BasicInfo.Update();
                            }
                    });*/

                //Update existing ones
                foreach (ProgramFormCatalog programFormCatalog in programFormCatalogList.Where(e => this.ProgramFormCatalogList.Select(s => s.ID).Contains(e.ID)))
                {
                    int currentBasicInfoID = 0;

                    using (ProgramFormCatalog auxProgramFormCatalog = new ProgramFormCatalog(programFormCatalog.ID))
                    {
                        auxProgramFormCatalog.SetDataByID();
                        currentBasicInfoID = auxProgramFormCatalog.BasicInfoID;
                    }

                    ProgramFormCatalog newProgramFormCatalog = new ProgramFormCatalog(
                        programFormCatalog.ID,
                        this.ID,
                        programFormCatalog.FormID,
                        programFormCatalog.StartDate,
                        currentBasicInfoID,
                        creationUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.UPDATED);

                    int result = newProgramFormCatalog.Update();
                }

                //Create new ones
                foreach (ProgramFormCatalog programFormCatalog in programFormCatalogList.Where(e => !this.ProgramFormCatalogList.Select(s => s.ID).Contains(e.ID)))
                {
                    ProgramFormCatalog newProgramFormCatalog = new ProgramFormCatalog(
                        this.ID,
                        programFormCatalog.FormID,
                        programFormCatalog.StartDate,
                        creationUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.CREATED);

                    int result = newProgramFormCatalog.Insert();
                }

                return Results.Program.UpdateProgramFormCatalogList.CODE.SUCCESS;
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