using System;
using System.Threading.Tasks;
using MassTransit;

namespace Nakatomi.Messaging
{
    class ErrorQueueConfig : ISendObserver
    {
        private Uri faultAddress;

        public ErrorQueueConfig(Uri faultAddress)
        {
            this.faultAddress = faultAddress;
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            context.FaultAddress = faultAddress;
            return Task.CompletedTask;
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
