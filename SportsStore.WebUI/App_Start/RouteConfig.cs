using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //cải thiện Url của từng page vd: ?page=3 -> Page3
            //Lưu ý: hệ thống xử lý các route theo thứ tự từ trên xuống

            routes.MapRoute(
                null,
                "",
                new
                {
                    controller = "Product",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
                null,
                "Page{page}",
                new
                {
                    controller = "Product",
                    action = "List",
                    category = (string)null,
                },
                new { page = @"\d+" }
            );



            routes.MapRoute(
                name: null,
                url: "{category}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    page = 1
                }
            );

            routes.MapRoute(
                null,
                "{category}/Page{page}",
                new
                {
                    controller = "Product",
                    action = "List",

                },
                new { page = @"\d+" }
            );


            routes.MapRoute(
                name: null,
                url: "{controller}/{action}"

            );
        }
    }
}
