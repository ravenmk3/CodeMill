using System;
using System.Collections.Generic;
using System.IO;
using CodeMill.Core.Common;
using CodeMill.Core.Model;
using CodeMill.Core.Xml;
using CodeMill.Engine.Razor.Models;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace CodeMill.Engine.Razor
{
    public static class Generator
    {
        static Generator()
        {
            var config = new TemplateServiceConfiguration()
            {
                Debug = false,
                DisableTempFileLocking = true,
                BaseTemplateType = typeof(CodeTemplate<>),
                Namespaces = new HashSet<string>()
                {
                    "System",
                    "System.Collections",
                    "System.Collections.Generic",
                    "System.Linq",
                    "CodeMill",
                    "CodeMill.Core.Common",
                    "CodeMill.Core.Model",
                    "CodeMill.Core.Model.Enums",
                    "CodeMill.Engine.Razor",
                    "CodeMill.Engine.Razor.Models",
                    "CodeMill.Engine.Razor.Helpers",
                },
            };
            RazorEngine.Engine.Razor = RazorEngineService.Create(config);
        }

        public static void Execute(string dataModelPath, string projectPath, string outputPath = null)
        {
            Execute(dataModelPath, projectPath, outputPath, new ConsoleTextOutput());
        }

        public static void Execute(string dataModelPath, string projectPath, string outputPath, ITextOutput output)
        {
            output.WriteLine("Execution: Begin");

            var dataModel = DataModelReader.Read(dataModelPath);
            output.WriteLine("Load Model: {0}", dataModelPath);

            var project = ProjectSchemaReader.Read(projectPath);
            output.WriteLine("Load Project: {0}", projectPath);

            if (!String.IsNullOrEmpty(outputPath))
            {
                output.WriteLine("Override Output Directory: {0}", outputPath);
                project.OutputDirectory = outputPath;
            }
            else
            {
                output.WriteLine("Output Directory: {0}", project.OutputDirectory);
            }

            var workDir = Path.GetDirectoryName(PathUtility.ResolvePath(projectPath));
            var config = new TemplateServiceConfiguration()
            {
                Debug = false,
                BaseTemplateType = typeof(CodeTemplate<>),
                DisableTempFileLocking = true,
                Namespaces = new HashSet<string>()
                {
                    "System",
                    "System.Collections",
                    "System.Collections.Generic",
                    "System.Linq",
                    "CodeMill",
                    "CodeMill.Core.Common",
                    "CodeMill.Core.Model",
                    "CodeMill.Core.Model.Enums",
                    "CodeMill.Engine.Razor",
                    "CodeMill.Engine.Razor.Models",
                    "CodeMill.Engine.Razor.Helpers",
                },
            };
            if (project.Namespaces != null)
            {
                project.Namespaces.ForEach(n => config.Namespaces.Add(n));
            }
            var razor = RazorEngineService.Create(config);
            var context = new GeneratingContext(razor, workDir, dataModel, project);

            output.WriteLine("LoadTemplates: Enum");
            var enumTemplates = LoadTemplates(project.EnumTemplates, context.TemplateDirectory);
            output.WriteLine("LoadTemplates: Entity");
            var entityTemplates = LoadTemplates(project.EntityTemplates, context.TemplateDirectory);
            output.WriteLine("LoadTemplates: Project");
            var dataModelTemplates = LoadTemplates(project.DataModelTemplates, context.TemplateDirectory);

            foreach (var template in enumTemplates)
            {
                if (razor.IsTemplateCached(template.Key, typeof(EnumTemplateModel)))
                {
                    output.WriteLine("TemplateCached(Enum): {0}", template.Entry.Name);
                    continue;
                }
                razor.Compile(template.Source, template.Key, typeof(EnumTemplateModel));
                output.WriteLine("CompileTemplate(Enum): {0}", template.Entry.Name);
            }

            foreach (var template in entityTemplates)
            {
                if (razor.IsTemplateCached(template.Key, typeof(EntityTemplateModel)))
                {
                    output.WriteLine("TemplateCached(Entity): {0}", template.Entry.Name);
                    continue;
                }
                razor.Compile(template.Source, template.Key, typeof(EntityTemplateModel));
                output.WriteLine("CompileTemplate(Entity): {0}", template.Entry.Name);
            }

            foreach (var template in dataModelTemplates)
            {
                if (razor.IsTemplateCached(template.Key, typeof(DataModelTemplateModel)))
                {
                    output.WriteLine("TemplateCached(DataModel): {0}", template.Entry.Name);
                    continue;
                }
                razor.Compile(template.Source, template.Key, typeof(DataModelTemplateModel));
                output.WriteLine("CompileTemplate(DataModel): {0}", template.Entry.Name);
            }

            foreach (var template in enumTemplates)
            {
                foreach (var @enum in dataModel.Enums)
                {
                    output.WriteLine("RenderTemplate(Enum) - Template:{0}, Enum:{1}", template.Entry.Name, @enum.Name);
                    RenderTemplate(@enum, template, context);
                }
            }

            foreach (var template in entityTemplates)
            {
                foreach (var entity in dataModel.Entities)
                {
                    output.WriteLine("RenderTemplate(Entity) - Template:{0}, Entity:{1}", template.Entry.Name, entity.Name);
                    RenderTemplate(entity, template, context);
                }
            }

            foreach (var template in dataModelTemplates)
            {
                output.WriteLine("RenderTemplate(DataModel) - Template:{0}", template.Entry.Name);
                RenderTemplate(dataModel, template, context);
            }

            output.WriteLine("Execution: End");
        }

        private static void RenderTemplate(EnumSchema @enum, TemplateDetail template, GeneratingContext context)
        {
            var model = new EnumTemplateModel(context.DataModel, context.Project, @enum);
            var result = context.Razor.Run(template.Key, typeof(EnumTemplateModel), model);
            var filename = context.ResolveOutputPath(template.Entry.OutputPattern, @enum.Name, context.Project.Name);
            SaveFile(filename, result);
        }

        private static void RenderTemplate(EntitySchema entity, TemplateDetail template, GeneratingContext context)
        {
            if (!template.Entry.AllowKeyOnly && entity.Properties.Count == entity.Key.Properties.Count)
            {
                return;
            }
            var model = new EntityTemplateModel(context.DataModel, context.Project, entity);
            var result = context.Razor.Run(template.Key, typeof(EntityTemplateModel), model);
            var filename = context.ResolveOutputPath(template.Entry.OutputPattern, entity.Name, context.Project.Name);
            SaveFile(filename, result);
        }

        private static void RenderTemplate(DataModel dataModel, TemplateDetail template, GeneratingContext context)
        {
            var model = new DataModelTemplateModel(dataModel, context.Project);
            var result = context.Razor.Run(template.Key, typeof(DataModelTemplateModel), model);
            var filename = context.ResolveOutputPath(template.Entry.OutputPattern, dataModel.Name);
            SaveFile(filename, result);
        }

        private static void SaveFile(string filename, string content)
        {
            var dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(filename, content, System.Text.Encoding.UTF8);
        }

        private static TemplateDetail[] LoadTemplates(IList<TemplateEntry> templates, string dir)
        {
            var list = new List<TemplateDetail>();
            foreach (var template in templates)
            {
                list.Add(LoadTemplate(template, dir));
            }
            return list.ToArray();
        }

        private static TemplateDetail LoadTemplate(TemplateEntry template, string dir)
        {
            var filepath = Path.Combine(dir, String.Concat(template.Name, ".cshtml"));
            return new TemplateDetail(template, new FileInfo(filepath));
        }
    }
}
