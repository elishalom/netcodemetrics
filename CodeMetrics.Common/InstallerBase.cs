using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CodeMetrics.Common
{
    public abstract class InstallerBase : IWindsorInstaller
    {
        public abstract void Install(IWindsorContainer container, IConfigurationStore store);

        [Obsolete("Not used installation method. Please remove")]
        protected void InstallFromAssembly(IWindsorContainer container, Type type)
        {
            var installingAssembly = type.Assembly;
            container.Register(Classes.FromAssembly(installingAssembly).Pick().WithServiceDefaultInterfaces());
        }
    }
}
