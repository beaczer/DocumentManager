using LastChance.Models.DocumentData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LastChance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
            //AppDomain.CurrentDomain.SetData("DataDirectory", Server.MapPath("~/App_Data/"));
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../App_Data"));
            //Database.SetInitializer(new
            //MigrateDatabaseToLatestVersion<DocumentDbContext, Migrations.Configuration>() );
            //// database not created yet

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
