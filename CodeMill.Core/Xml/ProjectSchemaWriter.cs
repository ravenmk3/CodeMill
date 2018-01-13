using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;

namespace CodeMill.Core.Xml
{
    public static class ProjectSchemaWriter
    {
        public static void Write(ProjectSchema schema, string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var baseDir = Path.GetDirectoryName(filename);

            var xml = new XmlDocument();
            var root = xml.CreateElement("Project");
            root.SetAttribute("Name", schema.Name);
            xml.AppendChild(root);

            SetOptions(root, schema);
            AppendDescription(root, schema);
            AppendParameters(root, schema);
            AppendMaps(root, schema, baseDir);
            AppendNamespaces(root, schema);
            AppendTemplates(root, "EnumTemplates", schema.EnumTemplates);
            AppendTemplates(root, "EntityTemplates", schema.EntityTemplates);
            AppendTemplates(root, "DataModelTemplates", schema.DataModelTemplates);

            xml.SaveOptimized(filename);
        }

        private static void SetOptions(XmlElement root, ProjectSchema schema)
        {
            var elm = root.OwnerDocument.CreateElement("Options");
            root.AppendChild(elm);
            elm.SetAttribute("MapDirectory", schema.MapDirectory);
            elm.SetAttribute("TemplateDirectory", schema.TemplateDirectory);
            elm.SetAttribute("OutputDirectory", schema.OutputDirectory);
        }

        private static void AppendDescription(XmlElement root, ProjectSchema schema)
        {
            if (!String.IsNullOrWhiteSpace(schema.Description))
            {
                var descriptionNode = root.OwnerDocument.CreateElement("Description");
                descriptionNode.InnerText = schema.Description;
                root.AppendChild(descriptionNode);
            }
        }

        private static void AppendParameters(XmlElement root, ProjectSchema schema)
        {
            if (schema.Parameters == null)
            {
                return;
            }
            var container = root.OwnerDocument.CreateElement("Parameters");
            root.AppendChild(container);
            foreach (var key in schema.Parameters.Keys)
            {
                var node = root.OwnerDocument.CreateElement("Item");
                container.AppendChild(node);
                node.SetAttribute("Name", key);
                node.InnerText = schema.Parameters[key] ?? String.Empty;
            }
        }

        private static void AppendMaps(XmlElement root, ProjectSchema schema, string baseDir)
        {
            if (schema.Maps == null)
            {
                return;
            }
            var container = root.OwnerDocument.CreateElement("Maps");
            root.AppendChild(container);
            foreach (var entry in schema.Maps)
            {
                var node = root.OwnerDocument.CreateElement("Item");
                container.AppendChild(node);
                node.SetAttribute("Name", entry.Key);
                WriteMap(Path.Combine(baseDir, schema.MapDirectory), entry.Key, entry.Value);
            }
        }

        private static void WriteMap(string mapDir, string name, INamedMap map)
        {
            PathUtility.EnsureDirectory(mapDir);
            var filename = Path.Combine(mapDir, name + ".xml");
            MapWriter.Write(map, filename);
        }

        private static void AppendNamespaces(XmlElement root, ProjectSchema schema)
        {
            if (schema.Namespaces == null || schema.Namespaces.Count < 1)
            {
                return;
            }
            var container = root.OwnerDocument.CreateElement("Namespaces");
            root.AppendChild(container);
            foreach (var value in schema.Namespaces)
            {
                var node = root.OwnerDocument.CreateElement("Item");
                container.AppendChild(node);
                node.InnerText = value;
            }
        }

        private static void AppendTemplates(XmlElement root, string containerName, IList<TemplateEntry> items)
        {
            if (items == null)
            {
                return;
            }
            var container = root.OwnerDocument.CreateElement(containerName);
            root.AppendChild(container);
            foreach (var item in items)
            {
                var node = root.OwnerDocument.CreateElement("Item");
                container.AppendChild(node);
                node.SetAttribute("Name", item.Name);
                node.SetAttribute("Output", item.OutputPattern);
                if (item.AllowKeyOnly)
                {
                    node.SetAttribute("AllowKeyOnly", true.ToString());
                }
            }
        }
    }
}
