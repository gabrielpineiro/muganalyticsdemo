using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUGAnalyticsDemo.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int numberOfMessages = 100;
            string connectionString = ConfigurationManager.AppSettings["Analytics.EventHub.ConnectionString"];
            string eventHubName = ConfigurationManager.AppSettings["Analytics.EventHub.Name"];
            int messageRetentionInDays = int.Parse(ConfigurationManager.AppSettings["Analytics.EventHub.MessageRetentionInDays"]);
            int partitionCount = int.Parse(ConfigurationManager.AppSettings["Analytics.EventHub.PartitionCount"]);
            string sharedAccessKeyName = ConfigurationManager.AppSettings["Analytics.EventHub.SharedAccessKeyName"];
            string sharedAccessKey = ConfigurationManager.AppSettings["Analytics.EventHub.SharedAccessKey"];
            string documentDBEndpointUrl = ConfigurationManager.AppSettings["Analytics.DocumentDB.Endpoint"];
            string documentDBAuthKey = ConfigurationManager.AppSettings["Analytics.DocumentDB.AccountKey"];

            var sender = new EventSender(connectionString, eventHubName, numberOfMessages);
            sender.SendEvents();
        }
    }
}
