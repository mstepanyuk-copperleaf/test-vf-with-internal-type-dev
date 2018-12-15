using System;
using System.IO;
using System.Reflection;

namespace MeasureFormula.UITests.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// A method that returns a full path for a files located in 'tools' folder
        /// </summary>
        public static string GenerateFullFilePath(this string fileName)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (assemblyPath == null)
            {
                return string.Empty;
            }
            var localPath = new Uri(assemblyPath).LocalPath;
            return new FileInfo(Path.Combine(localPath, @"..\..\..\tools\" + fileName)).FullName;
        }
    }
}
