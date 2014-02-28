using System;
using System.IO;

namespace Utility
{
    public class Utility
    {
        public static string FindAssemblyFile(string filePath)
        {
            if (File.Exists(filePath))
                return filePath;

            var fileName = Path.GetFileName(filePath);
            if (File.Exists(fileName))
                return fileName;

            var folder = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var checkPath = Path.Combine(folder, filePath);
            if (File.Exists(checkPath))
                return checkPath;

            checkPath = Path.Combine(folder, fileName);
            if (File.Exists(checkPath))
                return checkPath;

            folder = System.Reflection.Assembly.GetCallingAssembly().Location;
            checkPath = Path.Combine(folder, fileName);
            if (File.Exists(checkPath))
                return checkPath;

            checkPath = Path.Combine(folder, fileName);
            if (File.Exists(checkPath))
                return checkPath;

            return null;
            //throw new Exception(string.Format("Missing {0}", filePath));
        }

        /// <summary>
        /// Loads a resource from file if found, or from embedded resource
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="resourceLookupFunction"></param>
        /// <returns></returns>
        public static string LoadResource(string filePath, Func<string> resourceLookupFunction)
        {
            var foundFile = Utility.FindAssemblyFile(filePath);
            if (!string.IsNullOrEmpty(foundFile))
            {
                return File.ReadAllText(foundFile);
            }

            return resourceLookupFunction();
        }
    }
}
