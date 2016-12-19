using Nakatomi.Contracts.Commands;
using Nakatomi.Messaging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MessageHost
{
    class MessageProducer
    {
        private IBus bus;

        public MessageProducer(IBus bus)
        {
            this.bus = bus;
        }

        public async Task ProduceMessages(int numberOfMessages)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Console.WriteLine($"Sending {numberOfMessages.ToString("N0")} Messages...");

            await Task.Run(() => Parallel.For(0, numberOfMessages, i =>
            {
                bus.Send(new ExecuteBuyOrder
                {
                    CustomerId = Guid.NewGuid(),
                    Symbol = "MSFT",
                    Timestamp = DateTimeOffset.UtcNow,
                    Quantity = new Random().Next(1000),
                    Price = 150.51m
                });
            }));

            stopWatch.Stop();
            
            Console.WriteLine($"{numberOfMessages} Messages sent in {(stopWatch.ElapsedMilliseconds.ToString("N0"))} milliseconds.");
        }
    }
}
