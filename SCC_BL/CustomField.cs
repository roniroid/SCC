using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class CustomField : IDisposable
	{
		public int ID { get; set; }
		public int FormID { get; set; }
		public int CustomControlID { get; set; }
		public int Order { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public CustomField()
		{
		}

		//For SelectByID and DeleteByID
		public CustomField(int id)
		{
			this.ID = id;
		}

		//For Insert
		public static CustomField CustomFieldForInsert(int formID, int customControlID, int order, int creationUserID, int statusID)
		{
			CustomField @object = new CustomField();

			@object.FormID = formID;
			@object.CustomControlID = customControlID;
			@object.Order = order;

			@object.BasicInfo = new BasicInfo(creationUserID, statusID);

			return @object;
		}

		public static CustomField CustomFieldWithFormID(int formID)
		{
			CustomField @object = new CustomField();
			@object.FormID = formID;
			return @object;
		}

		//For Update
		public CustomField(int id, int formID, int customControlID, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.FormID = formID;
			this.CustomControlID = customControlID;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public CustomField(int id, int formID, int customControlID, int order, int basicInfoID)
		{
			this.ID = id;
			this.FormID = formID;
			this.CustomControlID = customControlID;
			this.Order = order;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.CustomField repoCustomField = new SCC_DATA.Repositories.CustomField())
			{
				DataRow dr = repoCustomField.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByID.ResultFields.ID]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByID.ResultFields.FORMID]);
				this.CustomControlID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByID.ResultFields.CUSTOMCONTROLID]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByID.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<CustomField> SelectByFormID()
		{
			List<CustomField> customFieldList = new List<CustomField>();

			using (SCC_DATA.Repositories.CustomField repoCustomField = new SCC_DATA.Repositories.CustomField())
			{
				DataTable dt = repoCustomField.SelectByFormID(this.FormID);

				foreach (DataRow dr in dt.Rows)
				{
					CustomField customField = new CustomField(
						this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByFormID.ResultFields.ID]),
						this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByFormID.ResultFields.FORMID]),
						this.CustomControlID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByFormID.ResultFields.CUSTOMCONTROLID]),
						this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByFormID.ResultFields.ORDER]),
						this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectByFormID.ResultFields.BASICINFOID])
					);

					customField.BasicInfo = new BasicInfo(customField.BasicInfoID);
					customField.BasicInfo.SetDataByID();

					customFieldList.Add(customField);
				}
			}

			return customFieldList;
		}

		public List<CustomField> SelectAll()
		{
			List<CustomField> customFieldList = new List<CustomField>();

			using (SCC_DATA.Repositories.CustomField repoCustomField = new SCC_DATA.Repositories.CustomField())
			{
				DataTable dt = repoCustomField.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					CustomField customField = new CustomField(
						this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectAll.ResultFields.ID]),
						this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectAll.ResultFields.FORMID]),
						this.CustomControlID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectAll.ResultFields.CUSTOMCONTROLID]),
						this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectAll.ResultFields.ORDER]),
						this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomField.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					customField.BasicInfo = new BasicInfo(customField.BasicInfoID);
					customField.BasicInfo.SetDataByID();

					customFieldList.Add(customField);
				}
			}

			return customFieldList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.CustomField repoCustomField = new SCC_DATA.Repositories.CustomField())
			{
				int response = repoCustomField.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.CustomField repoCustomField = new SCC_DATA.Repositories.CustomField())
			{
				this.ID = repoCustomField.Insert(this.FormID, this.CustomControlID, this.Order, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.CustomField repoCustomField = new SCC_DATA.Repositories.CustomField())
			{
				return repoCustomField.Update(this.ID, this.FormID, this.CustomControlID, this.Order);
			}
		}

		public void Dispose()
		{
		}
	}
}