using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;

namespace CodeMill.Core.Xml
{
    public static class ProjectSchemaReader
    {
        public static ProjectSchema Read(string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var baseDir = Path.GetDirectoryName(filename);
            var schema = new ProjectSchema();
            var xml = new XmlDocument();
            xml.Load(filename);

            schema.Name = xml.DocumentElement.GetAttribute("Name");
            schema.Description = GetDescription(xml);

            LoadOptions(schema, xml);
            LoadParameters(schema, xml);
            LoadMaps(schema, xml, baseDir);
            LoadNamespaces(schema, xml);

            ReadTemplates(xml, "/Project/EnumTemplates/Item", schema.EnumTemplates);
            ReadTemplates(xml, "/Project/EntityTemplates/Item", schema.EntityTemplates);
            ReadTemplates(xml, "/Project/DataModelTemplates/Item", schema.DataModelTemplates);

            return schema;
        }

        private static void ReadTemplates(XmlDocument xml, string xpath, IList<TemplateEntry> list)
        {
            var nodes = xml.SelectNodes(xpath);
            var tpls = ReadTemplates(nodes);
            list.Clear();
            tpls.ForEach(item => list.Add(item));
        }

        private static TemplateEntry[] ReadTemplates(XmlDocument xml, string xpath)
        {
            var nodes = xml.SelectNodes(xpath);
            return ReadTemplates(nodes);
        }

        private static TemplateEntry[] ReadTemplates(XmlNodeList nodes)
        {
            return nodes.ToArray(node =>
            {
                var name = node.GetAttributeValue("Name");
                var tpl = new TemplateEntry(name)
                {
                    OutputPattern = node.GetAttributeValue("Output")
                };
                node.FetchAttributeValue("AllowKeyOnly", v => tpl.AllowKeyOnly = Convert.ToBoolean(v));
                return tpl;
            });
        }

        private static string GetDescription(XmlDocument xml)
        {
            var node = xml.SelectSingleNode("/Project/Description");
            return node?.InnerText;
        }

        private static void LoadOptions(ProjectSchema model, XmlDocument xml)
        {
            var node = xml.SelectSingleNode("/Project/Options");
            node.FetchAttributeValue("MapDirectory", v => model.MapDirectory = v)
                .FetchAttributeValue("TemplateDirectory", v => model.TemplateDirectory = v)
                .FetchAttributeValue("OutputDirectory", v => model.OutputDirectory = v);
        }

        private static void LoadParameters(ProjectSchema model, XmlDocument xml)
        {
            var nodes = xml.SelectNodes("/Project/Parameters/Item");
            foreach (XmlNode node in nodes)
            {
                var name = node.GetAttributeValue("Name");
                var value = node.InnerText;
                model.Parameters.Add(name, value);
            }
        }

        private static void LoadMaps(ProjectSchema model, XmlDocument xml, string baseDir)
        {
            var mapDir = PathUtility.ResolvePath(baseDir, model.MapDirectory);
            xml.SelectNodes("/Project/Maps/Item")
                .ToArray(n => n.GetAttributeValue("Name"))
                .Select(n => MapReader.Read(Path.Combine(mapDir, String.Concat(n, ".xml"))))
                .ForEach(m => model.Maps.Add(m.Name, m));
        }

        private static void LoadNamespaces(ProjectSchema model, XmlDocument xml)
        {
            var nodes = xml.SelectNodes("/Project/Namespaces/Item");
            foreach (XmlNode node in nodes)
            {
                model.Namespaces.Add(node.InnerText);
            }
        }
    }
}
