using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace Gacha.Table
{
    public class RandomNumberCreator
    {
        private static readonly Lazy<RandomNumberCreator> _instance 
            = new Lazy<RandomNumberCreator>(() => new RandomNumberCreator());
        MT19937Generator _mtRandom = new MT19937Generator();

        public static RandomNumberCreator Instance => _instance.Value;
        public MT19937Generator MTRandom => _mtRandom;
    }
}
