using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha;
using Gacha.Table;
using NUnit.Framework;
using Troschuetz.Random.Generators;

namespace GachaTest
{
    /// <summary>
    /// Gacha logic test
    /// MTRandom number generator test
    /// </summary>
    public class GachaLogicTests
    {
        public class MockMachine : VendingMachine
        {
            public MockMachine() : base()
            {
            }
            public new int GetRandomGrade(GachaData gachaData)
            {
                return base.GetRandomGrade(gachaData);
            }
            public new int GetRandomGradeSub(GachaData gachaData)
            {
                return base.GetRandomGradeSub(gachaData);
            }

            public new int PickOneHero(int grade)
            {
                return base.PickOneHero(grade);
            }
        }

        /// <summary>
        /// Test on success case
        /// </summary>
        // Check if Id returned from PickOneHero() is correct
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void PickOneHeroSuccess(int gachaId)
        {
            MockMachine mock = new MockMachine();
            GachaData gachaData = new GachaData();
            GachaTable.Instance.SelectByGachaId(gachaId).TryGetValue(gachaId, out gachaData);

            var grade = mock.GetRandomGrade(gachaData);
            var heroId = mock.PickOneHero(grade);
            var heroIdLists = HeroTable.Instance.GetHeroIDList(grade);
            Assert.That(true, Is.EqualTo(heroIdLists.Contains(heroId)));
        }

        [Test]
        public void GetRandomGradeDistributionTest()
        {
            var totalProbability = 100;
            var cumulative = 0;
            var probabilities = new Dictionary<int, int>();
            probabilities.Add(1, 50);
            probabilities.Add(2, 30);
            probabilities.Add(3, 20);

            var probabilityDistribution = new int[probabilities.Count + 1];

            int randomNumber = 0;

            for (int i = 0; i < 5; i++)
            {
                randomNumber = RandomNumberCreator.Instance.MTRandom.Next(0, totalProbability);
                cumulative = 0;
                foreach (var v in probabilities)
                {
                    cumulative += v.Value;
                    if (randomNumber < cumulative)
                    {
                        probabilityDistribution[v.Key]++; // 1 ~ 6 성
                        break;
                    }
                }
            }
            Debug.WriteLine("{0}: {1}", 1, probabilityDistribution[1]);
            Debug.WriteLine("{0}: {1}", 2, probabilityDistribution[2]);
            Debug.WriteLine("{0}: {1}", 3, probabilityDistribution[3]);
        }

        [TestCase(1)]
        public void SubRandom(int gachaId)
        {
            MockMachine mock = new MockMachine();
            GachaData gachaData = new GachaData();
            GachaTable.Instance.SelectByGachaId(gachaId).TryGetValue(gachaId, out gachaData);
            int[] grades = new int[3];
            int[] expected = new int[3] { 50, 30, 20 };

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < gachaData.TotalProbability; i++)
                {
                    var grade = mock.GetRandomGradeSub(gachaData);
                    grades[grade - 1]++;
                }

                Debug.WriteLine("=====   {0}회   =====", j+1);
                for (int i = 0; i < 3; i++)
                {
                    var diff = Math.Abs(expected[i] - grades[i]);
                    Debug.WriteLine("[{0}성] 값: {1} / 차이: {2}", i + 1, grades[i], diff);
                }
                grades.ClearAll();
            }
        }

        [TestCase(1)]
        public void MTRandom(int gachaId)
        {
            MockMachine mock = new MockMachine();
            GachaData gachaData = new GachaData();
            GachaTable.Instance.SelectByGachaId(gachaId).TryGetValue(gachaId, out gachaData);
            int[] grades = new int[3];
            int[] expected = new int[3] { 50, 30, 20 };

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < gachaData.TotalProbability; i++)
                {
                    var grade = mock.GetRandomGrade(gachaData);
                    grades[grade - 1]++;
                }

                Debug.WriteLine("=====   {0}회   =====", j + 1);
                for (int i = 0; i < 3; i++)
                {
                    var diff = Math.Abs(expected[i] - grades[i]);
                    Debug.WriteLine("[{0}성] 값: {1} / 차이: {2}", i + 1, grades[i], diff);
                }
                grades.ClearAll();
            }
        }

        [TestCase(1)]
        public void RandomTest(int gachaId)
        {
            MockMachine mock = new MockMachine();
            GachaData gachaData = new GachaData();
            GachaTable.Instance.SelectByGachaId(gachaId).TryGetValue(gachaId, out gachaData);

            SubRandom(gachaId);
            MTRandom(gachaId);    
        }

        [TestCase(1)]
        public void Test(int gachaId)
        {
            GachaData gachaData = new GachaData();
            GachaTable.Instance.SelectByGachaId(gachaId).TryGetValue(gachaId, out gachaData);
            int MULTI = 10000;
            int ret = 0;
            int totalProbability = gachaData.TotalProbability * MULTI;

            // Select none-zero columns.
            var probabilities = gachaData.Probability.Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value * MULTI);
            foreach (var value in probabilities)
            {
                Debug.WriteLine("{0} / {1}", value.Key, value.Value);
            }

        }
    }
}
