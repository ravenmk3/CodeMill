using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CodeMill.Core.Common;
using CodeMill.Core.Common.Xml;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Xml
{
    public static class DataModelReader
    {
        public static DataModel Read(string filename)
        {
            filename = PathUtility.ResolvePath(filename);
            var baseDir = Path.GetDirectoryName(filename);
            var xml = new XmlDocument();
            xml.Load(filename);

            var schema = new DataModel
            {
                Name = xml.DocumentElement.GetAttribute("Name")
            };

            var enumsNode = xml.SelectSingleNode("/DataModel/Enums");
            enumsNode.FetchAttributeValue("Directory", v => schema.EnumDirectory = v);

            var entitiesNode = xml.SelectSingleNode("/DataModel/Entities");
            entitiesNode.FetchAttributeValue("Directory", v => schema.EntityDirectory = v);

            var descNode = xml.SelectSingleNode("/DataModel/Description");
            if (descNode != null)
            {
                schema.Description = descNode.InnerText;
            }

            var enumNodes = xml.SelectNodes("/DataModel/Enums/Item");
            schema.EnumNames = enumNodes.ToArray(x => x.GetAttributeValue("Name"));
            ReadEnums(schema, baseDir);

            var entityNodes = xml.SelectNodes("/DataModel/Entities/Item");
            schema.EntityNames = entityNodes.ToArray(x => x.GetAttributeValue("Name"));
            ReadEntities(schema, baseDir);

            FillEntityRelations(schema.Entities);
            FillPropertyEnums(schema);

            return schema;
        }

        private static void FillEntityRelations(IList<EntitySchema> entities)
        {
            foreach (var entity in entities)
            {
                foreach (var relation in entity.Relations)
                {
                    relation.RelatedEntity = entities.FirstOrDefault(x => x.Name == relation.RelatedEntityName);
                    if (relation.RelatedEntity != null)
                    {
                        relation.RelatedProperty = relation.RelatedEntity.Properties.FirstOrDefault(x => x.Name == relation.RelatedPropertyName);
                    }
                }
            }
        }

        private static void FillPropertyEnums(DataModel schema)
        {
            foreach (var entity in schema.Entities)
            {
                foreach (var prop in entity.Properties)
                {
                    if (prop.Type == PropertyDataType.Enum)
                    {
                        prop.Enum = schema.Enums.FirstOrDefault(x => x.Name == prop.EnumName);
                        break;
                    }
                }
            }
        }

        private static void ReadEnums(DataModel schema, string baseDir)
        {
            var enumDir = PathUtility.ResolvePath(baseDir, schema.EnumDirectory);
            schema.Enums = schema.EnumNames
                .Select(x => EnumSchemaReader.Read(Path.Combine(enumDir, String.Concat(x, ".xml"))))
                .ToList();
        }

        private static void ReadEntities(DataModel schema, string baseDir)
        {
            var entityDir = PathUtility.ResolvePath(baseDir, schema.EntityDirectory);
            schema.Entities = schema.EntityNames
                .Select(x => EntitySchemaReader.Read(Path.Combine(entityDir, String.Concat(x, ".xml"))))
                .ToArray();
        }
    }
}
