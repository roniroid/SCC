using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class CustomControlValueCatalog : IDisposable
	{
		public int ID { get; set; }
		public int CustomControlID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public bool IsDefaultValue { get; set; }
		public int Order { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public CustomControlValueCatalog()
		{
		}

		//For SelectByID and DeleteByID
		public CustomControlValueCatalog(int id)
		{
			this.ID = id;
		}

		//For SelectByCustomControlIDAndName
		public CustomControlValueCatalog(int customControlID, string value)
		{
			this.CustomControlID = customControlID;
			this.Value = value;
		}

		//For Insert
		public CustomControlValueCatalog(int customControlID, string name, string value, bool isDefaultValue, int order, int creationUserID, int statusID)
		{
			this.CustomControlID = customControlID;
			this.Name = name;
			this.Value = value;
			this.IsDefaultValue = isDefaultValue;
			this.Order = order;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByCustomControlID
		public static CustomControlValueCatalog CustomControlValueCatalogWithCustomControlID(int customControlID)
		{
			CustomControlValueCatalog @object = new CustomControlValueCatalog();
			@object.CustomControlID = customControlID;
			return @object;
		}

		//For Update
		public CustomControlValueCatalog(int id, int customControlID, string name, string value, bool isDefaultValue, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.CustomControlID = customControlID;
			this.Name = name;
			this.Value = value;
			this.IsDefaultValue = isDefaultValue;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public CustomControlValueCatalog(int id, int customControlID, string name, string value, bool isDefaultValue, int order, int basicInfoID)
		{
			this.ID = id;
			this.CustomControlID = customControlID;
			this.Name = name;
			this.Value = value;
			this.IsDefaultValue = isDefaultValue;
			this.Order = order;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.CustomControlValueCatalog repoCustomControlValueCatalog = new SCC_DATA.Repositories.CustomControlValueCatalog())
			{
				DataRow dr = repoCustomControlValueCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.CustomControlID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.CUSTOMCONTROLID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.Value = Convert.ToString(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.VALUE]);
				this.IsDefaultValue = Convert.ToBoolean(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.ISDEFAULTVALUE]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public void SetDataByCustomControlIDAndValue()
		{
			using (SCC_DATA.Repositories.CustomControlValueCatalog repoCustomControlValueCatalog = new SCC_DATA.Repositories.CustomControlValueCatalog())
			{
				DataRow dr = repoCustomControlValueCatalog.SelectByCustomControlIDAndValue(this.CustomControlID, this.Value);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.ID]);
				this.CustomControlID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.CUSTOMCONTROLID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.NAME]);
				this.Value = Convert.ToString(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.VALUE]);
				this.IsDefaultValue = Convert.ToBoolean(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.ISDEFAULTVALUE]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlIDAndValue.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<CustomControlValueCatalog> SelectByCustomControlID()
		{
			List<CustomControlValueCatalog> customControlValueCatalogList = new List<CustomControlValueCatalog>();

			using (SCC_DATA.Repositories.CustomControlValueCatalog repoCustomControlValueCatalog = new SCC_DATA.Repositories.CustomControlValueCatalog())
			{
				DataTable dt = repoCustomControlValueCatalog.SelectByCustomControlID(this.CustomControlID);

				foreach (DataRow dr in dt.Rows)
				{
					CustomControlValueCatalog customControlValueCatalog = new CustomControlValueCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.CUSTOMCONTROLID]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.VALUE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.ISDEFAULTVALUE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControlValueCatalog.StoredProcedures.SelectByCustomControlID.ResultFields.BASICINFOID])
					);

					customControlValueCatalog.BasicInfo = new BasicInfo(customControlValueCatalog.BasicInfoID);
					customControlValueCatalog.BasicInfo.SetDataByID();

					customControlValueCatalogList.Add(customControlValueCatalog);
				}
			}

			return customControlValueCatalogList;
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.CustomControlValueCatalog repoCustomControlValueCatalog = new SCC_DATA.Repositories.CustomControlValueCatalog())
			{
				int response = repoCustomControlValueCatalog.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.CustomControlValueCatalog repoCustomControlValueCatalog = new SCC_DATA.Repositories.CustomControlValueCatalog())
			{
				this.ID = repoCustomControlValueCatalog.Insert(this.CustomControlID, this.Name, this.Value, this.IsDefaultValue, this.Order, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.CustomControlValueCatalog repoCustomControlValueCatalog = new SCC_DATA.Repositories.CustomControlValueCatalog())
			{
				return repoCustomControlValueCatalog.Update(this.ID, this.CustomControlID, this.Name, this.Value, this.IsDefaultValue, this.Order);
			}
		}

		public void Dispose()
		{
		}
	}
}