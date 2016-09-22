using Gacha.Config;
using NLog; 

namespace Gacha.LogIn
{
    public class LogInController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private LogInModel _model;
        private LogInLogic _logic;
        
        public ResponseLogIn Perform(string userInput)
        {
            var ret = new ResponseLogIn();

            // Model
            _model = new LogInModel();
            var retFromModel = _model.Refine(userInput);
            var requestLogIn = retFromModel.Item2;
            if (ResultCode.SUCCESS != retFromModel.Item1)
            {
                ret.ResultCode = retFromModel.Item1;
                return ret;
            }

            // Logic
            _logic = new LogInLogic();
            var retFromLogic = _logic.Execute(requestLogIn);
            ret.ResultCode = retFromLogic.Item1;
            ret.UserId = retFromLogic.Item2;
            ret.HeroCollection = retFromLogic.Item3;

            // Log
            Logger.Info($"UserId: {ret.UserId} / ResultCode: {ret.ResultCode}");

            return ret;
        }
    }
}