using Castle.Windsor;
using Castle.Windsor.Installer;

namespace CodeMetrics.Common
{
    public class ContainerFactory
    {
        public static WindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.Install(new RepositoriesInstaller());
            return container;
        }
    }
}