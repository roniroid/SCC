using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_DATA.Queries
{
	public class BusinessIntelligenceField
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string NAME = "Name";
			public const string DESCRIPTION = "Description";
			public const string PARENTBIFIELDID = "ParentBIFieldID";
			public const string HASFORCEDCOMMENT = "HasForcedComment";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldInsert]";

				public struct Parameters
				{
					public const string NAME = "@name";
					public const string DESCRIPTION = "@description";
					public const string PARENTBIFIELDID = "@parentBIFieldID";
					public const string HASFORCEDCOMMENT = "@hasForcedComment";
					public const string BASICINFOID = "@basicInfoID";
					public const string ORDER = "@order";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string PARENTBIFIELDID = "ParentBIFieldID";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string BASICINFOID = "BasicInfoID";
					public const string ORDER = "Order";
				}
			}

			public struct SelectByParentIDAndName
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldSelectByParentIDAndName]";

				public struct Parameters
				{
					public const string PARENT_ID = "@parentID";
					public const string NAME = "@name";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string PARENTBIFIELDID = "ParentBIFieldID";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string BASICINFOID = "BasicInfoID";
                    public const string ORDER = "Order";
                }
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string PARENTBIFIELDID = "ParentBIFieldID";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string BASICINFOID = "BasicInfoID";
                    public const string ORDER = "Order";
                }
			}

			public struct SelectChildren
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldSelectChildren]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string PARENTBIFIELDID = "ParentBIFieldID";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string BASICINFOID = "BasicInfoID";
                    public const string ORDER = "Order";
                }

                public struct Parameters
                {
                    public const string PARENTBIFIELDID = "@parentBIFieldID";
                }
            }

			public struct SelectByProgramID
            {
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldSelectByProgramID]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string NAME = "Name";
					public const string DESCRIPTION = "Description";
					public const string PARENTBIFIELDID = "ParentBIFieldID";
					public const string HASFORCEDCOMMENT = "HasForcedComment";
					public const string BASICINFOID = "BasicInfoID";
                    public const string ORDER = "Order";
                }

                public struct Parameters
                {
                    public const string PROGRAM_ID_LIST = "@programIDList";
                }
            }

            public struct SelectHierarchyByFormID
            {
                public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldSelectHierarchyByFormID]";

                public struct Parameters
                {
                    public const string FORMID = "@formID";
                }

                public struct ResultFields
                {
                    public const string ID = "ID";
                    public const string NAME = "Name";
                    public const string DESCRIPTION = "Description";
                    public const string PARENTBIFIELDID = "ParentBIFieldID";
                    public const string HASFORCEDCOMMENT = "HasForcedComment";
                    public const string BASICINFOID = "BasicInfoID";
                    public const string ORDER = "Order";
                }
            }

            public struct Update
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string NAME = "@name";
					public const string DESCRIPTION = "@description";
					public const string PARENTBIFIELDID = "@parentBIFieldID";
					public const string HASFORCEDCOMMENT = "@hasForcedComment";
					public const string ORDER = "@order";
				}
			}

			public struct UpdateOrder
			{
				public const string NAME = "[dbo].[usp_BusinessIntelligenceFieldUpdateOrder]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string ORDER = "@order";
				}
			}
		}
	}
}