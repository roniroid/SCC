using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class TransactionCommentary : IDisposable
	{
		public int ID { get; set; }
		public int TypeID { get; set; }
		public int TransactionID { get; set; }
		public string Comment { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public TransactionCommentary()
		{
		}

		//For DeleteByID
		public TransactionCommentary(int id)
		{
			this.ID = id;
		}

		//For Insert
		public TransactionCommentary(int typeID, int transactionID, string comment, int creationUserID, int statusID)
		{
			this.TypeID = typeID;
			this.TransactionID = transactionID;
			this.Comment = comment;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByTransactionID
		public static TransactionCommentary TransactionCommentaryWithTransactionID(int transactionID)
		{
			TransactionCommentary @object = new TransactionCommentary();
			@object.TransactionID = transactionID;
			return @object;
		}

		//For Update
		public TransactionCommentary(int id, int typeID, int transactionID, string comment, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.TypeID = typeID;
			this.TransactionID = transactionID;
			this.Comment = comment;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByTransactionID (RESULT)
		public TransactionCommentary(int id, int typeID, int transactionID, string comment, int basicInfoID)
		{
			this.ID = id;
			this.TypeID = typeID;
			this.TransactionID = transactionID;
			this.Comment = comment;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.TransactionCommentary repoTransactionCommentary = new SCC_DATA.Repositories.TransactionCommentary())
			{
				DataRow dr = repoTransactionCommentary.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.ID]);
				this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.TYPEID]);
				this.TransactionID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.COMMENT]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<TransactionCommentary> SelectByTransactionID()
		{
			List<TransactionCommentary> transactionCommentaryList = new List<TransactionCommentary>();

			using (SCC_DATA.Repositories.TransactionCommentary repoTransactionCommentary = new SCC_DATA.Repositories.TransactionCommentary())
			{
				DataTable dt = repoTransactionCommentary.SelectByTransactionID(this.TransactionID);

				foreach (DataRow dr in dt.Rows)
				{
					TransactionCommentary transactionCommentary = new TransactionCommentary(
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.TYPEID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.TRANSACTIONID]),
						Convert.ToString(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.COMMENT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.TransactionCommentary.StoredProcedures.SelectByTransactionID.ResultFields.BASICINFOID])
					);

					transactionCommentary.BasicInfo = new BasicInfo(transactionCommentary.BasicInfoID);
					transactionCommentary.BasicInfo.SetDataByID();

					transactionCommentaryList.Add(transactionCommentary);
				}
			}

			return transactionCommentaryList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.TransactionCommentary repoTransactionCommentary = new SCC_DATA.Repositories.TransactionCommentary())
			{
				int response = repoTransactionCommentary.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.TransactionCommentary repoTransactionCommentary = new SCC_DATA.Repositories.TransactionCommentary())
			{
				this.ID = repoTransactionCommentary.Insert(this.TypeID, this.TransactionID, this.Comment, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.TransactionCommentary repoTransactionCommentary = new SCC_DATA.Repositories.TransactionCommentary())
			{
				return repoTransactionCommentary.Update(this.ID, this.TypeID, this.TransactionID, this.Comment);
			}
		}

		public void Dispose()
		{
		}
	}
}