using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MUGAnalyticsDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Updates()
        {
            var documentDBEndpointUrl = ConfigurationManager.AppSettings["Analytics.DocumentDB.Endpoint"];
            var documentDBAuthKey = ConfigurationManager.AppSettings["Analytics.DocumentDB.AccountKey"];

            var client = new DocumentClient(new Uri(documentDBEndpointUrl), documentDBAuthKey);

            var database = client.CreateDatabaseQuery().Where(db => db.Id == "analyticsdata").AsEnumerable().FirstOrDefault();

            var documentCollection = client.CreateDocumentCollectionQuery("dbs/" + database.Id).Where(c => c.Id == "demoCollection").AsEnumerable().FirstOrDefault();

            var results = client.CreateDocumentQuery(documentCollection.SelfLink, "SELECT c.count, c.UserName, c.Timestamp FROM c").ToList();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(results);

            return Content(string.Format("data: {0}\n\n", json), "text/event-stream");
        }
    }
}