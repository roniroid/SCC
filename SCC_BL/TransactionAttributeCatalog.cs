using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class TransactionAttributeCatalog : IDisposable
	{
		public int ID { get; set; }
		public int TransactionID { get; set; }
		public int AttributeID { get; set; }
		public string Comment { get; set; }
		public int? ValueID { get; set; }
		public int ScoreValue { get; set; }
		public bool Checked { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionAttributeCatalog()
		{
		}

		//For DeleteByID
		public TransactionAttributeCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public TransactionAttributeCatalog(int transactionID, int attributeID, string comment, int? valueID, int scoreValue, bool @checked, int creationUserID, int statusID)
		{
			this.TransactionID = transactionID;
			this.AttributeID = attributeID;
			this.Comment = comment;
			this.ValueID = valueID;
			this.ScoreValue = scoreValue;
			this.Checked = @checked;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		public static TransactionAttributeCatalog TransactionAttributeCatalogWithTransactionID(int transactionID)
		{
			TransactionAttributeCatalog @object = new TransactionAttributeCatalog();
			@object.TransactionID = transactionID;
			return @object;
		}

		public static TransactionAttributeCatalog TransactionAttributeCatalogWithTransactionIDAndAttributeID(int transactionID, int attributeID)
		{
			TransactionAttributeCatalog @object = new TransactionAttributeCatalog();
			@object.TransactionID = transactionID;
			@object.AttributeID = attributeID;
			return @object;
		}

		//For Update
		public TransactionAttributeCatalog(int id, int transactionID, int attributeID, string comment, int? valueID, int scoreValue, bool @checked, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.AttributeID = attributeID;
			this.Comment = comment;
			this.ValueID = valueID;
			this.ScoreValue = scoreValue;
			this.Checked = @checked;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByTransactionID (RESULT)
		public TransactionAttributeCatalog(int id, int transactionID, int attributeID, string comment, int? valueID, int scoreValue, bool @checked, int basicInfoID)
		{
			this.ID = id;
			this.TransactionID = transactionID;
			this.AttributeID = attributeID;
			this.Comment = comment;
			this.ValueID = valueID;
			this.ScoreValue = scoreValue;
			this.Checked = @checked;
			this.BasicInfoID = basicInfoID;
		}

		public List<TransactionAttributeCatalog> SelectByTransactionID()
		{
			List<TransactionAttributeCatalog> transactionAttributeCatalogList = new List<TransactionAttributeCatalog>();

			using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
			{
				DataTable dt = repoTransactionAttributeCatalog.SelectByTransactionID(this.TransactionID);

				foreach (DataRow dr in dt.Rows)
				{
					int? valueID = null;

                    try
                    {
						valueID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.VALUEID]);
					}
                    catch (Exception ex)
                    {
                    }

					TransactionAttributeCatalog transactionAttributeCatalog = new TransactionAttributeCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.ATTRIBUTEID]),
						Convert.ToString(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.COMMENT]),
						valueID,
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.SCORE_VALUE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.CHECKED]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID])
					);

					transactionAttributeCatalog.BasicInfo = new BasicInfo(transactionAttributeCatalog.BasicInfoID);
					transactionAttributeCatalog.BasicInfo.SetDataByID();

					transactionAttributeCatalogList.Add(transactionAttributeCatalog);
				}
			}

			return transactionAttributeCatalogList;
        }

        public int[] SelectAttributeIDArrayByTransactionID()
        {
            int[] attributeIDArray = new int[0];

            using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
            {
                DataTable dt = repoTransactionAttributeCatalog.SelectAttributeIDListByTransactionID(this.TransactionID);

                attributeIDArray = new int[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    attributeIDArray[i] = Convert.ToInt32(dt.Rows[i][SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectAttributeIDListByTransactionID.ResultFields.ATTRIBUTE_ID]);
                }
            }

            return attributeIDArray;
        }

        public void SetDataByID()
		{
			using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
			{
				DataRow dr = repoTransactionAttributeCatalog.SelectByID(this.ID);

				if (dr.ItemArray.Length <= 1)
				{
					this.ID = Convert.ToInt32(dr[0]);
                    return;
                }

                int? valueID = null;

				try
				{
					valueID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.VALUEID]);
				}
				catch (Exception ex)
				{
				}

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.TransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.TRANSACTIONID]);
				this.AttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.ATTRIBUTEID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.COMMENT]);
				this.ValueID = valueID;
				this.ScoreValue = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.SCORE_VALUE]);
				this.Checked = Convert.ToBoolean(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.CHECKED]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

        public void SetDataByTransactionIDAndAttributeID()
		{
			using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
			{
				DataRow dr = repoTransactionAttributeCatalog.SelectByTransactionIDAndAttributeID(this.TransactionID, this.AttributeID);

				if (dr.ItemArray.Length <= 1)
				{
					this.ID = Convert.ToInt32(dr[0]);
                    return;
                }

                int? valueID = null;

				try
				{
					valueID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.VALUEID]);
				}
				catch (Exception ex)
				{
				}

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.ID]);
				this.TransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.TRANSACTIONID]);
				this.AttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.ATTRIBUTEID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.COMMENT]);
				this.ValueID = valueID;
				this.ScoreValue = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.SCORE_VALUE]);
				this.Checked = Convert.ToBoolean(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.CHECKED]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionAttributeCatalog.StoredProcedures.SelectByTransactionIDAndAttributeID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
			{
				int response = repoTransactionAttributeCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
			{
				this.ID = repoTransactionAttributeCatalog.Insert(this.TransactionID, this.AttributeID, this.Comment, this.ValueID, this.ScoreValue, this.Checked, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionAttributeCatalog repoTransactionAttributeCatalog = new SCC_DATA.Repositories.TransactionAttributeCatalog())
			{
				return repoTransactionAttributeCatalog.Update(this.ID, this.TransactionID, this.AttributeID, this.Comment, this.ValueID, this.ScoreValue, this.Checked);
			}
		}

		public void Dispose()
		{
		}
	}
}