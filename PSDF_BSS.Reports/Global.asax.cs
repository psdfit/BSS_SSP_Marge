using System;
using System.Web.Http;
using System.Web.Routing;

namespace PSDF_BSS.Reports
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterComponents();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var p = Request.Path.ToLower().Trim();
            if (p.EndsWith("/crystalimagehandler.aspx") && p != "/crystalimagehandler.aspx")
            {
                var fullPath = Request.Url.AbsoluteUri.ToLower();
                var NewURL = fullPath.Replace(".aspx", "");
                Response.Redirect(NewURL);
            }
        }
    }
}