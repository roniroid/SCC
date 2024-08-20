using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class UserWorkspaceCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int WorkspaceID { get; set; }
		public DateTime StartDate { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UserWorkspaceCatalog()
		{
		}

		//For DeleteByID
		public UserWorkspaceCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public UserWorkspaceCatalog(int userID, int workspaceID, DateTime startDate, int creationUserID, int statusID)
		{
			this.UserID = userID;
			this.WorkspaceID = workspaceID;
			this.StartDate = startDate;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByUserID
		public static UserWorkspaceCatalog UserWorkspaceCatalogWithUserID(int userID)
		{
			UserWorkspaceCatalog @object = new UserWorkspaceCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For Update
		public UserWorkspaceCatalog(int id, int userID, int workspaceID, DateTime startDate, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.WorkspaceID = workspaceID;
			this.StartDate = startDate;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByUserID (RESULT)
		public UserWorkspaceCatalog(int id, int userID, int workspaceID, DateTime startDate, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.WorkspaceID = workspaceID;
			this.StartDate = startDate;
			this.BasicInfoID = basicInfoID;
		}

		public List<UserWorkspaceCatalog> SelectByUserID()
		{
			List<UserWorkspaceCatalog> userWorkspaceCatalogList = new List<UserWorkspaceCatalog>();

			using (SCC_DATA.Repositories.UserWorkspaceCatalog repoUserWorkspaceCatalog = new SCC_DATA.Repositories.UserWorkspaceCatalog())
			{
				DataTable dt = repoUserWorkspaceCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserWorkspaceCatalog userWorkspaceCatalog = new UserWorkspaceCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.ResultFields.WORKSPACEID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.ResultFields.STARTDATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserWorkspaceCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userWorkspaceCatalog.BasicInfo = new BasicInfo(userWorkspaceCatalog.BasicInfoID);
					userWorkspaceCatalog.BasicInfo.SetDataByID();

					userWorkspaceCatalogList.Add(userWorkspaceCatalog);
				}
			}

			return userWorkspaceCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserWorkspaceCatalog repoUserWorkspaceCatalog = new SCC_DATA.Repositories.UserWorkspaceCatalog())
			{
				int response = repoUserWorkspaceCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserWorkspaceCatalog repoUserWorkspaceCatalog = new SCC_DATA.Repositories.UserWorkspaceCatalog())
			{
				this.ID = repoUserWorkspaceCatalog.Insert(this.UserID, this.WorkspaceID, this.StartDate, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserWorkspaceCatalog repoUserWorkspaceCatalog = new SCC_DATA.Repositories.UserWorkspaceCatalog())
			{
				return repoUserWorkspaceCatalog.Update(this.ID, this.UserID, this.WorkspaceID, this.StartDate);
			}
		}

		public void Dispose()
		{
		}
	}
}