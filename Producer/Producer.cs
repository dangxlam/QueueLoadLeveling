using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace MessageProvider
{
    class Program
    {
        static string connectionString = GetServiceBusConnectionString();
        static string  queueName = "thequeue";
        static ServiceBusClient client;
        static ServiceBusSender sender;
        static int index = 1;

        static void Main(string[] args)
        {
           UploadQueue(2);
        }

        public static void UploadQueue(int numBatch)
        {
            for (int i = 0; i < numBatch; i ++)
            {
                Console.Write($"Sending batch {i}");
                SendBatch(1000);
                Console.WriteLine($" - Done. Last Message - {index}"); 
            }
        }

        public static void SendBatch(int batchSize)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);
            using ServiceBusMessageBatch messageBatch = sender.CreateMessageBatchAsync().GetAwaiter().GetResult();

            for (int i = 0; i < batchSize; i++)
            {
                messageBatch.TryAddMessage(new ServiceBusMessage(JsonConvert.SerializeObject(new { id = index++})));
            }

            try
            {
                sender.SendMessagesAsync(messageBatch).GetAwaiter().GetResult();
            }
            finally
            {
                sender.DisposeAsync().GetAwaiter().GetResult();
                client.DisposeAsync().GetAwaiter().GetResult();
            }

        }
        static string GetServiceBusConnectionString()
        {
            // Read the JSON file
            string json = File.ReadAllText("config.json");

            // Parse the JSON
            JObject config = JObject.Parse(json);

            // Get the connection string
            string connectionString = config["ServiceBusConnectionString"].ToString();

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string is missing in the configuration file.");
            }

            return connectionString;
        }
    }
}