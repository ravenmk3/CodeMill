using System;
using CodeMill.Core.Model;

namespace CodeMill.Core.Engine
{
    public interface IEngine
    {
        string Name { get; }

        string TemplateExtension { get; }

        void Execute(DataModel dataModel, ProjectSchema project);
    }
}
