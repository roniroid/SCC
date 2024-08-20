using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using static SCC_BL.Settings.AppValues.ViewData.Transaction.FormView.CustomControlList.CallID;

namespace SCC_BL
{
	public class Form : IDisposable
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int TypeID { get; set; }
		public string Comment { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<CustomField> CustomFieldList { get; set; } = new List<CustomField>();
		public List<FormBIFieldCatalog> FormBIFieldCatalogList { get; set; } = new List<FormBIFieldCatalog>();
		public List<Attribute> AttributeList { get; set; } = new List<Attribute>();
		public List<ProgramFormCatalog> ProgramFormCatalogList { get; set; } = new List<ProgramFormCatalog>();
		//----------------------------------------------------
		public List<CustomControl> CustomControlList { get; set; } = new List<CustomControl>();
		public List<BusinessIntelligenceField> BusinessIntelligenceFieldList { get; set; } = new List<BusinessIntelligenceField>();
		public string ProgramName { get; set; }


        public Form()
		{
		}

		//For SelectByID and DeleteByID
		public Form(int id)
		{
			this.ID = id;
		}

		//For SelectByName
		public Form(string name)
		{
			this.Name = name;
		}

		//For Insert
		public Form(string name, int typeID, string comment, int creationUserID, int statusID)
		{
			this.Name = name;
			this.TypeID = typeID;
			this.Comment = comment;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Form(int id, string name, int typeID, string comment, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Name = name;
			this.TypeID = typeID;
			this.Comment = comment;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public Form(int id, string name, int typeID, string comment, int basicInfoID)
		{
			this.ID = id;
			this.Name = name;
			this.TypeID = typeID;
			this.Comment = comment;
			this.BasicInfoID = basicInfoID;
		}

		public SCC_BL.CustomTools.FormUploadInfo GetFormUploadInfo(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rowCollection)
		{
			SCC_BL.CustomTools.FormUploadInfo formUploadInfo = new CustomTools.FormUploadInfo();

			//formUploadInfo.FillErrorTypeInfo(rowCollection);

			return formUploadInfo;
		}

		void SetProgramName()
        {
			if (this.ProgramFormCatalogList.Count > 0)
            {
                using (Program program = new Program(this.ProgramFormCatalogList.LastOrDefault().ProgramID))
                {
                    program.SetDataByID();
                    this.ProgramName = program.Name;
                }
            }
        }

		void SetProgramFormCatalogList()
        {
			this.ProgramFormCatalogList = new List<ProgramFormCatalog>();
            this.ProgramFormCatalogList = ProgramFormCatalog.ProgramFormCatalogWithFormID(this.ID).SelectByFormID();
        }

		void SetCustomFieldList()
        {
            this.CustomFieldList = 
				CustomField.CustomFieldWithFormID(this.ID)
					.SelectByFormID()
					.OrderBy(e => e.Order)
					.ToList();
        }

        public int? GetProgramID()
        {
            if (this.ProgramFormCatalogList.Count > 0)
            {
                return this.ProgramFormCatalogList.LastOrDefault().ProgramID;
            }

            return null;
        }

        public void SetCustomControlList(bool includeDeleted = false)
        {
			this.CustomControlList = new List<CustomControl>();

            foreach (CustomField customField in this.CustomFieldList)
            {
                using (CustomControl customControl = new CustomControl(customField.CustomControlID))
                {
					customControl.SetDataByID();

					if (
						(customControl.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DELETED ||
                        customControl.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL.DISABLED) &&
                        !includeDeleted
                    )
					{
						continue;
					}

					this.CustomControlList.Add(customControl);
                }
            }
		}

		void SetBusinessIntelligenceFieldList()
        {
            this.BusinessIntelligenceFieldList = new List<BusinessIntelligenceField>();

            using (BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField())
                this.BusinessIntelligenceFieldList = businessIntelligenceField.SelectHierarchyByFormID(this.ID, true);

			this.BusinessIntelligenceFieldList =
				this.BusinessIntelligenceFieldList
					.Where(e =>
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DELETED &&
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DISABLED)
					.ToList();

            /*foreach (FormBIFieldCatalog formBIFieldCatalog in this.FormBIFieldCatalogList)
            {
                using (BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField(formBIFieldCatalog.BIFieldID))
                {
					businessIntelligenceField.SetDataByID();

                    if (
                        businessIntelligenceField.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DELETED ||
                        businessIntelligenceField.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_BI_FIELD.DISABLED
                    )
                    {
                        continue;
                    }

                    this.BusinessIntelligenceFieldList.Add(businessIntelligenceField);
                }
            }*/
		}

		void SetBIFieldList()
        {
            this.FormBIFieldCatalogList = 
				FormBIFieldCatalog.FormBIFieldCatalogWithFormID(this.ID)
					.SelectByFormID()
					.OrderBy(e => e.Order)
					.ToList();
        }

		public void SetDataByID(bool simpleData = false)
		{
			using (SCC_DATA.Repositories.Form repoForm = new SCC_DATA.Repositories.Form())
			{
				DataRow dr = repoForm.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByID.ResultFields.TYPEID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByID.ResultFields.COMMENT]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				if (!simpleData)
				{
					this.SetCustomFieldList();
					this.SetBIFieldList();
                    this.SetAttributeList(simpleData);
                    this.SetProgramFormCatalogList();

					this.SetProgramName();
					this.SetCustomControlList();
					this.SetBusinessIntelligenceFieldList();
				}
			}
		}

		/*void SetAttributeList()
        {
			//this.AttributeList = Attribute.AttributeWithFormID(this.ID).SelectByFormID();

			this.AttributeList = new List<Attribute>();

            int[] attributeIDArray = new int[0];

            using (Attribute attribute = Attribute.AttributeWithFormID(this.ID))
                attributeIDArray = attribute.SelectIDArrayByFormID();

			Parallel.ForEach(attributeIDArray, async (e) => {
				using (Attribute attribute = new Attribute(e))
				{
					attribute.SetDataByID();
                    this.AttributeList.Add(attribute);
				}
			});
        }*/

        void SetAttributeList(bool simpleData = false)
        {
            if (!simpleData)
            {
                this.AttributeList = new List<Attribute>();

                using (Attribute attribute = Attribute.AttributeWithFormID(this.ID))
                    this.AttributeList = attribute.SelectHierarchyByFormID(simpleData);

                this.AttributeList =
                    this.AttributeList
                        .Where(e => e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.DELETED)
                        .OrderBy(e => e.Order)
                        .ToList();
            }
        }

        /*void SetAttributeList()
        {
            //this.AttributeList = Attribute.AttributeWithFormID(this.ID).SelectByFormID();

            this.AttributeList = new List<Attribute>();

            int[] attributeIDArray = new int[0];

            using (Attribute attribute = Attribute.AttributeWithFormID(this.ID))
                attributeIDArray = attribute.SelectIDArrayByFormID();

            // Create a list to store the tasks
            List<Task> tasks = new List<Task>();

            foreach (int e in attributeIDArray)
            {
                // Start a new task for each iteration
                Task task = Task.Run(async () =>
                {
                    using (Attribute attribute = new Attribute(e))
                    {
                        attribute.SetDataByID();
                        lock (this.AttributeList)
                        {
                            this.AttributeList.Add(attribute);
                        }
                    }
                });

                tasks.Add(task);
            }

            // Wait for all the tasks to complete
            Task.WaitAll(tasks.ToArray());

			this.AttributeList =
				this.AttributeList
					.OrderBy(e => e.Order)
					.ToList();
        }*/

        public void SetDataByName(bool simpleData = false)
		{
			using (SCC_DATA.Repositories.Form repoForm = new SCC_DATA.Repositories.Form())
			{
				DataRow dr = repoForm.SelectByName(this.Name);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByName.ResultFields.ID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByName.ResultFields.NAME]);
				this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByName.ResultFields.TYPEID]);
				this.Comment = Convert.ToString(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByName.ResultFields.COMMENT]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectByName.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				if (!simpleData)
                {
                    this.SetCustomFieldList();
                    this.SetBIFieldList();
                    this.SetAttributeList();
                    this.SetProgramFormCatalogList();

                    this.SetProgramName();
                    this.SetCustomControlList();
					this.SetBusinessIntelligenceFieldList();
				}
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Form repoForm = new SCC_DATA.Repositories.Form())
			{
				int response = repoForm.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Form repoForm = new SCC_DATA.Repositories.Form())
			{
				this.ID = repoForm.Insert(this.Name, this.TypeID, this.Comment, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Form repoForm = new SCC_DATA.Repositories.Form())
			{
				return repoForm.Update(this.ID, this.Name, this.TypeID, this.Comment);
			}
		}

		public List<Form> SelectAll(bool simpleData = false)
		{
			List<Form> formList = new List<Form>();

			using (SCC_DATA.Repositories.Form repoForm = new SCC_DATA.Repositories.Form())
			{
				DataTable dt = repoForm.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					Form form = new Form(
						Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectAll.ResultFields.NAME]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectAll.ResultFields.TYPEID]),
						Convert.ToString(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectAll.ResultFields.COMMENT]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Form.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					form.BasicInfo = new BasicInfo(form.BasicInfoID);
					form.BasicInfo.SetDataByID();

                    if (!simpleData)
                    {
                        form.SetCustomFieldList();
                        form.SetBIFieldList();
                        form.SetAttributeList();
						form.SetProgramFormCatalogList();

                        form.SetProgramName();
                        form.SetCustomControlList();
						form.SetBusinessIntelligenceFieldList();
					}

					formList.Add(form);
				}
			}

			return formList
				.OrderBy(o => o.Name)
				.ToList();
		}

		//OLD
		/*public Results.Form.UpdateCustomFieldList.CODE UpdateCustomFieldList(int[] customControlIDList, int creationUserID)
		{
			try
			{
				if (customControlIDList == null) customControlIDList = new int[0];

				//Delete old ones
				this.CustomFieldList
					.ForEach(e => {
						if (!customControlIDList.Contains(e.CustomControlID))
                            try
                            {
                                e.DeleteByID();
                            }
                            catch (Exception ex)
                            {
                                e.BasicInfo.ModificationUserID = creationUserID;
                                e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.DELETED;
                                e.BasicInfo.Update();
                            }
                    });

                //Create new ones
                int cont = 1;

                foreach (int customControlID in customControlIDList)
				{
					if (!this.CustomFieldList.Select(e => e.CustomControlID).Contains(customControlID))
					{
						CustomField customField = CustomField.CustomFieldForInsert(this.ID, customControlID, cont, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.CREATED);
						customField.Insert();
					}

					cont++;
                }

                //Activate old and deleted ones

                this.CustomFieldList
                    .Where(e =>
                        e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.DISABLED ||
                        e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.DELETED)
                    .ToList()
                    .ForEach(e => {
                        if (customControlIDList.Contains(e.CustomControlID))
                        {
                            CustomField customField = new CustomField(e.ID, this.ID, e.CustomControlID, cont, e.BasicInfoID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.UPDATED);
                            customField.Update();

                            cont++;
                        }
                    });

                return Results.Form.UpdateCustomFieldList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/

		public Results.Form.UpdateCustomFieldList.CODE UpdateCustomFieldList(int[] customControlIDList, int creationUserID)
		{
			try
			{
				if (customControlIDList == null) customControlIDList = new int[0];

				//Delete old ones
				this.CustomFieldList
					.ForEach(e => {
						if (!customControlIDList.Contains(e.CustomControlID))
                            try
                            {
                                e.DeleteByID();
                            }
                            catch (Exception ex)
                            {
                                e.BasicInfo.ModificationUserID = creationUserID;
                                e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.DELETED;
                                e.BasicInfo.Update();
                            }
                    });

                int cont = 1;

                foreach (int customControlID in customControlIDList)
				{
                    //Create if it does not exist
                    if (!this.CustomFieldList.Select(e => e.CustomControlID).Contains(customControlID))
                    {
                        CustomField customField = 
							CustomField.CustomFieldForInsert(
								this.ID, 
								customControlID, 
								cont, 
								creationUserID, 
								(int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.CREATED);

                        customField.Insert();
                    }
					else
                    //Update if it does exist
                    if (this.CustomFieldList.Select(e => e.CustomControlID).Contains(customControlID))
                    {
						CustomField currentCustomField = 
							this.CustomFieldList
								.Where(e => 
									e.CustomControlID == customControlID)
								.FirstOrDefault();

                        CustomField customField = 
							new CustomField(
								currentCustomField.ID, 
								this.ID, 
								customControlID, 
								cont, 
								currentCustomField.BasicInfoID, 
								creationUserID, 
								(int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.CREATED);

                        customField.Update();
                    }

                    cont++;
                }

                //Activate old and deleted ones
                this.CustomFieldList
                    .Where(e =>
                        e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.DISABLED ||
                        e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.DELETED)
                    .ToList()
                    .ForEach(e => {
                        if (customControlIDList.Contains(e.CustomControlID))
                        {
                            CustomField customField = 
								new CustomField(
									e.ID, 
									this.ID, 
									e.CustomControlID, 
									cont, 
									e.BasicInfoID, 
									creationUserID, 
									(int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_FIELD.UPDATED);

                            customField.Update();

                            cont++;
                        }
                    });

                return Results.Form.UpdateCustomFieldList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Form.UpdateProgramFormCatalogList.CODE UpdateProgramFormCatalogList(List<ProgramFormCatalog> programFormCatalogList, int creationUserID)
		{
			try
			{
				if (programFormCatalogList == null) programFormCatalogList = new List<ProgramFormCatalog>();

				//Delete old ones
				this.ProgramFormCatalogList
					.ForEach(e => {
						if (programFormCatalogList
							.Where(w => 
								w.FormID == e.FormID &&
                                w.ProgramID == e.ProgramID)
							.Count() == 0)
							try
                            {
                                e.Delete();
                            }
							catch (Exception ex)
                            {
                                e.BasicInfo.ModificationUserID = creationUserID;
                                e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.DELETED;
                                e.BasicInfo.Update();
                            }
					});

				//Update existing ones
				foreach (ProgramFormCatalog programFormCatalog in programFormCatalogList.Where(e => this.ProgramFormCatalogList.Select(s => s.ID).Contains(e.ID)))
                {
                    int currentBasicInfoID = 0;

                    using (ProgramFormCatalog auxProgramFormCatalog = new ProgramFormCatalog(programFormCatalog.ID))
                    {
                        auxProgramFormCatalog.SetDataByID();
                        currentBasicInfoID = auxProgramFormCatalog.BasicInfoID;
                    }

                    ProgramFormCatalog newProgramFormCatalog = new ProgramFormCatalog(
                        programFormCatalog.ID,
                        programFormCatalog.ProgramID,
                        this.ID,
                        programFormCatalog.StartDate,
                        currentBasicInfoID,
                        creationUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.UPDATED);

                    int result = newProgramFormCatalog.Update();
                }

				//Create new ones
				foreach (ProgramFormCatalog programFormCatalog in programFormCatalogList.Where(e => !this.ProgramFormCatalogList.Select(s => s.ID).Contains(e.ID)))
                {
                    ProgramFormCatalog newProgramFormCatalog = new ProgramFormCatalog(
                        programFormCatalog.ProgramID,
                        this.ID,
                        programFormCatalog.StartDate,
                        creationUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM_FORM_CATALOG.CREATED);

                    int result = newProgramFormCatalog.Insert();
                }

				return Results.Form.UpdateProgramFormCatalogList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Form.UpdateAttributeList.CODE UpdateAttributeList(List<Attribute> attributeList, List<AttributeValueCatalog> attributeValueCatalogList, int creationUserID)
		{
			try
			{
				if (attributeList == null) attributeList = new List<Attribute>();

				//Delete old ones
				this.AttributeList
					.ForEach(e => {
                        if (!attributeList
							.Where(w => 
								w.ID != null && 
								w.ID > 0)
							.Select(s => s.ID)
							.Contains(e.ID))
						{
                            try
                            {
                                e.DeleteByID();
                            }
                            catch (Exception ex)
                            {
                                e.BasicInfo.ModificationUserID = creationUserID;
                                e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.DELETED;
                                e.BasicInfo.Update();
                            }
                        }
					});

				//Update existing ones
				List<Attribute> updatedAttributeList = new List<Attribute>();

				foreach (Attribute attribute in attributeList.Where(e => e.HasChanged))
                {
                    if (this.AttributeList.Select(e => e.ID).Contains(attribute.ID))
                    {
                        int currentBasicInfoID = 0;

						using (Attribute auxAttribute = new Attribute(attribute.ID))
						{
							auxAttribute.SetDataByID();
							currentBasicInfoID = auxAttribute.BasicInfoID;
                        }

                        /*int parentID = 0;

                        if (attribute.ParentAttributeGhostID >= SCC_BL.Settings.Overall.MIN_EXISTING_ATTRIBUTE_GHOST_ID)
                        {
                            parentID = attributeList.Where(e => e.AttributeGhostID == attribute.ParentAttributeGhostID).First().ID;
                        }
                        else
                        if (attribute.ParentAttributeGhostID >= SCC_BL.Settings.Overall.MIN_NON_EXISTING_ATTRIBUTE_GHOST_ID)
                        {
                            parentID = updatedAttributeList.Where(e => e.AttributeGhostID == attribute.ParentAttributeGhostID).First().ID;
                        }*/

                        Attribute newAttribute = new Attribute(
							attribute.ID,
							this.ID, 
							attribute.Name, 
							attribute.Description, 
							attribute.ErrorTypeID, 
							attribute.ParentAttributeID,
							attribute.MaxScore,
							attribute.TopDownScore,
							attribute.HasForcedComment,
							attribute.IsKnown,
							attribute.IsControllable,
							attribute.IsScorable,
							attribute.Order,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.UPDATED);


                        int result = newAttribute.Update();

                        if (result > 0)
						{
							updatedAttributeList.Add(attribute);

							newAttribute.SetDataByID();

							newAttribute.UpdateAttributeValueCatalogList(
								attributeValueCatalogList
									.Where(e => e.AttributeID == attribute.ID)
									.OrderBy(e => e.Order)
									.ToList(),
								creationUserID);
						}
					}
				}

				//Create new ones
				List<Attribute> insertedAttributeList = new List<Attribute>();

				foreach (Attribute attribute in attributeList/*.Where(e => e.HasChanged)*/)
				{
					if (!this.AttributeList.Select(e => e.ID).Contains(attribute.ID))
                    {
						int? parentID = attribute.ParentAttributeID;

						if (attribute.ParentAttributeGhostID >= SCC_BL.Settings.Overall.MIN_EXISTING_ATTRIBUTE_GHOST_ID)
						{
							parentID = attributeList.Where(e => e.AttributeGhostID == attribute.ParentAttributeGhostID).First().ID;
						}
                        else
                        if (attribute.ParentAttributeGhostID >= SCC_BL.Settings.Overall.MIN_NON_EXISTING_ATTRIBUTE_GHOST_ID)
                        {
                            parentID = insertedAttributeList.Where(e => e.AttributeGhostID == attribute.ParentAttributeGhostID).First().ID;
                        }

                        parentID = parentID == 0 ? null : parentID;

                        Attribute newAttribute = new Attribute(
							this.ID, 
							attribute.Name, 
							attribute.Description, 
							attribute.ErrorTypeID,
                            parentID,
							attribute.MaxScore,
							attribute.TopDownScore,
							attribute.HasForcedComment,
							attribute.IsKnown,
							attribute.IsControllable,
							attribute.IsScorable,
							attribute.Order,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.CREATED);

                        int result = newAttribute.Insert();

                        if (result > 0)
						{
							attribute.ID = result;

							insertedAttributeList.Add(attribute);

							newAttribute.SetDataByID();

							newAttribute.UpdateAttributeValueCatalogList(
								attributeValueCatalogList
									.Where(e => e.AttributeGhostID == attribute.AttributeGhostID)
									.OrderBy(e => e.Order)
									.ToList(),
								creationUserID);
						}
					}
				}

				return Results.Form.UpdateAttributeList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Results.Form.UpdateBIFieldList.CODE UpdateBIFieldList(int[] biFieldIDList, int creationUserID)
		{
			try
			{
				if (biFieldIDList == null) biFieldIDList = new int[0];

				//Delete old ones
				foreach (FormBIFieldCatalog formBIFieldCatalog in this.FormBIFieldCatalogList)
                {
                    if (!biFieldIDList.Contains(formBIFieldCatalog.BIFieldID))
                    {
                        BusinessIntelligenceField auxBusinessIntelligenceField = new BusinessIntelligenceField(formBIFieldCatalog.BIFieldID);
                        auxBusinessIntelligenceField.SetDataByID();

						if (auxBusinessIntelligenceField.ParentBIFieldID != null && auxBusinessIntelligenceField.ParentBIFieldID != 0)
							continue;

                        try
                        {
                            formBIFieldCatalog.DeleteByID();

							foreach (BusinessIntelligenceField childBusinessIntelligenceField in auxBusinessIntelligenceField.ChildList)
                            {
								FormBIFieldCatalog childFormBIFieldCatalog = 
									this.FormBIFieldCatalogList
										.Where(e => 
											e.BIFieldID == childBusinessIntelligenceField.ID)
										.FirstOrDefault();

								if (childFormBIFieldCatalog == null) continue;

                                try
                                {
                                    childFormBIFieldCatalog.DeleteByID();
                                }
                                catch (Exception ex)
                                {
                                    childFormBIFieldCatalog.BasicInfo.ModificationUserID = creationUserID;
                                    childFormBIFieldCatalog.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.DELETED;
                                    childFormBIFieldCatalog.BasicInfo.Update();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            formBIFieldCatalog.BasicInfo.ModificationUserID = creationUserID;
                            formBIFieldCatalog.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.DELETED;
                            formBIFieldCatalog.BasicInfo.Update();
                        }
                    }
                }

                int count = Convert.ToInt32($"{ this.ID }1");

				foreach (int biFieldID in biFieldIDList)
                {
                    //Create new ones
                    if (!this.FormBIFieldCatalogList.Select(e => e.BIFieldID).Contains(biFieldID))
					{
						FormBIFieldCatalog biFieldCatalog = 
							FormBIFieldCatalog.FormBIFieldCatalogForInsert(
								this.ID, 
								biFieldID, 
								count, 
								creationUserID, 
								(int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.CREATED);

						biFieldCatalog.Insert();

						//Insert children
						using (BusinessIntelligenceField businessIntelligenceField = new BusinessIntelligenceField(biFieldID))
                        {
                            int actualCount = 1;
                            int childCount = Convert.ToInt32($"{count}{businessIntelligenceField.Order}");

                            businessIntelligenceField.SetDataByID();

							foreach (BusinessIntelligenceField childBIField in businessIntelligenceField.ChildList)
                            {
                                childCount = Convert.ToInt32($"{count}{childBIField.Order}");

                                FormBIFieldCatalog childBIFieldCatalog = FormBIFieldCatalog.FormBIFieldCatalogForInsert(this.ID, childBIField.ID, childCount, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.CREATED);
                                childBIFieldCatalog.Insert();

                                actualCount++;
                            }
						}
                    }
                    else
                    //Update if it does exist
                    if (this.FormBIFieldCatalogList.Select(e => e.BIFieldID).Contains(biFieldID))
                    {
                        FormBIFieldCatalog currentFormBIField =
							this.FormBIFieldCatalogList
                                .Where(e =>
                                    e.BIFieldID == biFieldID)
                                .FirstOrDefault();

                        FormBIFieldCatalog formBIField =
                            new FormBIFieldCatalog(
								currentFormBIField.ID,
                                this.ID,
                                biFieldID,
                                count,
                                currentFormBIField.BasicInfoID,
                                creationUserID,
                                (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.CREATED);

                        formBIField.Update();
                    }

                    count++;
                }

                //Activate old and deleted ones
                this.FormBIFieldCatalogList
                    .Where(e =>
                        e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.DISABLED ||
                        e.BasicInfo.StatusID == (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.DELETED)
                    .ToList()
                    .ForEach(e => {
                        if (biFieldIDList.Contains(e.BIFieldID))
                        {
                            FormBIFieldCatalog formBIFieldCatalog =
                                new FormBIFieldCatalog(
                                    e.ID,
                                    this.ID,
                                    e.BIFieldID,
                                    count,
                                    e.BasicInfoID,
                                    creationUserID,
                                    (int)SCC_BL.DBValues.Catalog.STATUS_FORM_BI_FIELD_CATALOG.UPDATED);

                            formBIFieldCatalog.Update();

                            count++;
                        }
                    });

                return Results.Form.UpdateBIFieldList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static Results.Form.CheckNCEScore.CODE CheckNCEScore(List<Attribute> attributeList)
		{
			Results.Form.CheckNCEScore.CODE result = Results.Form.CheckNCEScore.CODE.SUCCESS;

			if (attributeList.Where(e => e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE).Sum(e => e.MaxScore) > 100)
			{
				result = Results.Form.CheckNCEScore.CODE.ERROR_GREATER_THAN_100;
            }
			else if (attributeList.Where(e => e.ErrorTypeID == (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.NCE).Sum(e => e.MaxScore) < 100)
            {
                result = Results.Form.CheckNCEScore.CODE.ERROR_LESS_THAN_100;
            }

			return result;
        }

		public void Dispose()
		{
		}
    }
}