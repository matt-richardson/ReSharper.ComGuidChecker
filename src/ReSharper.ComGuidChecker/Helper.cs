using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharper.ComGuidChecker
{
    public static class Helper
    {
        public static ICSharpLiteralExpression GetExpression(IAttributeSection attributeSection)
        {
            var firstAttribute = attributeSection.AttributeList.FirstChild as IAttribute;
            var argument = firstAttribute.Arguments.First();
            var expression = argument.Value as ICSharpLiteralExpression;
            return expression;
        }

        public static bool IsValidTarget(IAttributeSection attributeSection)
        {
            if (attributeSection.Target.Name != "assembly") return false;
            if (attributeSection.AttributeList == null) return false;
            var firstAttribute = attributeSection.AttributeList.FirstChild as IAttribute;
            if (firstAttribute == null) return false;
            if (firstAttribute.Name.QualifiedName != "Guid") return false;

            if (firstAttribute.Arguments.Count == 0) return false;
            var argument = firstAttribute.Arguments.First();
            var expression = argument.Value as ICSharpLiteralExpression;
            if (expression == null) return false;
            return true;
        } 
    }
}