using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeMetrics.Calculators;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Parsing;
using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Common
{
    public class CommonInstaller : InstallerBase
    {
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBranchesVisitorFactory>().AsFactory());
            container.Register(Component.For<IConditionsVisitorFactory>().AsFactory());
            container.Register(Component.For<IConstructorFactory>().AsFactory());
            container.Register(Component.For<ILocationFactory>().AsFactory());
            container.Register(Component.For<IMethodFactory>().AsFactory());
            container.Register(Component.For<IPropertyFactory>().AsFactory());
            container.Register(Component.For<IPropertyAccessorFactory>().AsFactory());
            container.Register(Component.For<IMethodDeclarationFactory>().AsFactory());
            container.Register(Component.For<IPropertyDeclarationFactory>().AsFactory());
            container.Register(Component.For<IMethodsExtractorFactory>().AsFactory());
            container.Register(Component.For<IMethodsVisitorFactory>().AsFactory());
            container.Register(Component.For<ICyclomaticComplexityFactory>().AsFactory());
            container.Register(Component.For<ICyclomaticComplexityCalculatorFactory>().AsFactory());

            container.Register(Component.For<IUnderlyingObject>().ImplementedBy<UnderlyingObject>().LifeStyle.Transient);
            container.Register(Component.For<IUnderlyingObjectFactory>().ImplementedBy<UnderlyingObjectFactory>().LifeStyle.Singleton);
            container.Register(Component.For<IConstructor>().ImplementedBy<Constructor>().LifeStyle.Transient);
            container.Register(Component.For<ILocation>().ImplementedBy<Location>().LifeStyle.Transient);
            container.Register(Component.For<IMethod>().ImplementedBy<Method>().LifeStyle.Transient);
            container.Register(Component.For<IProperty>().ImplementedBy<Parsing.Property>().LifeStyle.Transient);
            container.Register(Component.For<IPropertyAccessor>().ImplementedBy<PropertyAccessor>().LifeStyle.Transient);
            container.Register(Component.For<IMethodDeclaration>().ImplementedBy<MethodDeclaration>().LifeStyle.Transient);
            container.Register(Component.For<IPropertyDeclaration>().ImplementedBy<PropertyDeclaration>().LifeStyle.Transient);
            container.Register(Component.For<ICyclomaticComplexity>().ImplementedBy<CyclomaticComplexity>().LifeStyle.Transient);
        }
    }
}