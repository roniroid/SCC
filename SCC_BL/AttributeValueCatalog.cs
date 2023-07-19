using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class AttributeValueCatalog : IDisposable
	{
		public int ID { get; set; }
		public int AttributeID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public bool TriggersChildVisualization { get; set; }
		public int Order { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public int AttributeGhostID { get; set; } = 0;

		public AttributeValueCatalog()
		{
		}

		//For SelectByID and DeleteByID
		public AttributeValueCatalog(int id)
		{
			this.ID = id;
		}

		//For SelectByAttributeIDAndValue
		public AttributeValueCatalog(int attributeID, string value)
		{
			this.AttributeID = attributeID;
			this.Value = value;
		}

		//For Insert
		public AttributeValueCatalog(int attributeID, string name, string value, bool triggersChildVisualization, int order, int creationUserID, int statusID)
		{
			this.AttributeID = attributeID;
			this.Name = name;
			this.Value = value;
			this.TriggersChildVisualization = triggersChildVisualization;
			this.Order = order;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		public static AttributeValueCatalog AttributeValueCatalogWithAttributeID(int attributeID)
		{
			AttributeValueCatalog @object = new AttributeValueCatalog();
			@object.AttributeID = attributeID;
			return @object;
		}

		//For Update
		public AttributeValueCatalog(int id, int attributeID, string name, string value, bool triggersChildVisualization, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.AttributeID = attributeID;
			this.Name = name;
			this.Value = value;
			this.TriggersChildVisualization = triggersChildVisualization;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public AttributeValueCatalog(int id, int attributeID, string name, string value, bool triggersChildVisualization, int order, int basicInfoID)
		{
			this.ID = id;
			this.AttributeID = attributeID;
			this.Name = name;
			this.Value = value;
			this.TriggersChildVisualization = triggersChildVisualization;
			this.Order = order;
			this.BasicInfoID = basicInfoID;
		}

		public int DeleteByID(int modificationUserID)
		{
			using (SCC_DATA.Repositories.AttributeValueCatalog repoAttributeValueCatalog = new SCC_DATA.Repositories.AttributeValueCatalog())
			{
				int response = 0;

				//int response = repoAttributeValueCatalog.DeleteByID(this.ID);
				//this.BasicInfo.DeleteByID();

				this.BasicInfo.ModificationUserID = modificationUserID;
				this.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.DELETED;
				this.BasicInfo.Update();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.AttributeValueCatalog repoAttributeValueCatalog = new SCC_DATA.Repositories.AttributeValueCatalog())
			{
				this.ID = repoAttributeValueCatalog.Insert(this.AttributeID, this.Name, this.Value, this.TriggersChildVisualization, this.Order, this.BasicInfoID);

				return this.ID;
			}
		}

		public List<AttributeValueCatalog> SelectByAttributeID()
		{
			List<AttributeValueCatalog> attributeValueCatalogList = new List<AttributeValueCatalog>();

			using (SCC_DATA.Repositories.AttributeValueCatalog repoAttributeValueCatalog = new SCC_DATA.Repositories.AttributeValueCatalog())
			{
				DataTable dt = repoAttributeValueCatalog.SelectByAttributeID(this.AttributeID);

				foreach (DataRow dr in dt.Rows)
				{
					AttributeValueCatalog attributeValueCatalog = new AttributeValueCatalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.ATTRIBUTEID]),
						Convert.ToString(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.VALUE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.TRIGGERSCHILDVISUALIZATION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeID.ResultFields.BASICINFOID])
					);

					attributeValueCatalog.BasicInfo = new BasicInfo(attributeValueCatalog.BasicInfoID);
					attributeValueCatalog.BasicInfo.SetDataByID();

					attributeValueCatalogList.Add(attributeValueCatalog);
				}
			}

			return attributeValueCatalogList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.AttributeValueCatalog repoAttributeValueCatalog = new SCC_DATA.Repositories.AttributeValueCatalog())
			{
				DataRow dr = repoAttributeValueCatalog.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
				this.AttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.ATTRIBUTEID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.Value = Convert.ToString(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.VALUE]);
				this.TriggersChildVisualization = Convert.ToBoolean(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.TRIGGERSCHILDVISUALIZATION]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public void SetDataByAttributeIDAndValue()
		{
			using (SCC_DATA.Repositories.AttributeValueCatalog repoAttributeValueCatalog = new SCC_DATA.Repositories.AttributeValueCatalog())
			{
				DataRow dr = repoAttributeValueCatalog.SelectByAttributeIDAndValue(this.AttributeID, this.Value);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.ID]);
				this.AttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.ATTRIBUTEID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.NAME]);
				this.Value = Convert.ToString(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.VALUE]);
				this.TriggersChildVisualization = Convert.ToBoolean(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.TRIGGERSCHILDVISUALIZATION]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.AttributeValueCatalog.StoredProcedures.SelectByAttributeIDAndValue.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.AttributeValueCatalog repoAttributeValueCatalog = new SCC_DATA.Repositories.AttributeValueCatalog())
			{
				return repoAttributeValueCatalog.Update(this.ID, this.AttributeID, this.Name, this.Value, this.TriggersChildVisualization, this.Order);
			}
		}

		public void Dispose()
		{
		}
	}
}