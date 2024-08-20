using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class RolPermissionCatalog : IDisposable
	{
		public int ID { get; set; }
		public int RolID { get; set; }
		public int PermissionID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public RolPermissionCatalog()
		{
		}

		//For DeleteByID
		public RolPermissionCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static RolPermissionCatalog RolPermissionCatalogForInsert(int rolID, int permissionID, int creationUserID, int statusID)
		{
			RolPermissionCatalog @object = new RolPermissionCatalog();

			@object.RolID = rolID;
			@object.PermissionID = permissionID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByRolID
		public static RolPermissionCatalog RolPermissionCatalogWithRolID(int rolID)
		{
			RolPermissionCatalog @object = new RolPermissionCatalog();
			@object.RolID = rolID;
			return @object;
		}

		//For Update
		public RolPermissionCatalog(int id, int rolID, int permissionID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.RolID = rolID;
			this.PermissionID = permissionID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByRolID (RESULT)
		public RolPermissionCatalog(int id, int rolID, int permissionID, int basicInfoID)
		{
			this.ID = id;
			this.RolID = rolID;
			this.PermissionID = permissionID;
			this.BasicInfoID = basicInfoID;
		}

		public List<RolPermissionCatalog> SelectByRolID()
		{
			List<RolPermissionCatalog> rolPermissionCatalogList = new List<RolPermissionCatalog>();

			using (SCC_DATA.Repositories.RolPermissionCatalog repoRolPermissionCatalog = new SCC_DATA.Repositories.RolPermissionCatalog())
			{
				DataTable dt = repoRolPermissionCatalog.SelectByRolID(this.RolID);

				foreach (DataRow dr in dt.Rows)
				{
					RolPermissionCatalog rolPermissionCatalog = new RolPermissionCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.RolPermissionCatalog.StoredProcedures.SelectByRolID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.RolPermissionCatalog.StoredProcedures.SelectByRolID.ResultFields.ROLID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.RolPermissionCatalog.StoredProcedures.SelectByRolID.ResultFields.PERMISSIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.RolPermissionCatalog.StoredProcedures.SelectByRolID.ResultFields.BASICINFOID])
					);

					rolPermissionCatalog.BasicInfo = new BasicInfo(rolPermissionCatalog.BasicInfoID);
					rolPermissionCatalog.BasicInfo.SetDataByID();

					rolPermissionCatalogList.Add(rolPermissionCatalog);
				}
			}

			return rolPermissionCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.RolPermissionCatalog repoRolPermissionCatalog = new SCC_DATA.Repositories.RolPermissionCatalog())
			{
				int response = repoRolPermissionCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.RolPermissionCatalog repoRolPermissionCatalog = new SCC_DATA.Repositories.RolPermissionCatalog())
			{
				this.ID = repoRolPermissionCatalog.Insert(this.RolID, this.PermissionID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.RolPermissionCatalog repoRolPermissionCatalog = new SCC_DATA.Repositories.RolPermissionCatalog())
			{
				return repoRolPermissionCatalog.Update(this.ID, this.RolID, this.PermissionID);
			}
		}

		public void Dispose()
		{
		}
	}
}