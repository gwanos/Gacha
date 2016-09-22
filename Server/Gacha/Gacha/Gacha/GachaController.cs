using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using Newtonsoft.Json;
using NLog;

namespace Gacha
{
    public class GachaController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private GachaModel _model;  
        private GachaLogic _logic;  

        public string Perform(string userInput)
        {
            string ret = "";
            ResponseGacha packet = new ResponseGacha();

            // Model
            _model = new GachaModel();
            var retFromModel =  _model.Refine(userInput);
            // Invalid data from client
            if (ResultCode.SUCCESS != retFromModel.Item1)
            {
                packet.ResultCode = retFromModel.Item1;
                packet.HeroIds = null;
                ret = JsonConvert.SerializeObject(packet);
                return ret;
            }
             
            // Logic
            _logic = new GachaLogic();
            {
                var retFromLogic = _logic.Execute(retFromModel.Item2);
                Logger.Info($"GachaId: {retFromModel.Item2.GachaId} / UserId: {retFromModel.Item2.UserId} --- HeroIdList: {retFromLogic.Item2.StringJoin(",")}");

                // Serialize to JSON
                packet.ResultCode = retFromLogic.Item1;
                packet.HeroIds = retFromLogic.Item2;
                ret = JsonConvert.SerializeObject(packet);
                return ret;
            }
        }
    }
}