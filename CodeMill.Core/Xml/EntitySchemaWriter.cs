using System;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Xml
{
    public static class EntitySchemaWriter
    {
        public static void Write(EntitySchema schema, string filename)
        {
            var xml = new XmlDocument();

            var root = xml.CreateElement("Entity");
            root.SetAttribute("Name", schema.Name);

            if (!String.IsNullOrEmpty(schema.PluralName))
            {
                root.SetAttribute("Plural", schema.PluralName);
            }

            if (!String.IsNullOrEmpty(schema.DisplayName))
            {
                root.SetAttribute("Display", schema.DisplayName);
            }

            if (!String.IsNullOrEmpty(schema.StorageName))
            {
                root.SetAttribute("StorageName", schema.StorageName);
            }

            xml.AppendChild(root);
            var propsNode = xml.CreateElement("Properties");
            var keyNode = xml.CreateElement("Key");
            var relationsNode = xml.CreateElement("Relations");
            var commentsNode = xml.CreateElement("Comments");
            var commentNode = xml.CreateElement("Comment");
            commentNode.InnerText = schema.Comment;

            AppendProperties(propsNode, schema);
            AppendKeyProperties(keyNode, schema);
            AppendRelations(relationsNode, schema);
            AppendComments(commentsNode, schema);

            root.AppendChild(propsNode);
            root.AppendChild(keyNode);
            root.AppendChild(relationsNode);
            root.AppendChild(commentsNode);
            root.AppendChild(commentNode);

            filename = PathUtility.ResolvePath(filename);
            xml.SaveOptimized(filename);
        }

        private static void AppendProperties(XmlElement parent, EntitySchema schema)
        {
            var xml = parent.OwnerDocument;
            foreach (var prop in schema.Properties)
            {
                var node = xml.CreateElement("Property");
                parent.AppendChild(node);
                node.SetAttribute("Name", prop.Name);
                node.SetAttribute("Type", prop.Type.ToString());
                if (prop.IsUnsigned)
                {
                    node.SetAttribute("Unsigned", "True");
                }
                if (prop.Nullable)
                {
                    node.SetAttribute("Nullable", "True");
                }
                if (prop.AutoValueType != AutoValueType.None)
                {
                    node.SetAttribute("AutoValue", prop.AutoValueType.ToString());
                }
                if (prop.AutoValueType == AutoValueType.Identity)
                {
                    if (prop.Seed != 1)
                    {
                        node.SetAttribute("Seed", prop.Seed.ToString());
                    }
                    if (prop.Increment != 1)
                    {
                        node.SetAttribute("Increment", prop.Increment.ToString());
                    }
                }
                if (prop.IsUnique)
                {
                    node.SetAttribute("Unique", "True");
                }
                if (prop.Size > 0)
                {
                    node.SetAttribute("Size", prop.Size.ToString());
                }
                if (prop.IsFixedSize)
                {
                    node.SetAttribute("FixedSize", "True");
                }
                if (!String.IsNullOrEmpty(prop.EnumName))
                {
                    node.SetAttribute("EnumName", prop.EnumName);
                }
                if (prop.Precision != 0)
                {
                    node.SetAttribute("Precision", prop.Precision.ToString());
                }
                if (prop.Scale != 0)
                {
                    node.SetAttribute("Scale", prop.Scale.ToString());
                }
                if (!prop.IsUnicode)
                {
                    node.SetAttribute("Unicode", "False");
                }
                if (prop.HasDefault)
                {
                    node.SetAttribute("HasDefault", "True");
                }
                if (prop.DefaultValue != null)
                {
                    node.SetAttribute("Default", prop.DefaultValue);
                }
                if (!String.IsNullOrEmpty(prop.DisplayName))
                {
                    node.SetAttribute("Display", prop.DisplayName);
                }
                if (!String.IsNullOrEmpty(prop.StorageType))
                {
                    node.SetAttribute("StorageType", prop.StorageType);
                }
                if (!String.IsNullOrEmpty(prop.StorageName))
                {
                    node.SetAttribute("StorageName", prop.StorageName);
                }
            }
        }

        private static void AppendKeyProperties(XmlElement parent, EntitySchema schema)
        {
            if (schema.Key == null)
            {
                return;
            }
            if (!String.IsNullOrWhiteSpace(schema.Key.Name))
            {
                parent.SetAttribute("Name", schema.Key.Name);
            }
            var xml = parent.OwnerDocument;
            foreach (var keyProp in schema.Key.Properties)
            {
                var node = xml.CreateElement("Property");
                parent.AppendChild(node);
                node.SetAttribute("Name", keyProp.Name);
                if (keyProp.Order == SortDirection.Descending)
                {
                    node.SetAttribute("Order", keyProp.Order.ToString());
                }
            }
        }

        private static void AppendRelations(XmlElement parent, EntitySchema schema)
        {
            var xml = parent.OwnerDocument;
            foreach (var relation in schema.Relations)
            {
                var node = xml.CreateElement("Relation");
                node.SetAttribute("Property", relation.PropertyName);
                node.SetAttribute("RelatedEntity", relation.RelatedEntityName);
                node.SetAttribute("RelatedProperty", relation.RelatedPropertyName);
                parent.AppendChild(node);
            }
        }

        private static void AppendComments(XmlElement parent, EntitySchema schema)
        {
            var xml = parent.OwnerDocument;
            foreach (var prop in schema.Properties)
            {
                if (String.IsNullOrWhiteSpace(prop.Comment))
                {
                    continue;
                }
                var node = xml.CreateElement("Property");
                parent.AppendChild(node);
                node.SetAttribute("Name", prop.Name);
                node.InnerText = prop.Comment;
            }
        }
    }
}
