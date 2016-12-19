using MassTransit;
using Nakatomi.Contracts.Commands;
using Nakatomi.Transactions;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Nakatomi.Consumers
{
    /// <summary>
    /// Example of a transactional consumer, using the [Transactional] marker attribute on the consumer
    /// </summary>
    [Transactional]
    class SellOrderProcessor : IConsumer<ExecuteSellOrder>
    {
        public Task Consume(ConsumeContext<ExecuteSellOrder> context)
        {
            //transactional
            TransactionContext transactionContext;
            if (context.TryGetPayload(out transactionContext))
            {
                using (var transaction = new TransactionScope(transactionContext.Transaction))
                {
                    var message = context.Message;
                    Console.WriteLine($"Sell Order processed. Customer: {message.CustomerId}, {message.Symbol} {message.Quantity} @ {message.Price} at " + DateTime.Now);
                }
            }
            return Task.CompletedTask;
        }
    }
}
