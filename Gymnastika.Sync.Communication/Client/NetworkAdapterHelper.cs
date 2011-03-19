using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace Gymnastika.Sync.Communication.Client
{
    public class NetworkAdapterHelper
    {
        public static NetworkAdapterCollection GetAdapters()
        {
            NetworkAdapterCollection networkAdapters = new NetworkAdapterCollection();

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection nics = mc.GetInstances();
            foreach (ManagementObject nic in nics)
            {
                if (Convert.ToBoolean(nic["ipEnabled"]) == true)
                {
                    NetworkAdapter adapter = new NetworkAdapter();
                    var ipAddresses = nic["IPAddress"] as string[];
                    if (ipAddresses != null)
                        adapter.IpAddress = ipAddresses[0];

                    var subnets = nic["IPSubnet"] as string[];
                    if(subnets != null)
                        adapter.SubnetMask = subnets[0];

                    var gateways = nic["DefaultIPGateway"] as string[];
                    if(gateways != null)
                        adapter.DefaultGateway = gateways[0];

                    networkAdapters.Add(adapter);
                }
            }

            return networkAdapters;
        }
    }
}
