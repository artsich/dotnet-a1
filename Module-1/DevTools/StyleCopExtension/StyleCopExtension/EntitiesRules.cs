using StyleCop;
using StyleCop.CSharp;
using System;
using System.Linq;
using System.Reflection;
using DataContractAttribute = System.Runtime.Serialization.DataContractAttribute;

namespace Custom.StyleCop
{
    [SourceAnalyzer(typeof(CsParser))]
    public class ControllerRule : SourceAnalyzer
    {
        private const string OnlyPublicRule = "EntitiesClassOnlyPublicRule";
        private const string MarkedDataContractRule = "EntitiesMarkedDataContractRule";
        private const string ContainsIdMemberRule = "EntitiesContainsIdRule";
        private const string ContainsNameMemberRule = "EntitiesContainsNameRule";

        public override void AnalyzeDocument(CodeDocument document)
        {
            var csDocument = (CsDocument) document;
            csDocument.WalkDocument(VisitElement);
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            if (element is Class elClass && elClass.FullNamespaceName.EndsWith("Entities"))
            {
                var type = Type.GetType(elClass.Name);
                if (IsTypePublic(type)) AddViolation(element, OnlyPublicRule);

                if (IsMarkedDataContractAttribute(type)) AddViolation(element, MarkedDataContractRule);

                if (IsContainsPublicMemberId(type)) AddViolation(element, ContainsIdMemberRule);

                if (IsContainsPublicMemberName(type)) AddViolation(element, ContainsNameMemberRule);

                return false;
            }

            return true;
        }

        private static bool IsTypePublic(Type type)
        {
            return type.IsPublic;
        }

        public static bool IsMarkedDataContractAttribute(Type type)
        {
            return type
                .GetCustomAttributes(typeof(DataContractAttribute)).Any();
        }

        private static bool IsContainsPublicMemberId(Type type)
        {
            return type.GetMembers().Any(x => x.Name.Equals("Id"));
        }

        private static bool IsContainsPublicMemberName(Type type)
        {
            return type.GetMembers().Any(x => x.Name.Equals("Name"));
        }
    }
}