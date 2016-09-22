using System;
using Gacha.Config;
using Gacha.Table;
using Newtonsoft.Json;
using Troschuetz.Random;

namespace Gacha
{
    public class GachaModel //: IGachaModel
    {
        public Tuple<ResultCode, RequestGacha> Refine(string userInput)
        {
            try
            {
                // Parse JSON string
                var gachaRequest = JsonConvert.DeserializeObject<RequestGacha>(userInput);
                
                // Out of range
                if (gachaRequest.GachaId > GachaTable.Instance.NumberOfGachaIds || gachaRequest.GachaId < 0)
                {
                    return new Tuple<ResultCode, RequestGacha>(ResultCode.LCGACHA0001, null);
                }
                    
                return new Tuple<ResultCode, RequestGacha>(ResultCode.SUCCESS, gachaRequest);
            }
            catch (Exception)
            {
                return new Tuple<ResultCode, RequestGacha>(ResultCode.LCGACHA0001, null);
            }
        }
    }

    //public static class MyExtension
    //{
    //    public static bool IsSuccess(this ResultCode value)
    //    {
    //        return ResultCode.SUCCESS == value;
    //    }

    //    public static int ToIntValue(this ResultCode value)
    //    {
    //        return value.ToInt32();
    //    }
    //}
}