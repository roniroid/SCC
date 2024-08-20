using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class BusinessIntelligenceValueCatalog_DELETED_FROM_DB : IDisposable
	{
		public int ID { get; set; }
		public int BIFieldID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public bool TriggersChildVisualization { get; set; }
		public int Order { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public BusinessIntelligenceValueCatalog_DELETED_FROM_DB()
		{
		}

		//For SelectByID and DeleteByID
		public BusinessIntelligenceValueCatalog_DELETED_FROM_DB(int id)
		{
			this.ID = id;
		}

		//For Insert
		public BusinessIntelligenceValueCatalog_DELETED_FROM_DB(int bIFieldID, string name, string value, bool triggersChildVisualization, int order, int creationUserID, int statusID)
		{
			this.BIFieldID = bIFieldID;
			this.Name = name;
			this.Value = value;
			this.TriggersChildVisualization = triggersChildVisualization;
			this.Order = order;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For SelectByBIFieldID
		public static BusinessIntelligenceValueCatalog_DELETED_FROM_DB BusinessIntelligenceValueCatalogWithBIFieldID(int bIFieldID)
		{
            BusinessIntelligenceValueCatalog_DELETED_FROM_DB @object = new BusinessIntelligenceValueCatalog_DELETED_FROM_DB();
			@object.BIFieldID = bIFieldID;
			return @object;
		}

		//For Update
		public BusinessIntelligenceValueCatalog_DELETED_FROM_DB(int id, int bIFieldID, string name, string value, bool triggersChildVisualization, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.BIFieldID = bIFieldID;
			this.Name = name;
			this.Value = value;
			this.TriggersChildVisualization = triggersChildVisualization;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public BusinessIntelligenceValueCatalog_DELETED_FROM_DB(int id, int bIFieldID, string name, string value, bool triggersChildVisualization, int order, int basicInfoID)
		{
			this.ID = id;
			this.BIFieldID = bIFieldID;
			this.Name = name;
			this.Value = value;
			this.TriggersChildVisualization = triggersChildVisualization;
			this.Order = order;
			this.BasicInfoID = basicInfoID;
		}

		public List<BusinessIntelligenceValueCatalog_DELETED_FROM_DB> SelectByBIFieldID()
		{
			List<BusinessIntelligenceValueCatalog_DELETED_FROM_DB> businessIntelligenceValueCatalogList = new List<BusinessIntelligenceValueCatalog_DELETED_FROM_DB>();

			using (SCC_DATA.Repositories.BusinessIntelligenceValueCatalog repoBusinessIntelligenceValueCatalog = new SCC_DATA.Repositories.BusinessIntelligenceValueCatalog())
			{
				DataTable dt = repoBusinessIntelligenceValueCatalog.SelectByBIFieldID(this.BIFieldID);

				foreach (DataRow dr in dt.Rows)
				{
                    BusinessIntelligenceValueCatalog_DELETED_FROM_DB businessIntelligenceValueCatalog = new BusinessIntelligenceValueCatalog_DELETED_FROM_DB(
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.BIFIELDID]),
						Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.VALUE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.TRIGGERSCHILDVISUALIZATION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByBIFieldID.ResultFields.BASICINFOID])
					);

					businessIntelligenceValueCatalog.BasicInfo = new BasicInfo(businessIntelligenceValueCatalog.BasicInfoID);
					businessIntelligenceValueCatalog.BasicInfo.SetDataByID();

					businessIntelligenceValueCatalogList.Add(businessIntelligenceValueCatalog);
				}
			}

			return businessIntelligenceValueCatalogList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.BusinessIntelligenceValueCatalog repoBusinessIntelligenceValueCatalog = new SCC_DATA.Repositories.BusinessIntelligenceValueCatalog())
			{
				DataRow dr = repoBusinessIntelligenceValueCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.BIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.BIFIELDID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.Value = Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.VALUE]);
				this.TriggersChildVisualization = Convert.ToBoolean(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.TRIGGERSCHILDVISUALIZATION]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceValueCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.BusinessIntelligenceValueCatalog repoBusinessIntelligenceValueCatalog = new SCC_DATA.Repositories.BusinessIntelligenceValueCatalog())
			{
				int response = repoBusinessIntelligenceValueCatalog.DeleteByID(this.ID);
				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.BusinessIntelligenceValueCatalog repoBusinessIntelligenceValueCatalog = new SCC_DATA.Repositories.BusinessIntelligenceValueCatalog())
			{
				this.ID = repoBusinessIntelligenceValueCatalog.Insert(this.BIFieldID, this.Name, this.Value, this.TriggersChildVisualization, this.Order, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.BusinessIntelligenceValueCatalog repoBusinessIntelligenceValueCatalog = new SCC_DATA.Repositories.BusinessIntelligenceValueCatalog())
			{
				return repoBusinessIntelligenceValueCatalog.Update(this.ID, this.BIFieldID, this.Name, this.Value, this.TriggersChildVisualization, this.Order);
			}
		}

		public void Dispose()
		{
		}
	}
}