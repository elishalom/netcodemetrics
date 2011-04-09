using System;

namespace CodeMetrics.Parsing
{
    public interface IMethod
    {
        Location Start { get; }
    }

    class Method : IMethod
    {
        public Method(Location start)
        {
            Start = start;
        }

        public Location Start { get; private set; }
    }
}