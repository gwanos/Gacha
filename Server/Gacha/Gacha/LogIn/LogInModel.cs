using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using Newtonsoft.Json;

namespace Gacha.LogIn
{
    public class LogInModel
    {
        public Tuple<ResultCode, RequestLogIn> Refine(string userInput)
        {
            try
            {
                var requestLogIn = new RequestLogIn();

                // Parsing
                var signInRequest = JsonConvert.DeserializeObject<RequestLogIn>(userInput);

                // Mapping
                requestLogIn.UserId = signInRequest.UserId;
                requestLogIn.Password = signInRequest.Password;

                var ret = new Tuple<ResultCode, RequestLogIn>(ResultCode.SUCCESS, requestLogIn);
                return ret;
            }
            catch (Exception)
            {
                var ret = new Tuple<ResultCode, RequestLogIn>(ResultCode.LCA0002, null);
                return ret;
            }
        }
    }
}