namespace CodeMetrics.Parsing.Contracts
{
    public interface ILocationFactory
    {
        ILocation Create(int line, int column);
    }
}