using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
    public class UserNotificationUrl : IDisposable
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public int BasicInfoID { get; set; }
        //----------------------------------------------------
        public BasicInfo BasicInfo { get; set; }

        public UserNotificationUrl()
        {
        }

        //For SelectByID and DeleteByID
        public UserNotificationUrl(int id)
        {
            this.ID = id;
        }

        //For Pre-insert
        public UserNotificationUrl(string content, string description)
        {
            this.Content = content;
            this.Description = description;
        }

        //For Insert
        public UserNotificationUrl(string content, string description, int creationUserID, int statusID)
        {
            this.Content = content;
            this.Description = description;

            this.BasicInfo = new BasicInfo(creationUserID, statusID);
        }

        //For Update
        public UserNotificationUrl(int id, string content, string description, int basicInfoID, int modificationUserID, int statusID)
        {
            this.ID = id;
            this.Content = content;
            this.Description = description;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
        }

        //For SelectByID (RESULT)
        public UserNotificationUrl(int id, string content, string description, int basicInfoID)
        {
            this.ID = id;
            this.Content = content;
            this.Description = description;
            this.BasicInfoID = basicInfoID;
        }

        public int DeleteByID()
        {
            using (SCC_DATA.Repositories.UserNotificationUrl repoUserNotificationUrl = new SCC_DATA.Repositories.UserNotificationUrl())
            {
                int response = repoUserNotificationUrl.DeleteByID(this.ID);

                this.BasicInfo.DeleteByID();

                return response;
            }
        }

        public int Insert()
        {
            this.BasicInfoID = this.BasicInfo.Insert();

            using (SCC_DATA.Repositories.UserNotificationUrl repoUserNotificationUrl = new SCC_DATA.Repositories.UserNotificationUrl())
            {
                this.ID = repoUserNotificationUrl.Insert(this.Content, this.Description, this.BasicInfoID);

                return this.ID;
            }
        }

        public void SetDataByID()
        {
            using (SCC_DATA.Repositories.UserNotificationUrl repoUserNotificationUrl = new SCC_DATA.Repositories.UserNotificationUrl())
            {
                DataRow dr = repoUserNotificationUrl.SelectByID(this.ID);

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrl.StoredProcedures.SelectByID.ResultFields.ID]);
                this.Content = Convert.ToString(dr[SCC_DATA.Queries.UserNotificationUrl.StoredProcedures.SelectByID.ResultFields.CONTENT]);
                this.Description = Convert.ToString(dr[SCC_DATA.Queries.UserNotificationUrl.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrl.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();
            }
        }

        public int Update()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.UserNotificationUrl repoUserNotificationUrl = new SCC_DATA.Repositories.UserNotificationUrl())
            {
                return repoUserNotificationUrl.Update(this.ID, this.Content, this.Description);
            }
        }

        public void Dispose()
        {
        }
    }
}