using System;
using StyleCop;
using StyleCop.CSharp;

namespace CustomStyleCop
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ControllerPostfixRule : SourceAnalyzer
    {
        private const string _ruleName = "ControllerPostfixRule";

        public override void AnalyzeDocument(CodeDocument document)
        {
            var csDocument = (CsDocument)document;
            csDocument.WalkDocument(new CodeWalkerElementVisitor<object>(VisitElement));
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            if (element is Class elClass)
            {
                if (!elClass.Name.EndsWith("Controller", StringComparison.Ordinal))
                {
                    this.AddViolation(element, _ruleName);
                }

                return false;
            }

            return true;
        }
    }
}
