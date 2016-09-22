using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using Gacha.SignIn;
using Newtonsoft.Json;
using NLog;

namespace Gacha
{
    public class SignInController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private SignInModel _model;
        private SignInLogic _logic;

        public ResponseSignIn Perform(string userInput)
        {
            var ret = new ResponseSignIn();

            // Model
            _model = new SignInModel();
            var userInfo = _model.Refine(userInput);
            if (userInfo.Item1 != ResultCode.SUCCESS)
            {
                ret.ResultCode = userInfo.Item1;
                return ret;
            }

            // Logic
            _logic = new SignInLogic();
            var execution = _logic.Execute(userInfo.Item2);
            {
                ret.ResultCode = execution;
            }

            // Log
            Logger.Info($"UserId: {userInfo.Item2.UserId} / Email: {userInfo.Item2.Email}");

            return ret;
        }
    }
}