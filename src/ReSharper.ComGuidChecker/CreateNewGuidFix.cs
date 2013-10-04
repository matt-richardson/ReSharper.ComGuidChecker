using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharper.ComGuidChecker
{
    [QuickFix]
    public class CreateNewGuidFix : QuickFixBase 
    {
        private readonly DuplicateComGuidsHighlighting highlighting;

        public CreateNewGuidFix(DuplicateComGuidsHighlighting highlighting)
        {
            this.highlighting = highlighting;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return true;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var factory = CSharpElementFactory.GetInstance(highlighting.Expression.GetPsiModule());
            var replacement = factory.CreateExpression(string.Format("\"{0}\"", Guid.NewGuid()));
            ModificationUtil.ReplaceChild(highlighting.Expression, replacement);
            
            return null;
        }

        public override string Text
        {
            get { return "Generate new Guid"; }
        }
    }
}