using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class UserProgramGroupCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int ProgramGroupID { get; set; }
		public int BasicInfoID { get; set; }
		//------------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		//For SelectAll
		public UserProgramGroupCatalog()
		{
		}

		//For SelectByID and DeleteByID
		public UserProgramGroupCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static UserProgramGroupCatalog UserProgramGroupCatalogForInsert(int userID, int programGroupID, int creationUserID, int statusID)
		{
			UserProgramGroupCatalog @object = new UserProgramGroupCatalog();

			@object.UserID = userID;
			@object.ProgramGroupID = programGroupID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByUserID
		public static UserProgramGroupCatalog UserProgramGroupCatalogWithUserID(int userID)
		{
			UserProgramGroupCatalog @object = new UserProgramGroupCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For Update
		public UserProgramGroupCatalog(int id, int userID, int programGroupID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.ProgramGroupID = programGroupID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectAll (RESULT) and SelectByID (RESULT) and SelectByUserID (RESULT)
		public UserProgramGroupCatalog(int id, int userID, int programGroupID, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.ProgramGroupID = programGroupID;
			this.BasicInfoID = basicInfoID;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserProgramGroupCatalog repoUserProgramGroupCatalog = new SCC_DATA.Repositories.UserProgramGroupCatalog())
			{
				int response = repoUserProgramGroupCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserProgramGroupCatalog repoUserProgramGroupCatalog = new SCC_DATA.Repositories.UserProgramGroupCatalog())
			{
				this.ID = repoUserProgramGroupCatalog.Insert(this.UserID, this.ProgramGroupID, this.BasicInfoID);

				return this.ID;
			}
		}

		public List<UserProgramGroupCatalog> SelectAll()
		{
			List<UserProgramGroupCatalog> userProgramGroupCatalogList = new List<UserProgramGroupCatalog>();

			using (SCC_DATA.Repositories.UserProgramGroupCatalog repoUserProgramGroupCatalog = new SCC_DATA.Repositories.UserProgramGroupCatalog())
			{
				DataTable dt = repoUserProgramGroupCatalog.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					UserProgramGroupCatalog userProgramGroupCatalog = new UserProgramGroupCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectAll.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectAll.ResultFields.PROGRAMGROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					userProgramGroupCatalog.BasicInfo = new BasicInfo(userProgramGroupCatalog.BasicInfoID);
					userProgramGroupCatalog.BasicInfo.SetDataByID();

					userProgramGroupCatalogList.Add(userProgramGroupCatalog);
				}
			}

			return userProgramGroupCatalogList;
		}

		public List<UserProgramGroupCatalog> SelectByUserID()
		{
			List<UserProgramGroupCatalog> userProgramGroupCatalogList = new List<UserProgramGroupCatalog>();

			using (SCC_DATA.Repositories.UserProgramGroupCatalog repoUserProgramGroupCatalog = new SCC_DATA.Repositories.UserProgramGroupCatalog())
			{
				DataTable dt = repoUserProgramGroupCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserProgramGroupCatalog userProgramGroupCatalog = new UserProgramGroupCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.PROGRAMGROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userProgramGroupCatalog.BasicInfo = new BasicInfo(userProgramGroupCatalog.BasicInfoID);
					userProgramGroupCatalog.BasicInfo.SetDataByID();

					userProgramGroupCatalogList.Add(userProgramGroupCatalog);
				}
			}

			return userProgramGroupCatalogList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.UserProgramGroupCatalog repoUserProgramGroupCatalog = new SCC_DATA.Repositories.UserProgramGroupCatalog())
			{
				DataRow dr = repoUserProgramGroupCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.UserID = Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByID.ResultFields.USERID]);
				this.ProgramGroupID = Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByID.ResultFields.PROGRAMGROUPID]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramGroupCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserProgramGroupCatalog repoUserProgramGroupCatalog = new SCC_DATA.Repositories.UserProgramGroupCatalog())
			{
				return repoUserProgramGroupCatalog.Update(this.ID, this.UserID, this.ProgramGroupID);
			}
		}

		public void Dispose()
		{
		}
	}
}