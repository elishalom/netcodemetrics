namespace CodeMetrics.Parsing
{
    public interface IMethodsVisitorFactory
    {
        IBranchesVisitor CreateBranchesVisitor();
        IMethodsVisitor CreateMethodsVisitor();
    }
}