using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class UserSupervisorCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int SupervisorID { get; set; }
		public DateTime StartDate { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UserSupervisorCatalog()
		{
		}

		//For DeleteByID
		public UserSupervisorCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public UserSupervisorCatalog(int userID, int supervisorID, DateTime startDate, int creationUserID, int statusID)
		{
			this.UserID = userID;
			this.SupervisorID = supervisorID;
			this.StartDate = startDate;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByUserID
		public static UserSupervisorCatalog UserSupervisorCatalogWithUserID(int userID)
		{
			UserSupervisorCatalog @object = new UserSupervisorCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For Update
		public UserSupervisorCatalog(int id, int userID, int supervisorID, DateTime startDate, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.SupervisorID = supervisorID;
			this.StartDate = startDate;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByUserID (RESULT)
		public UserSupervisorCatalog(int id, int userID, int supervisorID, DateTime startDate, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.SupervisorID = supervisorID;
			this.StartDate = startDate;
			this.BasicInfoID = basicInfoID;
		}

		public List<UserSupervisorCatalog> SelectByUserID()
		{
			List<UserSupervisorCatalog> userSupervisorCatalogList = new List<UserSupervisorCatalog>();

			using (SCC_DATA.Repositories.UserSupervisorCatalog repoUserSupervisorCatalog = new SCC_DATA.Repositories.UserSupervisorCatalog())
			{
				DataTable dt = repoUserSupervisorCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserSupervisorCatalog userSupervisorCatalog = new UserSupervisorCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.ResultFields.SUPERVISORID]),
						Convert.ToDateTime(dr[SCC_DATA.Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.ResultFields.STARTDATE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserSupervisorCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userSupervisorCatalog.BasicInfo = new BasicInfo(userSupervisorCatalog.BasicInfoID);
					userSupervisorCatalog.BasicInfo.SetDataByID();

					userSupervisorCatalogList.Add(userSupervisorCatalog);
				}
			}

			return userSupervisorCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserSupervisorCatalog repoUserSupervisorCatalog = new SCC_DATA.Repositories.UserSupervisorCatalog())
			{
				int response = repoUserSupervisorCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserSupervisorCatalog repoUserSupervisorCatalog = new SCC_DATA.Repositories.UserSupervisorCatalog())
			{
				this.ID = repoUserSupervisorCatalog.Insert(this.UserID, this.SupervisorID, this.StartDate, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserSupervisorCatalog repoUserSupervisorCatalog = new SCC_DATA.Repositories.UserSupervisorCatalog())
			{
				return repoUserSupervisorCatalog.Update(this.ID, this.UserID, this.SupervisorID, this.StartDate);
			}
		}

		public void Dispose()
		{
		}
	}
}