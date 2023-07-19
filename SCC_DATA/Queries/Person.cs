using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_DATA.Queries
{
	public class Person
	{
		public struct Fields
		{
			public const string ID = "ID";
			public const string IDENTIFICATION = "Identification";
			public const string FIRSTNAME = "FirstName";
			public const string SURNAME = "SurName";
			public const string LASTNAME = "LastName";
            public const string COUNTRY_ID = "CountryID";
            public const string BASICINFOID = "BasicInfoID";
		}

		public struct StoredProcedures
		{
			public struct CheckExistence
			{
				public const string NAME = "[dbo].[usp_PersonCustomCheckExistence]";

				public struct Parameters
				{
					public const string IDENTIFICATION = "@identification";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct DeleteByID
			{
				public const string NAME = "[dbo].[usp_PersonDeleteByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}
			}

			public struct Insert
			{
				public const string NAME = "[dbo].[usp_PersonInsert]";

				public struct Parameters
				{
					public const string IDENTIFICATION = "@identification";
					public const string FIRSTNAME = "@firstName";
					public const string SURNAME = "@surName";
					public const string LASTNAME = "@lastName";
					public const string COUNTRY_ID = "@countryID";
					public const string BASICINFOID = "@basicInfoID";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
				}
			}

			public struct SelectByID
			{
				public const string NAME = "[dbo].[usp_PersonSelectByID]";

				public struct Parameters
				{
					public const string ID = "@id";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFICATION = "Identification";
					public const string FIRSTNAME = "FirstName";
					public const string SURNAME = "SurName";
					public const string LASTNAME = "LastName";
					public const string COUNTRY_ID = "CountryID";
					public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct SelectByIdentification
			{
				public const string NAME = "[dbo].[usp_PersonSelectByIdentification]";

				public struct Parameters
				{
					public const string IDENTIFICATION = "@identification";
				}

				public struct ResultFields
				{
					public const string ID = "ID";
					public const string IDENTIFICATION = "Identification";
					public const string FIRSTNAME = "FirstName";
					public const string SURNAME = "SurName";
					public const string LASTNAME = "LastName";
                    public const string COUNTRY_ID = "CountryID";
                    public const string BASICINFOID = "BasicInfoID";
				}
			}

			public struct Update
			{
				public const string NAME = "[dbo].[usp_PersonUpdate]";

				public struct Parameters
				{
					public const string ID = "@id";
					public const string IDENTIFICATION = "@identification";
					public const string FIRSTNAME = "@firstName";
					public const string SURNAME = "@surName";
					public const string LASTNAME = "@lastName";
                    public const string COUNTRY_ID = "@countryID";
                }
			}

		}
	}
}