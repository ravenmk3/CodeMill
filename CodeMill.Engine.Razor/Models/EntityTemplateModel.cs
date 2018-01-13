using System;
using CodeMill.Core.Model;

namespace CodeMill.Engine.Razor.Models
{
    [Serializable]
    public class EntityTemplateModel : TemplateModelBase
    {
        public EntityTemplateModel(DataModel dataModel, ProjectSchema project, EntitySchema entity)
            : base(dataModel, project)
        {
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public EntitySchema Entity { get; protected set; }
    }
}
