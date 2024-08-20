using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Helpers.User.Search
{
    public class UserSearchHelper
    {
        public int? IdentificationTypeID { get; set; }
        public string Identification { get; set; }
        public int? FirstNameTypeID { get; set; }
        public string FirstName { get; set; }
        public int? SurNameTypeID { get; set; }
        public string SurName { get; set; }
        public int[] CountryIDList { get; set; }
        public int[] LanguageIDList { get; set; }


        public int? UsernameTypeID { get; set; }
        public string Username { get; set; }
        public int? EmailTypeID { get; set; }
        public string Email { get; set; }
        public bool? HasPassPermission { get; set; }
        public bool? IsActive { get; set; }
        public int[] UserStatusIDList { get; set; }
        public int[] GroupIDList { get; set; }
        public int[] PermissionIDList { get; set; }
        public int[] ProgramIDList { get; set; }
        public int[] RoleIDList { get; set; }
        public int[] SupervisorIDList { get; set; }
        public int[] WorkspaceIDList { get; set; }
    }
}
