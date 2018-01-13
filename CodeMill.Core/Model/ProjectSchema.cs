using System;
using System.Collections.Generic;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class ProjectSchema
    {
        public ProjectSchema()
        {
            this.MapDirectory = "Maps";
            this.TemplateDirectory = "Templates";
            this.OutputDirectory = "Output";
            this.Parameters = new Dictionary<string, string>();
            this.Maps = new Dictionary<string, INamedMap>();
            this.Namespaces = new HashSet<string>();
            this.EnumTemplates = new List<TemplateEntry>();
            this.EntityTemplates = new List<TemplateEntry>();
            this.DataModelTemplates = new List<TemplateEntry>();
        }

        public string Name { get; set; }

        public string MapDirectory { get; set; }

        public string TemplateDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public string Description { get; set; }

        public IDictionary<string, string> Parameters { get; protected set; }

        public IDictionary<string, INamedMap> Maps { get; protected set; }

        public ISet<string> Namespaces { get; protected set; }

        public IList<TemplateEntry> EnumTemplates { get; protected set; }

        public IList<TemplateEntry> EntityTemplates { get; protected set; }

        public IList<TemplateEntry> DataModelTemplates { get; protected set; }
    }
}
