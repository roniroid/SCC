using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SCC_BL
{
    public class UserNotification : IDisposable
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public int TypeID { get; set; }
        public int BasicInfoID { get; set; }
        //----------------------------------------------------
        public BasicInfo BasicInfo { get; set; }
        public List<UserNotificationUrlCatalog> UserNotificationUrlCatalogList { get; set; } = new List<UserNotificationUrlCatalog>();
        //----------------------------------------------------
        public List<UserNotificationUrl> UserNotificationUrlList { get; set; } = new List<UserNotificationUrl>();

        public UserNotification()
        {
        }

        //For SelectByID and DeleteByID
        public UserNotification(int id)
        {
            this.ID = id;
        }

        public static UserNotification UserNotificationWithUserID(int userID)
        {
            UserNotification @object = new UserNotification();
            @object.UserID = userID;
            return @object;
        }

        //For Insert
        public UserNotification(int userID, string message, int typeID, int creationUserID, int statusID)
        {
            this.UserID = userID;
            this.Message = message;
            this.TypeID = typeID;

            this.BasicInfo = new BasicInfo(creationUserID, statusID);
        }

        //For Update
        public UserNotification(int id, int userID, string message, int typeID, int basicInfoID, int modificationUserID, int statusID)
        {
            this.ID = id;
            this.UserID = userID;
            this.Message = message;
            this.TypeID = typeID;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
        }

        //For SelectByID (RESULT)
        public UserNotification(int id, int userID, string message, int typeID, int basicInfoID)
        {
            this.ID = id;
            this.UserID = userID;
            this.Message = message;
            this.TypeID = typeID;
            this.BasicInfoID = basicInfoID;
        }

        void SetUserNotificationUrlCatalogList()
        {
            this.UserNotificationUrlCatalogList = new List<UserNotificationUrlCatalog>();

            using (UserNotificationUrlCatalog userNotificationUrlCatalog = UserNotificationUrlCatalog.UserNotificationUrlCatalogWithUserNotificationID(this.ID))
            {
                this.UserNotificationUrlCatalogList = userNotificationUrlCatalog.SelectByUserNotificationID();
            }
        }

        void SetUserNotificationUrlList()
        {
            this.UserNotificationUrlList = new List<UserNotificationUrl>();

            foreach (UserNotificationUrlCatalog userNotificationUrlCatalog in this.UserNotificationUrlCatalogList)
            {
                using (UserNotificationUrl userNotificationUrl = new UserNotificationUrl(userNotificationUrlCatalog.UserNotificationUrlID))
                {
                    userNotificationUrl.SetDataByID();
                    this.UserNotificationUrlList.Add(userNotificationUrl);
                }
            }
        }

        public int DeleteByID()
        {
            using (SCC_DATA.Repositories.UserNotification repoUserNotification = new SCC_DATA.Repositories.UserNotification())
            {
                int response = repoUserNotification.DeleteByID(this.ID);

                this.BasicInfo.DeleteByID();

                return response;
            }
        }

        public int Insert()
        {
            this.BasicInfoID = this.BasicInfo.Insert();

            using (SCC_DATA.Repositories.UserNotification repoUserNotification = new SCC_DATA.Repositories.UserNotification())
            {
                this.ID = repoUserNotification.Insert(this.UserID, this.Message, this.TypeID, this.BasicInfoID);

                return this.ID;
            }
        }

        public void SetDataByID()
        {
            using (SCC_DATA.Repositories.UserNotification repoUserNotification = new SCC_DATA.Repositories.UserNotification())
            {
                DataRow dr = repoUserNotification.SelectByID(this.ID);

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByID.ResultFields.ID]);
                this.UserID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByID.ResultFields.USERID]);
                this.Message = Convert.ToString(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByID.ResultFields.MESSAGE]);
                this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByID.ResultFields.TYPEID]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();

                this.SetUserNotificationUrlCatalogList();
                this.SetUserNotificationUrlList();
            }
        }

        public List<UserNotification> SelectByUserID(int userID)
        {
            List<UserNotification> userNotificationList = new List<UserNotification>();

            using (SCC_DATA.Repositories.UserNotification repoUserNotification = new SCC_DATA.Repositories.UserNotification())
            {
                DataTable dt = repoUserNotification.SelectByUserID(userID);

                foreach (DataRow dr in dt.Rows)
                {
                    UserNotification userNotification = new UserNotification(
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByUserID.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByUserID.ResultFields.USERID]),
                        Convert.ToString(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByUserID.ResultFields.MESSAGE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByUserID.ResultFields.TYPEID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotification.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
                    );

                    userNotification.BasicInfo = new BasicInfo(userNotification.BasicInfoID);
                    userNotification.BasicInfo.SetDataByID();

                    userNotification.SetUserNotificationUrlCatalogList();
                    userNotification.SetUserNotificationUrlList();

                    userNotificationList.Add(userNotification);
                }
            }

            return userNotificationList;
        }

        public int Update()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.UserNotification repoUserNotification = new SCC_DATA.Repositories.UserNotification())
            {
                return repoUserNotification.Update(this.ID, this.UserID, this.Message, this.TypeID);
            }
        }

        public void Dispose()
        {
        }
    }
}