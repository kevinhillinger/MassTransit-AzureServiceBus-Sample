using MassTransit;
using Nakatomi.Messaging.Topology;
using System.Threading;
using System.Threading.Tasks;

namespace Nakatomi.Messaging
{
    class InternalBus : IBus
    {
        IBusControl busControl;
        BusHandle busHandle;
        private IAddressProvider addressProvider;

        public string Name { get; }

        public InternalBus(string name, IBusControl busControl, IAddressProvider addressProvider)
        {
            Name = name;
            this.busControl = busControl;
            this.addressProvider = addressProvider;
        }

        public async Task Send<T>(object message, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var sender = await GetSender<T>();
            sender.ConnectSendObserver(new ErrorQueueConfig(addressProvider.GetAddress("error")));
            await sender.Send<T>(message, cancellationToken);
        }

        public async Task Send<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var sender = await GetSender<T>();
            sender.ConnectSendObserver(new ErrorQueueConfig(addressProvider.GetAddress("error")));
            await sender.Send(message, cancellationToken);
        }

        public async Task Start(CancellationToken cancellationToken = default(CancellationToken))
        {
            busHandle = await busControl.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task Stop(CancellationToken cancellationToken = default(CancellationToken))
        {
            await busHandle.StopAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task<ISendEndpoint> GetSender<T>()
        {
            var address = addressProvider.GetAddress<T>();
            return await busControl.GetSendEndpoint(address);
        }
    }
}
