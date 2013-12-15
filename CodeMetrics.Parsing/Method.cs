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

        public override string ToString()
        {
            string declaration = Decleration.ToShortString();
            string bodyStart = BodyStart.ToShortString();
            string bodyEnd = BodyEnd.ToShortString();
            return string.Format("Method:[{0}-{1}-{2}]",declaration, bodyStart, bodyEnd);
        }
    }
}