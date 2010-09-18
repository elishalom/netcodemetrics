using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CodeMetrics.Calculators;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CodeMetrics.Addin
{
    public class ComplexityViewManager : IDisposable
    {
        private readonly DTE2 dte;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private readonly List<ComplexityViewHost> complexityViews;
        private readonly ComplexityCalculator complexityCalculator;
        private readonly SelectionEvents selectionEvents;


        public ComplexityViewManager(DTE2 dte)
        {
            this.dte = dte;

            complexityCalculator = new ComplexityCalculator();
            complexityViews = new List<ComplexityViewHost>();
            
            selectionEvents = new SelectionEvents(dte);
            HookSelectionEvents(selectionEvents);
        }

        void OnTextViewActivated(IVsTextView textView)
        {
            if (!CanUpdate())
            {
                return;
            }

            ClearOldComplexityViews();

            IEnumerable<CodeFunction2> methods = GetCodeModel().GetMethodsWithBody();

            foreach (var method in methods)
            {
                POINT methodLocation = method.GetLocation(textView);

                string methodBody = method.GetBody();
                IComplexity methodComplexity = complexityCalculator.Calculate(methodBody);

                ShowAndStoreComplexityView(textView, methodComplexity, methodLocation);
            }
        }

        private FileCodeModel GetCodeModel()
        {
            return dte.ActiveWindow.ProjectItem.FileCodeModel;
        }

        private void ShowAndStoreComplexityView(IVsTextView textView, IComplexity methodComplexity, POINT methodLocation)
        {
            var complexityView = new ComplexityViewHost(methodComplexity)
                                     {
                                         Left = methodLocation.x,
                                         Top = methodLocation.y
                                     };

            SetParent(complexityView.Handle, textView.GetWindowHandle());
            ShowWindow(complexityView.Handle, 8);
            complexityViews.Add(complexityView);
        }

        private IEnumerable<CodeFunction2> GetMethods()
        {
            var fileCodeModel = dte.ActiveWindow.ProjectItem.FileCodeModel;
            return fileCodeModel.CodeElements.OfType<CodeNamespace>().SelectMany(
                ns => ns.Children.OfType<CodeType>())
                .Where(codeType => codeType.Kind == vsCMElement.vsCMElementClass)
                .SelectMany(type => type.Children.OfType<CodeFunction2>());
        }

        private bool CanUpdate()
        {
            if (dte.ActiveDocument == null || dte.ActiveDocument.Object("TextDocument") == null)
            {
                return false;
            }
            var textDocument = (TextDocument)dte.ActiveDocument.Object("TextDocument");


            if (!textDocument.Language.Equals("CSharp", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (dte.ActiveWindow.ProjectItem == null)
            {
                return false;
            }

            if (dte.ActiveWindow.ProjectItem.FileCodeModel == null)
            {
                return false;
            }

            return true;
        }

        private void ClearOldComplexityViews()
        {
            complexityViews.ForEach(complexityView =>
                                        {
                                            complexityView.Hide();
                                            complexityView.Dispose();
                                        });

            complexityViews.Clear();
        }

        private void HookSelectionEvents(SelectionEvents selectionEvents)
        {
            selectionEvents.OnSelectionChange += OnTextViewActivated;
            selectionEvents.OnViewChange += RefereshViews;
        }

        private void RefereshViews()
        {
            complexityViews.ForEach(complexityView => complexityView.Refresh());
        }


        public void Dispose()
        {
            selectionEvents.Dispose();
        }
    }
}