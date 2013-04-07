namespace CodeMetrics.Parsing
{
    public static class NRefactoryExtensions
    {
        public static Location AsLocation(this ICSharpCode.NRefactory.Location location)
        {
            return new Location(location.Line - 1, location.Column - 1);
        }
    }
}