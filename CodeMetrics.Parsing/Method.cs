namespace CodeMetrics.Parsing
{
    public class Method : IMethod
    {
        public Method(Location methodDeclaration, Location bodyStart, Location bodyEnd)
        {
            Decleration = methodDeclaration;
            BodyStart = bodyStart;
            BodyEnd = bodyEnd;
        }

        public Location Decleration { get; private set; }
        public Location BodyStart { get; private set; }
        public Location BodyEnd { get; private set; }
    }
}