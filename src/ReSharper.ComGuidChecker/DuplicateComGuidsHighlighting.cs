using System;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;

namespace ReSharper.ComGuidChecker
{
    [StaticSeverityHighlighting(Severity.ERROR, CSharpLanguage.Name)]
    public class DuplicateComGuidsHighlighting : IHighlighting
    {
        private readonly string currentGuid;

        public DuplicateComGuidsHighlighting(string currentGuid)
        {
            this.currentGuid = currentGuid;
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
            get { return string.Format("TypeLib guid {0} is also used by another project in this solution.", currentGuid); }
        }
    }
}
