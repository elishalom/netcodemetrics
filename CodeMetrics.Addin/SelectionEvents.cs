using System;
using System.Runtime.InteropServices;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace CodeMetrics.Addin
{
    public class SelectionEvents : IVsSelectionEvents, IDisposable
    {
        private readonly IVsMonitorSelection vsMonitorSelection;
        private readonly uint pdwCookie;

        public Action<IVsTextView> OnSelectionChange = ptr => { };
        public Action OnViewChange = () => { };

        public SelectionEvents(DTE2 dte)
        {
            vsMonitorSelection = GetMonitorSelection(dte);
            vsMonitorSelection.AdviseSelectionEvents(this, out pdwCookie);
        }

        private static IVsMonitorSelection GetMonitorSelection(DTE2 dte)
        {
            var sid = typeof(SVsShellMonitorSelection).GUID;
            var iid = typeof(IVsMonitorSelection).GUID;
            IntPtr output;

            ((IServiceProvider)dte).QueryService(ref sid, ref iid, out output);
            return (IVsMonitorSelection)Marshal.GetObjectForIUnknown(output);
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            IVsTextView view = GetTextView(varValueNew);
            if (view != null)
            {
                OnSelectionChange(view);
            }

            return VSConstants.S_OK;
        }

        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            OnViewChange();
            return VSConstants.S_OK;
        }

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            OnViewChange();
            return VSConstants.S_OK;
        }

        private static IVsTextView GetTextView(object varValueNew)
        {
            IVsWindowFrame newWindowFrame =  GetWindowFrame(varValueNew);
            if (newWindowFrame == null)
            {
                return null;
            }

            IVsCodeWindow vsCodeWindow = GetCodeView(newWindowFrame);
            if(vsCodeWindow == null)
            {
                return null;
            }

            return GetTextView(vsCodeWindow);
        }

        private static IVsTextView GetTextView(IVsCodeWindow vsCodeWindow)
        {
            IVsTextView view;
            vsCodeWindow.GetPrimaryView(out view);

            return view;
        }

        private static IVsCodeWindow GetCodeView(IVsWindowFrame newWindowFrame)
        {
            object docView;
            int result = newWindowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out docView);

            bool didGetView = result == VSConstants.S_OK;
            if (!didGetView)
            {
                return null;
            }

            bool isCodeWindow = docView is IVsCodeWindow;
            if (!isCodeWindow)
            {
                return null;
            }

            return (IVsCodeWindow)docView;
        }

        private static IVsWindowFrame GetWindowFrame(object varValueNew)
        {
            bool isNewWindow = varValueNew != null && varValueNew is IVsWindowFrame;
            if (!isNewWindow)
            {
                return null;
            }
            return (IVsWindowFrame)varValueNew;
        }

        public void Dispose()
        {
            vsMonitorSelection.UnadviseSelectionEvents(pdwCookie);
        }
    }
}