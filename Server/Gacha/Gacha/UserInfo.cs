using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Gacha
{ 
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public void Set(string userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }

    public class UserInfoClient : UserInfo
    {
        public string Password { get; set; }

        public void Set(string userId, string password, string email)
        {
            UserId = userId;
            Password = password;
            Email = email;
        }
    }

    public class UserInfoDb : UserInfo
    {
        public ObjectId Id { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public List<int> HeroCollection { get; set; }
    }
}
