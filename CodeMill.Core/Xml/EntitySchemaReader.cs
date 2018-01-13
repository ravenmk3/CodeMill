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
    public static class EntitySchemaReader
    {
        public static EntitySchema Read(string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var schema = new EntitySchema();
            var xml = new XmlDocument();
            xml.Load(filename);

            schema.Name = xml.DocumentElement.GetAttribute("Name");

            xml.DocumentElement.FetchAttributeValue("Plural", v => schema.PluralName = v);
            if (String.IsNullOrEmpty(schema.PluralName))
            {
                schema.PluralName = schema.Name;
            }

            xml.DocumentElement.FetchAttributeValue("Display", v => schema.DisplayName = v);
            if (String.IsNullOrEmpty(schema.DisplayName))
            {
                schema.DisplayName = schema.Name;
            }

            xml.DocumentElement.FetchAttributeValue("StorageName", v => schema.StorageName = v);
            if (String.IsNullOrEmpty(schema.StorageName))
            {
                schema.StorageName = schema.Name;
            }

            var commentNode = xml.SelectSingleNode("/Entity/Comment");
            if (commentNode != null)
            {
                schema.Comment = commentNode.InnerText;
            }

            var propNodes = xml.SelectNodes("/Entity/Properties/Property");
            propNodes.Cast<XmlNode>()
                .Select(x => ConvertToProperty(x))
                .ToList()
                .ForEach(x => schema.Properties.Add(x));

            var commentNodes = xml.SelectNodes("/Entity/Comments/Property");
            foreach (XmlNode item in commentNodes)
            {
                FillComment(item, schema.Properties);
            }

            var keyNode = xml.SelectSingleNode("/Entity/Key");
            if (keyNode != null)
            {
                schema.Key = new EntityKey()
                {
                    Name = keyNode.GetAttributeValue("Name")
                };
            }

            var keyPropNodes = xml.SelectNodes("/Entity/Key/Property");
            FillEntityKey(schema, keyPropNodes);

            var relationNodes = xml.SelectNodes("/Entity/Relations/Relation");
            var relations = relationNodes.ToArray(n => ConvertToRelation(schema, n)).ToList();
            relations.ForEach(x => schema.Relations.Add(x));
            return schema;
        }

        private static EntityRelation ConvertToRelation(EntitySchema entity, XmlNode node)
        {
            var schema = new EntityRelation
            {
                PropertyName = node.GetAttributeValue("Property"),
                RelatedEntityName = node.GetAttributeValue("RelatedEntity"),
                RelatedPropertyName = node.GetAttributeValue("RelatedProperty")
            };
            schema.Property = entity.Properties.FirstOrDefault(p => p.Name == schema.PropertyName);
            return schema;
        }

        private static PropertySchema ConvertToProperty(XmlNode node)
        {
            var prop = new PropertySchema
            {
                Name = node.GetAttributeValue("Name")
            };
            node.FetchAttributeValue("Type", v => prop.Type = ParsePropertyType(v))
                .FetchAttributeValue("Nullable", v => prop.Nullable = Convert.ToBoolean(v))
                .FetchAttributeValue("AutoValue", v => prop.AutoValueType = ParseAutoValueType(v))
                .FetchAttributeValue("Seed", v => prop.Seed = Convert.ToInt32(v))
                .FetchAttributeValue("Increment", v => prop.Increment = Convert.ToInt32(v))
                .FetchAttributeValue("Unique", v => prop.IsUnique = Convert.ToBoolean(v))
                .FetchAttributeValue("Size", v => prop.Size = Convert.ToInt32(v))
                .FetchAttributeValue("FixedSize", v => prop.IsFixedSize = Convert.ToBoolean(v))
                .FetchAttributeValue("EnumName", v => prop.EnumName = v)
                .FetchAttributeValue("Precision", v => prop.Precision = Convert.ToInt32(v))
                .FetchAttributeValue("Scale", v => prop.Scale = Convert.ToInt32(v))
                .FetchAttributeValue("Unsigned", v => prop.IsUnsigned = Convert.ToBoolean(v))
                .FetchAttributeValue("Unicode", v => prop.IsUnicode = Convert.ToBoolean(v))
                .FetchAttributeValue("HasDefault", v => prop.HasDefault = Convert.ToBoolean(v))
                .FetchAttributeValue("Default", v => prop.DefaultValue = v)
                .FetchAttributeValue("Display", v => prop.DisplayName = v)
                .FetchAttributeValue("StorageType", v => prop.StorageType = v)
                .FetchAttributeValue("StorageName", v => prop.StorageName = v);
            return prop;
        }

        private static void FillEntityKey(EntitySchema schema, XmlNodeList keyNodes)
        {
            var keyProps = keyNodes.ToArray(node =>
            {
                var keyProp = new KeyProperty
                {
                    Name = node.GetAttributeValue("Name")
                };
                node.FetchAttributeValue("Order", v => keyProp.Order = ParseSortDirection(v));
                keyProp.Property = schema.Properties.FirstOrDefault(x => x.Name == keyProp.Name);
                if (keyProp.Property == null)
                {
                    throw new CodeMillException(String.Format("KeyProperty not found: {0}", keyProp.Name));
                }
                keyProp.Property.IsKeyMember = true;
                return keyProp;
            });
            keyProps.ToList().ForEach(x => schema.Key.Properties.Add(x));
        }

        private static void FillComment(XmlNode node, IEnumerable<PropertySchema> props)
        {
            node.FetchAttributeValue("Name", name =>
            {
                foreach (var prop in props)
                {
                    if (prop.Name == name)
                    {
                        prop.Comment = node.InnerText;
                    }
                }
            });
        }

        private static PropertyDataType ParsePropertyType(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return PropertyDataType.None;
            }
            switch (s.ToLowerInvariant())
            {
                case "boolean":
                case "bool":
                case "bit":
                    return PropertyDataType.Boolean;
                case "byte":
                case "tinyint":
                    return PropertyDataType.Byte;
                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                    return PropertyDataType.DateTime;
                case "decimal":
                case "numeric":
                    return PropertyDataType.Decimal;
                case "enum":
                    return PropertyDataType.Enum;
                case "money":
                case "smallmoney":
                case "currency":
                    return PropertyDataType.Currency;
                case "double":
                    return PropertyDataType.Double;
                case "single":
                case "float":
                    return PropertyDataType.Currency;
                case "guid":
                case "uuid":
                case "uniqueid":
                case "uniqueidentifier":
                    return PropertyDataType.Guid;
                case "int16":
                case "smallint":
                case "shortint":
                case "short":
                    return PropertyDataType.Int16;
                case "int24":
                case "mediumint":
                    return PropertyDataType.Int24;
                case "int32":
                case "integer":
                case "int":
                    return PropertyDataType.Int32;
                case "int64":
                case "bigint":
                case "longint":
                case "long":
                    return PropertyDataType.Int64;
                case "string":
                case "text":
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                    return PropertyDataType.String;
                case "timestamp":
                    return PropertyDataType.TimeStamp;
                case "binary":
                case "bytearray":
                case "byte[]":
                    return PropertyDataType.Binary;
                case "json":
                    return PropertyDataType.Json;
                case "xml":
                    return PropertyDataType.Xml;
                default:
                    return PropertyDataType.None;
            }
        }

        private static AutoValueType ParseAutoValueType(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return AutoValueType.None;
            }
            switch (s.ToLowerInvariant())
            {
                case "none":
                    return AutoValueType.None;
                case "identity":
                    return AutoValueType.Identity;
                case "guid":
                    return AutoValueType.Guid;
                case "computed":
                    return AutoValueType.Computed;
                default:
                    return AutoValueType.None;
            }
        }

        private static SortDirection ParseSortDirection(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return SortDirection.Ascending;
            }
            switch (s.ToLowerInvariant())
            {
                case "asc":
                    return SortDirection.Ascending;
                case "desc":
                    return SortDirection.Descending;
                case "ascending":
                    return SortDirection.Ascending;
                case "descending":
                    return SortDirection.Descending;
                default:
                    return SortDirection.Ascending;
            }
        }
    }
}
