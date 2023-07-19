using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class UserGroupCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int GroupID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UserGroupCatalog()
		{
		}

		//For DeleteByID
		public UserGroupCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static UserGroupCatalog UserGroupCatalogForInsert(int userID, int groupID, int creationUserID, int statusID)
		{
			UserGroupCatalog @object = new UserGroupCatalog();

			@object.UserID = userID;
			@object.GroupID = groupID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByUserID
		public static UserGroupCatalog UserGroupCatalogWithUserID(int userID)
		{
			UserGroupCatalog @object = new UserGroupCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For SelectByGroupID
		public static UserGroupCatalog UserGroupCatalogWithGroupID(int groupID)
		{
			UserGroupCatalog @object = new UserGroupCatalog();
			@object.GroupID = groupID;
			return @object;
		}

		//For Update
		public UserGroupCatalog(int id, int userID, int groupID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.GroupID = groupID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByUserID (RESULT)
		public UserGroupCatalog(int id, int userID, int groupID, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.GroupID = groupID;
			this.BasicInfoID = basicInfoID;
		}

		public List<UserGroupCatalog> SelectByUserID()
		{
			List<UserGroupCatalog> userGroupCatalogList = new List<UserGroupCatalog>();

			using (SCC_DATA.Repositories.UserGroupCatalog repoUserGroupCatalog = new SCC_DATA.Repositories.UserGroupCatalog())
			{
				DataTable dt = repoUserGroupCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserGroupCatalog userGroupCatalog = new UserGroupCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.GROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userGroupCatalog.BasicInfo = new BasicInfo(userGroupCatalog.BasicInfoID);
					userGroupCatalog.BasicInfo.SetDataByID();

					userGroupCatalogList.Add(userGroupCatalog);
				}
			}

			return userGroupCatalogList;
		}

		public List<UserGroupCatalog> SelectByGroupID()
		{
			List<UserGroupCatalog> userGroupCatalogList = new List<UserGroupCatalog>();

			using (SCC_DATA.Repositories.UserGroupCatalog repoUserGroupCatalog = new SCC_DATA.Repositories.UserGroupCatalog())
			{
				DataTable dt = repoUserGroupCatalog.SelectByGroupID(this.GroupID);

				foreach (DataRow dr in dt.Rows)
				{
					UserGroupCatalog userGroupCatalog = new UserGroupCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByGroupID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByGroupID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByGroupID.ResultFields.GROUPID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserGroupCatalog.StoredProcedures.SelectByGroupID.ResultFields.BASICINFOID])
					);

					userGroupCatalog.BasicInfo = new BasicInfo(userGroupCatalog.BasicInfoID);
					userGroupCatalog.BasicInfo.SetDataByID();

					userGroupCatalogList.Add(userGroupCatalog);
				}
			}

			return userGroupCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserGroupCatalog repoUserGroupCatalog = new SCC_DATA.Repositories.UserGroupCatalog())
			{
				int response = repoUserGroupCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserGroupCatalog repoUserGroupCatalog = new SCC_DATA.Repositories.UserGroupCatalog())
			{
				this.ID = repoUserGroupCatalog.Insert(this.UserID, this.GroupID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserGroupCatalog repoUserGroupCatalog = new SCC_DATA.Repositories.UserGroupCatalog())
			{
				return repoUserGroupCatalog.Update(this.ID, this.UserID, this.GroupID);
			}
		}

		public void Dispose()
		{
		}
	}
}