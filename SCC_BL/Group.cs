using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class Group : IDisposable
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int ApplicableModuleID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<UserGroupCatalog> UserList { get; set; } = new List<UserGroupCatalog>();

		public Group()
		{
		}

		//For SelectByID and DeleteByID
		public Group(int id)
		{
			this.ID = id;
		}

		//For Insert
		public Group(string name, int applicableModuleID, int creationUserID, int statusID)
		{
			this.Name = name;
			this.ApplicableModuleID = applicableModuleID;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Group(int id, string name, int applicableModuleID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Name = name;
			this.ApplicableModuleID = applicableModuleID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public Group(int id, string name, int applicableModuleID, int basicInfoID)
		{
			this.ID = id;
			this.Name = name;
			this.ApplicableModuleID = applicableModuleID;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Group repoGroup = new SCC_DATA.Repositories.Group())
			{
				DataRow dr = repoGroup.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.ApplicableModuleID = Convert.ToInt32(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectByID.ResultFields.APPLICABLEMODULEID]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.UserList = UserGroupCatalog.UserGroupCatalogWithGroupID(this.ID).SelectByGroupID();
			}
		}

		public List<Group> SelectAll()
		{
			List<Group> groupList = new List<Group>();

			using (SCC_DATA.Repositories.Group repoGroup = new SCC_DATA.Repositories.Group())
			{
				DataTable dt = repoGroup.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					Group group = new Group(
						Convert.ToInt32(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectAll.ResultFields.APPLICABLEMODULEID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Group.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					group.BasicInfo = new BasicInfo(group.BasicInfoID);
					group.BasicInfo.SetDataByID();

					group.UserList = UserGroupCatalog.UserGroupCatalogWithGroupID(group.ID).SelectByGroupID();

					groupList.Add(group);
				}
			}

			return groupList
				.OrderBy(o => o.Name)
				.ToList();
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Group repoGroup = new SCC_DATA.Repositories.Group())
			{
				int response = repoGroup.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Group repoGroup = new SCC_DATA.Repositories.Group())
			{
				this.ID = repoGroup.Insert(this.Name, this.ApplicableModuleID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Group repoGroup = new SCC_DATA.Repositories.Group())
			{
				return repoGroup.Update(this.ID, this.Name, this.ApplicableModuleID);
			}
		}

		public Results.Group.UpdateUserList.CODE UpdateUserList(int[] userIDList, int creationUserID)
		{
			try
			{
				if (userIDList == null) userIDList = new int[0];

				//Delete old ones
				this.UserList
					.ForEach(e => {
						if (!userIDList.Contains(e.UserID))
							e.DeleteByID();
					});

				//Create new ones
				foreach (int userID in userIDList)
				{
					if (!this.UserList.Select(e => e.UserID).Contains(userID))
					{
						UserGroupCatalog userGroupCatalog = UserGroupCatalog.UserGroupCatalogForInsert(userID, this.ID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_GROUP_CATALOG.CREATED);
						userGroupCatalog.Insert();
					}
				}

				return Results.Group.UpdateUserList.CODE.SUCCESS;
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