using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SCC_BL
{
    public class Calibration : IDisposable
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int TypeID { get; set; }
        public int ExperiencedUserID { get; set; }
        public bool HasNotificationToBeSent { get; set; }
        public int BasicInfoID { get; set; }
        //----------------------------------------------------
        public BasicInfo BasicInfo { get; set; }

        public List<CalibrationUserCatalog> CalibrationUserCatalogList { get; set; } = new List<CalibrationUserCatalog>();
        public List<CalibrationGroupCatalog> CalibratorUserGroupList { get; set; } = new List<CalibrationGroupCatalog>();
        public List<CalibrationTransactionCatalog> TransactionList { get; set; } = new List<CalibrationTransactionCatalog>();
        public List<CalibrationCallIdentifierCatalog> CallIdentifierList { get; set; } = new List<CalibrationCallIdentifierCatalog>();
        //----------------------------------------------------
        public User ExperiencedUser { get; set; }
        public List<User> CalibratorUserList { get; set; } = new List<User>();
        public string TypeDescription { get; set; }
        public string StatusDescription { get; set; }
        public List<Transaction> CalibrationList { get; set; } = new List<Transaction>();
        public int[] ProgramIDArray = null;

        public Calibration()
        {
        }

        //For SelectByID and DeleteByID
        public Calibration(int id)
        {
            this.ID = id;
        }

        //For Insert
        public Calibration(DateTime startDate, DateTime endDate, string description, int typeID, int experiencedUserID, bool hasNotificationToBeSent, int creationUserID, int statusID)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Description = description;
            this.TypeID = typeID;
            this.ExperiencedUserID = experiencedUserID;
            this.HasNotificationToBeSent = hasNotificationToBeSent;

            this.BasicInfo = new BasicInfo(creationUserID, statusID);
        }

        //For Update
        public Calibration(int id, DateTime startDate, DateTime endDate, string description, int typeID, int experiencedUserID, bool hasNotificationToBeSent, int basicInfoID, int modificationUserID, int statusID)
        {
            this.ID = id;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Description = description;
            this.TypeID = typeID;
            this.ExperiencedUserID = experiencedUserID;
            this.HasNotificationToBeSent = hasNotificationToBeSent;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
        }

        //For SelectByID (RESULT) AND SelectAll
        public Calibration(int id, DateTime startDate, DateTime endDate, string description, int typeID, int experiencedUserID, bool hasNotificationToBeSent, int basicInfoID)
        {
            this.ID = id;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Description = description;
            this.TypeID = typeID;
            this.ExperiencedUserID = experiencedUserID;
            this.HasNotificationToBeSent = hasNotificationToBeSent;
            this.BasicInfoID = basicInfoID;
        }

        public int DeleteByID()
        {
            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                int response = repoCalibration.DeleteByID(this.ID);

                this.BasicInfo.DeleteByID();
                return response;
            }
        }

        public int Insert()
        {
            this.BasicInfoID = this.BasicInfo.Insert();

            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                this.ID = repoCalibration.Insert(this.StartDate, this.EndDate, this.Description, this.TypeID, this.ExperiencedUserID, this.HasNotificationToBeSent, this.BasicInfoID);

                return this.ID;
            }
        }

        public void SetDataByID()
        {
            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                DataRow dr = repoCalibration.SelectByID(this.ID);

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.ID]);
                this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.STARTDATE]);
                this.EndDate = Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.ENDDATE]);
                this.Description = Convert.ToString(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.DESCRIPTION]);
                this.TypeID = Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.TYPEID]);
                this.ExperiencedUserID = Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.EXPERIENCEDUSERID]);
                this.HasNotificationToBeSent = Convert.ToBoolean(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.HASNOTIFICATIONTOBESENT]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();

                this.SetCalibrationUserCatalogList();
                this.SetCalibratorUserGroupList();
                this.SetTransactionList();
                this.SetCallIdentifierList();
                this.SetExperiencedUser();
                this.SetCalibratorUserList();
                this.SetTypeDescription();
                this.SetStatusDescription();
                this.SetCalibrationList();
            }
        }

        void SetCalibratorUserGroupList()
        {
            this.CalibratorUserGroupList =
                CalibrationGroupCatalog.CalibrationGroupCatalogWithCalibrationID(this.ID).SelectByCalibrationID();
        }

        void SetTransactionList()
        {
            this.TransactionList = CalibrationTransactionCatalog.CalibrationTransactionCatalogWithCalibrationID(this.ID).SelectByCalibrationID();
        }

        void SetCallIdentifierList()
        {
            this.CallIdentifierList = CalibrationCallIdentifierCatalog.CalibrationCallIdentifierCatalogWithCalibrationID(this.ID).SelectByCalibrationID();
        }

        void SetCalibrationUserCatalogList()
        {
            this.CalibrationUserCatalogList = CalibrationUserCatalog.CalibrationUserCatalogWithCalibrationID(this.ID).SelectByCalibrationID();
        }

        void SetExperiencedUser()
        {
            using (User experiencedUser = new User(this.ExperiencedUserID))
            {
                experiencedUser.SetDataByID();
                this.ExperiencedUser = experiencedUser;
            }
        }

        void SetCalibrationList(bool simpleData = false)
        {
            foreach (CalibrationTransactionCatalog calibrationTransactionCatalog in this.TransactionList)
            {
                using (Transaction transaction = Transaction.TransactionWithCalibratedTransactionID(calibrationTransactionCatalog.TransactionID))
                {
                    List<Transaction> calibrationList = transaction.SelectByCalibratedTransactionID(simpleData);

                    this.CalibrationList.AddRange(calibrationList.Where(e => !this.CalibrationList.Select(s => s.ID).Contains(e.ID)));
                }
            }

            foreach (CalibrationCallIdentifierCatalog calibrationCallIdentifierCatalog in this.CallIdentifierList)
            {
                Transaction auxTransaction = new Transaction();

                int? transactionID = auxTransaction.GetIdByCallIdentifier(calibrationCallIdentifierCatalog.CallIdentifier);

                if (transactionID == null)
                    continue;

                if (this.TransactionList.Select(e => e.TransactionID).Contains(transactionID.Value))
                    continue;

                auxTransaction = new Transaction(transactionID.Value);
                auxTransaction.SetDataByID();

                auxTransaction.SetCallIdentifier();

                this.CalibrationList.Add(auxTransaction);
            }
        }

        void SetTypeDescription()
        {
            using (Catalog catalog = new Catalog(this.TypeID))
            {
                catalog.SetDataByID();
                this.TypeDescription = catalog.Description;
            }
        }

        void SetStatusDescription()
        {
            using (Catalog catalog = new Catalog(this.BasicInfo.StatusID))
            {
                catalog.SetDataByID();
                this.StatusDescription = catalog.Description;
            }
        }

        void SetCalibratorUserList()
        {
            this.CalibratorUserList = this.GetUserList();
        }

        public List<Calibration> SelectAll()
        {
            List<Calibration> calibrationList = new List<Calibration>();

            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                DataTable dt = repoCalibration.SelectAll();

                foreach (DataRow dr in dt.Rows)
                {
                    Calibration calibration = new Calibration(
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.ID]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.STARTDATE]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.ENDDATE]),
                        Convert.ToString(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.DESCRIPTION]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.TYPEID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.EXPERIENCEDUSERID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.HASNOTIFICATIONTOBESENT]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
                    );

                    calibration.BasicInfo = new BasicInfo(calibration.BasicInfoID);
                    calibration.BasicInfo.SetDataByID();

                    calibration.SetCalibrationUserCatalogList();
                    calibration.SetCalibratorUserGroupList();
                    calibration.SetTransactionList();
                    calibration.SetCallIdentifierList();
                    calibration.SetExperiencedUser();
                    calibration.SetCalibratorUserList();
                    calibration.SetTypeDescription();
                    calibration.SetStatusDescription();
                    calibration.SetCalibrationList(true);

                    calibration.SetProgramIDArray();

                    calibrationList.Add(calibration);
                }
            }

            return calibrationList
                .OrderByDescending(o => o.BasicInfo.CreationDate)
                .ToList();
        }

        public List<Calibration> SelectByProgramID(int programID)
        {
            List<Calibration> calibrationList = new List<Calibration>();

            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                DataTable dt = repoCalibration.SelectByProgramID(programID);

                foreach (DataRow dr in dt.Rows)
                {
                    Calibration calibration = new Calibration(
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.ID]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.STARTDATE]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.ENDDATE]),
                        Convert.ToString(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.DESCRIPTION]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.TYPEID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.EXPERIENCEDUSERID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.HASNOTIFICATIONTOBESENT]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByProgramID.ResultFields.BASICINFOID])
                    );

                    calibration.BasicInfo = new BasicInfo(calibration.BasicInfoID);
                    calibration.BasicInfo.SetDataByID();

                    calibration.SetCalibrationUserCatalogList();
                    calibration.SetCalibratorUserGroupList();
                    calibration.SetTransactionList();
                    calibration.SetCallIdentifierList();
                    calibration.SetExperiencedUser();
                    calibration.SetCalibratorUserList();
                    calibration.SetTypeDescription();
                    calibration.SetStatusDescription();
                    calibration.SetCalibrationList(true);

                    calibration.SetProgramIDArray();

                    calibrationList.Add(calibration);
                }
            }

            return calibrationList
                .OrderByDescending(o => o.BasicInfo.CreationDate)
                .ToList();
        }

        public List<Calibration> SelectByUserID(int userID)
        {
            List<Calibration> calibrationList = new List<Calibration>();

            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                DataTable dt = repoCalibration.SelectByUserID(userID);

                foreach (DataRow dr in dt.Rows)
                {
                    Calibration calibration = new Calibration(
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.ID]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.STARTDATE]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.ENDDATE]),
                        Convert.ToString(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.DESCRIPTION]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.TYPEID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.EXPERIENCEDUSERID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.HASNOTIFICATIONTOBESENT]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.Calibration.StoredProcedures.SelectByUserID.ResultFields.BASICINFOID])
                    );

                    calibration.BasicInfo = new BasicInfo(calibration.BasicInfoID);
                    calibration.BasicInfo.SetDataByID();

                    calibration.SetCalibrationUserCatalogList();
                    calibration.SetCalibratorUserGroupList();
                    calibration.SetTransactionList();
                    calibration.SetCallIdentifierList();
                    calibration.SetExperiencedUser();
                    calibration.SetCalibratorUserList();
                    calibration.SetTypeDescription();
                    calibration.SetStatusDescription();
                    calibration.SetCalibrationList(true);

                    calibration.SetProgramIDArray();

                    calibrationList.Add(calibration);
                }
            }

            return calibrationList
                .OrderByDescending(o => o.BasicInfo.CreationDate)
                .ToList();
        }

        void SetProgramIDArray()
        {
            int[] transactionIDArray = this.TransactionList.Select(e => e.TransactionID).ToArray();
            List<int> programIDList = new List<int>();

            for (int i = 0; i < transactionIDArray.Length; i++)
            {
                using (Transaction transaction = new Transaction(transactionIDArray[i]))
                {
                    int currentProgramID = transaction.GetProgramID();

                    if (!programIDList.Contains(currentProgramID))
                        programIDList.Add(currentProgramID);
                }
            }

            this.ProgramIDArray = programIDList.ToArray();
        }

        public int Update()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.Calibration repoCalibration = new SCC_DATA.Repositories.Calibration())
            {
                return repoCalibration.Update(this.ID, this.StartDate, this.EndDate, this.Description, this.TypeID, this.ExperiencedUserID, this.HasNotificationToBeSent);
            }
        }

        public List<User> GetUserList()
        {
            List<int> calibratorIDList = new List<int>();
            List<User> calibratorUserList = new List<User>();

            foreach (CalibrationUserCatalog calibrationUserCatalog in this.CalibrationUserCatalogList)
            {
                if (!calibratorIDList.Contains(calibrationUserCatalog.UserID))
                    calibratorIDList.Add(calibrationUserCatalog.UserID);
            }

            foreach (CalibrationGroupCatalog calibrationGroupCatalog in this.CalibratorUserGroupList)
            {
                UserGroupCatalog tempUserGroupCatalog = UserGroupCatalog.UserGroupCatalogWithGroupID(calibrationGroupCatalog.GroupID);
                List<UserGroupCatalog> tempUserGroupCatalogList = tempUserGroupCatalog.SelectByGroupID();

                foreach (UserGroupCatalog userGroupCatalog in tempUserGroupCatalogList)
                {
                    if (!calibratorIDList.Contains(userGroupCatalog.UserID))
                        calibratorIDList.Add(userGroupCatalog.UserID);
                }
            }

            foreach (int userID in calibratorIDList)
            {
                using (User user = new User(userID))
                {
                    user.SetDataByID();
                    calibratorUserList.Add(user);
                }
            }

            return calibratorUserList;
        }

        public Results.Calibration.UpdateCalibratorUserList.CODE UpdateCalibratorUserList(int[] calibratorUserIDList, int creationUserID)
        {
            try
            {
                if (calibratorUserIDList == null) calibratorUserIDList = new int[0];

                //Delete old ones
                this.CalibrationUserCatalogList
                    .ForEach(async e => {
                        if (!calibratorUserIDList.Contains(e.UserID))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach (int userID in calibratorUserIDList)
                {
                    if (!this.CalibrationUserCatalogList.Select(e => e.UserID).Contains(userID))
                    {
                        CalibrationUserCatalog calibrationUserCatalog = CalibrationUserCatalog.CalibrationUserCatalogForInsert(this.ID, userID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION_USER_CATALOG.CREATED);
                        calibrationUserCatalog.Insert();
                    }
                }

                return Results.Calibration.UpdateCalibratorUserList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.Calibration.UpdateCalibratorUserGroupList.CODE UpdateCalibratorUserGroupList(int[] calibratorUserGroupIDList, int creationUserID)
        {
            try
            {
                if (calibratorUserGroupIDList == null) calibratorUserGroupIDList = new int[0];

                //Delete old ones
                this.CalibratorUserGroupList
                    .ForEach(async e => {
                        if (!calibratorUserGroupIDList.Contains(e.GroupID))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach (int userGroupID in calibratorUserGroupIDList)
                {
                    if (!this.CalibratorUserGroupList.Select(e => e.GroupID).Contains(userGroupID))
                    {
                        CalibrationGroupCatalog calibrationUserCatalog = CalibrationGroupCatalog.CalibrationGroupCatalogForInsert(this.ID, userGroupID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION_GROUP_CATALOG.CREATED);
                        calibrationUserCatalog.Insert();
                    }
                }

                return Results.Calibration.UpdateCalibratorUserGroupList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.Calibration.UpdateTransactionList.CODE UpdateTransactionList(int[] transactionIDList, int creationUserID)
        {
            try
            {
                if (transactionIDList == null) transactionIDList = new int[0];

                //Delete old ones
                this.TransactionList
                    .ForEach(async e => {
                        if (!transactionIDList.Contains(e.TransactionID))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach (int transactionID in transactionIDList)
                {
                    if (!this.TransactionList.Select(e => e.TransactionID).Contains(transactionID))
                    {
                        CalibrationTransactionCatalog calibrationTransactionCatalog = CalibrationTransactionCatalog.CalibrationTransactionCatalogForInsert(this.ID, transactionID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION_TRANSACTION_CATALOG.CREATED);
                        calibrationTransactionCatalog.Insert();
                    }
                }

                return Results.Calibration.UpdateTransactionList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*public async Task<Results.Calibration.UpdateCallIdentifierList.CODE> UpdateCallIdentifierList(string[] callIdentifierList, int creationUserID)
		{
			try
			{
				if (callIdentifierList == null) callIdentifierList = new string[0];

				//Delete old ones
				this.CallIdentifierList
					.ForEach(async e => {
						if (!callIdentifierList.Contains(e.CallIdentifier))
							await e.DeleteByID();
					});

				//Create new ones
				foreach (string currentCallIdentifier in callIdentifierList)
				{
					if (!this.CallIdentifierList.Select(e => e.CallIdentifier).ToList().Contains(currentCallIdentifier))
					{
						CalibrationCallIdentifierCatalog calibrationCallIdentifierCatalog = await CalibrationCallIdentifierCatalog.CalibrationCallIdentifierCatalogForInsert(this.ID, currentCallIdentifier, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION_TRANSACTION_CATALOG.CREATED);
						await calibrationCallIdentifierCatalog.Insert();
					}
				}

				return Results.Calibration.UpdateCallIdentifierList.CODE.SUCCESS;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/

        public Results.Calibration.UpdateCallIdentifierList.CODE UpdateCallIdentifierList((string, int)[] callIdentifierList, int creationUserID)
        {
            try
            {
                if (callIdentifierList == null) callIdentifierList = new (string, int)[0];

                //Delete old ones
                this.CallIdentifierList
                    .ForEach(async e => {
                        if (!callIdentifierList.Select(s => s.Item1).Contains(e.CallIdentifier))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach ((string, int) currentCallIdentifier in callIdentifierList)
                {
                    if (!this.CallIdentifierList.Select(e => e.CallIdentifier).ToList().Contains(currentCallIdentifier.Item1))
                    {
                        CalibrationCallIdentifierCatalog calibrationCallIdentifierCatalog = CalibrationCallIdentifierCatalog.CalibrationCallIdentifierCatalogForInsert(
                            this.ID,
                            currentCallIdentifier.Item1,
                            currentCallIdentifier.Item2,
                            creationUserID,
                            (int)SCC_BL.DBValues.Catalog.STATUS_CALIBRATION_TRANSACTION_CATALOG.CREATED);
                        calibrationCallIdentifierCatalog.Insert();
                    }
                }

                return Results.Calibration.UpdateCallIdentifierList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
        }
    }
}