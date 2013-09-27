using System;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch;
using JetBrains.ReSharper.Psi.Services.CSharp.StructuralSearch.Placeholders;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ComGuidChecker
{
    [ElementProblemAnalyzer(new[] { typeof(IAttributeSection) }, HighlightingTypes = new[] { typeof(DuplicateComGuidsHighlighting) })]
    public class DuplicateComGuidsProblemAnalyzer : ElementProblemAnalyzer<IAttributeSection>
    {
        protected override void Run(IAttributeSection attributeSection, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (attributeSection.Target.Name != "assembly") return;
            if (attributeSection.AttributeList == null) return;
            var firstAttribute = attributeSection.AttributeList.FirstChild as IAttribute;
            if (firstAttribute == null) return;
            if (firstAttribute.Name.QualifiedName != "Guid") return;

            if (firstAttribute.Arguments.Count == 0) return;
            var argument = firstAttribute.Arguments.First();
            var expression = argument.Value as ICSharpLiteralExpression;
            if (expression == null) return;
            
            var currentGuid = expression.Literal.GetText();

            var found = DoesThisGuidExistInAnyOtherProject(attributeSection.GetSolution(), attributeSection.GetProject(), currentGuid);
            if (found)
            {
                consumer.AddHighlighting(new DuplicateComGuidsHighlighting(currentGuid), expression.GetDocumentRange(), attributeSection.GetContainingFile());
            }
        }

        private static bool DoesThisGuidExistInAnyOtherProject(IProjectCollection solution, IProject currentProject, string currentGuid)
        {
            var placeHolder = new ArgumentPlaceholder("guid", 1, 1);
            var pattern = new CSharpStructuralSearchPattern("[assembly: Guid($guid$)]", placeHolder);
            var matcher = pattern.CreateMatcher();

            var found = false;

            foreach (var project in solution.GetAllProjects().Where(p => p.IsProjectFromUserView()))
            {
                if (project.ProjectFileLocation != currentProject.ProjectFileLocation)
                {
                    foreach (var file in project.GetAllProjectFiles())
                    {
                        var psiFile = file.GetPrimaryPsiFile();
                        psiFile.ProcessChildren<IAttributeSection>(attributeSection =>
                        {
                            if (attributeSection.Target.Name != "assembly") return;
                            if (attributeSection.AttributeList == null) return;
                            var firstAttribute = attributeSection.AttributeList.FirstChild as IAttribute;
                            if (firstAttribute == null) return;

                            var structuralMatchResult = matcher.Match(firstAttribute);
                            if (structuralMatchResult.Matched)
                            {
                                var guid = structuralMatchResult.GetMatch("guid") as ICSharpLiteralExpression;
                                if (guid != null && guid.GetText() == currentGuid)
                                {
                                    found = true;
                                }
                            }
                        });
                        if (found) break;
                    }
                    if (found) break;
                }
            }
            return found;
        }
    }
}