using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FluidLib.Utils
{
    public static class Utils
    {
        public static string GenerateRandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            Random rnd = new Random();

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public static void Swap(ref object obj1, ref object obj2)
        {
            object temp = obj1;
            obj1 = obj2;
            obj2 = temp;
        }

        public static PhysicalAddress GetLocalMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }
        public static string GetLocalMacAddressToString()
        {
            return GetLocalMacAddress().ToString();
        }
    }
}
