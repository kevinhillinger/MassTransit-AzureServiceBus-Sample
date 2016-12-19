using StructureMap;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Nakatomi.Messaging.Configuration;
using Nakatomi.Messaging;
using MessageHost.Configuration;

namespace MessageHost
{
    class Program
    {
        static IContainer container = new Container(c =>
        {
            c.AddRegistry<BusFactoryRegistry>();
            c.For<MessageProducer>().Use<MessageProducer>();
        });

        static IBus Bus;

        static void Main(string[] args)
        {
            MainAsync().Wait();

            Console.WriteLine("\nPress any key to close...\n");
            Console.ReadLine();

            //stop
            Console.WriteLine("Stopping bus....");
            Bus.Stop().Wait();

            Console.WriteLine("Bus stopped.");
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            try
            {
                var config = BusConfig.GetConfig(container);
                var factory = container.GetInstance<IBusFactory>();

                Bus = factory.Create(config);

                Console.WriteLine($"Starting bus: {Bus.Name}");
                await Bus.Start();
                Console.WriteLine($"Bus Started");

                //example of sending a message
                var producer = container.GetInstance<MessageProducer>();
                Console.WriteLine("Enter how many buy orders you want to execute:");
                var numberOfBuyOrders = Console.ReadLine();

                await producer.ProduceMessages(int.Parse(numberOfBuyOrders));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception was thrown: {e.Message}");
            }
        }
    }
}
