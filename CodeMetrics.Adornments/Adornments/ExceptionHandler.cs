using System;
using CodeMetrics.Parsing.Contracts;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodeMetrics.Adornments
{
    /// <summary>
    /// Exception handler, to write message to the Output window
    /// see http://stackoverflow.com/questions/1094366/how-do-i-write-to-the-visual-studio-output-window-in-my-custom-tool
    /// </summary>
    /// <seealso cref="IExceptionHandler" />
    public class ExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
            WriteToCodeMetricsOutputWindow(exception);
        }

        private static IVsOutputWindowPane GetCodeMetricsPane(IVsOutputWindow outWindow)
        {
            var customGuid = new Guid(GuidList.CodeMetricsOutputWindow);
            outWindow.CreatePane(ref customGuid, "Code Metrics", 1, 1);

            IVsOutputWindowPane customPane;
            outWindow.GetPane(ref customGuid, out customPane);
            return customPane;
        }

        private static IVsOutputWindowPane GetGeneralPane(IVsOutputWindow outWindow)
        {
            var generalPaneGuid = VSConstants.GUID_OutWindowGeneralPane;
            IVsOutputWindowPane generalPane;
            outWindow.GetPane(ref generalPaneGuid, out generalPane);
            return generalPane;
        }

        private static void WriteExceptionToPane(Exception exception, IVsOutputWindowPane generalPane)
        {
            var outputString = string.Format("{0}{1}{0}{2}", Environment.NewLine, exception.Message,
                exception.StackTrace);
            generalPane.OutputString(outputString);
            generalPane.Activate();
        }

        private void WriteToCodeMetricsOutputWindow(Exception exception)
        {
            var outWindow = GetOutputWindow();
            var customPane = GetCodeMetricsPane(outWindow);
            WriteExceptionToPane(exception, customPane);
        }

        private void WriteToGeneralOutputWindow(Exception exception)
        {
            var outWindow = GetOutputWindow();
            var generalPane = GetGeneralPane(outWindow);
            WriteExceptionToPane(exception, generalPane);
        }

        private static IVsOutputWindow GetOutputWindow()
        {
            return Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
        }
    }
}