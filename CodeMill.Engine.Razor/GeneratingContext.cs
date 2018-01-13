using System;
using System.IO;
using CodeMill.Core.Model;
using RazorEngine.Templating;

namespace CodeMill.Engine.Razor
{
    public class GeneratingContext
    {
        public GeneratingContext(IRazorEngineService razor, string workDirectory, DataModel dataModel, ProjectSchema project)
        {
            this.Razor = razor ?? throw new ArgumentNullException(nameof(razor));
            this.WorkDirectory = workDirectory ?? throw new ArgumentNullException(nameof(workDirectory));
            this.DataModel = dataModel ?? throw new ArgumentNullException(nameof(dataModel));
            this.Project = project ?? throw new ArgumentNullException(nameof(project));

            this.TemplateDirectory = Path.IsPathRooted(project.TemplateDirectory)
                ? project.TemplateDirectory
                : Path.Combine(workDirectory, project.TemplateDirectory);

            this.OutputDirectory = Path.IsPathRooted(project.OutputDirectory)
                ? project.OutputDirectory
                : Path.Combine(workDirectory, project.OutputDirectory);
        }

        public IRazorEngineService Razor { get; protected set; }

        public string WorkDirectory { get; protected set; }

        public DataModel DataModel { get; protected set; }

        public ProjectSchema Project { get; protected set; }

        public string TemplateDirectory { get; protected set; }

        public string OutputDirectory { get; protected set; }

        public string ResolveOutputPath(string path)
        {
            return Path.Combine(this.OutputDirectory, path);
        }

        public string ResolveOutputPath(string pathFormat, params object[] args)
        {
            var path = String.Format(pathFormat, args);
            return Path.Combine(this.OutputDirectory, path);
        }
    }
}
