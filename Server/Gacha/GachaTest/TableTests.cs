using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using Gacha;
using Gacha.Table;
using NUnit.Framework;

namespace GachaTest
{
    public class MockHeroTable : HeroTable
    {
        public new Dictionary<int, HeroData> GetHeroesByGrade(int grade)
        {
            return base.GetHeroesByGrade(grade);
        }

        public new List<int> GetHeroIDList(int grade)
        {
            return base.GetHeroIDList(grade);
        }
    }

    public class MockGachaTable : GachaTable
    {
        public new Dictionary<int, GachaData> SelectByGachaId(int gachaId)
        {
            return base.SelectByGachaId(gachaId);
        }
    }

    [TestFixture]
    public class TableTests
    {
        protected delegate Dictionary<int, HeroData> HeroTableHandler(int grade);
        /// <summary>
        /// Success case
        /// </summary>
        [TestCase(1, 31)]
        [TestCase(2, 30)]
        [TestCase(3, 40)]
        [TestCase(4, 31)]
        [TestCase(5, 39)]
        [TestCase(6, 29)]
        public void GetHeroesByGradeSucceed(int grade, int expectedValue)
        {
            HeroTableHandler handler = delegate(int _grade)
            {
                var wholeHeroTable = HeroTable.Instance.WholeTable;
                var ret = wholeHeroTable.Where(x => x.Value.Grade == _grade).ToDictionary(x => x.Key, x => x.Value);
                return ret;
            };

            var heroes = handler(grade);
            Assert.That(expectedValue, Is.EqualTo(heroes.Count));
        }

        [TestCase(1, 31)]
        [TestCase(2, 30)]
        [TestCase(3, 40)]
        [TestCase(4, 31)]
        [TestCase(5, 39)]
        [TestCase(6, 29)]
        public void SelectHeroesByGradeSucceed(int grade, int expectedValue)
        {
            HeroTableHandler handler = delegate(int _grade)
            {
                var index = _grade - 1;
                return HeroTable.Instance.HeroesWithGrade[index];
            };
            var heroes = handler(grade);
            Assert.That(expectedValue, Is.EqualTo(heroes.Count));
        }

        [TestCase(1, "일반소환서(1-3)")]
        [TestCase(2, "일반소환서(2-4)")]
        [TestCase(3, "일반소환서(2-5)")]
        [TestCase(4, "고급소환서(3-5)")]
        [TestCase(5, "고급소환서(4-6)")]
        [TestCase(6, "전설소환서(6)")]
        public void SelectByGachaIdSuccess(int gachaId, string expectedValue)
        {
            MockGachaTable gachaTable = new MockGachaTable();
            var gacha = gachaTable.SelectByGachaId(gachaId);
            GachaData value = null;
            gacha.TryGetValue(gachaId, out value);
            Assert.That(expectedValue, Is.EqualTo(value.Name));
        }

        /// <summary>
        /// Failure case
        /// </summary>
        /// <param name="grade"></param>
        [TestCase(999)]
        [TestCase(-999)]
        public void GetHeroesByGradeFail(int grade)
        {
            MockHeroTable heroTable = new MockHeroTable();
            Assert.That( () => heroTable.GetHeroesByGrade(grade), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(999)]
        [TestCase(-999)]
        public void SelectHeroesByGradeFail(int grade)
        {
            MockHeroTable heroTable = new MockHeroTable();
            Assert.That( () => heroTable.SelectHeroesByGrade(grade), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(999)]
        [TestCase(-999)]
        public void GetHeroIDListFail(int grade)
        {
            MockHeroTable heroTable = new MockHeroTable();
            Assert.That(() => heroTable.SelectHeroesByGrade(grade), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}