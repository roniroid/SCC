using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class Role : IDisposable
	{
		public int ID { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<RolPermissionCatalog> PermissionList { get; set; } = new List<RolPermissionCatalog>();

		public Role()
		{
		}

		//For SelectByID and DeleteByID
		public Role(int id)
		{
			this.ID = id;
		}

		//For Insert
		public Role(string identifier, string name, int creationUserID, int statusID)
		{
			this.Identifier = identifier;
			this.Name = name;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Role(int id, string identifier, string name, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.Name = name;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public Role(int id, string identifier, string name, int basicInfoID)
		{
			this.ID = id;
			this.Identifier = identifier;
			this.Name = name;
			this.BasicInfoID = basicInfoID;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Role repoRole = new SCC_DATA.Repositories.Role())
			{
				int response = repoRole.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Role repoRole = new SCC_DATA.Repositories.Role())
			{
				this.ID = repoRole.Insert(this.Identifier, this.Name, this.BasicInfoID);

				return this.ID;
			}
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Role repoRole = new SCC_DATA.Repositories.Role())
			{
				DataRow dr = repoRole.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Identifier = Convert.ToString(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectByID.ResultFields.IDENTIFIER]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.PermissionList = RolPermissionCatalog.RolPermissionCatalogWithRolID(this.ID).SelectByRolID();
			}
		}

		public List<Role> SelectAll()
		{
			List<Role> roleList = new List<Role>();

			using (SCC_DATA.Repositories.Role repoRole = new SCC_DATA.Repositories.Role())
			{
				DataTable dt = repoRole.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					Role role = new Role(
						Convert.ToInt32(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectAll.ResultFields.IDENTIFIER]),
						Convert.ToString(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Role.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					role.BasicInfo = new BasicInfo(role.BasicInfoID);
					role.BasicInfo.SetDataByID();

					role.PermissionList = RolPermissionCatalog.RolPermissionCatalogWithRolID(role.ID).SelectByRolID();

					roleList.Add(role);
				}
			}

			return roleList
				.OrderBy(o => o.Name)
				.ToList();
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Role repoRole = new SCC_DATA.Repositories.Role())
			{
				return repoRole.Update(this.ID, this.Identifier, this.Name);
			}
		}

		public Results.Role.UpdatePermissionList.CODE UpdatePermissionList(int[] permissionIDList, int creationUserID)
		{
			try
			{
				if (permissionIDList == null) permissionIDList = new int[0];

				//Delete old ones
				this.PermissionList
					.ForEach(e => {
						if (!permissionIDList.Contains(e.PermissionID))
							e.DeleteByID();
					});

				//Create new ones
				foreach (int permissionID in permissionIDList)
				{
					if (!this.PermissionList.Select(e => e.PermissionID).Contains(permissionID))
					{
						RolPermissionCatalog rolePermissionCatalog = RolPermissionCatalog.RolPermissionCatalogForInsert(this.ID, permissionID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_ROL_PERMISSION_CATALOG.CREATED);
						rolePermissionCatalog.Insert();
					}
				}

				return Results.Role.UpdatePermissionList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Dispose()
		{
		}
	}
}