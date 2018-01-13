using System;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Xml
{
    public static class EnumSchemaWriter
    {
        public static void Write(EnumSchema schema, string filename)
        {
            var xml = ToXmlDocument(schema);
            filename = PathUtility.ResolvePath(filename);
            xml.SaveOptimized(filename);
        }

        public static XmlDocument ToXmlDocument(EnumSchema schema)
        {
            var xml = new XmlDocument();

            var root = xml.CreateElement("Enum");
            xml.AppendChild(root);
            root.SetAttribute("Name", schema.Name);
            if (schema.ValueType != EnumValueType.None)
            {
                root.SetAttribute("Type", schema.ValueType.ToString());
            }
            if (!String.IsNullOrWhiteSpace(schema.DisplayName))
            {
                root.SetAttribute("Display", schema.DisplayName);
            }
            if (!String.IsNullOrWhiteSpace(schema.StorageType))
            {
                root.SetAttribute("StorageType", schema.StorageType);
            }
            if (!String.IsNullOrWhiteSpace(schema.StorageName))
            {
                root.SetAttribute("StorageName", schema.StorageName);
            }

            var fieldsNode = xml.CreateElement("Fields");
            root.AppendChild(fieldsNode);
            AppendFields(fieldsNode, schema);

            if (!String.IsNullOrWhiteSpace(schema.Comment))
            {
                var descriptionNode = xml.CreateElement("Comment");
                descriptionNode.InnerText = schema.Comment;
                root.AppendChild(descriptionNode);
            }

            var commentsNode = xml.CreateElement("Comments");
            root.AppendChild(commentsNode);
            AppendComments(commentsNode, schema);

            return xml;
        }

        private static void AppendFields(XmlElement parent, EnumSchema schema)
        {
            var xml = parent.OwnerDocument;
            foreach (var field in schema.Fields)
            {
                var node = xml.CreateElement("Field");
                parent.AppendChild(node);
                node.SetAttribute("Name", field.Name);
                node.SetAttribute("Value", field.Value.ToString());
                if (!String.IsNullOrWhiteSpace(field.DisplayName))
                {
                    node.SetAttribute("Display", field.DisplayName);
                }
                if (!String.IsNullOrWhiteSpace(field.StorageName))
                {
                    node.SetAttribute("StorageName", field.StorageName);
                }
            }
        }

        private static void AppendComments(XmlElement parent, EnumSchema schema)
        {
            var xml = parent.OwnerDocument;
            foreach (var field in schema.Fields)
            {
                if (String.IsNullOrWhiteSpace(field.Comment))
                {
                    continue;
                }
                var node = xml.CreateElement("Field");
                parent.AppendChild(node);
                node.SetAttribute("Name", field.Name);
                node.InnerText = field.Comment;
            }
        }
    }
}
