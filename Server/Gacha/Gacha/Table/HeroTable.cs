using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Gacha.Config;
using NLog;

namespace Gacha.Table
{
    public class HeroTable : ITable<int, HeroData>
    {
        //private static readonly string _filePath = "../../../ExcelData/HeroTable.xlsx";
        private static readonly string _filePath = "C:/bitBucket/ncsoft-internship/Gacha/ExcelData/HeroTable.xlsx";
        private static readonly string _tableName = "Hero_Table";
        private static readonly string _columnID = "Id";
        private static readonly string _columnName = "NAME";
        private static readonly string _columnGrade = "GRADE";
        private static readonly string _columnProperty = "PROPERTY";
        private readonly int numberOfGrades = 6;
        private readonly int numberOfHeroes;

        private readonly Dictionary<int, HeroData> _wholeTable;
        private readonly List<Dictionary<int, HeroData>> _heroesWithGrade;

        private static readonly Lazy<HeroTable> _instance = new Lazy<HeroTable>(() => new HeroTable());
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static HeroTable Instance => _instance.Value;
        public Dictionary<int, HeroData> WholeTable => _wholeTable;
        public List<Dictionary<int, HeroData>> HeroesWithGrade => _heroesWithGrade;


        protected HeroTable()
        {
            TableLoader tableLoader = new TableLoader();

            // Create whole table
            try
            {
                _wholeTable = tableLoader.Load( 
                _filePath,
                _tableName,
                true,
                row => Convert.ToInt32(row.Field<double>(_columnID)),
                row => new HeroData()
                {
                    Id = Convert.ToInt32(row.Field<double>(_columnID)),
                    Name = row.Field<string>(_columnName),
                    Grade = Convert.ToInt32(row.Field<double>(_columnGrade)),
                    Property = Convert.ToInt32(row.Field<double>(_columnProperty))
                });

                numberOfHeroes = _wholeTable.Keys.Count;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            // Create table by grade(1~6)
           
            _heroesWithGrade = new List<Dictionary<int, HeroData>>();
            var count = GachaTable.Instance.WholeTable.Keys.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var heroes = GetHeroesByGrade(i + 1);
                    _heroesWithGrade.Add(heroes);
                }
                catch (ArgumentOutOfRangeException exception)
                {
                    throw exception;
                }
            }
        }
        
        // 
        protected delegate Dictionary<int, HeroData> HeroTableHandler(int grade);
        protected Dictionary<int, HeroData> GetHeroesByGrade(int grade)
        {
            // 입력 범위 초과
            if ( grade <= 0 || grade > numberOfGrades )
                throw new ArgumentOutOfRangeException();

            HeroTableHandler handler = delegate(int _grade)
            {
                var _ret = _wholeTable.Where(x => x.Value.Grade == _grade).ToDictionary(x => x.Key, x => x.Value);
                return _ret;
            };
            
            var ret = handler(grade);
            return ret;
        }
        
        // Select heroes by grade
        public Dictionary<int, HeroData> SelectHeroesByGrade(int grade)
        {
            // 입력 범위 초과
            if (grade <= 0 || grade > numberOfGrades)
                throw new ArgumentOutOfRangeException();

            HeroTableHandler handler = delegate(int _grade)
            {
                var index = _grade - 1;
                return HeroesWithGrade[index];
            };

            var ret = handler(grade);
            return ret;
        }

        // Get HeroIdList
        public List<int> GetHeroIDList(int grade)
        {
            // 입력 범위 초과
            if (grade <= 0 || grade > numberOfGrades)
                throw new ArgumentOutOfRangeException();

            var ret = WholeTable.Where(x => x.Value.Grade == grade).Select(x => x.Value.Id).ToList();
            return ret;
        }

        // 예외 처리
        public bool IsValid()
        {
            foreach (var hero in _wholeTable)
            {
                if (hero.Key != hero.Value.Id)
                {
                    throw new Exception("HeroTable.xlsx - Key is not equal to value.Id.");
                }
                if (hero.Value.Id > numberOfHeroes || hero.Value.Id < 0)
                {
                    throw new Exception("HeroTable.xlsx - A value is out of range.");
                }
                if (hero.Value.Grade > numberOfGrades || hero.Value.Grade < 1)
                {
                    throw new Exception("HeroTable.xlsx - A value is out of range.");
                }
                if (hero.Value.Property > 3 || hero.Value.Property < 1)
                {
                    throw new Exception("HeroTable.xlsx - A value is out of range.");
                }
            }
            return true;
        }
    }
}