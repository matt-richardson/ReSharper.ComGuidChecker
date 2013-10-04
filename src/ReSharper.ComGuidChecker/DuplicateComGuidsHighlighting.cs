using System;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharper.ComGuidChecker
{
    [StaticSeverityHighlighting(Severity.ERROR, CSharpLanguage.Name)]
    public class DuplicateComGuidsHighlighting : IHighlighting
    {
        private readonly ICSharpLiteralExpression expression;

        public DuplicateComGuidsHighlighting(ICSharpLiteralExpression expression)
        {
            this.expression = expression;
        }

        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        public bool IsValid()
        {
            return true;
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public string ToolTip
        {
            get { return string.Format("TypeLib guid {0} is also used by another project in this solution.", Expression.Literal.GetText()); }
        }

        internal ICSharpLiteralExpression Expression
        {
            get { return expression; }
        }
    }
}
