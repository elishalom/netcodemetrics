using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Common
{
    public class ContainerFactory
    {
        public static WindsorContainer CreateContainer(IExceptionHandler exceptionHandler)
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.AddFacility<TypedFactoryFacility>();

            container.Install(new CommonInstaller());

            var concreteInstaller = Activator.CreateInstance(Type.GetType(ContainerSettings.ContainerType));
            container.Install((IWindsorInstaller)concreteInstaller);

            container.Register(Component.For<IExceptionHandler>().Instance(exceptionHandler));
            return container;
        }
    }
}