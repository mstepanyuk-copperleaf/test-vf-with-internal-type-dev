using System;
using System.IO;
using System.Reflection;

namespace MeasureFormula.UITests.Helpers
{
    public static class AssemblyPathManager
    {
        public static string SetupAssemblyPath()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            return assemblyPath == null ? string.Empty : new Uri(assemblyPath).LocalPath;
        }
    }
}
