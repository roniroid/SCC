using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class Log : IDisposable
	{
		public int ID { get; set; }
		public int CategoryID { get; set; }
		public int ItemID { get; set; }
		public string Description { get; set; }
		public int StatusID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public Log()
		{
		}

		//For Delete
		public Log(int id)
		{
			this.ID = id;
		}

		//For SelectByItemID and DeleteByItemID
		public Log(int categoryID, int itemID)
		{
			this.CategoryID = categoryID;
			this.ItemID = itemID;
		}

		//For Insert
		public Log(int categoryID, int itemID, string description, int logStatusID, int? creationUserID, int statusID)
		{
			this.CategoryID = categoryID;
			this.ItemID = itemID;
			this.Description = description;
			this.StatusID = logStatusID;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByItemID (RESULT)
		public Log(int id, int categoryID, int itemID, string description, int statusID, int basicInfoID)
		{
			this.ID = id;
			this.CategoryID = categoryID;
			this.ItemID = itemID;
			this.Description = description;
			this.StatusID = statusID;
			this.BasicInfoID = basicInfoID;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Log repoLog = new SCC_DATA.Repositories.Log())
			{
				int response = repoLog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int DeleteByCategoryIDAndItemID()
		{
			using (SCC_DATA.Repositories.Log repoLog = new SCC_DATA.Repositories.Log())
			{
				int response = repoLog.DeleteByCategoryIDAndItemID(this.CategoryID, this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Log repoLog = new SCC_DATA.Repositories.Log())
			{
				this.ID = repoLog.Insert(this.CategoryID, this.ItemID, this.Description, this.StatusID, this.BasicInfoID);

				return this.ID;
			}
		}

		public List<Log> SelectByCategoryIDAndItemID()
		{
			List<Log> logList = new List<Log>();

			using (SCC_DATA.Repositories.Log repoLog = new SCC_DATA.Repositories.Log())
			{
				DataTable dt = repoLog.SelectByCategoryIDAndItemID(this.CategoryID, this.ItemID);

				foreach (DataRow dr in dt.Rows)
				{
					Log log = new Log(
						this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.ResultFields.ID]),
						this.CategoryID = Convert.ToInt32(dr[SCC_DATA.Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.ResultFields.CATEGORYID]),
						this.ItemID = Convert.ToInt32(dr[SCC_DATA.Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.ResultFields.ITEMID]),
						this.Description = Convert.ToString(dr[SCC_DATA.Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.ResultFields.DESCRIPTION]),
						this.StatusID = Convert.ToInt32(dr[SCC_DATA.Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.ResultFields.STATUSID]),
						this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Log.StoredProcedures.SelectByCategoryIDAndItemID.ResultFields.BASICINFOID])
					);

					log.BasicInfo = new BasicInfo(log.BasicInfoID);
					log.BasicInfo.SetDataByID();

					logList.Add(log);
				}
			}

			return logList;
		}

		public void Dispose()
		{
		}
	}
}