using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace ACABeacon
{
    class MACAddress
    {
        public static string[] get_mac_addresses() {
            var query = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n =>
                    n.OperationalStatus == OperationalStatus.Up && // only grabbing what's online
                    n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(_ => new
                {
                    PhysicalAddress = _.GetPhysicalAddress(),
                    IPProperties = _.GetIPProperties(),
                });

            var macs = query
                .Where(q => q.IPProperties.UnicastAddresses.Any(ua => true)).ToArray();

            string[] human_readable = new string[macs.Length];
            var index = 0;

            foreach (var mac in macs) {
                human_readable[index] = String.Join("-", mac.PhysicalAddress.GetAddressBytes().Select(b => b.ToString("X2")));
                index += 1;
            }

            return human_readable;
        }

        public static string get_username() {
            return Environment.UserName;
        }

        public static string get_domain_name() {
            return Environment.UserDomainName;
        }
    }
}
