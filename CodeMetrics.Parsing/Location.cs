using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public class Location : ILocation
    {
        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public int Line { get; }
        public int Column { get; }

        public string ToShortString()
        {
            return $"{Line},{Column}";
        }

        public override string ToString()
        {
            return $"Location:{ToShortString()}";
        }
    }
}