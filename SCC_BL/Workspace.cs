using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class Workspace : IDisposable
	{
		public int ID { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public bool Monitorable { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public Workspace()
		{
		}

		//For SelectByID and DeleteByID
		public Workspace(int id)
		{
			this.ID = id;
		}

		//For Insert
		public Workspace(string identifier, string name, bool monitorable, int creationUserID, int statusID)
		{
			this.Identifier = identifier;
			this.Name = name;
			this.Monitorable = monitorable;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Workspace(int id, string identifier, string name, bool monitorable, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.Name = name;
			this.Monitorable = monitorable;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT) and SelectAll
		public Workspace(int id, string identifier, string name, bool monitorable, int basicInfoID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.Name = name;
			this.Monitorable = monitorable;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Workspace repoWorkspace = new SCC_DATA.Repositories.Workspace())
			{
				DataRow dr = repoWorkspace.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Identifier = Convert.ToString(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectByID.ResultFields.IDENTIFIER]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.Monitorable = Convert.ToBoolean(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectByID.ResultFields.MONITORABLE]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<Workspace> SelectAll()
		{
			List<Workspace> workspaceList = new List<Workspace>();

			using (SCC_DATA.Repositories.Workspace repoWorkspace = new SCC_DATA.Repositories.Workspace())
			{
				DataTable dt = repoWorkspace.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					Workspace workspace = new Workspace(
						Convert.ToInt32(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectAll.ResultFields.IDENTIFIER]),
						Convert.ToString(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectAll.ResultFields.MONITORABLE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Workspace.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					workspace.BasicInfo = new BasicInfo(workspace.BasicInfoID);
					workspace.BasicInfo.SetDataByID();

					workspaceList.Add(workspace);
				}
			}

			return workspaceList
				.OrderBy(o => o.Name)
				.ToList();
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Workspace repoWorkspace = new SCC_DATA.Repositories.Workspace())
			{
				int response = repoWorkspace.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Workspace repoWorkspace = new SCC_DATA.Repositories.Workspace())
			{
				this.ID = repoWorkspace.Insert(this.Identifier, this.Name, this.Monitorable, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Workspace repoWorkspace = new SCC_DATA.Repositories.Workspace())
			{
				return repoWorkspace.Update(this.ID, this.Identifier, this.Name, this.Monitorable);
			}
		}

		public void Dispose()
		{
		}
	}
}