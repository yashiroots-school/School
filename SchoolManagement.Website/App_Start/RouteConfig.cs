using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SchoolManagement.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //        name: "StudentLogin",
            //        url: "{controller}/{action}/{id}",
            //        defaults: new { controller = "StudentLogin", action = "Index", id = UrlParameter.Optional }
            //);
            //routes.MapRoute(
            //      name: "Default",
            //      url: "{controller}/{action}/{id}",
            //      defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            //  );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
