using System;
using System.Collections.Generic;
using Gacha.Config;

namespace Gacha
{
    public interface IGachaLogic
    {
        Tuple<ResultCode, List<int>> Execute(IGachaModel model);
    }
}