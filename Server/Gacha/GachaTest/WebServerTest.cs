using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using Newtonsoft.Json;

namespace GachaTest
{
    /// <summary>
    /// Test on Owin and Nancy
    /// </summary>
    public class WebServerTest
    {
        public class Address
        {
            public string IP { get; set; }
            public int Port { get; set; }
        }
        
        /// <summary>
        /// Read config file
        /// Set IP address and port number
        /// </summary>
        /// <param name="filePath"></param>
        [TestCase("C:/bitBucket/ncsoft-internship/Gacha/Gacha/Config/config.cfg")]
        public void SetAddressSucceed(string filePath)
        {
            var text = File.ReadAllText(filePath);
            var address = JsonConvert.DeserializeObject<Address>(text);
            Assert.That("+", Is.EqualTo(address.IP));
            Assert.That(8080, Is.EqualTo(address.Port));
        }

        [TestCase("C:/bitBucket/ncsoft-internship/Gacha/Gacha/Config/config999.cfg")]
        public void SetAddressFail(string filePath)
        {
            Assert.That(() => File.ReadAllText(filePath), Throws.Exception);
        }
    }
}
