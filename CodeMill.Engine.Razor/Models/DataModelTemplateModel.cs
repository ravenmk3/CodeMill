using System;
using CodeMill.Core.Model;

namespace CodeMill.Engine.Razor.Models
{
    [Serializable]
    public class DataModelTemplateModel : TemplateModelBase
    {
        public DataModelTemplateModel(DataModel dataModel, ProjectSchema project)
            : base(dataModel, project)
        {
        }
    }
}
