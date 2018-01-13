using System;
using System.Collections.Generic;
using System.Linq;
using CodeMill.Core.Common;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace CodeMill.Engine.Razor.Helpers
{
    public class CSharpCodeHelper : TemplateHelperBase
    {
        public CSharpCodeHelper(TemplateBase template) : base(template)
        {
        }

        #region Enum

        public bool UnderlyingTypeRequired(EnumSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            return schema.ValueType != EnumValueType.Int32
                && schema.ValueType != EnumValueType.None;
        }

        protected string UnderlyingTypeString(EnumSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            switch (schema.ValueType)
            {
                case EnumValueType.None:
                    return "int";
                case EnumValueType.Int16:
                    return "short";
                case EnumValueType.Int32:
                    return "int";
                case EnumValueType.Int64:
                    return "long";
                case EnumValueType.Byte:
                    return "byte";
                case EnumValueType.UInt16:
                    return "uint";
                case EnumValueType.UInt32:
                    return "ushort";
                case EnumValueType.UInt64:
                    return "ulong";
                case EnumValueType.SByte:
                    return "sbyte";
                default:
                    return "int";
            }
        }

        public IEncodedString UnderlyingType(EnumSchema schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }
            return this.Raw(this.UnderlyingTypeString(schema));
        }

        #endregion Enum

        #region IsNullableType

        public bool IsNullableType(PropertyDataType type)
        {
            return type == PropertyDataType.String
                || type == PropertyDataType.Json
                || type == PropertyDataType.Xml
                || type == PropertyDataType.Binary
                || type == PropertyDataType.None;
        }

        public bool IsNullableType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.IsNullableType(property.Type);
        }

        /// <summary>
        /// 指示是否需要Nullable`1封装
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool IsNullableWrapRequired(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (this.IsNullableType(property))
            {
                return false;
            }
            return property.Nullable;
        }

        #endregion IsNullableType

        #region DefaultValue

        public IEncodedString DefaultValue(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Raw(this.DefaultValueString(property));
        }

        protected string DefaultValueString(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            switch (property.Type)
            {
                case PropertyDataType.None:
                    return "null";
                case PropertyDataType.Boolean:
                    return "false";
                case PropertyDataType.Byte:
                    return "0";
                case PropertyDataType.Int16:
                    return "0";
                case PropertyDataType.Int32:
                    return "0";
                case PropertyDataType.Int64:
                    return "0";
                case PropertyDataType.Decimal:
                    return "0m";
                case PropertyDataType.Currency:
                    return "0m";
                case PropertyDataType.Single:
                    return "0";
                case PropertyDataType.Double:
                    return "0";
                case PropertyDataType.Enum:
                    return "0";
                case PropertyDataType.DateTime:
                    return "DateTime.Now";
                case PropertyDataType.Guid:
                    return "Guid.NewGuid()";
                case PropertyDataType.TimeStamp:
                    return "new byte[0]";
                case PropertyDataType.String:
                    return "String.Empty";
                case PropertyDataType.Binary:
                    return "new byte[0]";
                default:
                    return "null";
            }
        }

        #endregion DefaultValue

        #region Field

        public IEncodedString FieldName(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Raw(this.FieldNameString(property));
        }

        protected string FieldNameString(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var name = (string)null;
            if (property.Name.Length > 2)
            {
                name = StringUtility.ToCamelCase(property.Name);
            }
            else
            {
                name = property.Name.ToLower();
            }
            return String.Concat("_", name);
        }

        #endregion Field

        #region Parameter

        public IEncodedString ParameterName(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Raw(this.ParameterNameString(property));
        }

        protected string ParameterNameString(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.Name.Length > 2)
            {
                return StringUtility.ToCamelCase(property.Name);
            }
            else
            {
                return property.Name.ToLower();
            }
        }

        #endregion Parameter

        #region DateType

        protected string DataType(PropertyDataType type)
        {
            switch (type)
            {
                case PropertyDataType.None:
                    return "object";
                case PropertyDataType.Boolean:
                    return "bool";
                case PropertyDataType.Byte:
                    return "byte";
                case PropertyDataType.Int16:
                    return "short";
                case PropertyDataType.Int32:
                    return "int";
                case PropertyDataType.Int64:
                    return "long";
                case PropertyDataType.Decimal:
                    return "decimal";
                case PropertyDataType.Currency:
                    return "decimal";
                case PropertyDataType.Single:
                    return "single";
                case PropertyDataType.Double:
                    return "double";
                case PropertyDataType.Enum:
                    return "object";
                case PropertyDataType.DateTime:
                    return "DateTime";
                case PropertyDataType.Guid:
                    return "Guid";
                case PropertyDataType.TimeStamp:
                    return "byte[]";
                case PropertyDataType.String:
                    return "string";
                case PropertyDataType.Binary:
                    return "byte[]";
                default:
                    return "object";
            }
        }

        public string DataTypeString(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var type = property.IsEnum ? property.EnumName : this.DataType(property.Type);
            if (this.IsNullableWrapRequired(property))
            {
                type = String.Concat(type, "?");
            }
            return type;
        }

        public IEncodedString DataType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Raw(this.DataTypeString(property));
        }

        #endregion DateType

        #region Key

        public string KeyArgumentsString(EntityKey key)
        {
            if (key == null || key.Properties == null || key.Properties.Count < 1)
            {
                return "object key";
            }
            if (key.IsComposite)
            {
                var list = new List<string>();
                foreach (var keyProp in key.Properties)
                {
                    list.Add(this.KeyPropertyArgumentDefinition(keyProp));
                }
                return String.Join(", ", list);
            }
            return KeyPropertyArgumentDefinition(key.Properties[0]);
        }

        private string KeyPropertyArgumentDefinition(KeyProperty keyProperty)
        {
            var argName = StringUtility.ToCamelCase(keyProperty.Name);
            var keyType = keyProperty.Property == null ? "object" : this.DataTypeString(keyProperty.Property);
            return String.Concat(keyType, " ", argName);
        }

        #endregion

        #region EntityFramework

        public bool EfMappingDetailRequired(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.IsKeyMember && !(property.AutoValueType == AutoValueType.Identity))
            {
                return true;
            }
            if (property.AutoValueType == AutoValueType.Computed)
            {
                return true;
            }
            if (property.IsFixedSize)
            {
                return true;
            }
            if (property.Size > 0)
            {
                return true;
            }
            if (!property.Nullable && this.IsNullableType(property))
            {
                return true;
            }
            if (property.Size < 1 && this.IsNullableType(property))
            {
                return true;
            }
            return false;
        }

        #endregion EntityFramework
    }
}
