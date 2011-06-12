using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeMetrics.Calculators;
using CodeMetrics.Parsing;

namespace CodeMetrics.Common
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<IMethodsVisitorFactory>().AsFactory());

            var parsingAssembly = typeof(IMethodsExtractor).Assembly;
            var calculatorsAssembly = typeof(IComplexityCalculator).Assembly;

            container.Register(Component.For<IBranchesVisitor>().ImplementedBy<BranchesVisitor>().LifeStyle.Transient);
            container.Register(Component.For<IMethodsVisitor>().ImplementedBy<MethodsVisitor>().LifeStyle.Transient);
            container.Register(AllTypes.FromAssembly(parsingAssembly).Pick().WithService.DefaultInterface());
            container.Register(AllTypes.FromAssembly(calculatorsAssembly).Pick().WithService.DefaultInterface());
        }
    }
}