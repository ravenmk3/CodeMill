using System;
using System.Collections.Generic;
using System.Linq;
using CodeMill.Core.Common;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;
using CodeMill.ReverseEngineering.MySql.Schema;

namespace CodeMill.ReverseEngineering.MySql
{
    public static class MySqlReverseEngineer
    {
        private static string[] FIXED_LENGTH_TYPES = new[] { "char", "binary" };

        public static DataModel Process(DatabaseSchema database)
        {
            var name = StringUtility.ToPascalCase(database.Name);
            var dm = new DataModel
            {
                Name = name,
                Description = name,
                EnumDirectory = "Enums",
                EntityDirectory = "Entities",
            };
            foreach (var table in database.Tables)
            {
                var entity = ConvertTable(table);
                dm.EntityNames.Add(entity.Name);
                dm.Entities.Add(entity);
            }
            return dm;
        }

        public static EntitySchema ConvertTable(TableSchema table)
        {
            var name = StringUtility.ToPascalCase(table.Name);
            var entity = new EntitySchema()
            {
                Name = name,
                PluralName = name,
                StorageName = table.Name,
                DisplayName = name,
                Comment = table.Comment,
            };
            var keyProps = new List<PropertySchema>();
            foreach (var column in table.Columns)
            {
                var property = ConvertColumn(column);
                entity.Properties.Add(property);
                if (column.IsPrimaryKey)
                {
                    keyProps.Add(property);
                }
            }
            entity.Key = new EntityKey() { Name = "Default" };
            keyProps.ForEach(p => entity.Key.Properties.Add(new KeyProperty()
            {
                Name = p.Name,
                Order = SortDirection.Ascending,
                Property = p
            }));
            return entity;
        }

        public static PropertySchema ConvertColumn(ColumnSchema column)
        {
            var name = StringUtility.ToPascalCase(column.Name);
            var prop = new PropertySchema()
            {
                Name = name,
                DisplayName = name,
                StorageName = column.Name,
                Comment = column.Comment,
                AutoValueType = column.IsAutoIncrement ? AutoValueType.Identity : AutoValueType.None,
                HasDefault = column.DefaultValue != null,
                DefaultValue = column.DefaultValue,
                EnumName = null,
                Enum = null,
                Increment = 1,
                Seed = 1,
                IsUnicode = true,
                Nullable = column.IsNullable,
                Size = column.MaxLength ?? 0,
                Precision = column.NumericPrecision ?? 0,
                Scale = column.NumericScale ?? 0,
                IsUnsigned = column.IsUnsigned,
                IsFixedSize = IsFixedSizeType(column.DataType),
                IsUnique = column.IsUniqueKey,
                StorageType = column.DataType,
                Type = ConvertType(column.DataType),
            };
            return prop;
        }

        private static bool IsFixedSizeType(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            type = type.ToLowerInvariant();
            return FIXED_LENGTH_TYPES.Contains(type);
        }

        private static PropertyDataType ConvertType(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            switch (type.ToLowerInvariant())
            {
                case "bool":
                case "boolean":
                    return PropertyDataType.Boolean;
                case "tinyint":
                    return PropertyDataType.Byte;
                case "smallint":
                    return PropertyDataType.Int16;
                case "mediumnint":
                    return PropertyDataType.Int24;
                case "int":
                case "integer":
                    return PropertyDataType.Int32;
                case "bigint":
                    return PropertyDataType.Int64;
                case "decimal":
                case "dec":
                case "numeric":
                    return PropertyDataType.Decimal;
                case "float":
                case "single":
                    return PropertyDataType.Single;
                case "double":
                    return PropertyDataType.Double;
                case "datetime":
                case "date":
                case "year":
                case "time":
                    return PropertyDataType.DateTime;
                case "timestamp":
                    return PropertyDataType.TimeStamp;
                case "json":
                    return PropertyDataType.Json;
                case "xml":
                    return PropertyDataType.Xml;
                case "char":
                case "varchar":
                case "text":
                case "mediumtext":
                case "longtext":
                case "enum":
                    return PropertyDataType.String;
                case "binary":
                case "varbinary":
                case "blob":
                case "mediumblob":
                case "longblob":
                    return PropertyDataType.Binary;
                default:
                    return PropertyDataType.None;
            }
        }
    }
}
