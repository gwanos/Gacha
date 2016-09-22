using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel.Log;
using Gacha.Config;
using NLog;
using LogManager = NLog.LogManager;

namespace Gacha.Table
{
    public class GachaTable : ITable<int, GachaData>
    {
        //private static readonly string _filePath = "../../../ExcelData/GachaTable.xlsx";
        private static readonly string _filePath = "C:/bitBucket/ncsoft-internship/Gacha/ExcelData/GachaTable.xlsx";
        private static readonly string _tableName = "Gacha_Table";
        private static readonly string _columnId = "Gacha_Id";
        private static readonly string _columnName = "NAME";
        private static readonly string _sumOfProbability = "확률 합";
        private static readonly string _probability1 = "1성";
        private static readonly string _probability2 = "2성";
        private static readonly string _probability3 = "3성";
        private static readonly string _probability4 = "4성";
        private static readonly string _probability5 = "5성";
        private static readonly string _probability6 = "6성";
        private readonly int _numberOfGachaIds;

        private static readonly Lazy<GachaTable> _instance = new Lazy<GachaTable>(() => new GachaTable());
        private readonly Dictionary<int, GachaData> _wholeTable;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static GachaTable Instance => _instance.Value;
        public Dictionary<int, GachaData> WholeTable => _wholeTable;
        public int NumberOfGachaIds { get; }

        protected GachaTable()
        {
            TableLoader tableLoader = new TableLoader();

            // Create whole table
            try
            {
                _wholeTable = tableLoader.Load(
                    _filePath,
                    _tableName,
                    true,
                    row => Convert.ToInt32(row.Field<double>(_columnId)),
                    row => new GachaData()
                    {
                        Id = Convert.ToInt32(row.Field<double>(_columnId)),
                        Name = row.Field<string>(_columnName),
                        TotalProbability = Convert.ToInt32(row.Field<double>(_sumOfProbability)),
                        Probability = new Dictionary<int, int>()
                        {
                            {1, Convert.ToInt32(row.Field<double>(_probability1))},
                            {2, Convert.ToInt32(row.Field<double>(_probability2))},
                            {3, Convert.ToInt32(row.Field<double>(_probability3))},
                            {4, Convert.ToInt32(row.Field<double>(_probability4))},
                            {5, Convert.ToInt32(row.Field<double>(_probability5))},
                            {6, Convert.ToInt32(row.Field<double>(_probability6))},
                        }
                    });

                NumberOfGachaIds = _wholeTable.Keys.Count;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        
        // Validity Test
        public bool IsValid()
        {
            foreach (var raw in _wholeTable)
            {
                // Check if Key(Id) and Value.Id is same
                if (raw.Key != raw.Value.Id)
                {
                    throw new Exception("GachaTable.xlsx - Key is not equal to value.Id");
                }

                // Check if value.id < 0
                if (raw.Value.Id < 0)
                {
                    throw new Exception("GachaTable.xlsx - A value is out of range");
                }

                // Check if probability < 0
                foreach (var probabilityPair in raw.Value.Probability)
                {
                    if (probabilityPair.Value < 0)
                    {
                        throw new Exception("GachaTable.xlsx - probability should be greater than 0.");
                    }
                }
                if (raw.Value.TotalProbability <= 0)
                {
                    throw new Exception("GachaTable.xlsx - Total probability should be greater than 0.");
                }

                // Check if totalProbability and sum of probabilities are same
                var sumOfProbabilities = raw.Value.Probability.Values.Sum();
                if (raw.Value.TotalProbability != sumOfProbabilities)
                {
                    throw new Exception("Total probability and sum of probabilities are different.");
                }
            }
            return true;
        }

        // Select gacha table by gachaId
        public Dictionary<int, GachaData> SelectByGachaId(int gachaId)
        {
            var ret = _wholeTable.Where(x => x.Key == gachaId).ToDictionary(x => x.Key, x => x.Value);
            return ret;
        }
    }
}