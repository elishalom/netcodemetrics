namespace CodeMetrics.Parsing.Contracts
{
    public interface ILocation
    {
        int Line { get; }
        int Column { get; }

        string ToShortString();
        string ToString();
    }
}