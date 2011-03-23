using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gymnastika.Modules.Sports.Facilities
{
    public static class PathFacility
    {
        public static string GetAbsolutePath(string path)
        {
            if (!path.Contains(":"))
            {
                string curDir = GetCurrentDir();
                path = curDir + path;
            }
            return path;
        }

        public static string GetCurrentDir()
        {
            return Directory.GetCurrentDirectory();
        }

        public static string GetDataAbsolutePath(string path)
        {
           return GetAbsolutePath(ConfigFile.DataDirectory) + path;
        }
    }
}
