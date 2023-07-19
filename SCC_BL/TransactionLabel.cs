using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class TransactionLabel : IDisposable
	{
		public int ID { get; set; }
		public string Description { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionLabel()
		{
		}

		//For SelectByID and DeleteByID
		public TransactionLabel(int id)
		{
			this.ID = id;
		}

		//For Insert
		public TransactionLabel(string description, int creationUserID, int statusID)
		{
			this.Description = description;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public TransactionLabel(int id, string description, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Description = description;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public TransactionLabel(int id, string description, int basicInfoID)
		{
			this.ID = id;
			this.Description = description;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.TransactionLabel repoTransactionLabel = new SCC_DATA.Repositories.TransactionLabel())
			{
				DataRow dr = repoTransactionLabel.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionLabel.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.TransactionLabel.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionLabel.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionLabel repoTransactionLabel = new SCC_DATA.Repositories.TransactionLabel())
			{
				int response = repoTransactionLabel.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionLabel repoTransactionLabel = new SCC_DATA.Repositories.TransactionLabel())
			{
				this.ID = repoTransactionLabel.Insert(this.Description, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionLabel repoTransactionLabel = new SCC_DATA.Repositories.TransactionLabel())
			{
				return repoTransactionLabel.Update(this.ID, this.Description);
			}
		}

		public void Dispose()
		{
		}
	}
}