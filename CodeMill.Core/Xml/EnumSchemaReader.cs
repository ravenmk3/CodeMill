using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Xml
{
    public static class EnumSchemaReader
    {
        public static EnumSchema Read(string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var xml = new XmlDocument();
            xml.Load(filename);
            return Read(xml);
        }

        public static EnumSchema Read(XmlDocument xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            var schema = new EnumSchema();
            var root = xml.DocumentElement;
            schema.Name = root.GetAttribute("Name");

            root.FetchAttributeValue("Type", v => schema.ValueType = ParseValueType(v));
            root.FetchAttributeValue("Display", v => schema.DisplayName = v);
            root.FetchAttributeValue("StorageType", v => schema.StorageType = v);
            root.FetchAttributeValue("StorageName", v => schema.StorageName = v);

            var commentNode = xml.SelectSingleNode("/Enum/Comment");
            if (commentNode != null)
            {
                schema.Comment = commentNode.InnerText;
            }

            var fieldNodes = xml.SelectNodes("/Enum/Fields/Field");
            schema.Fields.Clear();
            fieldNodes
                .Cast<XmlNode>()
                .Select(x => ConvertToField(x))
                .ToList()
                .ForEach(x => schema.Fields.Add(x));

            var commentNodes = xml.SelectNodes("/Enum/Comments/Field");
            foreach (XmlNode item in commentNodes)
            {
                FillComment(item, schema.Fields);
            }

            return schema;
        }

        private static EnumField ConvertToField(XmlNode node)
        {
            var field = new EnumField();
            node.FetchAttributeValue("Name", v => field.Name = v);
            node.FetchAttributeValue("Value", v => field.Value = Convert.ToInt64(v));
            node.FetchAttributeValue("Display", v => field.DisplayName = v);
            node.FetchAttributeValue("StorageName", v => field.StorageName = v);
            return field;
        }

        private static void FillComment(XmlNode node, IEnumerable<EnumField> fields)
        {
            var name = node.GetAttributeValue("Name");
            foreach (var field in fields)
            {
                if (field.Name == name)
                {
                    field.Comment = node.InnerText;
                    break;
                }
            }
        }

        private static EnumValueType ParseValueType(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return EnumValueType.None;
            }
            switch (s.ToLowerInvariant())
            {
                case "byte":
                    return EnumValueType.Byte;
                case "int16":
                    return EnumValueType.Int16;
                case "int":
                    return EnumValueType.Int32;
                case "int32":
                    return EnumValueType.Int32;
                case "int64":
                    return EnumValueType.Int64;
                default:
                    return EnumValueType.None;
            }
        }
    }
}
