using System;
using Hannon.FreightEngine.Models;
using System.Configuration;
using System.Diagnostics;
namespace Hannon.FreightEngine
{
    class Program
    {
        private static string _uSPSUserName;
        private static string _uSPSPassword;
        private static IFreightEngine _uspsFreightEngine;

        static void Main(string[] args)
        {
            _uSPSUserName = ConfigurationManager.AppSettings["USPSUserName"];
            _uSPSPassword = ConfigurationManager.AppSettings["USPSPassword"];

            _uspsFreightEngine = new USPSManager(_uSPSUserName, true);
            Address addr = new Address()
            {
                Address1 = "2438 Homewood Trail",
                Address2 = string.Empty,
                City = "Arlington",
                State = "TX",
                Zip = "76015",
                ZipPlus4 = string.Empty,
                Contact = "Patrick Hannon",
                ContactEmail = string.Empty,
                FirmName = String.Empty
            };

            var address = _uspsFreightEngine.ValidateAddress(addr);
            Debug.WriteLine(address);
        }
    }
}
