using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class UserProgramCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int ProgramID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UserProgramCatalog()
		{
		}

		//For DeleteByID
		public UserProgramCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static UserProgramCatalog UserProgramCatalogForInsert(int userID, int programID, int creationUserID, int statusID)
		{
			UserProgramCatalog @object = new UserProgramCatalog();

			@object.UserID = userID;
			@object.ProgramID = programID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByUserID
		public static UserProgramCatalog UserProgramCatalogWithUserID(int userID)
		{
			UserProgramCatalog @object = new UserProgramCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For Update
		public UserProgramCatalog(int id, int userID, int programID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.ProgramID = programID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByUserID (RESULT)
		public UserProgramCatalog(int id, int userID, int programID, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.ProgramID = programID;
			this.BasicInfoID = basicInfoID;
		}

		public List<UserProgramCatalog> SelectByUserID()
		{
			List<UserProgramCatalog> userProgramCatalogList = new List<UserProgramCatalog>();

			using (SCC_DATA.Repositories.UserProgramCatalog repoUserProgramCatalog = new SCC_DATA.Repositories.UserProgramCatalog())
			{
				DataTable dt = repoUserProgramCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserProgramCatalog userProgramCatalog = new UserProgramCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramCatalog.StoredProcedures.SelectByUserID.ResultFields.PROGRAMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserProgramCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userProgramCatalog.BasicInfo = new BasicInfo(userProgramCatalog.BasicInfoID);
					userProgramCatalog.BasicInfo.SetDataByID();

					userProgramCatalogList.Add(userProgramCatalog);
				}
			}

			return userProgramCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserProgramCatalog repoUserProgramCatalog = new SCC_DATA.Repositories.UserProgramCatalog())
			{
				int response = repoUserProgramCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserProgramCatalog repoUserProgramCatalog = new SCC_DATA.Repositories.UserProgramCatalog())
			{
				this.ID = repoUserProgramCatalog.Insert(this.UserID, this.ProgramID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserProgramCatalog repoUserProgramCatalog = new SCC_DATA.Repositories.UserProgramCatalog())
			{
				return repoUserProgramCatalog.Update(this.ID, this.UserID, this.ProgramID);
			}
		}

		public void Dispose()
		{
		}
	}
}