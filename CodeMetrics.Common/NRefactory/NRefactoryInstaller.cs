using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Calculators.Contracts.NRefactory;
using CodeMetrics.Calculators.NRefactory;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.NRefactory;
using CodeMetrics.Parsing.NRefactory;

namespace CodeMetrics.Common.NRefactory
{
    public class NRefactoryInstaller : InstallerBase
    {
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAstBranchesVisitorFactory>().AsFactory());
            container.Register(Component.For<IAstConditionsVisitorFactory>().AsFactory());
            container.Register(Component.For<IAstMethodsExtractorFactory>().AsFactory());
            container.Register(Component.For<IAstMethodsVisitorFactory>().AsFactory());
            container.Register(Component.For<IAstCyclomaticComplexityCalculatorFactory>().AsFactory());

            container.Register(Component.For<IBranchesVisitor, IAstBranchesVisitor>().ImplementedBy<AstBranchesVisitor>().LifeStyle.Transient);
            container.Register(Component.For<IConditionsVisitor, IAstConditionsVisitor>().ImplementedBy<AstConditionsVisitor>().LifeStyle.Transient);
            container.Register(Component.For<IMethodsVisitor, IAstMethodsVisitor>().ImplementedBy<AstMethodsVisitor>().LifeStyle.Transient);
            container.Register(Component.For<IMethodsExtractor, IAstMethodsExtractor>().ImplementedBy<AstMethodsExtractor>().LifeStyle.Transient);
            container.Register(Component.For<ICyclomaticComplexityCalculator, IAstCyclomaticComplexityCalculator>().ImplementedBy<AstCyclomaticComplexityCalculator>().LifeStyle.Transient);
        }
    }
}
