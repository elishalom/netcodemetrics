namespace CodeMetrics.Parsing
{
    public interface IMethod
    {
        Location Decleration { get; }
        Location BodyStart { get; }
        Location BodyEnd { get; }
    }
}