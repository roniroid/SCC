using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL
{
    public class UserNotificationUrlCatalog : IDisposable
    {
        public int ID { get; set; }
        public int UserNotificationID { get; set; }
        public int UserNotificationUrlID { get; set; }
        public int BasicInfoID { get; set; }
        //----------------------------------------------------
        public BasicInfo BasicInfo { get; set; }

        public UserNotificationUrlCatalog()
        {
        }

        //For SelectByID and DeleteByID
        public UserNotificationUrlCatalog(int id)
        {
            this.ID = id;
        }

        public static UserNotificationUrlCatalog UserNotificationUrlCatalogWithUserNotificationID(int userNotificationID)
        {
            UserNotificationUrlCatalog @object = new UserNotificationUrlCatalog();
            @object.UserNotificationID = userNotificationID;
            return @object;
        }

        //For Insert
        public static UserNotificationUrlCatalog UserNotificationUrlCatalogForInsert(int userNotificationID, int userNotificationUrlID, int creationUserID, int statusID)
        {
            UserNotificationUrlCatalog @object = new UserNotificationUrlCatalog();
            @object.UserNotificationID = userNotificationID;
            @object.UserNotificationUrlID = userNotificationUrlID;

            @object.BasicInfo = new BasicInfo(creationUserID, statusID);
            return @object;
        }

        //For Update
        public UserNotificationUrlCatalog(int id, int userNotificationID, int userNotificationUrlID, int basicInfoID, int modificationUserID, int statusID)
        {
            this.ID = id;
            this.UserNotificationID = userNotificationID;
            this.UserNotificationUrlID = userNotificationUrlID;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
        }

        //For SelectByID (RESULT)
        public UserNotificationUrlCatalog(int id, int userNotificationID, int userNotificationUrlID, int basicInfoID)
        {
            this.ID = id;
            this.UserNotificationID = userNotificationID;
            this.UserNotificationUrlID = userNotificationUrlID;
            this.BasicInfoID = basicInfoID;
        }

        public int DeleteByID()
        {
            using (SCC_DATA.Repositories.UserNotificationUrlCatalog repoUserNotificationUrlCatalog = new SCC_DATA.Repositories.UserNotificationUrlCatalog())
            {
                int response = repoUserNotificationUrlCatalog.DeleteByID(this.ID);

                this.BasicInfo.DeleteByID();

                return response;
            }
        }

        public int Insert()
        {
            this.BasicInfoID = this.BasicInfo.Insert();

            using (SCC_DATA.Repositories.UserNotificationUrlCatalog repoUserNotificationUrlCatalog = new SCC_DATA.Repositories.UserNotificationUrlCatalog())
            {
                this.ID = repoUserNotificationUrlCatalog.Insert(this.UserNotificationID, this.UserNotificationUrlID, this.BasicInfoID);

                return this.ID;
            }
        }

        public void SetDataByID()
        {
            using (SCC_DATA.Repositories.UserNotificationUrlCatalog repoUserNotificationUrlCatalog = new SCC_DATA.Repositories.UserNotificationUrlCatalog())
            {
                DataRow dr = repoUserNotificationUrlCatalog.SelectByID(this.ID);

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByID.ResultFields.ID]);
                this.UserNotificationID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByID.ResultFields.USERNOTIFICATIONID]);
                this.UserNotificationUrlID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByID.ResultFields.USERNOTIFICATIONURLID]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();
            }
        }

        public List<UserNotificationUrlCatalog> SelectByUserNotificationID()
        {
            List<UserNotificationUrlCatalog> userNotificationUrlCatalogist = new List<UserNotificationUrlCatalog>();

            using (SCC_DATA.Repositories.UserNotificationUrlCatalog repoUserNotificationUrlCatalog = new SCC_DATA.Repositories.UserNotificationUrlCatalog())
            {
                DataTable dt = repoUserNotificationUrlCatalog.SelectByUserNotificationID(this.UserNotificationID);

                foreach (DataRow dr in dt.Rows)
                {
                    UserNotificationUrlCatalog userNotificationUrlCatalog = new UserNotificationUrlCatalog(
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByUserNotificationID.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByUserNotificationID.ResultFields.USERNOTIFICATIONID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByUserNotificationID.ResultFields.USERNOTIFICATIONURLID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.UserNotificationUrlCatalog.StoredProcedures.SelectByUserNotificationID.ResultFields.BASICINFOID])
                    );

                    userNotificationUrlCatalog.BasicInfo = new BasicInfo(userNotificationUrlCatalog.BasicInfoID);
                    userNotificationUrlCatalog.BasicInfo.SetDataByID();

                    userNotificationUrlCatalogist.Add(userNotificationUrlCatalog);
                }
            }

            return userNotificationUrlCatalogist;
        }

        public int Update()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.UserNotificationUrlCatalog repoUserNotificationUrlCatalog = new SCC_DATA.Repositories.UserNotificationUrlCatalog())
            {
                return repoUserNotificationUrlCatalog.Update(this.ID, this.UserNotificationID, this.UserNotificationUrlID);
            }
        }

        public void Dispose()
        {
        }
    }
}