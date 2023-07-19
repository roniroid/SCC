using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class CustomControl : IDisposable
	{
		public int ID { get; set; }
		public string Label { get; set; } = string.Empty;
		public int ModuleID { get; set; }
		public bool IsRequired { get; set; } = false;
		public string Description { get; set; } = string.Empty;
		public int ControlTypeID { get; set; }
		public string CssClass { get; set; } = string.Empty;
		public string Mask { get; set; } = string.Empty;
		public string Pattern { get; set; } = string.Empty;
		public string DefaultValue { get; set; } = string.Empty;
		public int NumberOfRows { get; set; } = 2;
		public int NumberOfColumns { get; set; } = 10;
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<CustomControlValueCatalog> ValueList { get; set; } = new List<CustomControlValueCatalog>();
		public string ModuleName { get; set; }
		public string ControlTypeName { get; set; }

		public CustomControl()
		{
		}

		//For SelectByID and DeleteByID
		public CustomControl(int id)
		{
			this.ID = id;
		}

		//For SelectByLabel
		public CustomControl(string label)
		{
			this.Label = label;
		}

		//For Insert
		public CustomControl(string label, int moduleID, bool isRequired, string description, int controlTypeID, string cssClass, string mask, string pattern, string defaultValue, int numberOfRows, int numberOfColumns, int creationUserID, int statusID)
		{
			this.Label = label;
			this.ModuleID = moduleID;
			this.IsRequired = isRequired;
			this.Description = description;
			this.ControlTypeID = controlTypeID;
			this.CssClass = cssClass;
			this.Mask = mask;
			this.Pattern = pattern;
			this.DefaultValue = defaultValue;
			this.NumberOfRows = numberOfRows;
			this.NumberOfColumns = numberOfColumns;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public CustomControl(int id, string label, int moduleID, bool isRequired, string description, int controlTypeID, string cssClass, string mask, string pattern, string defaultValue, int numberOfRows, int numberOfColumns, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Label = label;
			this.ModuleID = moduleID;
			this.IsRequired = isRequired;
			this.Description = description;
			this.ControlTypeID = controlTypeID;
			this.CssClass = cssClass;
			this.Mask = mask;
			this.Pattern = pattern;
			this.DefaultValue = defaultValue;
			this.NumberOfRows = numberOfRows;
			this.NumberOfColumns = numberOfColumns;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public CustomControl(int id, string label, int moduleID, bool isRequired, string description, int controlTypeID, string cssClass, string mask, string pattern, string defaultValue, int numberOfRows, int numberOfColumns, int basicInfoID)
		{
			this.ID = id;
			this.Label = label;
			this.ModuleID = moduleID;
			this.IsRequired = isRequired;
			this.Description = description;
			this.ControlTypeID = controlTypeID;
			this.CssClass = cssClass;
			this.Mask = mask;
			this.Pattern = pattern;
			this.DefaultValue = defaultValue;
			this.NumberOfRows = numberOfRows;
			this.NumberOfColumns = numberOfColumns;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.CustomControl repoCustomControl = new SCC_DATA.Repositories.CustomControl())
			{
				DataRow dr = repoCustomControl.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Label = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.LABEL]);
				this.ModuleID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.MODULEID]);
				this.IsRequired = Convert.ToBoolean(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.ISREQUIRED]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
				this.ControlTypeID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.CONTROLTYPEID]);
				this.CssClass = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.CSSCLASS]);
				this.Mask = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.MASK]);
				this.Pattern = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.PATTERN]);
				this.DefaultValue = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.DEFAULTVALUE]);
				this.NumberOfRows = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.NUMBEROFROWS]);
				this.NumberOfColumns = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.NUMBEROFCOLUMNS]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				SetValueList();
				SetModuleName();
				SetControlTypeName();
			}
		}

		public void SetDataByLabel()
		{
			using (SCC_DATA.Repositories.CustomControl repoCustomControl = new SCC_DATA.Repositories.CustomControl())
			{
				DataRow dr = repoCustomControl.SelectByLabel(this.Label);

				if (dr == null)
				{
					this.ID = -1;
					return;
				}

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.ID]);
				this.Label = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.LABEL]);
				this.ModuleID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.MODULEID]);
				this.IsRequired = Convert.ToBoolean(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.ISREQUIRED]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.DESCRIPTION]);
				this.ControlTypeID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.CONTROLTYPEID]);
				this.CssClass = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.CSSCLASS]);
				this.Mask = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.MASK]);
				this.Pattern = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.PATTERN]);
				this.DefaultValue = Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.DEFAULTVALUE]);
				this.NumberOfRows = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.NUMBEROFROWS]);
				this.NumberOfColumns = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.NUMBEROFCOLUMNS]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectByLabel.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				SetValueList();
                SetModuleName();
                SetControlTypeName();
            }
		}

		public void SetValueList()
		{
			this.ValueList =
				CustomControlValueCatalog.CustomControlValueCatalogWithCustomControlID(this.ID).SelectByCustomControlID()
					.Where(e =>
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL_VALUE_CATALOG.DELETED &&
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL_VALUE_CATALOG.DISABLED)
					.ToList();
		}

		public List<CustomControl> SelectAll()
		{
			List<CustomControl> customControlList = new List<CustomControl>();

			using (SCC_DATA.Repositories.CustomControl repoCustomControl = new SCC_DATA.Repositories.CustomControl())
			{
				DataTable dt = repoCustomControl.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					CustomControl customControl = new CustomControl(
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.LABEL]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.MODULEID]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.ISREQUIRED]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.CONTROLTYPEID]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.CSSCLASS]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.MASK]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.PATTERN]),
						Convert.ToString(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.DEFAULTVALUE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.NUMBEROFROWS]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.NUMBEROFCOLUMNS]),
						Convert.ToInt32(dr[SCC_DATA.Queries.CustomControl.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					customControl.BasicInfo = new BasicInfo(customControl.BasicInfoID);
					customControl.BasicInfo.SetDataByID();

					customControl.SetValueList();
                    customControl.SetModuleName();
                    customControl.SetControlTypeName();

                    customControlList.Add(customControl);
				}
			}

			return customControlList;
		}

		void SetModuleName() {
			using (Catalog catalog = new Catalog(this.ModuleID))
			{
				catalog.SetDataByID();
				this.ModuleName = catalog.Description;
			}
		}

		void SetControlTypeName() {
			using (Catalog catalog = new Catalog(this.ControlTypeID))
			{
				catalog.SetDataByID();
				this.ControlTypeName = catalog.Description;
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.CustomControl repoCustomControl = new SCC_DATA.Repositories.CustomControl())
			{
				int response = repoCustomControl.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.CustomControl repoCustomControl = new SCC_DATA.Repositories.CustomControl())
			{
				this.ID = repoCustomControl.Insert(this.Label, this.ModuleID, this.IsRequired, this.Description, this.ControlTypeID, this.CssClass, this.Mask, this.Pattern, this.DefaultValue, this.NumberOfRows, this.NumberOfColumns, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.CustomControl repoCustomControl = new SCC_DATA.Repositories.CustomControl())
			{
				return repoCustomControl.Update(this.ID, this.Label, this.ModuleID, this.IsRequired, this.Description, this.ControlTypeID, this.CssClass, this.Mask, this.Pattern, this.DefaultValue, this.NumberOfRows, this.NumberOfColumns);
			}
		}

		public Results.CustomControl.UpdateCustomControlValueCatalogList.CODE UpdateCustomControlValueCatalogList(List<CustomControlValueCatalog> customControlValueCatalogList, int creationUserID)
		{
			try
			{
				if (customControlValueCatalogList == null) customControlValueCatalogList = new List<CustomControlValueCatalog>();

				//Delete old ones
				this.ValueList
					.ForEach(e => {
						if (!customControlValueCatalogList.Select(s => s.ID).Contains(e.ID))
						{
                            try
							{
								e.DeleteByID();
							}
                            catch (Exception ex)
                            {
								e.BasicInfo.ModificationUserID = creationUserID;
								e.BasicInfo.StatusID = (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL_VALUE_CATALOG.DELETED;

								e.BasicInfo.Update();
                            }
						}
					});

				//Update and create new ones
				customControlValueCatalogList
					.ForEach(e =>
					{
                        if (e.ID == null || e.ID <= 0)
                        {
							using (CustomControlValueCatalog customControlValueCatalog = new CustomControlValueCatalog(this.ID, e.Name, e.Value, e.IsDefaultValue, e.Order, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL_VALUE_CATALOG.CREATED))
                            {
								customControlValueCatalog.Insert();
							}
                        }
                        else
						{
                            using (CustomControlValueCatalog foundCustomControlValueCatalog = new CustomControlValueCatalog(e.ID))
							{
								foundCustomControlValueCatalog.SetDataByID();

								if (
									!e.Name.Equals(foundCustomControlValueCatalog.Name) ||
									!e.Value.Equals(foundCustomControlValueCatalog.Value) ||
									!e.IsDefaultValue.Equals(foundCustomControlValueCatalog.IsDefaultValue) ||
									!e.Order.Equals(foundCustomControlValueCatalog.Order)
								)
								{
									CustomControlValueCatalog newCustomControlValueCatalog = new CustomControlValueCatalog(e.ID, e.CustomControlID, e.Name, e.Value, e.IsDefaultValue, e.Order, foundCustomControlValueCatalog.BasicInfoID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CUSTOM_CONTROL_VALUE_CATALOG.UPDATED);
									newCustomControlValueCatalog.Update();
								}
							}
						}
					});

				return Results.CustomControl.UpdateCustomControlValueCatalogList.CODE.SUCCESS;
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