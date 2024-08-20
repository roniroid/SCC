using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class TransactionLabelCatalog : IDisposable
	{
		public int ID { get; set; }
		public int TransactionID { get; set; }
		public int LabelID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionLabelCatalog()
		{
		}

		//For DeleteByID
		public TransactionLabelCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static TransactionLabelCatalog TransactionLabelCatalogForInsert(int transactionID, int labelID, int creationUserID, int statusID)
		{
			TransactionLabelCatalog @object = new TransactionLabelCatalog();

			@object.TransactionID = transactionID;
			@object.LabelID = labelID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByTransactionID
		public static TransactionLabelCatalog TransactionLabelCatalogWithTransactionID(int transactionID)
		{
			TransactionLabelCatalog @object = new TransactionLabelCatalog();
			@object.TransactionID = transactionID;
			return @object;
		}

		//For Update
		public TransactionLabelCatalog(int id, int transactionID, int labelID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.LabelID = labelID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByTransactionID (RESULT)
		public TransactionLabelCatalog(int id, int transactionID, int labelID, int basicInfoID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.LabelID = labelID;
			this.BasicInfoID = basicInfoID;
		}

		public List<TransactionLabelCatalog> SelectByTransactionID()
		{
			List<TransactionLabelCatalog> transactionLabelCatalogList = new List<TransactionLabelCatalog>();

			using (SCC_DATA.Repositories.TransactionLabelCatalog repoTransactionLabelCatalog = new SCC_DATA.Repositories.TransactionLabelCatalog())
			{
				DataTable dt = repoTransactionLabelCatalog.SelectByTransactionID(this.TransactionID);

				foreach (DataRow dr in dt.Rows)
				{
					TransactionLabelCatalog transactionLabelCatalog = new TransactionLabelCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionLabelCatalog.StoredProcedures.SelectByTransactionID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionLabelCatalog.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionLabelCatalog.StoredProcedures.SelectByTransactionID.ResultFields.LABELID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionLabelCatalog.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID])
					);

					transactionLabelCatalog.BasicInfo = new BasicInfo(transactionLabelCatalog.BasicInfoID);
					transactionLabelCatalog.BasicInfo.SetDataByID();

					transactionLabelCatalogList.Add(transactionLabelCatalog);
				}
			}

			return transactionLabelCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionLabelCatalog repoTransactionLabelCatalog = new SCC_DATA.Repositories.TransactionLabelCatalog())
			{
				int response = repoTransactionLabelCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionLabelCatalog repoTransactionLabelCatalog = new SCC_DATA.Repositories.TransactionLabelCatalog())
			{
				this.ID = repoTransactionLabelCatalog.Insert(this.TransactionID, this.LabelID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionLabelCatalog repoTransactionLabelCatalog = new SCC_DATA.Repositories.TransactionLabelCatalog())
			{
				return repoTransactionLabelCatalog.Update(this.ID, this.TransactionID, this.LabelID);
			}
		}

		public void Dispose()
		{
		}
	}
}