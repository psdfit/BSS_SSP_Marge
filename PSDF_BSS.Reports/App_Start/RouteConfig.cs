using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PSDF_BSS.Reports
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*(CrystalImageHandler).*" });

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute("ReportViewer", "ReportViewer/", "~/ReportViewer.aspx");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "LeaveRequest", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
           );
        }

    }
}