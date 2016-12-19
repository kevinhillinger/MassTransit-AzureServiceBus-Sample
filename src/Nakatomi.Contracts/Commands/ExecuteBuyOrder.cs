using System;

namespace Nakatomi.Contracts.Commands
{
    public class ExecuteBuyOrder
    {
        public Guid CustomerId { get; set; }
        public string Symbol { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
