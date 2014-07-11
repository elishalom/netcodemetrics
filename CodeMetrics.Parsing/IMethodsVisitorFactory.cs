namespace CodeMetrics.Parsing
{
    public interface IBranchesVisitorFactory
    {
        IBranchesVisitor CreateBranchesVisitor();
    }

    public interface IMethodsVisitorFactory
    {
        IMethodsVisitor CreateMethodsVisitor();
    }
}