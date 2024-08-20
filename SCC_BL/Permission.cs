using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class Permission : IDisposable
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public Permission()
		{
		}

		//For SelectByID and DeleteByID
		public Permission(int id)
		{
			this.ID = id;
		}

		//For Insert
		public Permission(string description, int creationUserID, int statusID)
		{
			this.Description = description;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Permission(int id, string description, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Description = description;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT) and SelectTotalPermissionsByUserID
		public Permission(int id, string description, int basicInfoID)
		{
			this.ID = id;
			this.Description = description;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Permission repoPermission = new SCC_DATA.Repositories.Permission())
			{
				DataRow dr = repoPermission.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<Permission> SelectAll()
		{
			List<Permission> permissionList = new List<Permission>();

			using (SCC_DATA.Repositories.Permission repoPermission = new SCC_DATA.Repositories.Permission())
			{
				DataTable dt = repoPermission.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					Permission permission = new Permission(
						Convert.ToInt32(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectAll.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					permission.BasicInfo = new BasicInfo(permission.BasicInfoID);
					permission.BasicInfo.SetDataByID();

					permissionList.Add(permission);
				}
			}

			return permissionList
				.OrderBy(o => o.Description)
				.ToList();
		}

		public List<Permission> SelectTotalPermissionsByUserID(int userID)
		{
			List<Permission> permissionList = new List<Permission>();

			using (SCC_DATA.Repositories.Permission repoPermission = new SCC_DATA.Repositories.Permission())
			{
				DataTable dt = repoPermission.SelectTotalPermissionsByUserID(userID);

				foreach (DataRow dr in dt.Rows)
				{
					Permission permission = new Permission(
						Convert.ToInt32(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectTotalPermissionsByUser.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectTotalPermissionsByUser.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Permission.StoredProcedures.SelectTotalPermissionsByUser.ResultFields.BASICINFOID])
					);

					permission.BasicInfo = new BasicInfo(permission.BasicInfoID);
					permission.BasicInfo.SetDataByID();

					permissionList.Add(permission);
				}
			}

			return permissionList
				.OrderBy(o => o.Description)
				.ToList();
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Permission repoPermission = new SCC_DATA.Repositories.Permission())
			{
				int response = repoPermission.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Permission repoPermission = new SCC_DATA.Repositories.Permission())
			{
				this.ID = repoPermission.Insert(this.Description, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Permission repoPermission = new SCC_DATA.Repositories.Permission())
			{
				return repoPermission.Update(this.ID, this.Description);
			}
		}

		public void Dispose()
		{
		}
	}
}