using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nakatomi.Utilities
{
    class AssemblyScanner : IAssemblyScanner
    {
        Assembly[] assemblies => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(p => Assembly.LoadFile(p)).ToArray();
        Assembly[] assembliesToScan;

        public AssemblyScanner(Func<Assembly, bool> assembliesToScan)
        {
            this.assembliesToScan = assemblies.Where(assembliesToScan).ToArray();
        }

        public IEnumerable<Type> Scan(Func<Type, bool> predicate)
        {
            return assembliesToScan.SelectMany(a => a.GetTypes().Where(t => predicate(t))).ToList();
        }
    }
}
