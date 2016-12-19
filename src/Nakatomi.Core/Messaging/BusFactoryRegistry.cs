using StructureMap;

namespace Nakatomi.Messaging
{
    public sealed class BusFactoryRegistry : Registry
    {
        public BusFactoryRegistry()
        {
            For<IBusFactory>().Use<BusFactory>();
        }
    }
}
