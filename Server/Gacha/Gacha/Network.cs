using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using Newtonsoft.Json;
using NLog;

namespace Gacha
{
    ///<summary>
    /// Request packet from client
    /// </summary>

    public class RequestBase
    {
        public string UserId { get; set; }    
    }

    public class RequestGacha : RequestBase
    {
        public int GachaId { get; set; }
    }

    public class RequestSignIn : RequestBase
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class RequestLogIn : RequestBase
    {
        public string Password { get; set; }
    }

    /// <summary>
    /// Response packet
    /// </summary>
    public class ResponseBase
    {
        public ResultCode ResultCode { get; set; }
    }

    public class ResponseGacha : ResponseBase
    {
        public List<int> HeroIds { get; set; }
    }

    public class ResponseSignIn : ResponseBase
    {
        
    }

    public class ResponseLogIn : ResponseBase
    {
        public string UserId { get; set; }
        public List<int> HeroCollection { get; set; }
    }

    /// <summary>
    /// IP and Port Address
    /// </summary>
    public class Address
    {
        public string IP { get; set; }
        public int Port { get; set; }

        public void SetAddressByFile(string filePath)
        {
            try
            {
                var text = File.ReadAllText(filePath);
                var address = JsonConvert.DeserializeObject<Address>(text);

                IP = address.IP;
                Port = address.Port;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}