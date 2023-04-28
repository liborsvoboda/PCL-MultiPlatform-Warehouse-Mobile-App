using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using SocketLite.Extensions;
using static SocketLite.Extensions.NetworkExtensions;


namespace Terminal.UWP.PCLCommunication
{
    public partial class CommunicationsInterface
    {
        protected static IPAddress GetSubnetMask(UnicastIPAddressInformation ip)
        {
            // short circuit on null ip. 
            if (ip == null)
                return null;

            // TODO: Use native java network library rather than incomplete mono/.NET implementation: 
            // Move this into CommsInterface.cs and call once rather than iterating all adapters for each GetSubnetMaskCall. 
            var interfaces = NetworkInterface.GetAllNetworkInterfaces().ToList();
            var interfacesWithIPv4Addresses = interfaces
                                                .Where(ni => ((int)ni.OperationalStatus) == 1)
                                                .ToList();

            var ipAddress = ip.Address;

            // match the droid interface with the NetworkInterface interface on the IpAddress string
            var match = interfacesWithIPv4Addresses.FirstOrDefault(ni => ni.GetIPProperties().UnicastAddresses.Select(a => a.Address == ipAddress).FirstOrDefault());

            // no match, no good
            if (match == null)
                return null;

            // use the network prefix length to calculate the subnet address
            var networkPrefixLength = match.GetIPProperties().UnicastAddresses.Select(a => a.PrefixLength).FirstOrDefault();
            var netMask = UWPNetworkExtensions.GetSubnetAddress(ipAddress.ToString(), networkPrefixLength);

            return IPAddress.Parse(netMask);
        }
    }
}