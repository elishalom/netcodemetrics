using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using CodeMetrics.Calculators;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Window = System.Windows.Window;

namespace CodeMetrics.Addin
{
	public class Connect : IDTExtensibility2
	{
		public Connect()
		{
		}

		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			dte = (DTE2)application;
			addInInstance = (AddIn)addInInst;
		}

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        void WindowEvents_WindowActivated(IVsTextView textView)
        {
            if (dte.ActiveDocument == null || dte.ActiveDocument.Object("TextDocument") == null)
            {
                return;
            }
            var textDocument = (TextDocument) dte.ActiveDocument.Object("TextDocument");


            if (!textDocument.Language.Equals("CSharp", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (dte.ActiveWindow.ProjectItem == null)
            {
                return;
            }

            if (dte.ActiveWindow.ProjectItem.FileCodeModel == null)
            {
                return;
            }

            userControls.ForEach(control1 =>
                                     {
                                         control1.Hide();
                                         control1.Dispose();
                                     });
            userControls.Clear();

            var fileCodeModel = dte.ActiveWindow.ProjectItem.FileCodeModel;
            var methods = fileCodeModel.CodeElements.OfType<CodeNamespace>().SelectMany(
                ns => ns.Children.OfType<CodeType>())
                .Where(codeType => codeType.Kind == vsCMElement.vsCMElementClass)
                .SelectMany(type => type.Children.OfType<CodeFunction2>());

            foreach (var codeFunction2 in methods)
            {
                var startPoint = codeFunction2.GetStartPoint(vsCMPart.vsCMPartHeader);

                var point = new POINT[1];
                try
                {
                    textView.GetPointOfLineColumn(startPoint.Line, startPoint.DisplayColumn, point);

                    var startOfBody = codeFunction2.GetStartPoint(vsCMPart.vsCMPartBody);
                    var endOfBody = codeFunction2.GetEndPoint(vsCMPart.vsCMPartBody);
                    var text = startOfBody.CreateEditPoint().GetText(endOfBody.CreateEditPoint());

                    ;
                    var userControl1 = new ComplexityViewHost(complexityCalculator.Calculate(text));
                    userControl1.Left = point[0].x;
                    userControl1.Top = point[0].y;
                    //                    userControl1.label1.Text = new ComplexityCalculator().Calculate(text).Value.ToString();
                    SetParent(userControl1.Handle, textView.GetWindowHandle());
                    Print("Showing control");
                    
                    ShowWindow(userControl1.Handle, 8);
                    userControls.Add(userControl1);
                }
                catch (Exception e)
                {
                    Print("Failed showing control " + e.Message);
                    Console.WriteLine(e);
                }
            }
        }

	    public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
            vsMonitorSelection.UnadviseSelectionEvents(pdwCookie);
		}

		public void OnAddInsUpdate(ref Array custom)
		{
		}

		public void OnStartupComplete(ref Array custom)
		{
            var SID = typeof(SVsShellMonitorSelection).GUID;
            var IID = typeof(IVsMonitorSelection).GUID;
                        var output = (IntPtr)0;

            ((IServiceProvider) dte).QueryService(ref SID, ref IID, out output);
            vsMonitorSelection = (IVsMonitorSelection)Marshal.GetObjectForIUnknown(output);

		    var selectionEvents = new SelectionEvents(dte);
		    vsMonitorSelection.AdviseSelectionEvents(selectionEvents, out pdwCookie);

		    selectionEvents.OnChange += WindowEvents_WindowActivated;
		    selectionEvents.OnSmallChange += () => userControls.ForEach(control1 => control1.Refresh());
		}

        public class SelectionEvents : IVsSelectionEvents
        {
            public Action<IVsTextView> OnChange = ptr => { };
            public Action OnSmallChange = ()=> { };
            private readonly DTE2 dte2;

            public SelectionEvents(DTE2 dte2)
            {
                this.dte2 = dte2;
            }

            public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
            {
                Print("In OnSelectionChanged");
                OnSmallChange();
                return VSConstants.S_OK;
            }

            public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
            {
                Print("In OnElementValueChanged");
                if (varValueNew == null || !(varValueNew is IVsWindowFrame))
                {
                    return VSConstants.S_OK;
                }
                var vsNewWindowFrame = (IVsWindowFrame) varValueNew;
                object docView;
                int result = vsNewWindowFrame.GetProperty((int) __VSFPROPID.VSFPROPID_DocView, out docView);
                if (result != VSConstants.S_OK || !(docView is IVsCodeWindow))
                {
                    return VSConstants.S_OK;
                }

                try
                {
                    var vsCodeWindow = (IVsCodeWindow) docView;
                    
                    IVsTextView view;
                    vsCodeWindow.GetPrimaryView(out view);

                    Print("In OnElementValueChanged raising OnChange");
                    OnChange(view);
                    PrintLine();
                }
                catch (Exception e)
                {
                    Print("OnElementValueChanged failed: " + e.Message);
                }
                return VSConstants.S_OK;
            }

            public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
            {
                Print("In OnCmdUIContextChanged");
                OnSmallChange();
                return VSConstants.S_OK;
            }

            private void Print(string inOncmduicontextchanged)
            {
                var outputWindow = dte2.ToolWindows.OutputWindow;
                if(outputWindow == null)
                {
                    return;
                }
                if (outputWindow.ActivePane != null)
                {
                    outputWindow.ActivePane.OutputString(DateTime.Now.ToLongTimeString() + ": " + inOncmduicontextchanged + Environment.NewLine);
                }
            }

            private void PrintLine()
            {
                var outputWindow = dte2.ToolWindows.OutputWindow;
                if(outputWindow == null)
                {
                    return;
                }
                if (outputWindow.ActivePane != null)
                {
                    outputWindow.ActivePane.OutputString(Environment.NewLine);
                }
            }
        }

		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		private DTE2 dte;
		private AddIn addInInstance;
	    private uint pdwCookie;
	    private IVsMonitorSelection vsMonitorSelection;
	    private List<ComplexityViewHost> userControls = new List<ComplexityViewHost>();
	    private ComplexityCalculator complexityCalculator = new ComplexityCalculator();

	    private void Print(string inOncmduicontextchanged)
        {
            var outputWindow = dte.ToolWindows.OutputWindow;
            if (outputWindow == null)
            {
                return;
            }
            if (outputWindow.ActivePane != null)
            {
                outputWindow.ActivePane.OutputString(DateTime.Now.ToLongTimeString() + ": " + inOncmduicontextchanged + Environment.NewLine);
            }
        }
	}
}