using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using Gacha.Config;
using Gacha.Table;

namespace Gacha
{
    public interface IGachaModel
    {
        int GachaId { get; set; }
        //int Count { get; set; }
        //TableLoader TableLoader { get; }

        ResultCode Refine(string userInput);
    }
}