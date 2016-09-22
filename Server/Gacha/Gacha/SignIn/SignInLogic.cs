using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;

namespace Gacha.SignIn
{
    public class SignInLogic
    {
        private readonly DBManager _dbManager;
        public SignInLogic()
        {
            _dbManager = new DBManager();
        }
        
        // Execute
        public ResultCode Execute(UserInfoDb userInfoDb)
        {
            var ret = _dbManager.SignIn(userInfoDb);
            return ret;
        }
    }
}
