using System;
using System.Collections.Generic;
using Gacha.Config;

namespace Gacha
{ 
    public class GachaLogic
    {
        // Execute
        public Tuple<ResultCode, List<int>> Execute(RequestGacha requestGacha)
        {
            var vendingMachine = new VendingMachine();
            var dbManager = new DBManager();
            try
            {
                var heroList = vendingMachine.PickHeroes(requestGacha.GachaId);
                dbManager.UpdateHeroCollection(requestGacha.UserId, heroList);

                var ret = new Tuple<ResultCode, List<int>>(ResultCode.SUCCESS, heroList);
                return ret;
            }
            catch (Exception)
            {
                var ret = new Tuple<ResultCode, List<int>>(ResultCode.LCGACHA0002, null);
                return ret;
            }
        }
    }
}