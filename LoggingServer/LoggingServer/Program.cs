using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LoggingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            var logFile = new StreamWriter("WebApp.log", true);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    
                    var message = Encoding.UTF8.GetString(body);
                    logFile.Write(message);
                    logFile.FlushAsync();
                };
                channel.BasicConsume(queue: "Logs",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}