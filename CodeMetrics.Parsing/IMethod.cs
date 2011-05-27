namespace CodeMetrics.Parsing
{
    public interface IMethod
    {
        Location Start { get; }
        Location End { get; }
    }

    class Method : IMethod
    {
        public Method(Location start, Location end)
        {
            Start = start;
            End = end;
        }

        public Location Start { get; private set; }
        public Location End { get; private set; }
    }
}