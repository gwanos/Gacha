using System.Collections.Generic;

namespace Gacha.Table
{
    public class GachaData
    {
        private int _id;
        private string _name;
        private int _totalProbability;
        private Dictionary<int, int> _probability;

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public int TotalProbability { get { return _totalProbability; } set { _totalProbability = value; } }
        public Dictionary<int, int> Probability { get { return _probability; } set { _probability = value; } }
    }
}
