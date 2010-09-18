using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using CodeMetrics.Calculators;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Control = System.Windows.Forms.Control;
using System.Linq;

namespace CodeMetrics.Addin
{
    public class ComplexityViewManager : IDisposable
    {
        private readonly DTE2 dte;
        
        private readonly List<ComplexityViewHost> complexityViews;
        private readonly ComplexityCalculator complexityCalculator;
        private readonly SelectionEvents selectionEvents;
        private readonly ControlsPresenter controlsPresenter;
        private readonly Events2 events2;


        public ComplexityViewManager(DTE2 dte)
        {
            this.dte = dte;

            complexityCalculator = new ComplexityCalculator();
            complexityViews = new List<ComplexityViewHost>();
            
            selectionEvents = new SelectionEvents(dte);
            HookSelectionEvents(selectionEvents);
            controlsPresenter = new ControlsPresenter();
            events2 = (Events2) this.dte.Events;
        }

        void OnTextViewActivated(IVsTextView textView)
        {
            if (!CanUpdate())
            {
                return;
            }

            ClearOldComplexityViews();

            FileCodeModel fileCodeModel = GetCodeModel();
            
            IEnumerable<CodeFunction2> methods = fileCodeModel.GetMethodsWithBody();

            foreach (var method in methods)
            {
                POINT methodLocation = method.GetLocation(textView);

                var complexity = new Complexity(0);

                var complexityView = new ComplexityViewHost(complexity);
                controlsPresenter.ShowControl(complexityView, textView.GetWindowHandle(), methodLocation);
                complexityViews.Add(complexityView);

                CodeFunction2 currentMethod = method;
                ThreadPool.QueueUserWorkItem(state =>
                                                 {
                                                     IComplexity methodComplexity =
                                                         complexityCalculator.Calculate(currentMethod.GetBody());

                                                     complexity.Value = methodComplexity.Value;
                                                 });
            }
        }

        private FileCodeModel GetCodeModel()
        {
            return dte.ActiveWindow.ProjectItem.FileCodeModel;
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