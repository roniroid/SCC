using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class NotificationMatrix : IDisposable
	{
		public int ID { get; set; }
		public int EntityID { get; set; }
		public int ActionID { get; set; }

		public NotificationMatrix()
		{
		}

		//For Insert
		public NotificationMatrix(int entityID, int actionID)
		{
			this.EntityID = entityID;
			this.ActionID = actionID;
		}

		//For SelectAll (RESULT)
		public NotificationMatrix(int id, int entityID, int actionID)
		{
			this.ID = id;
			this.EntityID = entityID;
			this.ActionID = actionID;
		}

		public int DeleteAll()
		{
			using (SCC_DATA.Repositories.NotificationMatrix repoNotificationMatrix = new SCC_DATA.Repositories.NotificationMatrix())
			{
				int response = repoNotificationMatrix.DeleteAll();
				return response;
			}
		}

		public int Insert()
		{
			using (SCC_DATA.Repositories.NotificationMatrix repoNotificationMatrix = new SCC_DATA.Repositories.NotificationMatrix())
			{
				this.ID = repoNotificationMatrix.Insert(this.EntityID, this.ActionID);

				return this.ID;
			}
		}

		public List<NotificationMatrix> GetAllNotificationMatrixList()
		{
			List<NotificationMatrix> notificationMatrixList = new List<NotificationMatrix>();

			using (SCC_DATA.Repositories.NotificationMatrix repoNotificationMatrix = new SCC_DATA.Repositories.NotificationMatrix())
			{
				DataTable dt = repoNotificationMatrix.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					NotificationMatrix notificationMatrix = new NotificationMatrix(
						Convert.ToInt32(dr[SCC_DATA.Queries.NotificationMatrix.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.NotificationMatrix.StoredProcedures.SelectAll.ResultFields.ENTITYID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.NotificationMatrix.StoredProcedures.SelectAll.ResultFields.ACTIONID])
					);

					notificationMatrixList.Add(notificationMatrix);
				}
			}

			return notificationMatrixList;
		}

		public void Dispose()
		{
		}
	}
}