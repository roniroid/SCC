using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class UserRoleCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int RoleID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UserRoleCatalog()
		{
		}

		//For DeleteByID
		public UserRoleCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static UserRoleCatalog UserRoleCatalogForInsert(int userID, int roleID, int creationUserID, int statusID)
		{
			UserRoleCatalog @object = new UserRoleCatalog();

			@object.UserID = userID;
			@object.RoleID = roleID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByUserID
		public static UserRoleCatalog UserRoleCatalogWithUserID(int userID)
		{
			UserRoleCatalog @object = new UserRoleCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For Update
		public UserRoleCatalog(int id, int userID, int roleID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.RoleID = roleID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByUserID (RESULT)
		public UserRoleCatalog(int id, int userID, int roleID, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.RoleID = roleID;
			this.BasicInfoID = basicInfoID;
		}

		public List<UserRoleCatalog> SelectByUserID()
		{
			List<UserRoleCatalog> userRoleCatalogList = new List<UserRoleCatalog>();

			using (SCC_DATA.Repositories.UserRoleCatalog repoUserRoleCatalog = new SCC_DATA.Repositories.UserRoleCatalog())
			{
				DataTable dt = repoUserRoleCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserRoleCatalog userRoleCatalog = new UserRoleCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserRoleCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserRoleCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserRoleCatalog.StoredProcedures.SelectByUserID.ResultFields.ROLEID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserRoleCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userRoleCatalog.BasicInfo = new BasicInfo(userRoleCatalog.BasicInfoID);
					userRoleCatalog.BasicInfo.SetDataByID();

					userRoleCatalogList.Add(userRoleCatalog);
				}
			}

			return userRoleCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserRoleCatalog repoUserRoleCatalog = new SCC_DATA.Repositories.UserRoleCatalog())
			{
				int response = repoUserRoleCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserRoleCatalog repoUserRoleCatalog = new SCC_DATA.Repositories.UserRoleCatalog())
			{
				this.ID = repoUserRoleCatalog.Insert(this.UserID, this.RoleID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserRoleCatalog repoUserRoleCatalog = new SCC_DATA.Repositories.UserRoleCatalog())
			{
				return repoUserRoleCatalog.Update(this.ID, this.UserID, this.RoleID);
			}
		}

		public void Dispose()
		{
		}
	}
}