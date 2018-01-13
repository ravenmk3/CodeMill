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
    public static class DataModelWriter
    {
        public static void Write(DataModel schema, string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var baseDir = Path.GetDirectoryName(filename);
            var xml = new XmlDocument();

            var root = xml.CreateElement("DataModel");
            root.SetAttribute("Name", schema.Name);
            xml.AppendChild(root);

            var enumsNode = xml.CreateElement("Enums");
            var entitiesNode = xml.CreateElement("Entities");
            root.AppendChild(enumsNode);
            root.AppendChild(entitiesNode);
            enumsNode.SetAttribute("Directory", schema.EnumDirectory);
            entitiesNode.SetAttribute("Directory", schema.EntityDirectory);

            if (schema.Enums != null && schema.Enums.Count > 0)
            {
                var names = schema.Enums.Select(x => x.Name).ToArray();
                AppendEnums(enumsNode, names);
                var enumDir = Path.Combine(baseDir, schema.EnumDirectory);
                WriteEnums(schema, enumDir);
            }
            else if ((schema.Enums == null || schema.Enums.Count < 1) && schema.EnumNames != null)
            {
                AppendEnums(enumsNode, schema.EnumNames);
            }

            if (schema.Entities != null && schema.Entities.Count > 0)
            {
                var names = schema.Entities.Select(x => x.Name).ToArray();
                AppendEntities(entitiesNode, names);
                var entityDir = Path.Combine(baseDir, schema.EntityDirectory);
                WriteEntities(schema, entityDir);
            }
            else if ((schema.Entities == null || schema.Entities.Count < 1) && schema.EntityNames != null)
            {
                AppendEntities(entitiesNode, schema.EntityNames);
            }

            if (!String.IsNullOrWhiteSpace(schema.Description))
            {
                var descriptionNode = xml.CreateElement("Description");
                descriptionNode.InnerText = schema.Description;
                root.AppendChild(descriptionNode);
            }

            xml.SaveOptimized(filename);
        }

        private static void AppendEnums(XmlElement elm, IEnumerable<string> enumNames)
        {
            var xml = elm.OwnerDocument;
            foreach (var name in enumNames)
            {
                var node = xml.CreateElement("Item");
                node.SetAttribute("Name", name);
                elm.AppendChild(node);
            }
        }

        private static void AppendEntities(XmlElement elm, IEnumerable<string> names)
        {
            var xml = elm.OwnerDocument;
            foreach (var name in names)
            {
                var node = xml.CreateElement("Item");
                node.SetAttribute("Name", name);
                elm.AppendChild(node);
            }
        }

        private static void WriteEnums(DataModel schema, string dirPath)
        {
            PathUtility.EnsureDirectory(dirPath);
            foreach (var @enum in schema.Enums)
            {
                var path = Path.Combine(dirPath, String.Concat(@enum.Name, ".xml"));
                EnumSchemaWriter.Write(@enum, path);
            }
        }

        private static void WriteEntities(DataModel schema, string dirPath)
        {
            PathUtility.EnsureDirectory(dirPath);
            foreach (var entity in schema.Entities)
            {
                var path = Path.Combine(dirPath, String.Concat(entity.Name, ".xml"));
                EntitySchemaWriter.Write(entity, path);
            }
        }
    }
}
