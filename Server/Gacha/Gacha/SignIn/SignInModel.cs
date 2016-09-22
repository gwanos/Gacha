using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using Newtonsoft.Json;
using PommaLabs.Thrower.Validation;

namespace Gacha.SignIn
{
    public class SignInModel
    {
        public Tuple<ResultCode, UserInfoDb> Refine(string userInput)
        {
            var hashMaker = new HashMaker();
            var resultCode = ResultCode.SUCCESS;
            var userInfoDb = new UserInfoDb();

            try
            {
                // Parsing
                var signInRequest = JsonConvert.DeserializeObject<RequestSignIn>(userInput);

                // Refining
                var salt = hashMaker.GetSalt();
                var pwd = signInRequest.Password;
                string saltAndPwd = string.Concat(pwd, salt);
                string hashedPwd = hashMaker.GetSha256Hash(saltAndPwd);

                // Mapping
                userInfoDb.UserId = signInRequest.UserId.ToLower();
                userInfoDb.Email = signInRequest.Email;
                userInfoDb.Salt = salt;
                userInfoDb.HashedPassword = hashedPwd;
                userInfoDb.HeroCollection = new List<int>();

                var ret = new Tuple<ResultCode, UserInfoDb>(resultCode, userInfoDb);

                return ret;
            }
            catch (Exception)
            {
                resultCode = ResultCode.LCN0002;
                var ret = new Tuple<ResultCode, UserInfoDb>(resultCode, userInfoDb);
                return ret;
            }
        }
    }
}