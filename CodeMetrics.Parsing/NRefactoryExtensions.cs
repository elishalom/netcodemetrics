using ICSharpCode.NRefactory.Ast;

namespace CodeMetrics.Parsing
{
    public static class NRefactoryExtensions
    {
        public static Location AsLocation(this MethodDeclaration methodDeclaration)
        {
            return new Location(methodDeclaration.StartLocation.Line - 1, methodDeclaration.StartLocation.Column - 1);
        }
    }
}