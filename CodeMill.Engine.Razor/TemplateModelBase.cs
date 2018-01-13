using System;
using System.Collections.Generic;
using CodeMill.Core.Model;

namespace CodeMill.Engine.Razor
{
    [Serializable]
    public abstract class TemplateModelBase
    {
        public TemplateModelBase(DataModel dataModel, ProjectSchema project)
        {
            this.DataModel = dataModel ?? throw new ArgumentNullException(nameof(dataModel));
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public DataModel DataModel { get; protected set; }

        public ProjectSchema Project { get; protected set; }

        public IDictionary<string, string> Parameters
        {
            get { return this.Project.Parameters; }
        }

        public IDictionary<string, INamedMap> Maps
        {
            get { return this.Project.Maps; }
        }

        public string GetParameter(string name)
        {
            if (this.Parameters.TryGetValue(name, out var value))
            {
                return value;
            }
            return null;
        }

        public INamedMap GetMap(string name)
        {
            if (this.Maps.TryGetValue(name, out var value))
            {
                return value;
            }
            return null;
        }
    }
}
