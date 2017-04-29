namespace CodeMetrics.Common
{
    public class ContainerSettings
    {
        public const string ROSLYN_INSTALLER_TYPE_NAME = "CodeMetrics.Common.Roslyn.RoslynInstaller";
        public const string NREFACTORY_INSTALLER_TYPE_NAME = "CodeMetrics.Common.NRefactory.NRefactoryInstaller";

        //public static string ContainerType => NREFACTORY_INSTALLER_TYPE_NAME;
        public static string ContainerType => ROSLYN_INSTALLER_TYPE_NAME;
    }
}