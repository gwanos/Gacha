using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class User
    {
        private static User _instance;
        public static User Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new User();
                }
                return _instance;
            }
        }

        public string UserId { get; set; }
        public List<int> HeroCollection { get; set; }

        private User()
        {
            HeroCollection = new List<int>();
        }
    }
}