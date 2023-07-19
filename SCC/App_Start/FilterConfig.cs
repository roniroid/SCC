using System.Web;
using System.Web.Mvc;

namespace SCC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SCC.Filters.VerifyUserFilter());
        }
    }
}
