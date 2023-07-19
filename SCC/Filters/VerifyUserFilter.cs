using SCC_BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCC.Filters
{
    public class VerifyUserFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user = (User)HttpContext.Current.Session[SCC_BL.Settings.AppValues.Session.GLOBAL_ACTUAL_USER];

            if (user == null)
            {
                if (!(filterContext.Controller is Controllers.UserController))
                    filterContext.HttpContext.Response.Redirect(SCC_BL.Settings.Paths.User.LOGIN);
                else
                {
                    if (!SCC_BL.Settings.Paths.User.NotUserActionList.Contains(filterContext.ActionDescriptor.ActionName))
                        filterContext.HttpContext.Response.Redirect(SCC_BL.Settings.Paths.User.LOGIN);
                }
            }
            /*else
            {
                if (user.RoleID == (int)SCC_BL.DBValues.Catalog.Role.COLLABORATOR)
                {
                    bool found = false;

                    SCC_BL.Settings.Paths.User.NotAdminActionList
                        .ToList()
                        .ForEach(e => {
                            string protocolName = filterContext.Controller.ControllerContext.RequestContext.RouteData.Values.Values.FirstOrDefault().ToString();
                            if (e.ProtocolName.Equals(protocolName) && e.ActionName.Equals(filterContext.ActionDescriptor.ActionName))
                                found = true;
                        });

                    if (!found)
                        filterContext.HttpContext.Response.Redirect($"~/{ SCC_BL.Settings.Paths.User.NotAdminActionList[0].ProtocolName }/{ SCC_BL.Settings.Paths.User.NotAdminActionList[0].ActionName }");
                }
            }*/


            base.OnActionExecuting(filterContext);
        }
    }
}