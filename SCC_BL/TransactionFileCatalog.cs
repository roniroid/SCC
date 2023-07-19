using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class TransactionFileCatalog : IDisposable
	{
		public int ID { get; set; }
		public int TransactionID { get; set; }
		public int UploadedFileID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionFileCatalog()
		{
		}

		//For DeleteByID
		public TransactionFileCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static TransactionFileCatalog TransactionFileCatalogForInsert(int transactionID, int uploadedFileID, int creationUserID, int statusID)
		{
			TransactionFileCatalog @object = new TransactionFileCatalog();

			@object.TransactionID = transactionID;
			@object.UploadedFileID = uploadedFileID;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		//For SelectByTransactionID
		public static TransactionFileCatalog TransactionFileCatalogWithTransactionID(int transactionID)
		{
			TransactionFileCatalog @object = new TransactionFileCatalog();
			@object.TransactionID = transactionID;
			return @object;
		}

		//For Update
		public TransactionFileCatalog(int id, int transactionID, int uploadedFileID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.UploadedFileID = uploadedFileID;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByTransactionID (RESULT)
		public TransactionFileCatalog(int id, int transactionID, int uploadedFileID, int basicInfoID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.UploadedFileID = uploadedFileID;
			this.BasicInfoID = basicInfoID;
		}

		public List<TransactionFileCatalog> SelectByTransactionID()
		{
			List<TransactionFileCatalog> transactionFileCatalogList = new List<TransactionFileCatalog>();

			using (SCC_DATA.Repositories.TransactionFileCatalog repoTransactionFileCatalog = new SCC_DATA.Repositories.TransactionFileCatalog())
			{
				DataTable dt = repoTransactionFileCatalog.SelectByTransactionID(this.TransactionID);

				foreach (DataRow dr in dt.Rows)
				{
					TransactionFileCatalog transactionFileCatalog = new TransactionFileCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionFileCatalog.StoredProcedures.SelectByTransactionID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionFileCatalog.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionFileCatalog.StoredProcedures.SelectByTransactionID.ResultFields.UPLOADEDFILEID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionFileCatalog.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID])
					);

					transactionFileCatalog.BasicInfo = new BasicInfo(transactionFileCatalog.BasicInfoID);
					transactionFileCatalog.BasicInfo.SetDataByID();

					transactionFileCatalogList.Add(transactionFileCatalog);
				}
			}

			return transactionFileCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionFileCatalog repoTransactionFileCatalog = new SCC_DATA.Repositories.TransactionFileCatalog())
			{
				int response = repoTransactionFileCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionFileCatalog repoTransactionFileCatalog = new SCC_DATA.Repositories.TransactionFileCatalog())
			{
				this.ID = repoTransactionFileCatalog.Insert(this.TransactionID, this.UploadedFileID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionFileCatalog repoTransactionFileCatalog = new SCC_DATA.Repositories.TransactionFileCatalog())
			{
				return repoTransactionFileCatalog.Update(this.ID, this.TransactionID, this.UploadedFileID);
			}
		}

		public void Dispose()
		{
		}
	}
}