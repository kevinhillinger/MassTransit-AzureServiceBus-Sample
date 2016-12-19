using System;

namespace Nakatomi.Transactions
{
    /// <summary>
    /// Marker for making a consumer transactional
    /// </summary>
    /// <remarks>
    /// This could be expanded to set the transaction attributes such as the isolation level and timeout value
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TransactionalAttribute : Attribute
    {
    }
}
