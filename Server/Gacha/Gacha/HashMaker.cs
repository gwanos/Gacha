using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gacha
{
    public class HashMaker
    {
        private readonly RNGCryptoServiceProvider _rngCrypto;
        private readonly SHA256 _sha256;

        public HashMaker()
        {
            _rngCrypto = new RNGCryptoServiceProvider();
            _sha256 = SHA256.Create();
        }

        // Salt
        public string GetSalt()
        {
            byte[] buff = new byte[512];
            _rngCrypto.GetBytes(buff);   // salt
            var ret = Convert.ToBase64String(buff);

            return ret;
        }

        // Hash Function
        public string GetSha256Hash(string input)
        {
            byte[] data = _sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder ret = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            foreach (byte theByte in data)
            {
                ret.Append(theByte.ToString("x2"));
            }

            // Return the hexadecimal string.
            return ret.ToString();
        }
    }
}
