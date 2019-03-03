using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hannon.FreightEngine.Models;

namespace Hannon.FreightEngine
{
    public class USPSManager : IFreightEngine
    {
        #region Private Members
        private const string ProductionUrl = "http://production.shippingapis.com/ShippingAPI.dll";
        private const string TestingUrl = "http://testing.shippingapis.com/ShippingAPITest.dll";
        private WebClient web;
        private string _userid;
        #endregion

        /// <summary>
        /// Creates a new USPS Manager instance
        /// </summary>
        /// <param name="USPSWebtoolUserID">The UserID required by the USPS Web Tools</param>
        public USPSManager(string USPSWebtoolUserID)
        {
            web = new WebClient();
            _userid = USPSWebtoolUserID;
            _TestMode = false;

        }
        /// <summary>
        /// Creates a new USPS Manager instance
        /// </summary>
        /// <param name="USPSWebtoolUserID">The UserID required by the USPS Web Tools</param>
        /// <param name="testmode">If True, then the USPS Test URL will be used.</param>
        public USPSManager(string USPSWebtoolUserID, bool testmode)
        {
            _TestMode = testmode;
            web = new WebClient();
            _userid = USPSWebtoolUserID;
        }

        #region Properties
        private bool _TestMode;
        public bool TestMode
        {
            get { return _TestMode; }
            set { _TestMode = value; }
        }
        #endregion

        public Address ValidateAddress(Address address)
        {
            try
            {
                string validateUrl = "?API=Verify&XML=<AddressValidateRequest USERID=\"{0}\"><Address ID=\"{1}\"><Address1>{2}</Address1><Address2>{3}</Address2><City>{4}</City><State>{5}</State><Zip5>{6}</Zip5><Zip4>{7}</Zip4></Address></AddressValidateRequest>";
                string url = GetURL() + validateUrl;
                url = String.Format(url, _userid, address.ID.ToString(), address.Address1, address.Address2, address.City, address.State, address.Zip, address.ZipPlus4);
                string addressxml = web.DownloadString(url);
                if (addressxml.Contains("<Error>"))
                {
                    int idx1 = addressxml.IndexOf("<Description>") + 13;
                    int idx2 = addressxml.IndexOf("</Description>");
                    int l = addressxml.Length;
                    string errDesc = addressxml.Substring(idx1, idx2 - idx1);
                    throw new USPSManagerException(errDesc);
                }

                return Address.FromXml(addressxml);
            }
            catch (WebException ex)
            {
                throw new USPSManagerException(ex);
            }
        }
        #region Private methods
        private string GetURL()
        {
            string url = ProductionUrl;
            if (TestMode)
                url = TestingUrl;
            return url;
        }
        #endregion
    }
}
