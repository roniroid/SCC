using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class Attribute
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string FORMID = "FormID";
			public const string NAME = "Name";
			public const string DESCRIPTION = "Description";
			public const string ERRORTYPEID = "ErrorTypeID";
			public const string PARENTATTRIBUTEID = "ParentAttributeID";
			public const string MAXSCORE = "MaxScore";
			public const string TOPDOWNSCORE = "TopDownScore";
			public const string HASFORCEDCOMMENT = "HasForcedComment";
			public const string ISKNOWN = "IsKnown";
			public const string ISCONTROLLABLE = "IsControllable";
			public const string ISSCORABLE = "IsScorable";
			public const string ORDER = "Order";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_AttributeDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_AttributeInsert]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
					public const string NAME = "@name";
					public const string DESCRIPTION = "@description";
					public const string ERRORTYPEID = "@errorTypeID";
					public const string PARENTATTRIBUTEID = "@parentAttributeID";
					public const string MAXSCORE = "@maxScore";
					public const string TOPDOWNSCORE = "@topDownScore";
					public const string HASFORCEDCOMMENT = "@hasForcedComment";
					public const string ISKNOWN = "@isKnown";
					public const string ISCONTROLLABLE = "@isControllable";
					public const string ISSCORABLE = "@isScorable";
					public const string ORDER = "@order";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByFormID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectHierarchyByFormID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectHierarchyByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectIDListByFormID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectIDListByFormID]";

				public struct Parameters
				{
					public const string FORMID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByProgramAndErrorTypeID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectByProgramAndErrorTypeID]";

				public struct Parameters
				{
					public const string PROGRAM_ID_LIST = "@programIDList";
					public const string ERROR_TYPE_ID_LIST = "@errorTypeIDList";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByParentAttributeID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectByParentAttributeID]";

				public struct Parameters
				{
					public const string PARENTATTRIBUTEID = "@parentAttributeID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByName
			{
				public const string NAME = "[dbo].[usp_AttributeSelectByName]";

				public struct Parameters
				{
					public const string NAME = "@name";
					public const string FORM_ID = "@formID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByLevel
			{
				public const string NAME = "[dbo].[usp_AttributeSelectByLevel]";

				public struct Parameters
                {
                    public const string ATTRIBUTE_ID = "@attributeID";
                    public const string LEVEL = "@level";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectParentIDListByID
			{
				public const string NAME = "[dbo].[usp_AttributeSelectParentListByID]";

				public struct Parameters
                {
                    public const string ATTRIBUTE_ID = "@attributeID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectSubattributeByName
			{
				public const string NAME = "[dbo].[usp_AttributeSubattributeSelectByName]";

				public struct Parameters
				{
					public const string NAME = "@name";
					public const string PARENT_ATTRIBUTE_ID = "@parentAttributeID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string FORMID = "FormID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string ERRORTYPEID = "ErrorTypeID";
					public const string PARENTATTRIBUTEID = "ParentAttributeID";
					public const string MAXSCORE = "MaxScore";
					public const string TOPDOWNSCORE = "TopDownScore";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string ISKNOWN = "IsKnown";
					public const string ISCONTROLLABLE = "IsControllable";
					public const string ISSCORABLE = "IsScorable";
					public const string ORDER = "Order";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_AttributeUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string FORMID = "@formID";
					public const string NAME = "@name";
					public const string DESCRIPTION = "@description";
					public const string ERRORTYPEID = "@errorTypeID";
					public const string PARENTATTRIBUTEID = "@parentAttributeID";
					public const string MAXSCORE = "@maxScore";
					public const string TOPDOWNSCORE = "@topDownScore";
					public const string HASFORCEDCOMMENT = "@hasForcedComment";
					public const string ISKNOWN = "@isKnown";
					public const string ISCONTROLLABLE = "@isControllable";
					public const string ISSCORABLE = "@isScorable";
					public const string ORDER = "@order";
				}
			}

		}
	}
}