using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static SCC_BL.Settings.AppValues.ViewData.Role.Manage;
using static SCC_BL.Settings.AppValues.ViewData.User.AsignRolesAndPermissions;

namespace SCC_BL
{
    public class User : IDisposable
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public int LanguageID { get; set; }
        public bool HasPassPermission { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int BasicInfoID { get; set; }
        //--------------------------------------------------------------------------------------------------------
        public BasicInfo BasicInfo { get; set; }
        public string Token { get; set; }
        public Person Person { get; set; }
        public List<UserWorkspaceCatalog> UserWorkspaceCatalogList { get; set; } = new List<UserWorkspaceCatalog>();
        public List<UserSupervisorCatalog> SupervisorList { get; set; } = new List<UserSupervisorCatalog>();
        public List<UserRoleCatalog> RoleList { get; set; } = new List<UserRoleCatalog>();
        public List<UserPermissionCatalog> PermissionList { get; set; } = new List<UserPermissionCatalog>();
        public List<UserGroupCatalog> GroupList { get; set; } = new List<UserGroupCatalog>();
        public List<UserProgramCatalog> ProgramList { get; set; } = new List<UserProgramCatalog>();
        public List<UserProgramGroupCatalog> ProgramGroupList { get; set; } = new List<UserProgramGroupCatalog>();
        //--------------------------------------------------------------------------------------------------------
        public List<Permission> TotalPermissionList { get; set; } = new List<Permission>();
        public string RawPassword { get; set; }
        public List<Program> CurrentProgramList { get; set; } = new List<Program>();
        public List<Workspace> WorkspaceList { get; set; } = new List<Workspace>();
        //--------------------------------------------------------------------------------------------------------
        public int ExcelRowCount { get; set; } = 0;

        public User()
        {
        }

        //For CustomGetEmailByUsername and CustomGetSaltByUsername and SelectByUsername
        public User(string username)
        {
            this.Username = username;
        }

        //For CustomValidateLogIn
        public User(string username, byte[] password)
        {
            this.Username = username;
            this.Password = password;
        }

        //For password recovery
        public User(string username, string token)
        {
            this.Username = username;
            this.Token = token;
        }

        //For SelectByID and DeleteByID
        public User(int id)
        {
            this.ID = id;
        }

        //For Insert
        public User(int personID, string username, byte[] password, byte[] salt, string email, DateTime startDate, int languageID, bool hasPassPermission, DateTime lastLoginDate, int? creationUserID, int statusID)
        {
            this.PersonID = personID;
            this.Username = username;
            this.Password = password;
            this.Salt = salt;
            this.Email = email;
            this.StartDate = startDate;
            this.LanguageID = languageID;
            this.HasPassPermission = hasPassPermission;
            this.LastLoginDate = lastLoginDate;

            this.BasicInfo = new BasicInfo(creationUserID, statusID);
        }

        //For Update
        public User(int id, string username, string email, DateTime startDate, int languageID, bool hasPassPermission, int basicInfoID, int modificationUserID, int statusID)
        {
            this.ID = id;
            this.Username = username;
            this.Email = email;
            this.StartDate = startDate;
            this.LanguageID = languageID;
            this.HasPassPermission = hasPassPermission;

            this.BasicInfo = new BasicInfo(basicInfoID, modificationUserID, statusID);
        }

        //For SelectAll and SetDataByID and SelectByUsername (RESULT)
        public User(int id, int personID, string username, /*byte[] password, byte[] salt, */string email, DateTime startDate, int languageID, bool hasPassPermission, DateTime lastLoginDate, int basicInfoID)
        {
            this.ID = id;
            this.PersonID = personID;
            this.Username = username;
            /*this.Password = password;
			this.Salt = salt;*/
            this.Email = email;
            this.StartDate = startDate;
            this.LanguageID = languageID;
            this.HasPassPermission = hasPassPermission;
            this.LastLoginDate = lastLoginDate;
            this.BasicInfoID = basicInfoID;
        }

        public List<User> SelectByRoleID(int roleID, bool simplifiedData = false)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectByRoleID(roleID);

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.USERNAME]),
                        /*(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.PASSWORD]),
						(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.SALT]),*/
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        /*user.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(user.ID).SelectByUserID();
                        user.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(user.ID).SelectByUserID();
                        user.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(user.ID).SelectByUserID();
                        user.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(user.ID).SelectByUserID();
                        user.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(user.ID).SelectByUserID();*/

                        user.SetUserWorkspaceCatalogList();
                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();
                        user.SetProgramList();
                        user.SetProgramGroupList();
                        user.SetCurrentProgramList();
                        user.SetWorkspaceList();
                    }

                    userList.Add(user);
                }
            }

            /*return userList
				.OrderBy(o => new { o.Person.SurName, o.Person.FirstName })
				.ToList();*/

            return userList
                .OrderBy(o => o.Person.SurName)
                .ThenBy(o => o.Person.FirstName)
                .ToList();
        }

        public List<User> SelectByProgramID(int programID, bool simplifiedData = false, bool mustSetWorkspaces = false)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectByProgramID(programID);

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.USERNAME]),
                        /*(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.PASSWORD]),
						(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.SALT]),*/
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByProgramID.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        /*user.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(user.ID).SelectByUserID();
                        user.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(user.ID).SelectByUserID();
                        user.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(user.ID).SelectByUserID();
                        user.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(user.ID).SelectByUserID();
                        user.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(user.ID).SelectByUserID();*/

                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();
                        user.SetProgramList();
                        user.SetProgramGroupList();
                        user.SetCurrentProgramList();

                        mustSetWorkspaces = true;
                    }

                    if (mustSetWorkspaces)
                    {
                        user.SetUserWorkspaceCatalogList();
                        user.SetWorkspaceList();
                    }

                    userList.Add(user);
                }
            }

            /*return userList
				.OrderBy(o => new { o.Person.SurName, o.Person.FirstName })
				.ToList();*/

            return userList
                .OrderBy(o => o.Person.SurName)
                .ThenBy(o => o.Person.FirstName)
                .ToList();
        }

        public List<User> SelectByPermissionID(int permissionID, bool simplifiedData = false)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectByPermissionID(permissionID);

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.USERNAME]),
                        /*(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.PASSWORD]),
						(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.SALT]),*/
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByPermissionID.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        /*user.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(user.ID).SelectByUserID();
                        user.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(user.ID).SelectByUserID();
                        user.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(user.ID).SelectByUserID();
                        user.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(user.ID).SelectByUserID();
                        user.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(user.ID).SelectByUserID();*/

                        user.SetUserWorkspaceCatalogList();
                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();
                        user.SetProgramList();
                        user.SetProgramGroupList();
                        user.SetCurrentProgramList();
                        user.SetWorkspaceList();
                    }

                    userList.Add(user);
                }
            }

            /*return userList
				.OrderBy(o => new { o.Person.SurName, o.Person.FirstName })
				.ToList();*/

            return userList
                .OrderBy(o => o.Person.SurName)
                .ThenBy(o => o.Person.FirstName)
                .ToList();
        }

        public int[] SelectIDArrayByPermissionID(int permissionID)
        {
            int[] userIDArray = new int[0];

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectByPermissionID(permissionID);
                userIDArray = new int[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int userID = Convert.ToInt32(dt.Rows[i][SCC_DATA.Queries.User.StoredProcedures.SelectByRoleID.ResultFields.ID]);
                    userIDArray[i] = userID;
                }
            }

            return userIDArray;
        }

        public void SetUserWorkspaceCatalogList()
        {
            this.UserWorkspaceCatalogList = new List<UserWorkspaceCatalog>();
            this.UserWorkspaceCatalogList = (SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(this.ID)).SelectByUserID();
        }

        void SetSupervisorList()
        {
            this.SupervisorList = new List<UserSupervisorCatalog>();
            this.SupervisorList = (UserSupervisorCatalog.UserSupervisorCatalogWithUserID(this.ID)).SelectByUserID();
        }

        void SetRoleList()
        {
            this.RoleList = new List<UserRoleCatalog>();
            this.RoleList = (UserRoleCatalog.UserRoleCatalogWithUserID(this.ID)).SelectByUserID();
        }

        void SetPermissionList()
        {
            this.PermissionList = new List<UserPermissionCatalog>();
            this.PermissionList = (UserPermissionCatalog.UserPermissionCatalogWithUserID(this.ID)).SelectByUserID();
        }

        void SetGroupList()
        {
            this.GroupList = new List<UserGroupCatalog>();
            this.GroupList = (UserGroupCatalog.UserGroupCatalogWithUserID(this.ID)).SelectByUserID();
        }

        void SetProgramList()
        {
            this.ProgramList = new List<UserProgramCatalog>();
            this.ProgramList = (UserProgramCatalog.UserProgramCatalogWithUserID(this.ID)).SelectByUserID();
        }

        void SetProgramGroupList()
        {
            this.ProgramGroupList = new List<UserProgramGroupCatalog>();
            this.ProgramGroupList = (UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(this.ID)).SelectByUserID();
        }

        public void SetWorkspaceList()
        {
            this.WorkspaceList = new List<Workspace>();

            foreach (var userWorkspaceCatalog in this.UserWorkspaceCatalogList)
            {
                using (Workspace workspace = new Workspace(userWorkspaceCatalog.WorkspaceID))
                {
                    workspace.SetDataByID();
                    this.WorkspaceList.Add(workspace);

                }
            }
        }

        void SetCurrentProgramList()
        {
            this.CurrentProgramList = new List<Program>();

            List<Program> programList = new List<Program>();

            foreach (var userProgramCatalog in this.ProgramList)
            {
                Program tempProgram = new Program(userProgramCatalog.ProgramID);
                tempProgram.SetDataByID();

                if (!programList.Select(e => e.ID).Contains(tempProgram.ID))
                    programList.Add(tempProgram);
            }

            foreach (var userProgramGroupCatalog in this.ProgramGroupList)
            {
                ProgramGroup tempProgramGroup = new ProgramGroup(userProgramGroupCatalog.ProgramGroupID);
                tempProgramGroup.SetDataByID();

                foreach (ProgramGroupProgramCatalog programGroupProgramCatalog in tempProgramGroup.ProgramList)
                {
                    Program tempProgram = new Program(programGroupProgramCatalog.ProgramID);
                    tempProgram.SetDataByID();

                    if (!programList.Contains(tempProgram))
                        programList.Add(tempProgram);
                }
            }

            programList.OrderBy(e => e.Name);

            this.CurrentProgramList = programList;
        }

        void SetCurrentProgramListSync()
        {
            this.CurrentProgramList = new List<Program>();

            List<Program> programList = new List<Program>();

            foreach (UserProgramCatalog userProgramCatalog in this.ProgramList)
            {
                Program tempProgram = new Program(userProgramCatalog.ProgramID);
                tempProgram.SetDataByID();

                if (!programList.Select(e => e.ID).Contains(tempProgram.ID))
                    programList.Add(tempProgram);
            }

            foreach (UserProgramGroupCatalog userProgramGroupCatalog in this.ProgramGroupList)
            {
                ProgramGroup tempProgramGroup = new ProgramGroup(userProgramGroupCatalog.ProgramGroupID);
                tempProgramGroup.SetDataByID();

                foreach (ProgramGroupProgramCatalog programGroupProgramCatalog in tempProgramGroup.ProgramList)
                {
                    Program tempProgram = new Program(programGroupProgramCatalog.ProgramID);
                    tempProgram.SetDataByID();

                    if (!programList.Contains(tempProgram))
                        programList.Add(tempProgram);
                }
            }

            programList.OrderBy(e => e.Name);

            this.CurrentProgramList = programList;
        }

        public List<User> SelectEvaluatorList(bool simplifiedData = false)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectEvaluatorList();

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.USERNAME]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectEvaluatorList.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        /*user.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(user.ID).SelectByUserID();
                        user.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(user.ID).SelectByUserID();
                        user.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(user.ID).SelectByUserID();
                        user.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(user.ID).SelectByUserID();
                        user.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(user.ID).SelectByUserID();*/

                        user.SetUserWorkspaceCatalogList();
                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();
                        user.SetProgramList();
                        user.SetProgramGroupList();
                        user.SetCurrentProgramList();
                        user.SetWorkspaceList();
                    }

                    userList.Add(user);
                }
            }

            /*return userList
				.OrderBy(o => new { o.Person.SurName, o.Person.FirstName })
				.ToList();*/

            return userList
                .OrderBy(o => o.Person.SurName)
                .ThenBy(o => o.Person.FirstName)
                .ToList();
        }

        public List<User> SelectAll(bool simplifiedData = false, bool mustSetWorkspaces = false, bool mustSetPrograms = false)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectAll();

                //DataTable dt = Helpers.PersistentData.Users.GetAllUsers();

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.USERNAME]),
                        //(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.PASSWORD]),
                        //(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.SALT]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        //user.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(user.ID).SelectByUserID();
                        //user.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(user.ID).SelectByUserID();
                        //user.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(user.ID).SelectByUserID();
                        //user.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(user.ID).SelectByUserID();
                        //user.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(user.ID).SelectByUserID();
                        //user.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(user.ID).SelectByUserID();
                        //user.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(user.ID).SelectByUserID();

                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();

                        mustSetWorkspaces = true;
                        mustSetPrograms = true;
                    }

                    if (mustSetWorkspaces)
                    {
                        user.SetUserWorkspaceCatalogList();
                        user.SetWorkspaceList();
                    }

                    if (mustSetPrograms)
                    {
                        user.SetProgramList();
                        user.SetProgramGroupList();
                        user.SetCurrentProgramList();
                    }

                    userList.Add(user);
                }
            }

            return userList
                .OrderBy(o => o.Person.SurName)
                .ThenBy(o => o.Person.FirstName)
                .ToList();
        }

        /*public List<User>> SelectAll(bool simplifiedData = false, bool mustSetWorkspaces = false) {
			return SelectAllParallel(simplifiedData, mustSetWorkspaces);
		}*/

        public List<User> SelectAllParallel(bool simplifiedData = false, bool mustSetWorkspaces = false)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.SelectAll();

                foreach (DataRow dr in dt.Rows)
                {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.USERNAME]),
                        /*(byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.PASSWORD]),
                        (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.SALT]),*/
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        /*user.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(user.ID).SelectByUserID();
                        user.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(user.ID).SelectByUserID();
                        user.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(user.ID).SelectByUserID();
                        user.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(user.ID).SelectByUserID();
                        user.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(user.ID).SelectByUserID();
                        user.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(user.ID).SelectByUserID();*/

                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();
                        user.SetProgramList();
                        user.SetProgramGroupList();
                        user.SetCurrentProgramList();

                        mustSetWorkspaces = true;
                    }

                    if (mustSetWorkspaces)
                    {
                        user.SetUserWorkspaceCatalogList();
                        user.SetWorkspaceList();
                    }

                    userList.Add(user);
                }
            }

            return userList
                .OrderBy(o => o.Person.SurName)
                .ThenBy(o => o.Person.FirstName)
                .ToList();
        }

        /*public List<User>> SelectAllParallel(bool simplifiedData = false)
		{
			List<User> userList = new List<User>();

			using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
			{
                DataTable dt = repoUser.SelectAll();

                List<Task> tasks = new List<Task>();

				Parallel.ForEach<System.Data.DataRow>(dt.Rows.Cast<DataRow>().ToList(), async (dr) => {
                    User user = new User(
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.ID]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.PERSONID]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.USERNAME]),
                        Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.EMAIL]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.STARTDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.LANGUAGEID]),
                        Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.HASPASSPERMISSION]),
                        Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.LASTLOGINDATE]),
                        Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectAll.ResultFields.BASICINFOID])
                    );

                    user.BasicInfo = new BasicInfo(user.BasicInfoID);
                    user.BasicInfo.SetDataByID();

                    user.Person = new Person(user.PersonID);
                    user.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        user.SetUserWorkspaceCatalogList();
                        user.SetSupervisorList();
                        user.SetRoleList();
                        user.SetPermissionList();
                        user.SetGroupList();
                        user.SetProgramList();
                        user.SetProgramGroupList();

                        user.SetCurrentProgramList();
                        user.SetWorkspaceList();
                    }

                    userList.Add(user);

                    tasks.Add(Task.CompletedTask);
                });

                Task.WhenAll(tasks);
			}

			return userList
				.OrderBy(o => o.Person.SurName)
				.ThenBy(o => o.Person.FirstName)
				.ToList();
		}*/

        public void SetDataByID(bool simplifiedData = false)
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataRow dr = repoUser.SelectByID(this.ID);

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.ID]);
                this.PersonID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.PERSONID]);
                this.Username = Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.USERNAME]);
                /*this.Password = (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.PASSWORD]);
				this.Salt = (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.SALT]);*/
                this.Email = Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.EMAIL]);
                this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.STARTDATE]);
                this.LanguageID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.LANGUAGEID]);
                this.HasPassPermission = Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.HASPASSPERMISSION]);
                this.LastLoginDate = Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.LASTLOGINDATE]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByID.ResultFields.BASICINFOID]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();

                this.Person = new Person(this.PersonID);
                this.Person.SetDataByID();

                if (!simplifiedData)
                {
                    /*this.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(this.ID).SelectByUserID();
                    this.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(this.ID).SelectByUserID();
                    this.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(this.ID).SelectByUserID();
                    this.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(this.ID).SelectByUserID();
                    this.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(this.ID).SelectByUserID();
                    this.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(this.ID).SelectByUserID();
                    this.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(this.ID).SelectByUserID();*/

                    this.SetUserWorkspaceCatalogList();
                    this.SetSupervisorList();
                    this.SetRoleList();
                    this.SetPermissionList();
                    this.SetGroupList();
                    this.SetProgramList();
                    this.SetProgramGroupList();
                    this.SetCurrentProgramList();
                    this.SetWorkspaceList();
                }
            }
        }

        public void SetDataByName(string firstName, string surName, bool simplifiedData = false)
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataRow dr = repoUser.SelectByName(firstName, surName);

                if (dr == null)
                {
                    this.ID = -1;
                    return;
                }

                this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.ID]);
                this.PersonID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.PERSONID]);
                this.Username = Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.USERNAME]);
                /*this.Password = (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.PASSWORD]);
				this.Salt = (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.SALT]);*/
                this.Email = Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.EMAIL]);
                this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.STARTDATE]);
                this.LanguageID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.LANGUAGEID]);
                this.HasPassPermission = Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.HASPASSPERMISSION]);
                this.LastLoginDate = Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.LASTLOGINDATE]);
                this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByName.ResultFields.BASICINFOID]);

                this.BasicInfo = new BasicInfo(this.BasicInfoID);
                this.BasicInfo.SetDataByID();

                this.Person = new Person(this.PersonID);
                this.Person.SetDataByID();

                if (!simplifiedData)
                {
                    /*this.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(this.ID).SelectByUserID();
                    this.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(this.ID).SelectByUserID();
                    this.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(this.ID).SelectByUserID();
                    this.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(this.ID).SelectByUserID();
                    this.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(this.ID).SelectByUserID();
                    this.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(this.ID).SelectByUserID();
                    this.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(this.ID).SelectByUserID();*/

                    this.SetUserWorkspaceCatalogList();
                    this.SetSupervisorList();
                    this.SetRoleList();
                    this.SetPermissionList();
                    this.SetGroupList();
                    this.SetProgramList();
                    this.SetProgramGroupList();
                    this.SetCurrentProgramList();
                    this.SetWorkspaceList();
                }
            }
        }

        public void SetDataByUsername(bool simplifiedData = false)
        {
            try
            {
                using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
                {
                    DataRow dr = repoUser.SelectByUsername(this.Username);

                    if (dr == null)
                    {
                        this.ID = -1;
                        return;
                    }

                    this.ID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.ID]);
                    this.PersonID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.PERSONID]);
                    this.Username = Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.USERNAME]);
                    /*this.Password = (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.PASSWORD]);
					this.Salt = (byte[])(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.SALT]);*/
                    this.Email = Convert.ToString(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.EMAIL]);
                    this.StartDate = Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.STARTDATE]);
                    this.LanguageID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.LANGUAGEID]);
                    this.HasPassPermission = Convert.ToBoolean(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.HASPASSPERMISSION]);
                    this.LastLoginDate = Convert.ToDateTime(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.LASTLOGINDATE]);
                    this.BasicInfoID = Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.SelectByUsername.ResultFields.BASICINFOID]);

                    this.BasicInfo = new BasicInfo(this.BasicInfoID);
                    this.BasicInfo.SetDataByID();

                    this.Person = new Person(this.PersonID);
                    this.Person.SetDataByID();

                    if (!simplifiedData)
                    {
                        /*this.UserWorkspaceCatalogList = SCC_BL.UserWorkspaceCatalog.UserWorkspaceCatalogWithUserID(this.ID).SelectByUserID();
                        this.SupervisorList = UserSupervisorCatalog.UserSupervisorCatalogWithUserID(this.ID).SelectByUserID();
                        this.RoleList = UserRoleCatalog.UserRoleCatalogWithUserID(this.ID).SelectByUserID();
                        this.PermissionList = UserPermissionCatalog.UserPermissionCatalogWithUserID(this.ID).SelectByUserID();
                        this.GroupList = UserGroupCatalog.UserGroupCatalogWithUserID(this.ID).SelectByUserID();
                        this.ProgramList = UserProgramCatalog.UserProgramCatalogWithUserID(this.ID).SelectByUserID();
                        this.ProgramGroupList = UserProgramGroupCatalog.UserProgramGroupCatalogWithUserID(this.ID).SelectByUserID();*/

                        this.SetUserWorkspaceCatalogList();
                        this.SetSupervisorList();
                        this.SetRoleList();
                        this.SetPermissionList();
                        this.SetGroupList();
                        this.SetProgramList();
                        this.SetProgramGroupList();
                        this.SetCurrentProgramList();
                        this.SetWorkspaceList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetTotalPermissions()
        {
            List<Permission> permissionList = new List<Permission>();

            using (Permission permission = new Permission())
                permissionList = permission.SelectTotalPermissionsByUserID(this.ID);

            this.TotalPermissionList = permissionList;
        }

        public bool HasPermission(SCC_BL.DBValues.Catalog.Permission permission)
        {
            if (this.RoleList.Select(e => e.RoleID).Contains((int)SCC_BL.DBValues.Catalog.USER_ROLE.SUPERUSER)) return true;
            if (this.RoleList.Select(e => e.RoleID).Contains((int)SCC_BL.DBValues.Catalog.USER_ROLE.ADMINISTRATOR)) return true;

            return
                this.TotalPermissionList
                    .Select(e => e.ID)
                    .Contains((int)permission);
        }

        public bool HasRole(SCC_BL.DBValues.Catalog.USER_ROLE role)
        {
            return
                this.RoleList
                    .Select(e => e.RoleID)
                    .Contains((int)role);
        }

        public bool ValidateExcelData(DocumentFormat.OpenXml.Spreadsheet.Cell[] cells)
        {
            try
            {
                SCC_BL.Tools.ExcelParser excelParser = new Tools.ExcelParser();

                string excelFieldIdentification = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.IDENTIFICATION])).ToString().Trim();
                string excelFieldFirstName = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.FIRST_NAME])).ToString().Trim();
                string excelFieldSurName = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.SUR_NAME])).ToString().Trim();
                string excelFieldEmail = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.EMAIL])).ToString().Trim();
                /*string excelFieldLanguage = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.LANGUAGE]).ToString().Trim();
				string excelFieldWorkspace = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.WORKSPACE]).ToString().Trim();
				string excelFieldSupervisor = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.SUPERVISOR]).ToString().Trim();
				string excelFieldRole = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.ROLE]).ToString().Trim();
				string excelFieldGroup = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.GROUP]).ToString().Trim();
				string excelFieldProgram = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.PROGRAM]).ToString().Trim();
				string excelFieldHasPassPermission = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.HAS_PASS_PERMISSION]).ToString().Trim();
				string excelFieldStartDate = exce(excelParserlParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.START_DATE]).ToString().Trim();
				string excelFieldEndDate = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.END_DATE]).ToString().Trim();
				string excelFieldSupervisorStartDate = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MasiveImport.Fields.SUPERVISOR_START_DATE]).ToString().Trim();*/

                string surName = excelFieldSurName.Split(' ')[0];

                if (string.IsNullOrEmpty(excelFieldIdentification)) return false;
                if (string.IsNullOrEmpty(excelFieldFirstName)) return false;
                if (string.IsNullOrEmpty(surName)) return false;
                if (string.IsNullOrEmpty(excelFieldEmail)) return false;
                /*if (string.IsNullOrEmpty(excelFieldStartDate)) return false;
				if (string.IsNullOrEmpty(excelFieldEndDate)) return false;
				if (string.IsNullOrEmpty(excelFieldLanguage)) return false;
				if (string.IsNullOrEmpty(excelFieldWorkspace)) return false;
				if (string.IsNullOrEmpty(excelFieldSupervisor)) return false;
				if (string.IsNullOrEmpty(excelFieldSupervisorStartDate)) return false;
				if (string.IsNullOrEmpty(excelFieldRole)) return false;
				if (string.IsNullOrEmpty(excelFieldGroup)) return false;
				if (string.IsNullOrEmpty(excelFieldProgram)) return false;
				if (string.IsNullOrEmpty(excelFieldHasPassPermission)) return false;

				Convert.ToDateTime(excelFieldStartDate);
				Convert.ToDateTime(excelFieldEndDate);
				Convert.ToDateTime(excelFieldSupervisorStartDate);*/
            }
            catch (Exception ex)
            {
                String exceptionMessage = ex.ToString();
                return false;
            }

            return true;
        }

        public static User UserWithRow(DocumentFormat.OpenXml.Spreadsheet.Cell[] cells, int currentRowCount, int actualUserID, List<User> allUserList, List<Workspace> allWorkspaceList, List<Role> allRoleList, List<Group> allGroupList, List<Program> allProgramList, List<Catalog> allCountryList)
        {
            User result = new User();

            result.ExcelRowCount = currentRowCount;

            SCC_BL.Tools.ExcelParser excelParser = new Tools.ExcelParser();

            string excelFieldIdentification = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.IDENTIFICATION])).ToString().Trim();
            string excelFieldFirstName = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.FIRST_NAME])).ToString().Trim();
            string excelFieldSurName = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.SUR_NAME])).ToString().Trim();
            string excelFieldEmail = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.EMAIL])).ToString().Trim();
            string excelFieldStartDate = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.START_DATE])).ToString().Trim();
            string excelFieldEndDate = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.END_DATE])).ToString().Trim();
            string excelFieldLanguage = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.LANGUAGE])).ToString().Trim();
            string excelFieldWorkspace = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.WORKSPACE])).ToString().Trim();
            string excelFieldSupervisor = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.SUPERVISOR])).ToString().Trim();
            string excelFieldSupervisorStartDate = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.SUPERVISOR_START_DATE])).ToString().Trim();
            string excelFieldRole = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.ROLE])).ToString().Trim();
            string excelFieldGroup = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.GROUP])).ToString().Trim();
            string excelFieldProgram = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.PROGRAM])).ToString().Trim();
            string excelFieldHasPassPermission = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.HAS_PASS_PERMISSION])).ToString().Trim();
            string excelFieldCountry = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.COUNTRY])).ToString().Trim();
            string excelFieldWorkspaceStartDates = (excelParser.GetCellValue(cells[(int)SCC_BL.Settings.AppValues.ExcelTasks.User.MassiveImport.Fields.WORKSPACE_START_DATES])).ToString().Trim();

            if (result.ValidateExcelData(cells))
            {
                int personCountryID = 0;
                int defaultCountryID = (int)SCC_BL.DBValues.Catalog.PERSON_COUNTRY.COSTA_RICA;

                Catalog countryCatalogItem = new Catalog();

                try
                {
                    countryCatalogItem = allCountryList.Where(e => e.Description.Equals(excelFieldCountry)).FirstOrDefault();
                    personCountryID = countryCatalogItem.ID;
                }
                catch (Exception ex)
                {
                    String exceptionMessage = ex.ToString();
                    personCountryID = defaultCountryID;
                }

                Person person = new Person(excelFieldIdentification);

                Person newPerson =
                    new Person(
                        excelFieldIdentification,
                        excelFieldFirstName,
                        excelFieldSurName,
                        personCountryID,
                        actualUserID,
                        (int)SCC_BL.DBValues.Catalog.STATUS_PERSON.CREATED);

                result.Person = newPerson;

                byte[] salt = SCC_BL.Tools.Cryptographic.GenerateSalt();

                //result.RawPassword = SCC_BL.Settings.Overall.DEFAULT_PASSWORD;
                result.RawPassword = SCC_BL.Tools.Utils.GenerateRandomString();

                byte[] hashedPassword = SCC_BL.Tools.Cryptographic.HashPasswordWithSalt(System.Text.Encoding.UTF8.GetBytes(result.RawPassword), salt);

                Catalog languageCatalog = new Catalog(excelFieldLanguage);
                languageCatalog.SetDataByDescription();

                result.Username = newPerson.Identification;
                result.Password = hashedPassword;
                result.Salt = salt;
                result.Email = excelFieldEmail;

                DateTime currentUserStartDate = DateTime.Now;

                try
                {
                    string date = excelFieldStartDate.Split(' ')[0];
                    //string time = excelFieldStartDate.Split(' ')[1];

                    int year = Convert.ToInt32(date.Split('/')[2]);
                    int month = Convert.ToInt32(date.Split('/')[1]);
                    int day = Convert.ToInt32(date.Split('/')[0]);

                    /*int hour = Convert.ToInt32(time.Split(':')[0]);
                    int minute = Convert.ToInt32(time.Split(':')[1]);
                    int second = Convert.ToInt32(time.Split(':')[2]);*/

                    currentUserStartDate = new DateTime(year, month, day/*, hour, minute, second*/);
                }
                catch (Exception ex)
                {
                    String exceptionMessage = ex.ToString();
                }

                result.StartDate = currentUserStartDate;
                result.LanguageID = languageCatalog.ID > 0 ? languageCatalog.ID : (int)SCC_BL.DBValues.Catalog.USER_LANGUAGE.SPANISH;
                result.HasPassPermission = SCC_BL.Settings.AppValues.POSITIVE_ANSWERS.Contains(excelFieldHasPassPermission);
                result.LastLoginDate = DateTime.Now;

                result.BasicInfo = new BasicInfo(actualUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER.CREATED);

                string[] supervisorIdentifierList = System.Text.RegularExpressions.Regex.Split(excelFieldSupervisor, /*Environment.NewLine*/ "[,\r\n]").Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToArray();
                string[] workspaceIdentifierList = System.Text.RegularExpressions.Regex.Split(excelFieldWorkspace, "[,\r\n]").Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToArray();
                string[] roleIdentifierList = System.Text.RegularExpressions.Regex.Split(excelFieldRole, "[,\r\n]").Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToArray();
                string[] groupIdentifierList = System.Text.RegularExpressions.Regex.Split(excelFieldGroup, "[,\r\n]").Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToArray();
                string[] programIdentifierList = System.Text.RegularExpressions.Regex.Split(excelFieldProgram, "[,\r\n]").Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToArray();
                string[] workspaceStartDateList = System.Text.RegularExpressions.Regex.Split(excelFieldWorkspaceStartDates, "[,\r\n]").Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToArray();

                for (int i = 0; i < workspaceStartDateList.Length; i++)
                {
                    string resultDate = workspaceStartDateList[i];

                    try
                    {
                        resultDate = excelParser.FormatDate(resultDate);
                    }
                    catch (Exception ex)
                    {
                        string exceptionMessage = ex.ToString();
                    }

                    resultDate = resultDate.Trim();

                    workspaceStartDateList[i] = resultDate;
                }

                List<User> supervisorList = new List<User>();

                supervisorList =
                    allUserList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_USER.DISABLED &&
                            supervisorIdentifierList.Contains(e.Person.Identification))
                        .ToList();

                List<Workspace> workspaceList = new List<Workspace>();

                workspaceList =
                    allWorkspaceList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_WORKSPACE.DISABLED &&
                            (workspaceIdentifierList.Contains(e.Identifier) ||
                            workspaceIdentifierList.Contains(e.Name)))
                        .ToList();

                List<Role> roleList = new List<Role>();

                roleList =
                    allRoleList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_ROLE.DISABLED &&
                            (roleIdentifierList.Contains(e.Identifier) ||
                            roleIdentifierList.Contains(e.Name)))
                        .ToList();

                List<Group> groupList = new List<Group>();

                groupList =
                    allGroupList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_GROUP.DISABLED &&
                            groupIdentifierList.Contains(e.Name))
                        .ToList();

                List<Program> programList = new List<Program>();

                programList =
                    allProgramList
                        .Where(e =>
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DELETED &&
                            e.BasicInfo.StatusID != (int)SCC_BL.DBValues.Catalog.STATUS_PROGRAM.DISABLED &&
                            programIdentifierList.Contains(e.Name))
                        .ToList();

                result.SupervisorList =
                    supervisorList
                        .Select(e =>
                            new UserSupervisorCatalog(result.ID, e.ID, result.StartDate, result.BasicInfo.CreationUserID.Value, (int)SCC_BL.DBValues.Catalog.STATUS_USER_SUPERVISOR_CATALOG.CREATED))
                        .ToList();

                result.UserWorkspaceCatalogList = new List<UserWorkspaceCatalog>();

                for (int i = 0; i < workspaceIdentifierList.Length; i++)
                {
                    string currentWorkspaceIdentifier = workspaceIdentifierList[i];
                    DateTime currentWorkspaceStartDate = result.StartDate;

                    try
                    {
                        string date = workspaceStartDateList[i].Split(' ')[0];
                        string time = workspaceStartDateList[i].Split(' ')[1];

                        int year = Convert.ToInt32(date.Split('/')[2]);
                        int month = Convert.ToInt32(date.Split('/')[1]);
                        int day = Convert.ToInt32(date.Split('/')[0]);

                        int hour = Convert.ToInt32(time.Split(':')[0]);
                        int minute = Convert.ToInt32(time.Split(':')[1]);
                        int second = Convert.ToInt32(time.Split(':')[2]);

                        currentWorkspaceStartDate = new DateTime(year, month, day, hour, minute, second);
                    }
                    catch (Exception ex)
                    {
                        String exceptionMessage = ex.ToString();
                    }

                    Workspace auxWorkspace =
                        workspaceList
                            .Where(e =>
                                e.Identifier.Equals(currentWorkspaceIdentifier) ||
                                e.Name.Equals(currentWorkspaceIdentifier))
                            .FirstOrDefault();

                    if (auxWorkspace != null)
                    {
                        result.UserWorkspaceCatalogList.Add(
                            new UserWorkspaceCatalog(
                                result.ID,
                                auxWorkspace.ID,
                                currentWorkspaceStartDate,
                                result.BasicInfo.CreationUserID.Value,
                                (int)SCC_BL.DBValues.Catalog.STATUS_USER_WORKSPACE_CATALOG.CREATED)
                        );
                    }
                }

                result.RoleList = new List<UserRoleCatalog>();
                result.GroupList = new List<UserGroupCatalog>();
                result.ProgramList = new List<UserProgramCatalog>();

                roleList
                    .ForEach(async e =>
                        result.RoleList.Add(
                            UserRoleCatalog.UserRoleCatalogForInsert(
                                result.ID,
                                e.ID,
                                result.BasicInfo.CreationUserID.Value,
                                (int)SCC_BL.DBValues.Catalog.STATUS_USER_ROLE_CATALOG.CREATED)));

                groupList
                    .ForEach(async e =>
                        result.GroupList.Add(
                            UserGroupCatalog.UserGroupCatalogForInsert(
                                result.ID,
                                e.ID,
                                result.BasicInfo.CreationUserID.Value,
                                (int)SCC_BL.DBValues.Catalog.STATUS_USER_GROUP_CATALOG.CREATED)));

                programList
                    .ForEach(async e =>
                        result.ProgramList.Add(
                            UserProgramCatalog.UserProgramCatalogForInsert(
                                result.ID,
                                e.ID,
                                result.BasicInfo.CreationUserID.Value,
                                (int)SCC_BL.DBValues.Catalog.STATUS_USER_PROGRAM_CATALOG.CREATED)));


                /*Task.WhenAll(roleList.Select(async e =>
                {
                    result.RoleList.Add(UserRoleCatalog.UserRoleCatalogForInsert(result.ID, e.ID, result.BasicInfo.CreationUserID.Value, (int)SCC_BL.DBValues.Catalog.STATUS_USER_ROLE_CATALOG.CREATED));
                }));

                Task.WhenAll(groupList.Select(async e =>
                {
                    result.GroupList.Add(UserGroupCatalog.UserGroupCatalogForInsert(
                                result.ID,
                                e.ID,
                                result.BasicInfo.CreationUserID.Value,
                                (int)SCC_BL.DBValues.Catalog.STATUS_USER_GROUP_CATALOG.CREATED));
                }));

                Task.WhenAll(programList.Select(async e =>
                {
                    result.ProgramList.Add(UserProgramCatalog.UserProgramCatalogForInsert(result.ID, e.ID, result.BasicInfo.CreationUserID.Value, (int)SCC_BL.DBValues.Catalog.STATUS_USER_PROGRAM_CATALOG.CREATED));
                }));*/

                /*result.RoleList =
                    roleList
                        .Select(e =>
                            UserRoleCatalog.UserRoleCatalogForInsert(result.ID, e.ID, result.BasicInfo.CreationUserID.Value, (int)SCC_BL.DBValues.Catalog.STATUS_USER_ROLE_CATALOG.CREATED))
                        .ToList();

                result.GroupList =
                    groupList
                        .Select(e =>
                            UserGroupCatalog.UserGroupCatalogForInsert(result.ID, e.ID, result.BasicInfo.CreationUserID.Value, (int)SCC_BL.DBValues.Catalog.STATUS_USER_GROUP_CATALOG.CREATED))
                        .ToList();

                result.ProgramList =
                    programList
                        .Select(e =>
                            UserProgramCatalog.UserProgramCatalogForInsert(result.ID, e.ID, result.BasicInfo.CreationUserID.Value, (int)SCC_BL.DBValues.Catalog.STATUS_USER_PROGRAM_CATALOG.CREATED))
                        .ToList();*/
            }

            return result;
        }

        public string GetEmailByUsername()
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
                return repoUser.GetEmailByUsername(this.Username);
        }

        public byte[] GetSaltByUsername()
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
                return repoUser.GetSaltByUsername(this.Username);
        }

        public int ValidateLogIn()
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                int response = repoUser.ValidateLogIn(this.Username, this.Password);

                this.Salt =
                this.Password = null;

                return response;
            }
        }

        public int DeleteByID()
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                int response = repoUser.DeleteByID(this.ID);

                this.BasicInfo.DeleteByID();
                return response;
            }
        }

        public int Insert()
        {
            this.BasicInfoID = this.BasicInfo.Insert();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                this.ID = repoUser.Insert(this.PersonID, this.Username, this.Password, this.Salt, this.Email, this.StartDate, this.LanguageID, this.HasPassPermission, this.LastLoginDate, this.BasicInfoID);

                return this.ID;
            }
        }

        public int Update()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                return repoUser.Update(this.ID, this.Username, this.Email, this.StartDate, this.LanguageID, this.HasPassPermission);
            }
        }

        public Results.User.PasswordRecovery.CODE ProcessPasswordRecovery(string password, int modificationUserID, int statusID)
        {
            Results.User.PasswordRecovery.CODE result = Results.User.PasswordRecovery.CODE.ERROR;

            this.BasicInfo.ModificationUserID = modificationUserID;
            this.BasicInfo.StatusID = statusID;

            if (!string.IsNullOrEmpty(this.Token))
            {
                string generatedToken = GetToken();

                if (!generatedToken.Equals(this.Token))
                    return Results.User.PasswordRecovery.CODE.WRONG_TOKEN;
            }

            byte[] bytedPassword = Encoding.UTF8.GetBytes(password);

            this.Password = Tools.Cryptographic.HashPasswordWithSalt(bytedPassword, this.GetSaltByUsername());

            if (UpdatePassword() > 0)
                return Results.User.PasswordRecovery.CODE.SUCCESS;

            return result;
        }

        public string GetToken()
        {
            byte[] hashedUsername = Tools.Cryptographic.HashPasswordWithSalt(Encoding.UTF8.GetBytes(this.Username), GetSaltByUsername());

            byte[] tokenBytes = Tools.Cryptographic.HashPasswordWithSalt(hashedUsername, GetSaltByUsername());

            string generatedToken = string.Empty;

            tokenBytes
                .ToList()
                .ForEach(b =>
                    generatedToken += b.ToString());

            return generatedToken;
        }

        public int UpdatePassword()
        {
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
                return repoUser.UpdatePassword(this.ID, this.Password);
        }

        public int UpdateLastLogin(int modificationUserID)
        {
            this.BasicInfo.ModificationUserID = modificationUserID;
            this.BasicInfo.Update();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
                return repoUser.UpdateLastLogin(this.ID);
        }

        public Results.User.UpdateRoleList.CODE UpdateRoleList(int[] roleIDList, int creationUserID, bool keepOldOnes = false)
        {
            try
            {
                if (roleIDList == null) roleIDList = new int[0];

                //Delete old ones
                if (!keepOldOnes)
                {
                    this.RoleList
                        .ForEach(async e => {
                            if (!roleIDList.Contains(e.RoleID))
                                e.DeleteByID();
                        });
                }

                //Create new ones
                foreach (int roleID in roleIDList)
                {
                    if (!this.RoleList.Select(e => e.RoleID).Contains(roleID))
                    {
                        UserRoleCatalog userRoleCatalog = UserRoleCatalog.UserRoleCatalogForInsert(this.ID, roleID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_ROLE_CATALOG.CREATED);
                        userRoleCatalog.Insert();
                    }
                }

                return Results.User.UpdateRoleList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.User.UpdatePermissionList.CODE UpdatePermissionList(int[] permissionIDList, int creationUserID, bool keepOldOnes = false)
        {
            try
            {
                if (permissionIDList == null) permissionIDList = new int[0];

                //Delete old ones
                if (!keepOldOnes)
                {
                    this.PermissionList
                        .ForEach(async e => {
                            if (!permissionIDList.Contains(e.PermissionID))
                                e.DeleteByID();
                        });
                }

                //Create new ones
                foreach (int permissionID in permissionIDList)
                {
                    if (!this.PermissionList.Select(e => e.PermissionID).Contains(permissionID))
                    {
                        UserPermissionCatalog userPermissionCatalog = UserPermissionCatalog.UserPermissionCatalogForInsert(this.ID, permissionID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_PERMISSION_CATALOG.CREATED);
                        userPermissionCatalog.Insert();
                    }
                }

                return Results.User.UpdatePermissionList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.User.UpdateSupervisorList.CODE UpdateSupervisorList(int[] supervisorIDList, DateTime startDate, int creationUserID)
        {
            try
            {
                if (supervisorIDList == null) supervisorIDList = new int[0];

                //Delete old ones
                this.SupervisorList
                    .ForEach(async e => {
                        if (!supervisorIDList.Contains(e.SupervisorID))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach (int supervisorID in supervisorIDList)
                {
                    if (!this.SupervisorList.Select(e => e.SupervisorID).Contains(supervisorID))
                    {
                        UserSupervisorCatalog userSupervisorCatalog = new UserSupervisorCatalog(this.ID, supervisorID, startDate, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_SUPERVISOR_CATALOG.CREATED);
                        userSupervisorCatalog.Insert();
                    }
                }

                //Update old ones
                foreach (int supervisorID in supervisorIDList)
                {
                    if (this.SupervisorList.Select(e => e.SupervisorID).Contains(supervisorID))
                    {
                        UserSupervisorCatalog auxUserSupervisorCatalog =
                            this.SupervisorList
                                .Where(e =>
                                    e.SupervisorID == supervisorID)
                                .FirstOrDefault();

                        UserSupervisorCatalog userSupervisorCatalog = new UserSupervisorCatalog(
                            auxUserSupervisorCatalog.ID,
                            this.ID,
                            supervisorID,
                            startDate,
                            auxUserSupervisorCatalog.BasicInfoID,
                            creationUserID,
                            (int)SCC_BL.DBValues.Catalog.STATUS_USER_SUPERVISOR_CATALOG.UPDATED);

                        userSupervisorCatalog.Update();
                    }
                }

                return Results.User.UpdateSupervisorList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.User.UpdateWorkspaceList.CODE UpdateWorkspaceList(int[] workspaceIDList, DateTime startDate, int creationUserID)
        {
            try
            {
                if (workspaceIDList == null) workspaceIDList = new int[0];

                //Delete old ones
                this.UserWorkspaceCatalogList
                    .ForEach(async e => {
                        if (!workspaceIDList.Contains(e.WorkspaceID))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach (int workspaceID in workspaceIDList)
                {
                    if (!this.UserWorkspaceCatalogList.Select(e => e.WorkspaceID).Contains(workspaceID))
                    {
                        UserWorkspaceCatalog userWorkspaceCatalog = new UserWorkspaceCatalog(this.ID, workspaceID, startDate, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_WORKSPACE_CATALOG.CREATED);
                        userWorkspaceCatalog.Insert();
                    }
                }

                //Update old ones
                foreach (int workspaceID in workspaceIDList)
                {
                    if (this.UserWorkspaceCatalogList.Select(e => e.WorkspaceID).Contains(workspaceID))
                    {
                        UserWorkspaceCatalog auxUserWorkspaceCatalog =
                            this.UserWorkspaceCatalogList
                                .Where(e =>
                                    e.WorkspaceID == workspaceID)
                                .FirstOrDefault();

                        UserWorkspaceCatalog userWorkspaceCatalog = new UserWorkspaceCatalog(
                            auxUserWorkspaceCatalog.ID,
                            this.ID,
                            workspaceID,
                            startDate,
                            auxUserWorkspaceCatalog.BasicInfoID,
                            creationUserID,
                            (int)SCC_BL.DBValues.Catalog.STATUS_USER_WORKSPACE_CATALOG.UPDATED);

                        userWorkspaceCatalog.Update();
                    }
                }

                return Results.User.UpdateWorkspaceList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.User.UpdateGroupList.CODE UpdateGroupList(int[] groupIDList, int creationUserID)
        {
            try
            {
                if (groupIDList == null) groupIDList = new int[0];

                //Delete old ones
                this.GroupList
                    .ForEach(async e => {
                        if (!groupIDList.Contains(e.GroupID))
                            e.DeleteByID();
                    });

                //Create new ones
                foreach (int groupID in groupIDList)
                {
                    if (!this.GroupList.Select(e => e.GroupID).Contains(groupID))
                    {
                        UserGroupCatalog userGroupCatalog = UserGroupCatalog.UserGroupCatalogForInsert(this.ID, groupID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_GROUP_CATALOG.CREATED);
                        userGroupCatalog.Insert();
                    }
                }

                return Results.User.UpdateGroupList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.User.UpdateProgramList.CODE UpdateProgramList(int[] programIDList, int creationUserID, bool keepOldOnes = false)
        {
            try
            {
                if (programIDList == null) programIDList = new int[0];

                //Delete old ones
                //Disabled by request
                if (!keepOldOnes)
                {
                    this.ProgramList
                        .ForEach(async e => {
                            if (!programIDList.Contains(e.ProgramID))
                                e.DeleteByID();
                        });
                }

                //Create new ones
                foreach (int programID in programIDList)
                {
                    if (!this.ProgramList.Select(e => e.ProgramID).Contains(programID))
                    {
                        UserProgramCatalog userProgramCatalog = UserProgramCatalog.UserProgramCatalogForInsert(this.ID, programID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_PROGRAM_CATALOG.CREATED);
                        userProgramCatalog.Insert();
                    }
                }

                return Results.User.UpdateProgramList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Results.UserProgramGroupCatalog.UpdateProgramGroupList.CODE UpdateProgramGroupList(int[] programGroupIDList, int creationUserID, bool keepOldOnes = false)
        {
            try
            {
                if (programGroupIDList == null) programGroupIDList = new int[0];

                //Delete old ones
                //Disabled by request
                if (!keepOldOnes)
                {
                    this.ProgramGroupList
                        .ForEach(async e => {
                            if (!programGroupIDList.Contains(e.ProgramGroupID))
                                e.DeleteByID();
                        });
                }

                //Create new ones
                foreach (int programGroupID in programGroupIDList)
                {
                    if (!this.ProgramGroupList.Select(e => e.ProgramGroupID).Contains(programGroupID))
                    {
                        UserProgramGroupCatalog userProgramGroupCatalog = UserProgramGroupCatalog.UserProgramGroupCatalogForInsert(this.ID, programGroupID, creationUserID, (int)SCC_BL.DBValues.Catalog.STATUS_USER_PROGRAM_GROUP_CATALOG.CREATED);
                        userProgramGroupCatalog.Insert();
                    }
                }

                return Results.UserProgramGroupCatalog.UpdateProgramGroupList.CODE.SUCCESS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckExistence()
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                int response = repoUser.CheckExistence(this.Username);

                return response;
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

        public List<User> Search(SCC_BL.Helpers.User.Search.UserSearchHelper UserSearchHelper)
        {
            List<User> userList = new List<User>();

            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                DataTable dt = repoUser.Search(
                    UserSearchHelper.IdentificationTypeID,
                    UserSearchHelper.Identification,
                    UserSearchHelper.FirstNameTypeID,
                    UserSearchHelper.FirstName,
                    UserSearchHelper.SurNameTypeID,
                    UserSearchHelper.SurName,
                    UserSearchHelper.CountryIDList,
                    UserSearchHelper.LanguageIDList,

                    UserSearchHelper.UsernameTypeID,
                    UserSearchHelper.Username,
                    UserSearchHelper.EmailTypeID,
                    UserSearchHelper.Email,
                    UserSearchHelper.HasPassPermission,
                    UserSearchHelper.UserStatusIDList,
                    UserSearchHelper.GroupIDList,
                    UserSearchHelper.PermissionIDList,
                    UserSearchHelper.ProgramIDList,
                    UserSearchHelper.RoleIDList,
                    UserSearchHelper.SupervisorIDList,
                    UserSearchHelper.WorkspaceIDList);

                /*Parallel.ForEach<System.Data.DataRow>(dt.Rows.Cast<System.Data.DataRow>(), (row) => {
                    User User = new User(Convert.ToInt32(row[SCC_DATA.Queries.User.StoredProcedures.Search.ResultFields.UserID]));
                    User.SetDataByID();

                    UserList.Add(User);
                });*/

                foreach (DataRow dr in dt.Rows)
                {
                    User User = new User(Convert.ToInt32(dr[SCC_DATA.Queries.User.StoredProcedures.Search.ResultFields.ID]));
                    User.SetDataByID();

                    userList.Add(User);
                }
            }

            return userList;
        }
    }
}