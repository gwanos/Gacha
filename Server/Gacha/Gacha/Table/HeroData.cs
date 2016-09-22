namespace Gacha.Table
{
    public class HeroData
    {
        private int _id;
        private string _name;
        private int _grade;
        private int _property;

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public int Grade { get { return _grade; } set { _grade = value; } }
        public int Property { get { return _property; } set { _property = value; } }
    }
}
