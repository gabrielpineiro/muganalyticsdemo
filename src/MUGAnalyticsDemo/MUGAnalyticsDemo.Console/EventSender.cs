using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUGAnalyticsDemo.Console
{
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }

    public class EventSender
    {
        const int numberOfDevices = 1000;
        string _eventHubName;
        string _connectionString;
        int _numberOfMessages;
        private static Random random = new Random((int)DateTime.Now.Ticks);

        private List<string> _users = new List<string>
        {
            "Leonardo", "Augusto", "Gabriel", "Alejandro", "Pedro", "Natalia", "Celeste", "Ruben", "Adrian", "Martin"
        };

        public EventSender(string connectionString, string eventHubName, int numberOfMessages)
        {
            _connectionString = connectionString;
            _eventHubName = eventHubName;
            _numberOfMessages = numberOfMessages;
        }

        public void SendEvents()
        {
            // Create EventHubClient
            var client = EventHubClient.CreateFromConnectionString(_connectionString, _eventHubName);

            // Send messages to Event Hub

            Random random = new Random();
            System.IO.MemoryStream ms;

            for (int i = 0; i < _numberOfMessages; ++i)
            {
                var randomUser = _users.PickRandom();

                var message = new
                {
                    UserName = randomUser,
                    id = Guid.NewGuid()
                };

                ms = new System.IO.MemoryStream();
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ms, Encoding.ASCII);

                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                sw.Write(jsonMessage);
                sw.Flush();
                ms.Position = 0;

                // Send the metric to Event Hub
                client.Send(new EventData(ms));
            }

            client.Close();
        }
    }
}
