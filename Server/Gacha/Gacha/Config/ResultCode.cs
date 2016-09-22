using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha.Config
{
    public enum ResultCode
    {
        // SYSTEM
        SUCCESS                 = 0, // 성공
        
        // NETWORK
        LCN0001                 = 101,  // 서버 죽음
        LCN0002                 = 102,  // JSon Format error

        // ACCOUNT
        LCA0001                 = 1001, // 인증 필요
        LCA0002                 = 1002, // 동일한 ID 존재
        LCA0003                 = 1003, // ID가 DB에 존재하지 않음
        LCA0004                 = 1004, // Password 불일치
                
        // CONTENTS
        LCGACHA0001             = 2001, // 없는 소환서 선택(GachaId 범위 초과)
        LCGACHA0002             = 2002, // 영웅 5명 못 뽓음
    };
}
