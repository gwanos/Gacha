using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Xunit;
using System.Security.Cryptography;

namespace Gacha.LogIn
{
    public class LogInLogic
    {
        private readonly DBManager _dbManager;

        public LogInLogic()
        {
            _dbManager = new DBManager();
        }

        // Execute
        public Tuple<ResultCode, string, List<int>> Execute(RequestLogIn requestLogIn)
        {
            var ret = _dbManager.LogIn(requestLogIn.UserId, requestLogIn.Password);
            return ret;
        }
    }
}
