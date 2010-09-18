using System;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Calculators
{
    public class ComplexityCalculator
    {
        public IComplexity Calculate(string method)
        {
            BlockStatement blockStatement;
            using (var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(method)))
            {
                try
                {
                    blockStatement = parser.ParseBlock();
                }
                catch (NullReferenceException e)
                {
                    return new Complexity(1);
                }
            }

            var visitor = new Visitor();
            blockStatement.AcceptVisitor(visitor, null);


            return new Complexity(visitor.IfsCounter + 1);
        }
    }

    public class Visitor : AbstractAstVisitor
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

    public class Complexity : IComplexity
    {
        public int Value { get; private set; }

        public Complexity(int value)
        {
            Value = value;
        }
    }

    public interface IComplexity
    {
        int Value { get;  }
    }
}