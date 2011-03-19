using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gymnastika.Sync.Communication.Client
{
    public class Configuration
    {
        public static string GetConfiguration(string settingKey)
        {
            return ConfigurationManager.AppSettings[settingKey];
        }
    }
}
