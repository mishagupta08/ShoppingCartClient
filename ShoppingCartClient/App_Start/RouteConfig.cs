using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoppingCartClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "GetSelectedMenu",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "GetSelectedMenu", id = UrlParameter.Optional }
         );

            routes.MapRoute(
            name: "GetProductDetailView",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "GetProductDetailView", id = UrlParameter.Optional }
        );

            routes.MapRoute(
            name: "AddProductReview",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "AddProductReview", id = UrlParameter.Optional }
        ); 
        }
    }
}
