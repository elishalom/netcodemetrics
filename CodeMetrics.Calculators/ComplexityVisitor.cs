using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Calculators
{
    public class ComplexityVisitor : AbstractAstVisitor, IMethodsVisitor
    {
        public int IfsCounter { get; private set; }

        public override object VisitIfElseStatement(IfElseStatement ifElseStatement, object data)
        {
            IfsCounter++;
            if(ifElseStatement.HasElseStatements)
            {
                IfsCounter++;
            }
            return base.VisitIfElseStatement(ifElseStatement, data);
        }
    }
}