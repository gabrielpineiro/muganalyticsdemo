using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MUGAnalyticsDemo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentClient Client;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var documentDBEndpointUrl = ConfigurationManager.AppSettings["Analytics.DocumentDB.Endpoint"];
            var documentDBAuthKey = ConfigurationManager.AppSettings["Analytics.DocumentDB.AccountKey"];

            Client = new DocumentClient(new Uri(documentDBEndpointUrl), documentDBAuthKey);
        }
    }
}
