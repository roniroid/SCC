using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace SCC_BL.Helpers.PersistentData
{
    public static class Users
    {
        private static System.Data.DataTable AllUsersData { get; set; }
        private static DateTime? LastUpdate { get; set; } = null;
        private const int TimeLapseInSeconds = 120;

        public static System.Data.DataTable GetAllUsers()
        {
            if (LastUpdate != null)
            {
                if (DateTime.Now > LastUpdate.Value.AddSeconds(TimeLapseInSeconds))
                {
                    UpdateAllUsers();
                    LastUpdate = DateTime.Now;
                }
            }
            else
            {
                UpdateAllUsers();
                LastUpdate = DateTime.Now;
            }

            return AllUsersData;
        }

        private static void UpdateAllUsers()
        {
            using (SCC_DATA.Repositories.User repoUser = new SCC_DATA.Repositories.User())
            {
                AllUsersData = repoUser.SelectAll();
            }
        }
    }
}
