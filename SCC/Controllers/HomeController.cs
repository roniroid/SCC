using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Controllers
{
    public class HomeController : OverallController
    {
        public ActionResult Index()
        {
            User currentUser = GetCurrentUser();

            List<UserNotification> userNotificationList = new List<UserNotification>();

            using (UserNotification userNotification = UserNotification.UserNotificationWithUserID(currentUser.ID))
            {
                userNotificationList = userNotification.SelectByUserID(currentUser.ID);
            }

            return View(userNotificationList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}