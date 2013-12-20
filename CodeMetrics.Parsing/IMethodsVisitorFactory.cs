using System.Collections.Generic;

namespace CodeMetrics.Parsing
{
    public interface IBranchesVisitorFactory
    {
        IBranchesVisitor CreateBranchesVisitor();
    }

    public interface IMethodsVisitorFactory
    {
        IEnumerable<IMethodsVisitor> CreateMethodsVisitor();
    }
}