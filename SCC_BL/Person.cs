using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class Person : IDisposable
	{
		public int ID { get; set; }
		public string Identification { get; set; }
		public string FirstName { get; set; }
		public string SurName { get; set; }
		public int CountryID { get; set; } = (int)SCC_BL.DBValues.Catalog.PERSON_COUNTRY.COSTA_RICA;
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public Person()
		{
		}

		//For CustomCheckExistence and SetDataByIdentification
		public Person(string identification)
		{
			this.Identification = identification;
		}

		//For SelectByID and DeleteByID
		public Person(int id)
		{
			this.ID = id;
		}

		//For Insert
		public Person(string identification, string firstName, string surName, int countryID, int? creationUserID, int statusID)
		{
			this.Identification = identification;
			this.FirstName = firstName;
			this.SurName = surName;
			this.CountryID = countryID;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);

			this.FixIdentification();
        }

		//For Update
		public Person(int id, string identification, string firstName, string surName, int countryID, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.Identification = identification;
			this.FirstName = firstName;
			this.SurName = surName;
            this.CountryID = countryID;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);

            this.FixIdentification();
        }

		//For SelectByID (RESULT)
		public Person(int id, string identification, string firstName, string surName, int basicInfoID, int countryID)
		{
			this.ID = id;
			this.Identification = identification;
			this.FirstName = firstName;
			this.SurName = surName;
			this.CountryID = countryID;
			this.BasicInfoID = basicInfoID;
		}

		void FixIdentification()
        {
            this.Identification = this.FilterNumbers(this.Identification);
        }

		public int CheckExistence()
		{
			using (SCC_DATA.Repositories.Person repoPerson = new SCC_DATA.Repositories.Person())
			{
				int response = repoPerson.CheckExistence(this.Identification);

				return response;
			}
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.Person repoPerson = new SCC_DATA.Repositories.Person())
			{
				int response = repoPerson.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.Person repoPerson = new SCC_DATA.Repositories.Person())
			{
				this.ID = repoPerson.Insert(this.Identification, this.FirstName, this.SurName, this.CountryID, this.BasicInfoID);

				return this.ID;
			}
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Person repoPerson = new SCC_DATA.Repositories.Person())
			{
				DataRow dr = repoPerson.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByID.ResultFields.ID]);
				this.Identification = Convert.ToString(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByID.ResultFields.IDENTIFICATION]);
				this.FirstName = Convert.ToString(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByID.ResultFields.FIRSTNAME]);
				this.SurName = Convert.ToString(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByID.ResultFields.SURNAME]);
				this.CountryID = Convert.ToInt32(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByID.ResultFields.COUNTRY_ID]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public void SetDataByIdentification()
		{
			using (SCC_DATA.Repositories.Person repoPerson = new SCC_DATA.Repositories.Person())
			{
				DataRow dr = repoPerson.SelectByIdentification(this.Identification);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByIdentification.ResultFields.ID]);
				this.Identification = Convert.ToString(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByIdentification.ResultFields.IDENTIFICATION]);
				this.FirstName = Convert.ToString(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByIdentification.ResultFields.FIRSTNAME]);
				this.SurName = Convert.ToString(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByIdentification.ResultFields.SURNAME]);
                this.CountryID = Convert.ToInt32(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByIdentification.ResultFields.COUNTRY_ID]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Person.StoredProcedures.SelectByIdentification.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.Person repoPerson = new SCC_DATA.Repositories.Person())
			{
				return repoPerson.Update(this.ID, this.Identification, this.FirstName, this.SurName, this.CountryID);
			}
		}

		public void Dispose()
		{
        }

        string FilterNumbers(string input)
        {
            string pattern = @"\d";
            MatchCollection matches = Regex.Matches(input, pattern);

            string result = "";
            foreach (Match match in matches)
            {
                result += match.Value;
            }

            return result;
        }
    }
}