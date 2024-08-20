using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
	public class UploadedFile : IDisposable
	{
		public int ID { get; set; }
		public string FileName { get; set; }
		public string Extension { get; set; }
		public byte[] Data { get; set; }
		public int BasicInfoID { get; set; }
		//----------------------------------------------------
		public BasicInfo BasicInfo { get; set; }

		public UploadedFile()
		{
		}

		//For SelectByID and DeleteByID
		public UploadedFile(int id)
		{
			this.ID = id;
		}

		//For Insert
		public UploadedFile(string fileName, string extension, byte[] data, int creationUserID, int statusID)
		{
			this.FileName = fileName;
			this.Extension = extension;
			this.Data = data;

			this.BasicInfo = new BasicInfo(creationUserID, statusID);
		}

		//For Update
		public UploadedFile(int id, string fileName, string extension, byte[] data, int basicInfoID, int modificationUserID, int statusID)
		{
			this.ID = id;
			this.FileName = fileName;
			this.Extension = extension;
			this.Data = data;

			this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
		}

		//For SelectByID (RESULT)
		public UploadedFile(int id, string fileName, string extension, byte[] data, int basicInfoID)
		{
			this.ID = id;
			this.FileName = fileName;
			this.Extension = extension;
			this.Data = data;
			this.BasicInfoID = basicInfoID;
		}

		//For SelectAll
		public UploadedFile(int id, string fileName, string extension, int basicInfoID)
		{
			this.ID = id;
			this.FileName = fileName;
			this.Extension = extension;
			this.BasicInfoID = basicInfoID;
		}

		public void SetDataByID()
		{
			using (SCC_DATA.Repositories.UploadedFile repoUploadedFile = new SCC_DATA.Repositories.UploadedFile())
			{
				DataRow dr = repoUploadedFile.SelectByID(this.ID);

				this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectByID.ResultFields.ID]);
				this.FileName = Convert.ToString(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectByID.ResultFields.FILENAME]);
				this.Extension = Convert.ToString(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectByID.ResultFields.EXTENSION]);
				this.Data = (byte[])(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectByID.ResultFields.DATA]);
				this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

				this.BasicInfo = new BasicInfo(this.BasicInfoID);
				this.BasicInfo.SetDataByID();
			}
		}

		public List<UploadedFile> SelectAll()
		{
			List<UploadedFile> uploadedFileList = new List<UploadedFile>();

			using (SCC_DATA.Repositories.UploadedFile repoUploadedFile = new SCC_DATA.Repositories.UploadedFile())
			{
				DataTable dt = repoUploadedFile.SelectAll();

				foreach (DataRow dr in dt.Rows)
				{
					UploadedFile uploadedFile = new UploadedFile(
						Convert.ToInt32(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectAll.ResultFields.ID]),
						Convert.ToString(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectAll.ResultFields.FILENAME]),
						Convert.ToString(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectAll.ResultFields.EXTENSION]),
						Convert.ToInt32(dr[SCC_DATA.Queries.UploadedFile.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
					);

					uploadedFile.BasicInfo = new BasicInfo(uploadedFile.BasicInfoID);
					uploadedFile.BasicInfo.SetDataByID();

					uploadedFileList.Add(uploadedFile);
				}
			}

			return uploadedFileList
				.OrderByDescending(o => o.BasicInfo.CreationDate)
				.ToList();
		}

		public int DeleteByID()
		{
			using (SCC_DATA.Repositories.UploadedFile repoUploadedFile = new SCC_DATA.Repositories.UploadedFile())
			{
				int response = repoUploadedFile.DeleteByID(this.ID);

				this.BasicInfo.DeleteByID();
				return response;
			}
		}

		public int Insert()
		{
			this.BasicInfoID = this.BasicInfo.Insert();

			using (SCC_DATA.Repositories.UploadedFile repoUploadedFile = new SCC_DATA.Repositories.UploadedFile())
			{
				this.ID = repoUploadedFile.Insert(this.FileName, this.Extension, this.Data, this.BasicInfoID);

				return this.ID;
			}
		}

		public int Update()
		{
			this.BasicInfo.Update();

			using (SCC_DATA.Repositories.UploadedFile repoUploadedFile = new SCC_DATA.Repositories.UploadedFile())
			{
				return repoUploadedFile.Update(this.ID, this.FileName, this.Extension, this.Data);
			}
		}

		public void Dispose()
		{
		}
	}
}