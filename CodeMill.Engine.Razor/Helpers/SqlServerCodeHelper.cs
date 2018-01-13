using System;
using System.Linq;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace CodeMill.Engine.Razor.Helpers
{
    public class SqlServerCodeHelper : TemplateHelperBase
    {
        public SqlServerCodeHelper(TemplateBase template) : base(template)
        {
        }

        #region Order

        public IEncodedString Order(SortDirection order)
        {
            switch (order)
            {
                case SortDirection.Ascending:
                    return this.Raw("ASC");
                case SortDirection.Descending:
                    return this.Raw("DESC");
                default:
                    return this.Raw("ASC");
            }
        }

        public IEncodedString Order(KeyProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Order(property.Order);
        }

        #endregion Order

        #region PrimaryKey

        private string BuildPrimaryKeyMember(KeyProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.Order == SortDirection.Ascending)
            {
                return String.Format("[{0}]", property.Name);
            }
            return String.Format("[{0}] {1}", property.Name, this.Order(property.Order));
        }

        public IEncodedString PrimaryKeyMembers(EntitySchema entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var arr = entity.Key.Properties.Select(p => this.BuildPrimaryKeyMember(p));
            return this.Raw(String.Join(", ", arr));
        }

        #endregion PrimaryKey

        #region Identity

        public IEncodedString IdentityDefinition(PropertySchema property, bool upperCase = true)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var result = upperCase ? "IDENTITY" : "identity";
            if (property.Seed != 1 || property.Increment != 1)
            {
                result = String.Format("{0} ({1}, {2})", result, property.Seed, property.Increment);
            }
            return this.Raw(result);
        }

        #endregion Identity

        #region DefaultConstraint

        public IEncodedString DefaultConstraint(PropertySchema property, bool upperCase = true, bool space = true)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Raw(this.DefaultConstraintString(property, upperCase, space));
        }

        private string DefaultConstraintString(PropertySchema property, bool upperCase = true, bool space = true)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var strDefault = upperCase ? "DEFAULT" : "default";
            var strSpace = space ? " " : String.Empty;

            switch (property.Type)
            {
                case PropertyDataType.None:
                    return strDefault + strSpace + "('')";
                case PropertyDataType.Boolean:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Byte:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Int16:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Int32:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Int64:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Decimal:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Currency:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Single:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Double:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.Enum:
                    return strDefault + strSpace + "(0)";
                case PropertyDataType.DateTime:
                    return strDefault + strSpace + "(getdate())";
                case PropertyDataType.Guid:
                    return strDefault + strSpace + "(newid())";
                case PropertyDataType.TimeStamp:
                    return strDefault + strSpace + "('')";
                case PropertyDataType.String:
                    return strDefault + strSpace + "('')";
                case PropertyDataType.Binary:
                    return strDefault + strSpace + "('')";
                default:
                    return strDefault + strSpace + "('')";
            }
        }

        #endregion DefaultConstraint

        #region DataType

        private static string GetSqlDecimalType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.Precision < 1 && property.Scale < 1)
            {
                return "decimal";
            }
            var p = 38;
            var s = 0;
            if (property.Precision > 0)
            {
                p = property.Precision;
            }
            if (property.Scale > 0)
            {
                s = property.Scale;
            }
            return String.Format("decimal({0}, {1})", p, s);
        }

        private static string GetSqlEnumType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            switch (property.Enum.ValueType)
            {
                case EnumValueType.None:
                    return "int";
                case EnumValueType.Int16:
                    return "smallint";
                case EnumValueType.Int32:
                    return "int";
                case EnumValueType.Int64:
                    return "bigint";
                case EnumValueType.Byte:
                    return "tinyint";
                case EnumValueType.UInt16:
                    return "smallint";
                case EnumValueType.UInt32:
                    return "int";
                case EnumValueType.UInt64:
                    return "bigint";
                case EnumValueType.SByte:
                    return "tinyint";
                default:
                    return "int";
            }
        }

        private static string GetSqlStringType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.IsUnicode)
            {
                if (property.IsFixedSize)
                {
                    return String.Format("nchar({0})", property.Size);
                }
                else if (property.Size > 4000 || property.Size <= 1)
                {
                    return "nvarchar(max)";
                }
                else
                {
                    return String.Format("nvarchar({0})", property.Size);
                }
            }
            else
            {
                if (property.IsFixedSize)
                {
                    return String.Format("char({0})", property.Size);
                }
                else if (property.Size > 8000 || property.Size <= 1)
                {
                    return "varchar(max)";
                }
                else
                {
                    return String.Format("varchar({0})", property.Size);
                }
            }
        }

        private static string GetSqlBinaryType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.IsFixedSize)
            {
                return String.Format("bianry({0})", property.Size);
            }
            else if (property.Size > 8000 || property.Size <= 1)
            {
                return "varbinary(max)";
            }
            else
            {
                return String.Format("varbianry({0})", property.Size);
            }
        }

        private static string GetSqlType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            switch (property.Type)
            {
                case PropertyDataType.None:
                    return "sql_variant";
                case PropertyDataType.Boolean:
                    return "bit";
                case PropertyDataType.Byte:
                    return "tinyint";
                case PropertyDataType.Int16:
                    return "smallint";
                case PropertyDataType.Int32:
                    return "int";
                case PropertyDataType.Int64:
                    return "bigint";
                case PropertyDataType.Decimal:
                    return GetSqlDecimalType(property);
                case PropertyDataType.Currency:
                    return "money";
                case PropertyDataType.Single:
                    return "float";
                case PropertyDataType.Double:
                    return "real";
                case PropertyDataType.Enum:
                    return GetSqlEnumType(property);
                case PropertyDataType.DateTime:
                    return "datetime";
                case PropertyDataType.Guid:
                    return "uniqueidentifier";
                case PropertyDataType.TimeStamp:
                    return "timestamp";
                case PropertyDataType.String:
                    return GetSqlStringType(property);
                case PropertyDataType.Binary:
                    return GetSqlBinaryType(property);
                default:
                    return "sql_variant";
            }
        }

        private string DataTypeString(PropertySchema property, bool escape = true)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var str = GetSqlType(property);
            if (escape)
            {
                var sb = new System.Text.StringBuilder();
                var flag = true;
                sb.Append('[');
                foreach (var c in str)
                {
                    if (c == '(')
                    {
                        flag = false;
                        sb.Append(']');
                    }
                    sb.Append(c);
                }
                if (flag)
                {
                    sb.Append(']');
                }
                str = sb.ToString();
            }
            return str;
        }

        public IEncodedString DataType(PropertySchema property, bool escape = true)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return this.Raw(this.DataTypeString(property, escape));
        }

        #endregion DataType
    }
}
