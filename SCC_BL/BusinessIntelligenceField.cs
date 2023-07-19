using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class BusinessIntelligenceField : IDisposable
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int? ParentBIFieldID { get; set; } = null;
		public bool HasForcedComment { get; set; }
		public int BasicInfoID { get; set; }
		public int Order { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
        public int BIFieldGhostID { get; set; } = 0;
        public int ParentBIFieldGhostID { get; set; } = 0;

        //public List<BusinessIntelligenceValueCatalog> ValueList { get; set; } = new List<BusinessIntelligenceValueCatalog>();
        public List<BusinessIntelligenceField> ChildList { get; set; } = new List<BusinessIntelligenceField>();

        public static BusinessIntelligenceField BusinessIntelligenceFieldWithParentIDAndName(int parentBusinessIntelligenceFieldID, string name)
		{
			BusinessIntelligenceField @object = new BusinessIntelligenceField();
			@object.ParentBIFieldID = parentBusinessIntelligenceFieldID;
			@object.Name = name;
			return @object;
		}

		public BusinessIntelligenceField()
		{
		}

		//For SelectByID and DeleteByID
		public BusinessIntelligenceField(int id)
		{
			this.ID = id;
		}

		//For SelectByParentIDAndName
		public BusinessIntelligenceField(string name, int? parentID = null)
		{
			this.Name = name;
			this.ParentBIFieldID = parentID;
		}

		//For Insert
		public BusinessIntelligenceField(string name, string description, int? parentBIFieldID, bool hasForcedComment, int creationUserID, int statusID)
		{
			this.Name = name;
			this.Description = description;
			this.ParentBIFieldID = parentBIFieldID;
			this.HasForcedComment = hasForcedComment;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public BusinessIntelligenceField(int id, string name, string description, int? parentBIFieldID, bool hasForcedComment, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.ParentBIFieldID = parentBIFieldID;
			this.HasForcedComment = hasForcedComment;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For UpdateOrder
		public BusinessIntelligenceField(int id, int order)
		{
			this.ID = id;
			this.Order = order;
		}

		//For SelectByID (RESULT) and SELECTAll
		public BusinessIntelligenceField(int id, string name, string description, int? parentBIFieldID, bool hasForcedComment, int basicInfoID, int order)
		{
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.ParentBIFieldID = parentBIFieldID;
			this.HasForcedComment = hasForcedComment;
			this.BasicInfoID = basicInfoID;
			this.Order = order;
		}

		/*public void SetValueList()
		{
			this.ValueList =
				BusinessIntelligenceValueCatalog.BusinessIntelligenceValueCatalogWithBIFieldID(this.ID).SelectByBIFieldID()
					.Where(e =>
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD_VALUE_CATALOG.DELETED &&
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD_VALUE_CATALOG.DISABLED)
					.ToList();
		}*/

		public void SetChildList()
		{
			this.ChildList = this.SelectChildren();
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				DataRow dr = repoBusinessIntelligenceField.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
				try { this.ParentBIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.PARENTBIFIELDID]); } catch (Exception) { }
				this.HasForcedComment = Convert.ToBoolean(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.HASFORCEDCOMMENT]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByID.ResultFields.ORDER]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				//SetValueList();

				this.SetChildList();
            }
		}

		public void SetDataByParentIDAndName()
		{
			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				DataRow dr = repoBusinessIntelligenceField.SelectByParentIDAndName(this.Name, this.ParentBIFieldID);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.NAME]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.DESCRIPTION]);
				try { this.ParentBIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.PARENTBIFIELDID]); } catch (Exception) { }
				this.HasForcedComment = Convert.ToBoolean(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.HASFORCEDCOMMENT]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.BASICINFOID]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectByParentIDAndName.ResultFields.ORDER]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

                //SetValueList();

                this.SetChildList();
            }
		}

		public List<BusinessIntelligenceField> SelectAll()
		{
			List<BusinessIntelligenceField> biFieldList = new List<BusinessIntelligenceField>();

			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				DataTable dt = repoBusinessIntelligenceField.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					int? parentBIFieldID = null;

					try { parentBIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.PARENTBIFIELDID]); } catch (Exception) { }

					BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField(
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.DESCRIPTION]),
						parentBIFieldID,
						Convert.ToBoolean(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.BASICINFOID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectAll.ResultFields.ORDER])
					);

					businessIntelligenceField.BasicInfo = new BasicInfo(businessIntelligenceField.BasicInfoID);
					businessIntelligenceField.BasicInfo.SetDataByID();

                    //businessIntelligenceField.SetValueList();

                    businessIntelligenceField.SetChildList();

                    biFieldList.Add(businessIntelligenceField);
				}
			}

			return biFieldList;
		}

		public List<BusinessIntelligenceField> SelectChildren()
		{
			List<BusinessIntelligenceField> biFieldList = new List<BusinessIntelligenceField>();

			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				DataTable dt = repoBusinessIntelligenceField.SelectChildren(this.ID);

				foreach (DataRow dr in dt.Rows)
				{
					int? parentBIFieldID = null;

					try { parentBIFieldID = Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.PARENTBIFIELDID]); } catch (Exception) { }

					BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField(
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.DESCRIPTION]),
						parentBIFieldID,
						Convert.ToBoolean(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.BASICINFOID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.BusinessIntelligenceField.StoredProcedures.SelectChildren.ResultFields.ORDER])
					);

					businessIntelligenceField.BasicInfo = new BasicInfo(businessIntelligenceField.BasicInfoID);
					businessIntelligenceField.BasicInfo.SetDataByID();

                    //businessIntelligenceField.SetValueList();

                    businessIntelligenceField.SetChildList();

                    biFieldList.Add(businessIntelligenceField);
				}
			}

			return 
				biFieldList
					.OrderBy(e => e.Order)
					.ToList();
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				int response = repoBusinessIntelligenceField.DeleteByID(this.ID);
				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				this.ID = repoBusinessIntelligenceField.Insert(this.Name, this.Description, this.ParentBIFieldID <= 0 ? null : this.ParentBIFieldID, this.HasForcedComment, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.BusinessIntelligenceField repoBusinessIntelligenceField = new SCC_DATA.Repositories.BusinessIntelligenceField())
			{
				return repoBusinessIntelligenceField.Update(this.ID, this.Name, this.Description, this.ParentBIFieldID <= 0 ? null : this.ParentBIFieldID, this.HasForcedComment, this.Order);
			}
		}

        /*public Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.CODE UpdateBIFieldValueCatalogList(List<BusinessIntelligenceValueCatalog> biFieldValueCatalogList, int creationUserID)
		{
			try
			{
				if (biFieldValueCatalogList == null) biFieldValueCatalogList = new List<BusinessIntelligenceValueCatalog>();

				//Delete old ones
				this.ValueList
					.ForEach(e => {
						if (!biFieldValueCatalogList.Select(s => s.ID).Contains(e.ID))
						{
							try
							{
								e.DeleteByID();
							}
							catch (Exception ex)
							{
								e.BasicInfo.ModificationUserID = creationUserID;
								e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD_VALUE_CATALOG.DELETED;

								e.BasicInfo.Update();
							}
						}
					});

				//Update and create new ones
				biFieldValueCatalogList
					.ForEach(e =>
					{
						if (e.ID == null || e.ID <= 0)
						{
							using (BusinessIntelligenceValueCatalog biFieldValueCatalog = new BusinessIntelligenceValueCatalog(this.ID, e.Name, e.Value, e.TriggersChildVisualization, e.Order, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD_VALUE_CATALOG.CREATED))
							{
								biFieldValueCatalog.Insert();
							}
						}
						else
						{
							using (BusinessIntelligenceValueCatalog foundBIFieldValueCatalog = new BusinessIntelligenceValueCatalog(e.ID))
							{
								foundBIFieldValueCatalog.SetDataByID();

								if (
									!e.Name.Equals(foundBIFieldValueCatalog.Name) ||
									!e.Value.Equals(foundBIFieldValueCatalog.Value) ||
									!e.TriggersChildVisualization.Equals(foundBIFieldValueCatalog.TriggersChildVisualization) ||
									!e.Order.Equals(foundBIFieldValueCatalog.Order)
								)
								{
									BusinessIntelligenceValueCatalog newBIFieldValueCatalog = new BusinessIntelligenceValueCatalog(e.ID, e.BIFieldID, e.Name, e.Value, e.TriggersChildVisualization, e.Order, foundBIFieldValueCatalog.BasicInfoID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD_VALUE_CATALOG.UPDATED);
									newBIFieldValueCatalog.Update();
								}
							}
						}
					});

				return Results.BusinessIntelligenceField.UpdateBIFieldValueCatalogList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/

        public Results.BusinessIntelligenceField.UpdateBIFieldChildList.CODE UpdateBIFieldChildList(List<BusinessIntelligenceField> biFieldChildList, int creationUserID)
        {
            try
            {
                if (biFieldChildList == null) biFieldChildList = new List<BusinessIntelligenceField>();

				this.ChildList = this.ChildList ?? new List<BusinessIntelligenceField>();

                //Delete old ones
                this.ChildList
                    .ForEach(e => {
                        if (!biFieldChildList
                            .Where(w =>
                                w.ID != null &&
                                w.ID > 0)
                            .Select(s => s.ID)
                            .Contains(e.ID))
                            e.DeleteByID();
                    });

                //Create new ones
                List<BusinessIntelligenceField> insertedBIFieldChildList = new List<BusinessIntelligenceField>();

                foreach (BusinessIntelligenceField businessIntelligenceField in biFieldChildList)
                {
                    if (!this.ChildList.Select(e => e.ID).Contains(businessIntelligenceField.ID))
                    {
						int? currentParentID = 
							businessIntelligenceField.ParentBIFieldID == null || businessIntelligenceField.ParentBIFieldID == 0 
								? null 
								: businessIntelligenceField.ParentBIFieldID;
						int? currentParentGhostID = businessIntelligenceField.ParentBIFieldGhostID;

						int? finalParentID = null;

						if (currentParentID != null && currentParentID > 0)
							finalParentID = currentParentID;
						else 
						if (insertedBIFieldChildList.Where(e => e.BIFieldGhostID == currentParentGhostID).Count() > 0)
							finalParentID =
								insertedBIFieldChildList
									.Where(e => e.BIFieldGhostID == currentParentGhostID)
									.FirstOrDefault()
									.ID;
                        else
                        if (this.ID != null && this.ID > 0)
                            finalParentID = this.ID;

                        BusinessIntelligenceField newBusinessIntelligenceField = new BusinessIntelligenceField(
                            businessIntelligenceField.Name,
                            businessIntelligenceField.Description,
							finalParentID,
                            businessIntelligenceField.HasForcedComment,
                            creationUserID,
                            (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.CREATED);

                        /*BusinessIntelligenceField newBusinessIntelligenceField = new BusinessIntelligenceField(
                            businessIntelligenceField.Name,
                            businessIntelligenceField.Description,
                            businessIntelligenceField.ParentBIFieldGhostID > 0
                                ? insertedBIFieldChildList
                                    .Where(e => e.BIFieldGhostID == businessIntelligenceField.ParentBIFieldGhostID)
                                    .Select(e => e.ID)
                                    .FirstOrDefault()
                                : new int?(),
                            businessIntelligenceField.HasForcedComment,
                            creationUserID,
                            (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.CREATED);*/

                        int result = newBusinessIntelligenceField.Insert();

                        if (result > 0)
                        {
                            businessIntelligenceField.ID = result;

                            insertedBIFieldChildList.Add(businessIntelligenceField);

                            newBusinessIntelligenceField.SetDataByID();

							/*newBusinessIntelligenceField.UpdateBIFieldChildList(
								biFieldChildList
									.Where(e =>
										e.ParentBIFieldID == businessIntelligenceField.ID || 
										e.ParentBIFieldGhostID == businessIntelligenceField.BIFieldGhostID)
									.ToList(), 
								creationUserID);*/
                        }
                    }
                }

                //Update existing ones
                List<BusinessIntelligenceField> updatedBIFieldChildList = new List<BusinessIntelligenceField>();

				//Add inserted ones
				updatedBIFieldChildList.AddRange(insertedBIFieldChildList);

                foreach (BusinessIntelligenceField businessIntelligenceField in biFieldChildList)
                {
                    if (this.ChildList.Select(e => e.ID).Contains(businessIntelligenceField.ID))
                    {
                        int currentBasicInfoID = 0;
                        int? currentParentID =
                            businessIntelligenceField.ParentBIFieldID == null || businessIntelligenceField.ParentBIFieldID == 0
                                ? null
                                : businessIntelligenceField.ParentBIFieldID;
                        int? currentParentGhostID = businessIntelligenceField.ParentBIFieldGhostID;

                        int? finalParentID = null;

                        if (currentParentID != null && currentParentID > 0)
                            finalParentID = currentParentID;
                        else
                        if (updatedBIFieldChildList.Where(e => e.BIFieldGhostID == currentParentGhostID).Count() > 0)
                            finalParentID =
                                updatedBIFieldChildList
                                    .Where(e => e.BIFieldGhostID == currentParentGhostID)
                                    .FirstOrDefault()
                                    .ID;
                        else
                        if (this.ID != null && this.ID > 0)
                            finalParentID = this.ID;

                        using (BusinessIntelligenceField auxBusinessIntelligenceField = new BusinessIntelligenceField(businessIntelligenceField.ID))
                        {
                            auxBusinessIntelligenceField.SetDataByID();
                            currentBasicInfoID = auxBusinessIntelligenceField.BasicInfoID;
                        }

                        BusinessIntelligenceField newBusinessIntelligenceField = new BusinessIntelligenceField(
                            businessIntelligenceField.ID,
                            businessIntelligenceField.Name,
                            businessIntelligenceField.Description,
                            currentParentID ??
                                updatedBIFieldChildList
                                    .Where(e => e.BIFieldGhostID == currentParentGhostID)
                                    .FirstOrDefault()
                                    .ID,
                            businessIntelligenceField.HasForcedComment,
                            businessIntelligenceField.Order,
                            currentBasicInfoID,
                            creationUserID,
                            (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.UPDATED);

                        int result = newBusinessIntelligenceField.Update();

                        if (result > 0)
                        {
                            updatedBIFieldChildList.Add(businessIntelligenceField);

                            newBusinessIntelligenceField.SetDataByID();

                            /*newBusinessIntelligenceField.UpdateBIFieldChildList(
                                biFieldChildList
                                    .Where(e =>
                                        e.ParentBIFieldID == businessIntelligenceField.ID ||
                                        e.ParentBIFieldGhostID == businessIntelligenceField.BIFieldGhostID)
                                    .ToList(),
                                creationUserID);*/
                        }
                    }
                }

                return Results.BusinessIntelligenceField.UpdateBIFieldChildList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public void Dispose()
		{
		}
	}
}