using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class TransactionBIFieldCatalog : IDisposable
	{
		public int ID { get; set; }
		public int TransactionID { get; set; }
		public int BIFieldID { get; set; }
		public string Comment { get; set; }
		public bool Checked { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionBIFieldCatalog()
		{
		}

		//For DeleteByID
		public TransactionBIFieldCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public TransactionBIFieldCatalog(int transactionID, int bIFieldID, string comment, bool @checked, int creationUserID, int statusID)
		{
			this.TransactionID = transactionID;
			this.BIFieldID = bIFieldID;
			this.Comment = comment;
			this.Checked = @checked;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByTransactionID
		public static TransactionBIFieldCatalog TransactionBIFieldCatalogWithTransactionID(int transactionID)
		{
			TransactionBIFieldCatalog @object = new TransactionBIFieldCatalog();
			@object.TransactionID = transactionID;
			return @object;
		}

		//For Update
		public TransactionBIFieldCatalog(int id, int transactionID, int bIFieldID, string comment, bool @checked, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.BIFieldID = bIFieldID;
			this.Comment = comment;
            this.Checked = @checked;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByTransactionID (RESULT)
		public TransactionBIFieldCatalog(int id, int transactionID, int bIFieldID, string comment, bool @checked, int basicInfoID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.BIFieldID = bIFieldID;
			this.Comment = comment;
            this.Checked = @checked;

            this.BasicInfoID = basicInfoID;
        }

        public void SetDataByID()
        {
            using (SCC_DATA.Repositories.TransactionBIFieldCatalog repoTransactionBIFieldCatalog = new SCC_DATA.Repositories.TransactionBIFieldCatalog())
            {
                DataRow dr = repoTransactionBIFieldCatalog.SelectByID(this.ID);

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
                this.TransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.TRANSACTIONID]);
                this.BIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.BIFIELDID]);
                this.Comment = Convert.ToString(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.COMMENT]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);
                this.Checked = Convert.ToBoolean(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.CHECKED]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();
            }
        }

        public List<TransactionBIFieldCatalog> SelectByTransactionID()
		{
			List<TransactionBIFieldCatalog> transactionBIFieldCatalogList = new List<TransactionBIFieldCatalog>();

			using (SCC_DATA.Repositories.TransactionBIFieldCatalog repoTransactionBIFieldCatalog = new SCC_DATA.Repositories.TransactionBIFieldCatalog())
			{
				DataTable dt = repoTransactionBIFieldCatalog.SelectByTransactionID(this.TransactionID);

				foreach (DataRow dr in dt.Rows)
				{
					TransactionBIFieldCatalog transactionBIFieldCatalog = new TransactionBIFieldCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.BIFIELDID]),
						Convert.ToString(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.COMMENT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.CHECKED]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionBIFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID])
					);

					transactionBIFieldCatalog.BasicInfo = new BasicInfo(transactionBIFieldCatalog.BasicInfoID);
					transactionBIFieldCatalog.BasicInfo.SetDataByID();

					transactionBIFieldCatalogList.Add(transactionBIFieldCatalog);
				}
			}

			return transactionBIFieldCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionBIFieldCatalog repoTransactionBIFieldCatalog = new SCC_DATA.Repositories.TransactionBIFieldCatalog())
			{
				int response = repoTransactionBIFieldCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionBIFieldCatalog repoTransactionBIFieldCatalog = new SCC_DATA.Repositories.TransactionBIFieldCatalog())
			{
				this.ID = repoTransactionBIFieldCatalog.Insert(this.TransactionID, this.BIFieldID, this.Comment, this.Checked, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionBIFieldCatalog repoTransactionBIFieldCatalog = new SCC_DATA.Repositories.TransactionBIFieldCatalog())
			{
				return repoTransactionBIFieldCatalog.Update(this.ID, this.TransactionID, this.BIFieldID, this.Comment, this.Checked);
			}
		}

		public void Dispose()
		{
		}
	}
}