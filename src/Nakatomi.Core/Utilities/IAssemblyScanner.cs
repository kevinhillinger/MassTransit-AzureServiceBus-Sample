using System;
using System.Collections.Generic;

namespace Nakatomi.Utilities
{
    interface IAssemblyScanner
    {
        IEnumerable<Type> Scan(Func<Type, bool> predicate);
    }
}
