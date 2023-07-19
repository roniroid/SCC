using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class FormBIFieldCatalog : IDisposable
	{
		public int ID { get; set; }
		public int FormID { get; set; }
		public int BIFieldID { get; set; }
		public int BasicInfoID { get; set; }
		public int Order { get; set; }
		//------------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		//For SelectAll
		public FormBIFieldCatalog()
		{
		}

		//For SelectByID and DeleteByID
		public FormBIFieldCatalog(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static FormBIFieldCatalog FormBIFieldCatalogForInsert(int formID, int bIFieldID, int order, int creationUserID, int statusID)
		{
			FormBIFieldCatalog @object = new FormBIFieldCatalog();

			@object.FormID = formID;
			@object.BIFieldID = bIFieldID;
			@object.Order = order;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		public static FormBIFieldCatalog FormBIFieldCatalogWithFormID(int formID)
		{
			FormBIFieldCatalog @object = new FormBIFieldCatalog();
			@object.FormID = formID;
			return @object;
		}

		//For Update
		public FormBIFieldCatalog(int id, int formID, int bIFieldID, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.FormID = formID;
			this.BIFieldID = bIFieldID;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectAll (RESULT) and SelectByID (RESULT)
		public FormBIFieldCatalog(int id, int formID, int bIFieldID, int basicInfoID, int order)
		{
			this.ID = id;
			this.FormID = formID;
			this.BIFieldID = bIFieldID;
			this.BasicInfoID = basicInfoID;
			this.Order = order;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.FormBIFieldCatalog repoFormBIFieldCatalog = new SCC_DATA.Repositories.FormBIFieldCatalog())
			{
				int response = repoFormBIFieldCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.FormBIFieldCatalog repoFormBIFieldCatalog = new SCC_DATA.Repositories.FormBIFieldCatalog())
			{
				this.ID = repoFormBIFieldCatalog.Insert(this.FormID, this.BIFieldID, this.Order, this.BasicInfoID);

				return this.ID;
			}
		}

		public List<FormBIFieldCatalog> SelectAll()
		{
			List<FormBIFieldCatalog> formBIFieldCatalogList = new List<FormBIFieldCatalog>();

			using (SCC_DATA.Repositories.FormBIFieldCatalog repoFormBIFieldCatalog = new SCC_DATA.Repositories.FormBIFieldCatalog())
			{
				DataTable dt = repoFormBIFieldCatalog.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					FormBIFieldCatalog formBIFieldCatalog = new FormBIFieldCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectAll.ResultFields.FORMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectAll.ResultFields.BIFIELDID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectAll.ResultFields.BASICINFOID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectAll.ResultFields.ORDER])
					);

					formBIFieldCatalog.BasicInfo = new BasicInfo(formBIFieldCatalog.BasicInfoID);
					formBIFieldCatalog.BasicInfo.SetDataByID();

					formBIFieldCatalogList.Add(formBIFieldCatalog);
				}
			}

			return formBIFieldCatalogList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.FormBIFieldCatalog repoFormBIFieldCatalog = new SCC_DATA.Repositories.FormBIFieldCatalog())
			{
				DataRow dr = repoFormBIFieldCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.FORMID]);
				this.BIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.BIFIELDID]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByID.ResultFields.ORDER]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<FormBIFieldCatalog> SelectByFormID()
		{
			List<FormBIFieldCatalog> formBIFieldCatalogList = new List<FormBIFieldCatalog>();

			using (SCC_DATA.Repositories.FormBIFieldCatalog repoFormBIFieldCatalog = new SCC_DATA.Repositories.FormBIFieldCatalog())
			{
				DataTable dt = repoFormBIFieldCatalog.SelectByFormID(this.FormID);

				foreach (DataRow dr in dt.Rows)
				{
					FormBIFieldCatalog formBIFieldCatalog = new FormBIFieldCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByFormID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByFormID.ResultFields.FORMID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByFormID.ResultFields.BIFIELDID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByFormID.ResultFields.BASICINFOID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.FormBIFieldCatalog.StoredProcedures.SelectByFormID.ResultFields.ORDER])
					);

					formBIFieldCatalog.BasicInfo = new BasicInfo(formBIFieldCatalog.BasicInfoID);
					formBIFieldCatalog.BasicInfo.SetDataByID();

					formBIFieldCatalogList.Add(formBIFieldCatalog);
				}
			}

			return formBIFieldCatalogList;
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.FormBIFieldCatalog repoFormBIFieldCatalog = new SCC_DATA.Repositories.FormBIFieldCatalog())
			{
				return repoFormBIFieldCatalog.Update(this.ID, this.FormID, this.BIFieldID, this.Order);
			}
		}

		public void Dispose()
		{
		}
	}
}