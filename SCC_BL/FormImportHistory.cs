using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class FormImportHistory : IDisposable
	{
		public int ID { get; set; }
		public int FormID { get; set; }
		public int UploadedFileID { get; set; }

		public FormImportHistory()
		{
		}

		//For DeleteByID
		public FormImportHistory(int id)
		{
			this.ID = id;
		}

		//For Insert
		public FormImportHistory(int formID, int uploadedFileID)
		{
			this.FormID = formID;
			this.UploadedFileID = uploadedFileID;
		}

		//For SelectByFormID
		public static FormImportHistory FormImportHistoryWithFormID(int formID)
		{
			FormImportHistory @object = new FormImportHistory();
			@object.FormID = formID;
			return @object;
		}

		//For Update and SelectByFormID
		public FormImportHistory(int id, int formID, int uploadedFileID)
		{
			this.ID = id;
			this.FormID = formID;
			this.UploadedFileID = uploadedFileID;
		}

		public List<FormImportHistory> SelectByFormID()
		{
			List<FormImportHistory> formImportHistoryList = new List<FormImportHistory>();

			using (SCC_DATA.Repositories.FormImportHistory repoFormImportHistory = new SCC_DATA.Repositories.FormImportHistory())
			{
				DataTable dt = repoFormImportHistory.SelectByFormID(this.FormID);

				foreach (DataRow dr in dt.Rows)
				{
					FormImportHistory formImportHistory = new FormImportHistory(
						this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.FormImportHistory.StoredProcedures.SelectByFormID.ResultFields.ID]),
						this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.FormImportHistory.StoredProcedures.SelectByFormID.ResultFields.FORMID]),
						this.UploadedFileID = Convert.ToInt32(dr[SCC_DATA.Queries.FormImportHistory.StoredProcedures.SelectByFormID.ResultFields.UPLOADEDFILEID])
					);

					formImportHistoryList.Add(formImportHistory);
				}
			}

			return formImportHistoryList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.FormImportHistory repoFormImportHistory = new SCC_DATA.Repositories.FormImportHistory())
			{
				int response = repoFormImportHistory.DeleteByID(this.ID);
				return response;
			}
		}

		public int Insert()
		{
			using (SCC_DATA.Repositories.FormImportHistory repoFormImportHistory = new SCC_DATA.Repositories.FormImportHistory())
			{
				this.ID = repoFormImportHistory.Insert(this.FormID, this.UploadedFileID);

				return this.ID;
			}
		}

		public int Update()
		{
			using (SCC_DATA.Repositories.FormImportHistory repoFormImportHistory = new SCC_DATA.Repositories.FormImportHistory())
			{
				return repoFormImportHistory.Update(this.ID, this.FormID, this.UploadedFileID);
			}
		}

		public void Dispose()
		{
		}
	}
}