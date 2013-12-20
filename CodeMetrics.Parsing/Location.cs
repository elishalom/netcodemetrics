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

        public override string ToString()
        {
            return string.Format("Location:{0},{1}", Line, Column);
        }

        internal string ToShortString()
        {
            return string.Format("{0},{1}", Line, Column);
        }
    }
}