using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Calculators.Roslyn;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using CodeMetrics.Parsing.Roslyn;

namespace CodeMetrics.Common.Roslyn
{
    public class RoslynInstaller : InstallerBase
    {
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICSharpBranchesVisitorFactory>().AsFactory());
            container.Register(Component.For<ICSharpBranchesVisitor>().ImplementedBy<CSharpBranchesVisitor>().LifeStyle.Transient);
            container.Register(Component.For<ICSharpConditionsVisitorFactory>().AsFactory());
            container.Register(Component.For<ICSharpConditionsVisitor>().ImplementedBy<CSharpConditionsVisitor>().LifeStyle.Transient);

            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<IMethodTypeResolvingRule>()
                .WithService.Select(new[] { typeof(IMethodTypeResolvingRule) })
                .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<IMethodConvertor>()
                .WithService.Select(new[] { typeof(IMethodConvertor) })
                .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(
                Component.For<IMethodExtractor, ISyntaxNodeExtractor>()
                    .ImplementedBy<MethodExtractor>()
                    .LifeStyle.Transient);

            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<IPropertyTypeResolvingRule>()
                .WithService.Select(new[] { typeof(IPropertyTypeResolvingRule) })
                .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<IPropertyConvertor>()
                .WithService.Select(new[] { typeof(IPropertyConvertor) })
                .Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(
                Component.For<IPropertyExtractor, ISyntaxNodeExtractor>()
                    .ImplementedBy<PropertyExtractor>()
                    .LifeStyle.Transient);

            container.Register(
                Component.For<IPropertyAccessorExtractor, ISyntaxNodeExtractor>()
                    .ImplementedBy<PropertyAccessorExtractor>()
                    .LifeStyle.Transient);

            container.Register(
                Component.For<IConstructorExtractor, ISyntaxNodeExtractor>()
                    .ImplementedBy<ConstructorExtractor>()
                    .LifeStyle.Transient);

            container.Register(Component.For<IMethodTypeResolver>().ImplementedBy<MethodTypeResolver>().LifeStyle.Transient);
            container.Register(Component.For<IPropertyTypeResolver>().ImplementedBy<PropertyTypeResolver>().LifeStyle.Transient);
            container.Register(Component.For<IMethodsExtractor>().ImplementedBy<CSharpMethodsExtractor>().LifeStyle.Transient);
            container.Register(Component.For<ICyclomaticComplexityCalculator>().ImplementedBy<CSharpComplexityCalculator>().LifeStyle.Transient);
        }
    }
}