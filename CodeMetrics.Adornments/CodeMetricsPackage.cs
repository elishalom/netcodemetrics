using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using CodeMetrics.Options;
using Microsoft.VisualStudio;

namespace CodeMetrics
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.CodeMetricsPackage)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "Code Metrics", "General", 0, 0, true)]
    [ProvideProfileAttribute(typeof(GeneralOptionsPage), "Code Metrics", "General", 106, 107, true, DescriptionResourceID = 108)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    public class CodeMetricsPackage : Package
    {
        public static CodeMetricsPackage Instance { get; private set; }

        public CodeMetricsPackage()
        {
            Instance = this;
        }

        public GeneralOptionsPage GeneralOptions => (GeneralOptionsPage) GetDialogPage(typeof (GeneralOptionsPage));
    }
}
