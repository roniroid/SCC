using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class BasicInfo : IDisposable
	{
		public int ID { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime ModificationDate { get; set; }
		public int? CreationUserID { get; set; } = null;
		public int? ModificationUserID { get; set; } = null;
		public int StatusID { get; set; }

		public BasicInfo()
		{
		}

		//For Select and Delete
		public BasicInfo(int id)
		{
			this.ID = id;
		}

		//For Insert
		public BasicInfo(int? creationUserID, int statusID)
		{
			this.CreationUserID = creationUserID;
			this.StatusID = statusID;
		}

		//For Update
		public BasicInfo(int id, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.ModificationUserID = modificationUserID;
			this.StatusID = statusID;
		}

		//For Select (RESULT)
		public BasicInfo(int id, DateTime creationDate, DateTime modificationDate, int? creationUserID, int? modificationUserID, int statusID)
		{
			this.ID = id;
			this.CreationDate = creationDate;
			this.ModificationDate = modificationDate;
			this.CreationUserID = creationUserID;
			this.ModificationUserID = modificationUserID;
			this.StatusID = statusID;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.BasicInfo repoBasicInfo = new SCC_DATA.Repositories.BasicInfo())
			{
				int response = repoBasicInfo.Delete(this.ID);
				return response;
			}
		}

		public int Insert()
		{
			using (SCC_DATA.Repositories.BasicInfo repoBasicInfo = new SCC_DATA.Repositories.BasicInfo())
			{
				this.ID = repoBasicInfo.Insert(this.CreationUserID, this.StatusID);

				return this.ID;
			}
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.BasicInfo repoBasicInfo = new SCC_DATA.Repositories.BasicInfo())
			{
				DataRow dr = repoBasicInfo.Select(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.BasicInfo.StoredProcedures.Select.ResultFields.ID]);
				this.CreationDate = Convert.ToDateTime(dr[SCC_DATA.Queries.BasicInfo.StoredProcedures.Select.ResultFields.CREATION_DATE]);
				this.ModificationDate = Convert.ToDateTime(dr[SCC_DATA.Queries.BasicInfo.StoredProcedures.Select.ResultFields.MODIFICATION_DATE]);
                try { this.CreationUserID = Convert.ToInt32(dr[SCC_DATA.Queries.BasicInfo.StoredProcedures.Select.ResultFields.CREATION_USER_ID]); } catch (Exception) { };
				try { this.ModificationUserID = Convert.ToInt32(dr[SCC_DATA.Queries.BasicInfo.StoredProcedures.Select.ResultFields.MODIFICATION_USER_ID]); } catch (Exception) { }
				this.StatusID = Convert.ToInt32(dr[SCC_DATA.Queries.BasicInfo.StoredProcedures.Select.ResultFields.STATUS_ID]);
			}
		}

		public int Update()
		{
			using (SCC_DATA.Repositories.BasicInfo repoBasicInfo = new SCC_DATA.Repositories.BasicInfo())
			{
				return repoBasicInfo.Update(this.ID, this.ModificationUserID, this.StatusID);
			}
		}

		public void Dispose()
		{
		}
	}
}