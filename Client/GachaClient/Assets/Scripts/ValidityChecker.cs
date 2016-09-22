using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets.Scripts
{
    public class ValidityChecker
    {
        public void IsValid(string userId, string email, string password)
        {
            // 항목 비어있음
            if (userId.Length == 0 || email.Length == 0 || password.Length == 0)
            {
                throw new Exception("항목이 비었습니다.");
            }

            // 메일 형식 아님
            if (IsValidEmail(email) == false)
            {
                throw new Exception("메일 형식이 아닙니다.");
            }

            // 길이 너무짧음
            if (password.Length < 8)
            {
                throw new Exception("비밀번호를 8자 이상 입력하세요.");
            }
        }

        private bool IsValidEmail(string email)
        {
            bool valid = Regex.IsMatch(email,
                "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            return valid;
        }
    }
}
