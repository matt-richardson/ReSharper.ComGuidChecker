using System;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharper.ComGuidChecker
{
    [ElementProblemAnalyzer(new[] { typeof(IAttributeSection) }, HighlightingTypes = new[] { typeof(DuplicateComGuidsHighlighting) })]
    public class DuplicateComGuidsProblemAnalyzer : ElementProblemAnalyzer<IAttributeSection>
    {
        protected override void Run(IAttributeSection attributeSection, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            if (!Helper.IsValidTarget(attributeSection)) return;

            var expression = Helper.GetExpression(attributeSection);

            var currentGuid = expression.Literal.GetText();

            var found = DoesThisGuidExistInAnyOtherProject(attributeSection.GetSolution(), attributeSection.GetProject(), currentGuid);
            if (found)
            {
                consumer.AddHighlighting(new DuplicateComGuidsHighlighting(expression), expression.GetDocumentRange(), attributeSection.GetContainingFile());
            }
        }

        private static bool DoesThisGuidExistInAnyOtherProject(IProjectCollection solution, IProject currentProject, string currentGuid)
        {
            var projects = solution.GetAllProjects()
                .Where(p => p.IsProjectFromUserView())
                .Where(project => ProjectIsDifferent(currentProject, project));
            
            return projects.Any(project => HasAssemblyGuid(currentGuid, project));
        }

        private static bool ProjectIsDifferent(IProject currentProject, IProject project)
        {
            return project.ProjectFileLocation != currentProject.ProjectFileLocation;
        }

        private static bool HasAssemblyGuid(string currentGuid, IProject project)
        {
            return project.GetAllProjectFiles().Any(file => HasAssemblyGuid(currentGuid, file));
        }

        private static bool HasAssemblyGuid(string currentGuid, IProjectFile file)
        {
            var found = false;
            var psiFile = file.GetPrimaryPsiFile();
            psiFile.ProcessChildren<IAttributeSection>(attributeSection =>
            {
                found = found || HasAssemblyGuid(currentGuid, attributeSection);
            });
            return found;
        }

        private static bool HasAssemblyGuid(string currentGuid, IAttributeSection attributeSection)
        {
            if (!Helper.IsValidTarget(attributeSection))
                return false;
            
            var expression = Helper.GetExpression(attributeSection);
            var guid = expression.Literal.GetText();

            return guid == currentGuid;
        }
    }
}