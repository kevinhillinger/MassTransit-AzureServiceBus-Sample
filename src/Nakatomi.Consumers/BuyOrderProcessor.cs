using MassTransit;
using Nakatomi.Contracts.Commands;
using System;
using System.Threading.Tasks;

namespace Nakatomi.Consumers
{
    public class BuyOrderProcessor : IConsumer<ExecuteBuyOrder>
    {
        public Task Consume(ConsumeContext<ExecuteBuyOrder> context)
        {
            var message = context.Message;
            Console.WriteLine($"Sell Order processed. Customer: {message.CustomerId}, {message.Symbol} {message.Quantity} @ {message.Price} ");

            return Task.CompletedTask;
        }
    }
}
