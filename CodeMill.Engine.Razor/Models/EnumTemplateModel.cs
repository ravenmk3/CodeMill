using System;
using CodeMill.Core.Model;

namespace CodeMill.Engine.Razor.Models
{
    [Serializable]
    public class EnumTemplateModel : TemplateModelBase
    {
        public EnumTemplateModel(DataModel dataModel, ProjectSchema project, EnumSchema @enum)
            : base(dataModel, project)
        {
            this.Enum = @enum ?? throw new ArgumentNullException(nameof(@enum));
        }

        public EnumSchema Enum { get; protected set; }
    }
}
