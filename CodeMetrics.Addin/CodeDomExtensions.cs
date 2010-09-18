using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CodeMetrics.Addin
{
    public static class CodeDomExtensions
    {
        public static string GetBody(this CodeFunction2 method)
        {
            var startOfBody = method.GetStartPoint(vsCMPart.vsCMPartBody);
            var endOfBody = method.GetEndPoint(vsCMPart.vsCMPartBody);
            return startOfBody.CreateEditPoint().GetText(endOfBody.CreateEditPoint());
        }

        public static POINT GetLocation(this CodeFunction2 codeFunction2, IVsTextView textView)
        {
            var startPoint = codeFunction2.GetStartPoint(vsCMPart.vsCMPartHeader);
            var point = new POINT[1];
            textView.GetPointOfLineColumn(startPoint.Line, startPoint.DisplayColumn, point);
            return point[0];
        }

        public static IEnumerable<CodeFunction2> GetMethodsWithBody(this FileCodeModel codeModel)
        {
            return codeModel.CodeElements.OfType<CodeNamespace>()
                .SelectMany(ns => ns.Children.OfType<CodeType>())
                .Where(codeType => codeType.Kind == vsCMElement.vsCMElementClass)
                .SelectMany(type => type.Children.OfType<CodeFunction2>());
        }
    }
}