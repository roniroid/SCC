using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
	public class Catalog : IDisposable
	{
		public int ID { get; set; }
		public int? CategoryID { get; set; } = null;
		public string Description { get; set; }
		public bool Active { get; set; }

		public Catalog()
		{
		}

		//For Select and Delete
		public Catalog(int id)
		{
			this.ID = id;
		}

		//For SelectByDescription
		public Catalog(string description)
		{
			this.Description = description;
		}

		//For Insert
		public Catalog(int? categoryID, string description, bool active)
		{
			this.CategoryID = categoryID;
			this.Description = description;
			this.Active = active;
		}

		//For SelectByCategoryID
		public static Catalog CatalogWithCategoryID(int? categoryID)
		{
			Catalog @object = new Catalog();
			@object.CategoryID = categoryID;
			return @object;
		}

		//For Update and SelectByCategoryID (RESULT)
		public Catalog(int id, int? categoryID, string description, bool active)
		{
			this.ID = id;
			this.CategoryID = categoryID;
			this.Description = description;
			this.Active = active;
		}

		public int Delete()
		{
			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				int response = repoCatalog.Delete(this.ID);

				return response;
			}
		}

		public int Insert()
		{
			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				this.ID = repoCatalog.Insert(this.CategoryID, this.Description, this.Active);

				return this.ID;
			}
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				DataRow dr = repoCatalog.Select(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.Select.ResultFields.ID]);
				try { this.CategoryID = Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.Select.ResultFields.CATEGORYID]); } catch (Exception) { }
				this.Description = Convert.ToString(dr[SCC_DATA.Queries.Catalog.StoredProcedures.Select.ResultFields.DESCRIPTION]);
				this.Active = Convert.ToBoolean(dr[SCC_DATA.Queries.Catalog.StoredProcedures.Select.ResultFields.ACTIVE]);
			}
		}

		public int SetDataByDescription()
		{
			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				DataRow dr = repoCatalog.SelectByDescription(this.Description);

                if (dr.ItemArray.Length > 0)
                {
					int result = Convert.ToInt32(dr[0]);

					if (result <= 0) return result;

					this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByDescription.ResultFields.ID]);
					try { this.CategoryID = Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByDescription.ResultFields.CATEGORYID]); } catch (Exception) { }
					this.Description = Convert.ToString(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByDescription.ResultFields.DESCRIPTION]);
					this.Active = Convert.ToBoolean(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByDescription.ResultFields.ACTIVE]);

					return this.ID;
				}
			}

			return 0;
		}

		public List<Catalog> GetAllCatalogList()
		{
			List<Catalog> catalogList = new List<Catalog>();

			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				DataTable dt = repoCatalog.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					int?
						categoryID = null;

					try { categoryID = Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectAll.ResultFields.CATEGORYID]); } catch (Exception) { }

					Catalog catalog = new Catalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectAll.ResultFields.ID]),
						categoryID,
						Convert.ToString(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectAll.ResultFields.DESCRIPTION]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectAll.ResultFields.ACTIVE])
					);

					catalogList.Add(catalog);
				}
			}

			return catalogList
				.OrderBy(o => o.Description)
				.ToList();
		}

		public List<Catalog> SelectByCategoryID()
		{
			List<Catalog> catalogList = new List<Catalog>();

			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				DataTable dt = repoCatalog.SelectByCategoryID(this.CategoryID);

				foreach (DataRow dr in dt.Rows)
				{
					int?
						categoryID = null;

					try { categoryID = Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByCategoryID.ResultFields.CATEGORYID]); } catch (Exception) { }

					Catalog catalog = new Catalog(
						Convert.ToInt32(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByCategoryID.ResultFields.ID]),
						categoryID,
						Convert.ToString(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByCategoryID.ResultFields.DESCRIPTION]),
						Convert.ToBoolean(dr[SCC_DATA.Queries.Catalog.StoredProcedures.SelectByCategoryID.ResultFields.ACTIVE])
					);

					catalogList.Add(catalog);
				}
			}

			return catalogList
				.OrderBy(o => o.Description)
				.ToList();
		}

		public int Update()
		{
			using (SCC_DATA.Repositories.Catalog repoCatalog = new SCC_DATA.Repositories.Catalog())
			{
				return repoCatalog.Update(this.ID, this.CategoryID, this.Description, this.Active);
			}
		}

		public void Dispose()
		{
		}
	}
}