using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using StyleCop;
using StyleCop.CSharp;
using Attribute = System.Attribute;

namespace CustomStyleCop
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ControllerRule : SourceAnalyzer
    {
        private const string PostfixNameRule = "ControllerPostfixRule";
        private const string AuthorizeAttributeRule = "AuthorizeAttributeRule";

        public override void AnalyzeDocument(CodeDocument document)
        {
            var csDocument = (CsDocument) document;
            csDocument.WalkDocument(VisitElement);
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            if (element is Class elClass) elClass.WalkElement(VisitClass);

            return true;
        }

        private bool VisitClass(CsElement element, CsElement parentElement, object context)
        {
            if (!(element is Class elClass)) return true;

            if (elClass.BaseClass != nameof(Controller))
            {
                AddViolation(elClass, PostfixNameRule);

                if (elClass.Attributes.All(x => x.Text != nameof(AuthorizeAttribute))) elClass.WalkElement(VisitMethod);
            }

            return false;
        }

        private bool VisitMethod(CsElement element, CsElement parentElement, object context)
        {
            if (!(element is Method elMethod)) return false;
            if (element.Attributes.All(x => x.Text != nameof(AuthorizeAttribute)))
                AddViolation(elMethod, AuthorizeAttributeRule);

            return false;
        }
    }
}