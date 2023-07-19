using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class TransactionCustomFieldCatalog : IDisposable
	{
		public int ID { get; set; }
		public int TransactionID { get; set; }
		public int CustomFieldID { get; set; }
		public string Comment { get; set; }
		public int? ValueID { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionCustomFieldCatalog()
		{
		}

		//For DeleteByID
		public TransactionCustomFieldCatalog(int id)
		{
			this.ID = id;
		}

		//For SelectByTransactionID
		public static TransactionCustomFieldCatalog TransactionCustomFieldCatalogWithTransactionID(int transactionID)
		{
			TransactionCustomFieldCatalog @object = new TransactionCustomFieldCatalog();
			@object.TransactionID = transactionID;
			return @object;
		}

		//For Insert
		public TransactionCustomFieldCatalog(int transactionID, int customFieldID, string comment, int? valueID, int modificationUserID, int statusID)
		{
			this.TransactionID = transactionID;
			this.CustomFieldID = customFieldID;
			this.Comment = comment;
			this.ValueID = valueID;

            this.BasicInfo = new BasicInfo(modificationUserID, statusID);
        }

		//For Update
		public TransactionCustomFieldCatalog(int id, int transactionID, int customFieldID, string comment, int? valueID, int? basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.CustomFieldID = customFieldID;
			this.Comment = comment;
			this.ValueID = valueID;

            this.BasicInfo = new BasicInfo(basicInfoID.Value, modificationUserID, statusID);
        }

		//For SelectByTransactionID (RESULT)
		public TransactionCustomFieldCatalog(int id, int transactionID, int customFieldID, string comment, int? valueID, int basicInfoID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.CustomFieldID = customFieldID;
			this.Comment = comment;
			this.ValueID = valueID;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.TransactionCustomFieldCatalog repoTransactionCustomFieldCatalog = new SCC_DATA.Repositories.TransactionCustomFieldCatalog())
			{
				DataRow dr = repoTransactionCustomFieldCatalog.SelectByID(this.ID);

                int? valueID = null;

                try
                {
                    valueID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.ResultFields.VALUEID]);
                }
                catch (Exception ex)
                {
                }

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.TransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.ResultFields.TRANSACTIONID]);
				this.CustomFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.ResultFields.CUSTOMFIELDID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.ResultFields.COMMENT]);
				this.ValueID = valueID;
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<TransactionCustomFieldCatalog> SelectByTransactionID()
		{
			List<TransactionCustomFieldCatalog> transactionCustomFieldCatalogList = new List<TransactionCustomFieldCatalog>();

			using (SCC_DATA.Repositories.TransactionCustomFieldCatalog repoTransactionCustomFieldCatalog = new SCC_DATA.Repositories.TransactionCustomFieldCatalog())
			{
				DataTable dt = repoTransactionCustomFieldCatalog.SelectByTransactionID(this.TransactionID);

				foreach (DataRow dr in dt.Rows)
				{
					int? valueID = null;

					try
					{
						valueID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.VALUEID]);
                    }
					catch (Exception ex)
					{
					}

					TransactionCustomFieldCatalog transactionCustomFieldCatalog = new TransactionCustomFieldCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.CUSTOMFIELDID]),
						Convert.ToString(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.COMMENT]),
                        valueID,
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCustomFieldCatalog.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID])
					);

					transactionCustomFieldCatalog.BasicInfo = new BasicInfo(transactionCustomFieldCatalog.BasicInfoID);
					transactionCustomFieldCatalog.BasicInfo.SetDataByID();

					transactionCustomFieldCatalogList.Add(transactionCustomFieldCatalog);
				}
			}

			return transactionCustomFieldCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionCustomFieldCatalog repoTransactionCustomFieldCatalog = new SCC_DATA.Repositories.TransactionCustomFieldCatalog())
			{
				int response = repoTransactionCustomFieldCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionCustomFieldCatalog repoTransactionCustomFieldCatalog = new SCC_DATA.Repositories.TransactionCustomFieldCatalog())
			{
				this.ID = repoTransactionCustomFieldCatalog.Insert(this.TransactionID, this.CustomFieldID, this.Comment, this.ValueID, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionCustomFieldCatalog repoTransactionCustomFieldCatalog = new SCC_DATA.Repositories.TransactionCustomFieldCatalog())
			{
				return repoTransactionCustomFieldCatalog.Update(this.ID, this.TransactionID, this.CustomFieldID, this.Comment, this.ValueID);
			}
		}

		public void Dispose()
		{
		}
	}
}