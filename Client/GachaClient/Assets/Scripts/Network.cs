using System;
using UnityEngine;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Network
{
    public static class Urls
    {
        public static readonly string UrlGacha = "http://localhost:8080/Gacha";
        public static readonly string UrlSignIn = "http://localhost:8080/SignIn";
        public static readonly string UrlLogIn = "http://localhost:8080/LogIn";
        public static readonly Dictionary<string, string> headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } };
    }

    /// <summary>
    /// Request packet from client
    /// </summary>
    /// 
    public class RequestGacha
    {
        public int GachaId { get; set; }
        public string UserId { get; set; }
    }

    public class RequestLogIn
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class RequestSignIn : RequestLogIn
    {
        public string Email { get; set; }
    }



    /// <summary>
    /// Response packet from server
    /// </summary>
    public class ResponseBase
    {
        public ResultCode ResultCode { get; set; }
    }

    public class ResponseGacha : ResponseBase
    {
        public List<int> HeroIds { get; set; }

        public ResponseGacha()
        {
            HeroIds = new List<int>();
        }
    }

    public class ResponseSignIn : ResponseBase
    {

    }

    public class ResponseLogIn : ResponseBase
    {
        public string UserId { get; set; }
        public List<int> HeroCollection { get; set; }

        public ResponseLogIn()
        {
            HeroCollection = new List<int>();
        }
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