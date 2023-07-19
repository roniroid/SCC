using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class CustomControl
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string LABEL = "Label";
			public const string MODULEID = "ModuleID";
			public const string ISREQUIRED = "IsRequired";
			public const string DESCRIPTION = "Description";
			public const string CONTROLTYPEID = "ControlTypeID";
			public const string CSSCLASS = "CssClass";
			public const string MASK = "Mask";
			public const string PATTERN = "Pattern";
			public const string DEFAULTVALUE = "DefaultValue";
			public const string NUMBEROFROWS = "NumberOfRows";
			public const string NUMBEROFCOLUMNS = "NumberOfColumns";
			public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_CustomControlDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_CustomControlInsert]";

				public struct Parameters
				{
					public const string LABEL = "@label";
					public const string MODULEID = "@moduleID";
					public const string ISREQUIRED = "@isRequired";
					public const string DESCRIPTION = "@description";
					public const string CONTROLTYPEID = "@controlTypeID";
					public const string CSSCLASS = "@cssClass";
					public const string MASK = "@mask";
					public const string PATTERN = "@pattern";
					public const string DEFAULTVALUE = "@defaultValue";
					public const string NUMBEROFROWS = "@numberOfRows";
					public const string NUMBEROFCOLUMNS = "@numberOfColumns";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectAll
			{
				public const string NAME = "[dbo].[usp_CustomControlSelectAll]";

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string LABEL = "Label";
					public const string MODULEID = "ModuleID";
					public const string ISREQUIRED = "IsRequired";
					public const string DESCRIPTION = "Description";
					public const string CONTROLTYPEID = "ControlTypeID";
					public const string CSSCLASS = "CssClass";
					public const string MASK = "Mask";
					public const string PATTERN = "Pattern";
					public const string DEFAULTVALUE = "DefaultValue";
					public const string NUMBEROFROWS = "NumberOfRows";
					public const string NUMBEROFCOLUMNS = "NumberOfColumns";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_CustomControlSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string LABEL = "Label";
					public const string MODULEID = "ModuleID";
					public const string ISREQUIRED = "IsRequired";
					public const string DESCRIPTION = "Description";
					public const string CONTROLTYPEID = "ControlTypeID";
					public const string CSSCLASS = "CssClass";
					public const string MASK = "Mask";
					public const string PATTERN = "Pattern";
					public const string DEFAULTVALUE = "DefaultValue";
					public const string NUMBEROFROWS = "NumberOfRows";
					public const string NUMBEROFCOLUMNS = "NumberOfColumns";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByLabel
			{
				public const string NAME = "[dbo].[usp_CustomControlSelectByLabel]";

				public struct Parameters
				{
					public const string LABEL = "@label";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string LABEL = "Label";
					public const string MODULEID = "ModuleID";
					public const string ISREQUIRED = "IsRequired";
					public const string DESCRIPTION = "Description";
					public const string CONTROLTYPEID = "ControlTypeID";
					public const string CSSCLASS = "CssClass";
					public const string MASK = "Mask";
					public const string PATTERN = "Pattern";
					public const string DEFAULTVALUE = "DefaultValue";
					public const string NUMBEROFROWS = "NumberOfRows";
					public const string NUMBEROFCOLUMNS = "NumberOfColumns";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_CustomControlUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string LABEL = "@label";
					public const string MODULEID = "@moduleID";
					public const string ISREQUIRED = "@isRequired";
					public const string DESCRIPTION = "@description";
					public const string CONTROLTYPEID = "@controlTypeID";
					public const string CSSCLASS = "@cssClass";
					public const string MASK = "@mask";
					public const string PATTERN = "@pattern";
					public const string DEFAULTVALUE = "@defaultValue";
					public const string NUMBEROFROWS = "@numberOfRows";
					public const string NUMBEROFCOLUMNS = "@numberOfColumns";
				}
			}

		}
	}
}