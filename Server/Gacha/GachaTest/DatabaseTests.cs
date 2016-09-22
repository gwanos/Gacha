using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Gacha;
using Gacha.Config;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;

namespace GachaTest
{
    /// <summary>
    /// Test on database
    /// </summary>
    public class DatabaseTests
    {
        // Salt
        private string GetSalt()
        {
            byte[] buff = new byte[512];
            rngCrypto.GetBytes(buff);   // salt
            var ret = Convert.ToBase64String(buff);

            return ret;
        }

        // Hash Function
        static string GetSha256Hash(SHA256 sha256, string input)
        {
            byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder ret = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            foreach (byte theByte in data)
            {
                ret.Append(theByte.ToString("x2"));
            }

            // Return the hexadecimal string.
            return ret.ToString();
        }

        private RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
        private SHA256 sha256 = SHA256.Create();

        public class MockUserInfoClient : UserInfoClient
        {
            public static IEnumerable SignInTest
            {
                get
                {
                    yield return
                        new TestCaseData(new UserInfoClient()
                        {
                            UserId = "Alice",
                            Email = "alice@gmail.com",
                            Password = "alice1234"
                        }).Returns(ResultCode.SUCCESS);
                    yield return
                        new TestCaseData(new UserInfoClient()
                        {
                            UserId = "Alice",
                            Email = "alice@gmail.com",
                            Password = "alice1234"
                        }).Returns(ResultCode.LCA0002);
                }
            }
        }
        
        [Test, TestCaseSource(typeof(MockUserInfoClient), "SignInTest")]
        public ResultCode StoreOnDatabase(UserInfoClient userInfo)
        {
            ResultCode ret;
            var client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("UserInfoDB");
            var collection = db.GetCollection<UserInfoDb>("UserInfoDb");

            // ID 중복 체크
            var count = collection.FindAs<UserInfoDb>(Query.EQ("UserId", userInfo.UserId)).Count();
            if (count > 0)
            {
                ret = ResultCode.LCA0002;
                return ret;
            }

            // Database에 유저 정보 등록
            var salt = GetSalt();
            var pwd = userInfo.Password;
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd = GetSha256Hash(sha256, saltAndPwd);
            var _user = new UserInfoDb()
            {
                UserId = userInfo.UserId,
                Email = userInfo.Email,
                Salt = salt,
                HashedPassword = hashedPwd,
                HeroCollection = new List<int>()
            };
            collection.Save(_user);

            // 성공
            ret = ResultCode.SUCCESS;
            return ret;
        }


        /// <summary>
        /// Login Test
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        //=== Authentication ===//
        [TestCase("Alice", "aaaaa")]
        [TestCase("Bob", "bbbbb")]
        [TestCase("Charlie", "ccccc")]
        public void LoginSucceed(string userId, string pwd)
        {
            var client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("UserInfoDB");
            var collection = db.GetCollection<UserInfoDb>("User");
            var query = Query.EQ("UserId", userId);
            var document = collection.FindOne(query);

            // compare
            var saltAndPwd = String.Concat(pwd, document.Salt);
            var hashedPwd = GetSha256Hash(sha256, saltAndPwd);

            Assert.That(document.HashedPassword, Is.EqualTo(hashedPwd));
        }

        [TestCase("Alice", "111")]
        [TestCase("Bob", "222")]
        [TestCase("Charlie", "333")]
        public void LoginFail(string userId, string pwd)
        {
            var client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("UserInfoDB");
            var collection = db.GetCollection<UserInfoDb>("User");
            var query = Query.EQ("UserId", userId);
            var document = collection.FindOne(query);

            // compare
            var saltAndPwd = String.Concat(pwd, document.Salt);
            var hashedPwd = GetSha256Hash(sha256, saltAndPwd);
            Assert.That(document.HashedPassword, Is.Not.EqualTo(hashedPwd));
        }

        [TestCase("Alice")]
        [TestCase("Bob")]
        [TestCase("Charlie")]
        public void SaveOnHeroList(string userId)
        {
            var client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("UserInfoDB");
            var collection = db.GetCollection<UserInfoDb>("User");
            var query = Query.EQ("UserId", userId);
            var document = collection.FindOne(query);
            document.HeroCollection = new List<int>();
            var heroList = new List<int>() { 1, 2, 3, 4, 5 };

            var update = Update.Set("HeroCollection", new BsonArray(heroList));
            collection.Update(query, update, UpdateFlags.Upsert);
        }

        static object[] heroListSource =
        {
            new object[] {new List<int> {6, 7, 8, 9, 1}},
            new object[] {new List<int> {1, 2, 3, 4, 5}}
        };

        [Test, TestCaseSource("heroListSource")]
        public void SaveOnHeroList2(List<int> heroList)
        {
            var client = new MongoClient();
            var server = client.GetServer();
            var db = server.GetDatabase("UserInfoDB");
            var collection = db.GetCollection<UserInfoDb>("User");
            var query = Query.EQ("UserId", "Alice");
            var document = collection.FindOne(query);
            document.HeroCollection = new List<int>();
            Assert.NotNull(document);

            foreach (var v in heroList)
            {
                if (!document.HeroCollection.Contains(v))
                {
                    var update = Update.Set("HeroCollection", new BsonArray(heroList));
                    collection.Update(query, update, UpdateFlags.Upsert);
                }
            }
        }
    }
}