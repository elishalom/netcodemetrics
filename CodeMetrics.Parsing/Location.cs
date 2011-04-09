namespace CodeMetrics.Parsing
{
    public class Location
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }
}