using System;
using System.Collections.Generic;
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
        
        private readonly List<ComplexityViewHost> complexityViews;
        private readonly ComplexityCalculator complexityCalculator;
        private readonly SelectionEvents selectionEvents;
        private readonly ControlsPresenter controlsPresenter;


        public ComplexityViewManager(DTE2 dte)
        {
            this.dte = dte;

            complexityCalculator = new ComplexityCalculator();
            complexityViews = new List<ComplexityViewHost>();
            
            selectionEvents = new SelectionEvents(dte);
            HookSelectionEvents(selectionEvents);
            controlsPresenter = new ControlsPresenter();
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
            var complexityView = new ComplexityViewHost(methodComplexity);
            
            controlsPresenter.ShowControl(complexityView, textView.GetWindowHandle(), methodLocation);

            complexityViews.Add(complexityView);
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

        private void RefereshViews()
        {
            complexityViews.ForEach(complexityView => complexityView.Refresh());
        }

        private void HookSelectionEvents(SelectionEvents selectionEvents)
        {
            selectionEvents.OnSelectionChange += OnTextViewActivated;
            selectionEvents.OnViewChange += RefereshViews;
        }


        public void Dispose()
        {
            selectionEvents.Dispose();
        }
    }
}