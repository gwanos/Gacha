using System;
using System.Collections.Generic;
using System.Linq;
using Gacha.Config;
using Gacha.Table;
using Troschuetz.Random.Generators;

namespace Gacha
{
    public class VendingMachine
    {
        public static readonly int MaxNumberOfVending = 5;
        Random random = new Random();

        // Get random grade by probabilities
        protected int GetRandomGradeSub(GachaData gachaData)
        {
            const int MULTI = 1000000;
            int ret = 0;
            int totalProbability = gachaData.TotalProbability * MULTI;

            // Select none-zero columns.
            var probabilities = gachaData.Probability.Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value * MULTI);
            int cumulative = 0;

            int randomNumber = random.Next(0, totalProbability);
            foreach (var probability in probabilities)    // No exception
            {
                cumulative += probability.Value;
                if (randomNumber <= cumulative)  
                {
                    ret = probability.Key;
                    break;
                }
            }

            return ret; 
        }

        // Get random grade by probabilities
        protected int GetRandomGrade(GachaData gachaData)
        {
            const int MULTI = 1000000;
            int ret = 0;
            int totalProbability = gachaData.TotalProbability*MULTI;

            // Select none-zero columns.
            var probabilities = gachaData.Probability.Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value * MULTI);
            int cumulative = 0;

            int randomNumber = RandomNumberCreator.Instance.MTRandom.Next(0, totalProbability);
            foreach (var probability in probabilities)    // No exception
            {
                cumulative += probability.Value;
                if (randomNumber <= cumulative)
                {
                    ret = probability.Key;
                    break;
                }
            }

            return ret;
        }
        
        // Pick a hero
        protected int PickOneHero(int grade)
        {
            // Pick only possible heroes
            var heroIdList = HeroTable.Instance.GetHeroIDList(grade);
            var heroCount = heroIdList.Count();

            // Generate random number
            // Access array with randomNumber(index)
            var randomNumber = RandomNumberCreator.Instance.MTRandom.Next(0, heroCount);  
            var ret = heroIdList[randomNumber];

            // return Hero Id
            return ret; 
        }

        // Get a heroes' list
        public List<int> PickHeroes(int gachaId) 
        {
            var ret = new List<int>();
            var gachaData = new GachaData();

            // Set gachaData
            GachaTable.Instance.SelectByGachaId(gachaId).TryGetValue(gachaId, out gachaData);
            
            // Pick heroes
            for (int i = 0; i < MaxNumberOfVending; i++)
            {
                int grade = GetRandomGrade(gachaData);
                int heroId = PickOneHero(grade);
                ret.Add(heroId);
            }
            return ret;
        }
    }
}