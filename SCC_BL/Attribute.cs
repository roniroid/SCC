using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class Attribute : IDisposable
	{
		public int ID { get; set; }
		public int FormID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ErrorTypeID { get; set; }
		public int? ParentAttributeID { get; set; } = null;
		public int? MaxScore { get; set; } = null;
		public bool TopDownScore { get; set; }
		public bool HasForcedComment { get; set; }
		public bool IsKnown { get; set; }
		public bool IsControllable { get; set; }
		public bool IsScorable { get; set; }
		public int Order { get; set; }
		public int BasicInfoID { get; set; }
		//------------------------------------------------------
		public BasicInfo BasicInfo { get; set; }
		public List<AttributeValueCatalog> ValueList { get; set; } = new List<AttributeValueCatalog>();
		public List<Attribute> ChildrenList { get; set; } = new List<Attribute>();
		public int AttributeGhostID { get; set; } = 0;
		public int ParentAttributeGhostID { get; set; } = 0;
		public bool HasChanged { get; set; } = false;

		public Attribute()
		{
		}

		//For and SelectByID DeleteByID
		public Attribute(int id)
		{
			this.ID = id;
		}

		public static Attribute AttributeWithFormID(int formID)
		{
			Attribute @object = new Attribute();
			@object.FormID = formID;
			return @object;
		}

		public static Attribute AttributeWithFormIDAndName(int formID, string name)
		{
			Attribute @object = new Attribute();
			@object.FormID = formID;
			@object.Name = name;
			return @object;
		}

		public static Attribute AttributeWithParentAttributeIDAndName(int parentAttributeID, string name)
		{
			Attribute @object = new Attribute();
			@object.ParentAttributeID = parentAttributeID;
			@object.Name = name;
			return @object;
		}

		public static Attribute AttributeWithParentAttributeID(int parentAttributeID)
		{
			Attribute @object = new Attribute();
			@object.ParentAttributeID = parentAttributeID;
			return @object;
		}

		//For Insert
		public Attribute(int formID, string name, string description, int errorTypeID, int? parentAttributeID, int? maxScore, bool topDownScore, bool hasForcedComment, bool isKnown, bool isControllable, bool isScorable, int order, int creationUserID, int statusID)
		{
			this.FormID = formID;
			this.Name = name;
			this.Description = description;
			this.ErrorTypeID = errorTypeID;
			this.ParentAttributeID = parentAttributeID;
			this.MaxScore = maxScore;
			this.TopDownScore = topDownScore;
			this.HasForcedComment = hasForcedComment;
			this.IsKnown = isKnown;
			this.IsControllable = isControllable;
			this.IsScorable = isScorable;
			this.Order = order;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public Attribute(int id, int formID, string name, string description, int errorTypeID, int? parentAttributeID, int? maxScore, bool topDownScore, bool hasForcedComment, bool isKnown, bool isControllable, bool isScorable, int order, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.FormID = formID;
			this.Name = name;
			this.Description = description;
			this.ErrorTypeID = errorTypeID;
			this.ParentAttributeID = parentAttributeID;
			this.MaxScore = maxScore;
			this.TopDownScore = topDownScore;
			this.HasForcedComment = hasForcedComment;
			this.IsKnown = isKnown;
			this.IsControllable = isControllable;
			this.IsScorable = isScorable;
			this.Order = order;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public Attribute(int id, int formID, string name, string description, int errorTypeID, int? parentAttributeID, int? maxScore, bool topDownScore, bool hasForcedComment, bool isKnown, bool isControllable, bool isScorable, int order, int basicInfoID)
		{
			this.ID = id;
			this.FormID = formID;
			this.Name = name;
			this.Description = description;
			this.ErrorTypeID = errorTypeID;
			this.ParentAttributeID = parentAttributeID;
			this.MaxScore = maxScore;
			this.TopDownScore = topDownScore;
			this.HasForcedComment = hasForcedComment;
			this.IsKnown = isKnown;
			this.IsControllable = isControllable;
			this.IsScorable = isScorable;
			this.Order = order;
			this.BasicInfoID = basicInfoID;
		}

		string TranslateBoolean(string word)
		{
			switch (word)
			{
				case "verdadero":
					return "true";
				case "falso":
					return "false";
				default:
					return word;
			}
		}

		public Attribute (DocumentFormat.OpenXml.Spreadsheet.Cell[] cells, SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE attributeErrorType, int formID, int attributeNameIndex, int attributeDataStartIndex, int actualGhostID, int parentAttributeGhostID, int order, int creationUserID)
        {
			this.ErrorTypeID = (int)attributeErrorType;

            using (SCC_BL.Tools.ExcelParser excelParser = new Tools.ExcelParser())
			{
				string name = excelParser.GetCellValue(cells[attributeNameIndex]).ToString().Trim();
				this.Name = name;

				string description = excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.DESCRIPTION]).ToString().Trim();
				this.Description = description;

				int maxScore =
					!string.IsNullOrEmpty(excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.MAX_SCORE]).ToString().Trim())
					? Convert.ToInt32(excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.MAX_SCORE]).ToString().Trim())
					: 0;
				this.MaxScore = maxScore;

				string topDownScoreCellValue = excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.TOP_DOWN_SCORE]).ToString().Trim().ToLower();
				topDownScoreCellValue = TranslateBoolean(topDownScoreCellValue);

                bool topDownScore =
					!string.IsNullOrEmpty(topDownScoreCellValue)
						? Convert.ToBoolean(topDownScoreCellValue)
						: false;
				this.TopDownScore = topDownScore;

				string forceCommentCellValue = excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.FORCE_COMMENT]).ToString().Trim().ToLower();
				forceCommentCellValue = TranslateBoolean(forceCommentCellValue);

				bool forceComment =
					!string.IsNullOrEmpty(forceCommentCellValue)
					? Convert.ToBoolean(forceCommentCellValue)
					: false;
				this.HasForcedComment = forceComment;

				string knownCellValue = excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.KNOWN]).ToString().Trim().ToLower();
				knownCellValue = TranslateBoolean(knownCellValue);

				bool known =
					!string.IsNullOrEmpty(knownCellValue)
					? Convert.ToBoolean(knownCellValue)
					: false;
				this.IsKnown = known;

				string controllableCellValue = excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.CONTROLLABLE]).ToString().Trim().ToLower();
				controllableCellValue = TranslateBoolean(controllableCellValue);

				bool controllable =
					!string.IsNullOrEmpty(controllableCellValue)
					? Convert.ToBoolean(controllableCellValue)
					: false;
				this.IsControllable = controllable;

				string scorableCellValue = excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.SCORABLE]).ToString().Trim().ToLower();
				scorableCellValue = TranslateBoolean(scorableCellValue);

				bool scorable =
					!string.IsNullOrEmpty(scorableCellValue)
					? Convert.ToBoolean(scorableCellValue)
					: false;
				this.IsScorable = scorable;

				string defineAnswerType =
					!string.IsNullOrEmpty(excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.DEFINE_ANSWER_TYPE]).ToString().Trim())
					? excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.DEFINE_ANSWER_TYPE]).ToString().Trim()
					: string.Empty;

				bool apply =
					!string.IsNullOrEmpty(excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.APPLY]).ToString().Trim())
					? Convert.ToBoolean(excelParser.GetCellValue(cells[attributeDataStartIndex + (int)SCC_BL.Settings.AppValues.ExcelTasks.Form.UploadForm.AttributeFields.APPLY]).ToString().Trim().ToLower())
					: false;

				int defineAnswerOrder = 1;

                if (!string.IsNullOrEmpty(defineAnswerType))
				{
					for (int i = 0; i < defineAnswerType.Split('/').Length; i++)
					{
						AttributeValueCatalog attributeValueCatalog = new AttributeValueCatalog(0, defineAnswerType.Split('/')[i], defineAnswerType.Split('/')[i], i == defineAnswerType.Split('/').Length - 1, defineAnswerOrder, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.CREATED);
						attributeValueCatalog.AttributeGhostID = actualGhostID;

						this.ValueList.Add(attributeValueCatalog);

						defineAnswerOrder++;
					}
				}
			}

			this.FormID = formID;
			this.AttributeGhostID = actualGhostID;
			this.ParentAttributeGhostID = parentAttributeGhostID;
			this.Order = order;

			this.BasicInfo = new BasicInfo(creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.CREATED);
		}

		public void SetValueList()
		{
			this.ValueList =
				AttributeValueCatalog.AttributeValueCatalogWithAttributeID(this.ID).SelectByAttributeID()
					.Where(e =>
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.DELETED &&
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.DISABLED)
					.ToList();
		}

		public void SetChildrenList()
		{
			this.ChildrenList =
				Attribute.AttributeWithParentAttributeID(this.ID).SelectByParentAttributeID()
					.Where(e =>
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.DELETED &&
						e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE.DISABLED)
					.ToList();
		}

		public List<Attribute> SelectByFormID()
		{
			List<Attribute> attributeList = new List<Attribute>();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectByFormID(this.FormID);

				foreach (DataRow dr in dt.Rows)
				{
					int?
						parentAttributeID = null,
						maxScore = null;

					bool topDownScore = false;

                    try { parentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
                    try { maxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.MAXSCORE]); } catch (Exception) { }
                    try { topDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }

					Attribute attribute = new Attribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.FORMID]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.ERRORTYPEID]),
						parentAttributeID,
						maxScore,
						topDownScore,
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.ISKNOWN]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.ISCONTROLLABLE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.ISSCORABLE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByFormID.ResultFields.BASICINFOID])
					);

					attribute.BasicInfo = new BasicInfo(attribute.BasicInfoID);
					attribute.BasicInfo.SetDataByID();

					attribute.SetValueList();
					attribute.SetChildrenList();

					attributeList.Add(attribute);
				}
			}

			return attributeList;
		}

		public List<Attribute> SelectHierarchyByFormID(bool simplifiedInfo = false)
		{
			List<Attribute> attributeList = new List<Attribute>();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectHierarchyByFormID(this.FormID);

				foreach (DataRow dr in dt.Rows)
				{
					int?
						parentAttributeID = null,
						maxScore = null;

					bool topDownScore = false;

                    try { parentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
                    try { maxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.MAXSCORE]); } catch (Exception) { }
                    try { topDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }

					Attribute attribute = new Attribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.FORMID]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.ERRORTYPEID]),
						parentAttributeID,
						maxScore,
						topDownScore,
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.ISKNOWN]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.ISCONTROLLABLE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.ISSCORABLE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectHierarchyByFormID.ResultFields.BASICINFOID])
					);

					attribute.BasicInfo = new BasicInfo(attribute.BasicInfoID);
					attribute.BasicInfo.SetDataByID();

					attribute.SetValueList();

					if (!simplifiedInfo)
                        attribute.SetChildrenList();

                    attributeList.Add(attribute);
				}
			}

			return attributeList;
		}

		public int[] SelectIDArrayByFormID()
		{
			int[] idArray = new int[0];

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectIDListByFormID(this.FormID);

                idArray = new int[dt.Rows.Count];

				for (int i = 0; i < dt.Rows.Count; i++)
				{
					idArray[i] = Convert.ToInt32(dt.Rows[i][SCC_DATA.Queries.Attribute.StoredProcedures.SelectIDListByFormID.ResultFields.ID]);
                }
			}

			return idArray;
		}

		public List<Attribute> SelectByProgramAndErrorTypeID(string programIDArray, string errorTypeIDArray)
		{
			List<Attribute> attributeList = new List<Attribute>();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectByProgramAndErrorTypeID(programIDArray, errorTypeIDArray);

				foreach (DataRow dr in dt.Rows)
				{
					int?
						parentAttributeID = null,
						maxScore = null;

					bool topDownScore = false;

                    try { parentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
                    try { maxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.MAXSCORE]); } catch (Exception) { }
                    try { topDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }

					Attribute attribute = new Attribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.FORMID]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.ERRORTYPEID]),
						parentAttributeID,
						maxScore,
						topDownScore,
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.ISKNOWN]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.ISCONTROLLABLE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.ISSCORABLE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByProgramAndErrorTypeID.ResultFields.BASICINFOID])
					);

					attribute.BasicInfo = new BasicInfo(attribute.BasicInfoID);
					attribute.BasicInfo.SetDataByID();

					attribute.SetValueList();
					attribute.SetChildrenList();

					attributeList.Add(attribute);
				}
			}

			return attributeList;
		}

		public List<Attribute> SelectByParentAttributeID()
		{
			List<Attribute> attributeList = new List<Attribute>();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectByParentAttributeID(this.ParentAttributeID.Value);

				foreach (DataRow dr in dt.Rows)
				{
					int?
						parentAttributeID = null,
						maxScore = null;

					bool topDownScore = false;

                    try { parentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
                    try { maxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.MAXSCORE]); } catch (Exception) { }
                    try { topDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }

					Attribute attribute = new Attribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.FORMID]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.ERRORTYPEID]),
						parentAttributeID,
						maxScore,
						topDownScore,
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.ISKNOWN]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.ISCONTROLLABLE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.ISSCORABLE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByParentAttributeID.ResultFields.BASICINFOID])
					);

					attribute.BasicInfo = new BasicInfo(attribute.BasicInfoID);
					attribute.BasicInfo.SetDataByID();

					attribute.SetValueList();
					attribute.SetChildrenList();

					attributeList.Add(attribute);
				}
			}

			return attributeList;
		}

		public List<Attribute> SelectByLevel(int level, bool simpleData = false)
		{
			List<Attribute> attributeList = new List<Attribute>();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectByLevel(this.ID, level);

				foreach (DataRow dr in dt.Rows)
				{
					int?
						parentAttributeID = null,
						maxScore = null;

					bool topDownScore = false;

                    try { parentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
                    try { maxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.MAXSCORE]); } catch (Exception) { }
                    try { topDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }

					Attribute attribute = new Attribute(
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.ID]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.FORMID]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.NAME]),
						Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.DESCRIPTION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.ERRORTYPEID]),
						parentAttributeID,
						maxScore,
						topDownScore,
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.HASFORCEDCOMMENT]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.ISKNOWN]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.ISCONTROLLABLE]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.ISSCORABLE]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.ORDER]),
						Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByLevel.ResultFields.BASICINFOID])
					);

					attribute.BasicInfo = new BasicInfo(attribute.BasicInfoID);
					attribute.BasicInfo.SetDataByID();

					if (!simpleData)
                    {
                        attribute.SetValueList();
                        attribute.SetChildrenList();
                    }

					attributeList.Add(attribute);
				}
			}

			return attributeList;
		}

		public int[] SelectParentIDArrayByID()
		{
			int[] parentAttributeIDList = new int[0];

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataTable dt = repoAttribute.SelectParentIDListByID(this.ID);

                parentAttributeIDList = new int[dt.Rows.Count];

				for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parentAttributeIDList[i] = Convert.ToInt32(dt.Rows[i][SCC_DATA.Queries.Attribute.StoredProcedures.SelectParentIDListByID.ResultFields.ID]);
                }
			}

			return parentAttributeIDList;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataRow dr = repoAttribute.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.ID]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.FORMID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.NAME]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
				this.ErrorTypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.ERRORTYPEID]);
                try { this.ParentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
				try { this.MaxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.MAXSCORE]); } catch (Exception) { }
				try { this.TopDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }
				this.HasForcedComment = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.HASFORCEDCOMMENT]);
				this.IsKnown = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.ISKNOWN]);
				this.IsControllable = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.ISCONTROLLABLE]);
				this.IsScorable = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.ISSCORABLE]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.SetValueList();
				this.SetChildrenList();
			}
		}

		public void SetDataByName()
		{
			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataRow dr = repoAttribute.SelectByName(this.Name, this.FormID);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.ID]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.FORMID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.NAME]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.DESCRIPTION]);
				this.ErrorTypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.ERRORTYPEID]);
                try { this.ParentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
				try { this.MaxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.MAXSCORE]); } catch (Exception) { }
				try { this.TopDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }
				this.HasForcedComment = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.HASFORCEDCOMMENT]);
				this.IsKnown = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.ISKNOWN]);
				this.IsControllable = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.ISCONTROLLABLE]);
				this.IsScorable = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.ISSCORABLE]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectByName.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.SetValueList();
				this.SetChildrenList();
			}
		}

		public void SetSubattributeDataByName()
		{
			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				DataRow dr = repoAttribute.SelectSubattributeByName(this.Name, this.ParentAttributeID.Value);

                if (dr == null)
                {
					this.ID = -1;
					return;
                }

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.ID]);
				this.FormID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.FORMID]);
				this.Name = Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.NAME]);
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.DESCRIPTION]);
				this.ErrorTypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.ERRORTYPEID]);
                try { this.ParentAttributeID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.PARENTATTRIBUTEID]); } catch (Exception) { }
				try { this.MaxScore = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.MAXSCORE]); } catch (Exception) { }
				try { this.TopDownScore = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.TOPDOWNSCORE]); } catch (Exception) { }
				this.HasForcedComment = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.HASFORCEDCOMMENT]);
				this.IsKnown = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.ISKNOWN]);
				this.IsControllable = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.ISCONTROLLABLE]);
				this.IsScorable = Convert.ToBoolean(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.ISSCORABLE]);
				this.Order = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.ORDER]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Attribute.StoredProcedures.SelectSubattributeByName.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();

				this.SetValueList();
				this.SetChildrenList();
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				int response = repoAttribute.DeleteByID(this.ID);
				this.BasicInfo.DeleteByID();

				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				this.ID = repoAttribute.Insert(this.FormID, this.Name, this.Description, this.ErrorTypeID, this.ParentAttributeID, this.MaxScore, this.TopDownScore, this.HasForcedComment, this.IsKnown, this.IsControllable, this.IsScorable, this.Order, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Attribute repoAttribute = new SCC_DATA.Repositories.Attribute())
			{
				return repoAttribute.Update(this.ID, this.FormID, this.Name, this.Description, this.ErrorTypeID, this.ParentAttributeID > 0 ? this.ParentAttributeID : null, this.MaxScore, this.TopDownScore, this.HasForcedComment, this.IsKnown, this.IsControllable, this.IsScorable, this.Order);
			}
		}

		public Results.Attribute.UpdateAttributeValueCatalogList.CODE UpdateAttributeValueCatalogList(List<AttributeValueCatalog> atributeValueCatalogList, int creationUserID)
		{
			try
			{
				if (atributeValueCatalogList == null) atributeValueCatalogList = new List<AttributeValueCatalog>();

				//Delete old ones
				this.ValueList
					.ForEach(e => {
						if (!atributeValueCatalogList
								.Where(w =>
									w.ID != null &&
									w.ID > 0)
								.Select(s => s.ID)
								.Contains(e.ID))
						{
							e.DeleteByID(creationUserID);
						}
					});

				//Update existing ones
				List<AttributeValueCatalog> updatedAttributeValueCatalogList = new List<AttributeValueCatalog>();

				foreach (AttributeValueCatalog attributeValueCatalog in atributeValueCatalogList)
				{
					if (this.ValueList.Select(e => e.ID).Contains(attributeValueCatalog.ID))
					{
						int currentBasicInfoID = 0;

						using (AttributeValueCatalog auxAtributeValueCatalog = new AttributeValueCatalog(attributeValueCatalog.ID))
                        {
							auxAtributeValueCatalog.SetDataByID();
							currentBasicInfoID = auxAtributeValueCatalog.BasicInfoID;
                        }

						AttributeValueCatalog newAttributeValueCatalog = new AttributeValueCatalog(
							attributeValueCatalog.ID,
							this.ID,
							attributeValueCatalog.Name,
							attributeValueCatalog.Value,
							attributeValueCatalog.TriggersChildVisualization,
							attributeValueCatalog.Order,
							currentBasicInfoID,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.UPDATED);

						int result = newAttributeValueCatalog.Update();

						if (result > 0)
						{
							updatedAttributeValueCatalogList.Add(newAttributeValueCatalog);
						}
					}
				}

				//Create new ones
				List<AttributeValueCatalog> insertedAttributeValueCatalogList = new List<AttributeValueCatalog>();

				foreach (AttributeValueCatalog attributeValueCatalog in atributeValueCatalogList)
				{
					if (!this.ValueList.Select(e => e.ID).Contains(attributeValueCatalog.ID))
					{
						AttributeValueCatalog newAttributeValueCatalog = new AttributeValueCatalog(
							this.ID,
							attributeValueCatalog.Name,
							attributeValueCatalog.Value,
							attributeValueCatalog.TriggersChildVisualization,
							attributeValueCatalog.Order,
							creationUserID,
							(int)SCC_BL.DBValues.Catalog.STATUS_ATTRIBUTE_VALUE_CATALOG.CREATED);

						int result = newAttributeValueCatalog.Insert();

						if (result > 0)
						{
							updatedAttributeValueCatalogList.Add(newAttributeValueCatalog);
						}
					}
				}

				return Results.Attribute.UpdateAttributeValueCatalogList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<Attribute> GetAttributeListFromExcel(IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rows, SCC_BL.CustomTools.FormUploadInfo formUploadInfo, int headersCount, int creationUserID)
		{
			List<Attribute> attributeList = new List<Attribute>();

			int order = 1;
			int ghostIDCount = Settings.Overall.InitialAttributeGhostID;

			foreach (SCC_BL.CustomTools.FormUploadInfo.ErrorTypeInfo errorTypeInfo in formUploadInfo.ErrorTypeList)
			{
				List<Attribute> parentList = new List<Attribute>();
				int previousColumnIndex = 0;

				foreach (SCC_BL.CustomTools.FormUploadInfo.ErrorTypeInfo.AttributeListInfo attributeInfo in errorTypeInfo.AttributeList)
                {
                    if (attributeInfo.ColumnIndex == 1) parentList = new List<Attribute>();

					if (previousColumnIndex > attributeInfo.ColumnIndex)
                    {
                        parentList.Remove(parentList.Last());
                        for (int i = 0; i < previousColumnIndex - attributeInfo.ColumnIndex; i++)
                        {
                            try
                            {
                                parentList.Remove(parentList.Last());
                            }
                            catch (Exception ex)
                            {
                            }
                        }
					}

					using (SCC_BL.Tools.ExcelParser excelParser = new Tools.ExcelParser())
					{
						DocumentFormat.OpenXml.Spreadsheet.Cell[] auxCurrentCells = excelParser.GetRowCells(rows.ElementAt(attributeInfo.RowIndex), headersCount).ToArray();

                        Attribute attribute = new Attribute(
							auxCurrentCells, 
							errorTypeInfo.ErrorTypeEnum, 
							0, 
							attributeInfo.ColumnIndex, 
							errorTypeInfo.DescriptionIndex, 
							ghostIDCount, 
							parentList.Count > 0 
								? parentList.Last().AttributeGhostID 
								: 0, 
							order, 
							creationUserID);

						attributeList.Add(attribute);

                        try
						{
							if (!excelParser.CellHasData(excelParser.GetRowCells(rows.ElementAt(attributeInfo.RowIndex + 1), headersCount).ToArray(), attributeInfo.ColumnIndex))
								parentList.Add(attribute);
						}
                        catch (Exception ex)
                        {
                        }
					}

					previousColumnIndex = attributeInfo.ColumnIndex;

					order++;
					ghostIDCount++;
                }
            }

			return attributeList;
		}

		public static List<Catalog> GetAttributeErrorType(bool includeGlobal = false)
		{
			List<Catalog> errorTypeList = new List<Catalog>();

            using (Catalog catalog = Catalog.CatalogWithCategoryID((int)SCC_BL.DBValues.Catalog.Category.ATTRIBUTE_ERROR_TYPE))
                errorTypeList =
                    catalog.SelectByCategoryID()
                        .Where(e => e.Active)
                        .ToList();

			if (!includeGlobal)
			{
				errorTypeList =
					errorTypeList
						.Where(e => e.ID != (int)SCC_BL.DBValues.Catalog.ATTRIBUTE_ERROR_TYPE.GCE)
						.ToList();
            }

			return errorTypeList;
        }

		public void Dispose()
		{
		}
	}
}