using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Gacha
{
    public class DBManager
    {
        private readonly HashMaker _hashMaker;
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _database;
        
        public DBManager()
        {
            _hashMaker = new HashMaker();
            ConnectToDBServer();
        }

        private void ConnectToDBServer()
        {
            try
            {
                _client = new MongoClient();
                _server = _client.GetServer();
                _database = _server.GetDatabase("UserInfoDB");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        // Update
        public ResultCode UpdateHeroCollection(string userId, List<int> heroList)
        {
            var collection = _database.GetCollection<UserInfoDb>("User");
            var query = Query.EQ("UserId", userId.ToLower());
            var update = Update.AddToSetEach("HeroCollection", new BsonArray(heroList));
            collection.Update(query, update, UpdateFlags.Upsert);
            
            return ResultCode.SUCCESS;
        }

        // Sign In
        public ResultCode SignIn(UserInfoDb userInfoDb)
        {
            ResultCode ret;
            var collection = _database.GetCollection<UserInfoDb>("User");

            // ID 중복 체크
            var count = collection.FindAs<UserInfoDb>(Query.EQ("UserId", userInfoDb.UserId.ToLower())).Count();
            if (count > 0)
            {
                ret = ResultCode.LCA0002;
                return ret;
            }

            // Database에 유저 정보 등록
            collection.Save(userInfoDb);

            // 성공
            ret = ResultCode.SUCCESS;
            return ret;
        }

        // Log In
        public Tuple<ResultCode, string, List<int>> LogIn(string userId, string pwd)
        {
            // 
            var collection = _database.GetCollection<UserInfoDb>("User");
            var query = Query.EQ("UserId", userId.ToLower());
            var document = collection.FindOne(query);

            // ID 없음
            if (document == null)
            {
                return new Tuple<ResultCode, string, List<int>>(ResultCode.LCA0003, null, null);
            }

            // 비밀번호 비교
            var saltAndPwd = string.Concat(pwd, document.Salt);
            var hashedPwd = _hashMaker.GetSha256Hash(saltAndPwd);

            // 비밀번호 불일치
            if (document.HashedPassword != hashedPwd)
            {
                return new Tuple<ResultCode, string, List<int>>(ResultCode.LCA0004, null, null);
            }
            
            // 성공
            var heroCollection = document.HeroCollection;
            return new Tuple<ResultCode, string, List<int>>(ResultCode.SUCCESS, userId, heroCollection);
        }
    }
}