using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class UserPermissionCatalog : IDisposable
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int PermissionID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UserPermissionCatalog()
		{
		}

		//For DeleteByID
		public UserPermissionCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static UserPermissionCatalog UserPermissionCatalogForInsert(int userID, int permissionID, int creationUserID, int statusID)
		{
			UserPermissionCatalog @object = new UserPermissionCatalog();

			@object.UserID = userID;
			@object.PermissionID = permissionID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByUserID
		public static UserPermissionCatalog UserPermissionCatalogWithUserID(int userID)
		{
			UserPermissionCatalog @object = new UserPermissionCatalog();
			@object.UserID = userID;
			return @object;
		}

		//For Update
		public UserPermissionCatalog(int id, int userID, int permissionID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.UserID = userID;
			this.PermissionID = permissionID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByUserID (RESULT)
		public UserPermissionCatalog(int id, int userID, int permissionID, int basicInfoID)
		{
			this.ID = id;
			this.UserID = userID;
			this.PermissionID = permissionID;
			this.BasicInfoID = basicInfoID;
		}

		public List<UserPermissionCatalog> SelectByUserID()
		{
			List<UserPermissionCatalog> userPermissionCatalogList = new List<UserPermissionCatalog>();

			using (SCC_DATA.Repositories.UserPermissionCatalog repoUserPermissionCatalog = new SCC_DATA.Repositories.UserPermissionCatalog())
			{
				DataTable dt = repoUserPermissionCatalog.SelectByUserID(this.UserID);

				foreach (DataRow dr in dt.Rows)
				{
					UserPermissionCatalog userPermissionCatalog = new UserPermissionCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.UserPermissionCatalog.StoredProcedures.SelectByUserID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserPermissionCatalog.StoredProcedures.SelectByUserID.ResultFields.USERID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserPermissionCatalog.StoredProcedures.SelectByUserID.ResultFields.PERMISSIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UserPermissionCatalog.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
					);

					userPermissionCatalog.BasicInfo = new BasicInfo(userPermissionCatalog.BasicInfoID);
					userPermissionCatalog.BasicInfo.SetDataByID();

					userPermissionCatalogList.Add(userPermissionCatalog);
				}
			}

			return userPermissionCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UserPermissionCatalog repoUserPermissionCatalog = new SCC_DATA.Repositories.UserPermissionCatalog())
			{
				int response = repoUserPermissionCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UserPermissionCatalog repoUserPermissionCatalog = new SCC_DATA.Repositories.UserPermissionCatalog())
			{
				this.ID = repoUserPermissionCatalog.Insert(this.UserID, this.PermissionID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UserPermissionCatalog repoUserPermissionCatalog = new SCC_DATA.Repositories.UserPermissionCatalog())
			{
				return repoUserPermissionCatalog.Update(this.ID, this.UserID, this.PermissionID);
			}
		}

		public void Dispose()
		{
		}
	}
}