using System;
using System.Linq;
using CodeMill.Core.Common;
using CodeMill.Core.Model;
using CodeMill.Core.Model.Enums;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace CodeMill.Engine.Razor.Helpers
{
    public class MySqlCodeHelper : TemplateHelperBase
    {
        public MySqlCodeHelper(TemplateBase template) : base(template)
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
            var name = StringUtility.ToUnderscoreCase(property.Name);
            if (property.Order == SortDirection.Ascending)
            {
                return String.Format("`{0}`", name);
            }
            return String.Format("`{0}` {1}", name, this.Order(property.Order));
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

        #region AutoIncrement

        public IEncodedString AutoIncrement(PropertySchema property, bool upperCase = true)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return upperCase
                ? this.Raw("AUTO_INCREMENT")
                : this.Raw("auto_increment");
        }

        #endregion AutoIncrement

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
                    return strDefault + strSpace + "(CURTIME())";
                case PropertyDataType.Guid:
                    return strDefault + strSpace + "('')";
                case PropertyDataType.TimeStamp:
                    return strDefault + strSpace + "CURRENT_TIMESTAMP";
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
                return "DECIMAL";
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
            return String.Format("DECIMAL({0}, {1})", p, s);
        }

        private static string GetEnumType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            switch (property.Enum.ValueType)
            {
                case EnumValueType.None:
                    return "INT";
                case EnumValueType.Int16:
                    return "SMALLINT";
                case EnumValueType.Int32:
                    return "INT";
                case EnumValueType.Int64:
                    return "BIGINT";
                case EnumValueType.Byte:
                    return "TINYINT";
                case EnumValueType.UInt16:
                    return "SMALLINT";
                case EnumValueType.UInt32:
                    return "INT";
                case EnumValueType.UInt64:
                    return "BIGINT";
                case EnumValueType.SByte:
                    return "TINYINT";
                default:
                    return "MEDIUMINT";
            }
        }

        private static string GetStringType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var length = property.Size;
            if (length <= 6000)
            {
                if (property.IsFixedSize)
                {
                    return String.Format("CHAR({0})", property.Size);
                }
                return String.Format("VARCHAR({0})", property.Size);
            }
            else if (length <= 65535)
            {
                return "TEXT";
            }
            else if (length <= 16777215)
            {
                return "MEDIUMTEXT";
            }
            else
            {
                return "LONGTEXT";
            }
        }

        private static string GetBinaryType(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var length = property.Size;
            if (length <= 6000)
            {
                if (property.IsFixedSize)
                {
                    return String.Format("BINARY({0})", property.Size);
                }
                return String.Format("VARBINARY({0})", property.Size);
            }
            else if (length <= 65535)
            {
                return "BLOB";
            }
            else if (length <= 16777215)
            {
                return "MEDIUMBLOB";
            }
            else
            {
                return "LONGBLOB";
            }
        }

        protected string DataTypeString(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            switch (property.Type)
            {
                case PropertyDataType.None:
                    return "BLOB";
                case PropertyDataType.Boolean:
                    return "BOOLEAN";
                case PropertyDataType.Byte:
                    return "TINYINT";
                case PropertyDataType.Int16:
                    return "SMALLINT";
                case PropertyDataType.Int32:
                    return "INT";
                case PropertyDataType.Int64:
                    return "BIGINT";
                case PropertyDataType.Decimal:
                    return GetSqlDecimalType(property);
                case PropertyDataType.Currency:
                    return "DECIMAL(19,4)";
                case PropertyDataType.Single:
                    return "FLOAT";
                case PropertyDataType.Double:
                    return "DOUBLE";
                case PropertyDataType.Enum:
                    return GetEnumType(property);
                case PropertyDataType.DateTime:
                    return "DATETIME";
                case PropertyDataType.Guid:
                    return "CHAR(32)";
                case PropertyDataType.TimeStamp:
                    return "TIMESTAMP";
                case PropertyDataType.String:
                    return GetStringType(property);
                case PropertyDataType.Binary:
                    return GetBinaryType(property);
                default:
                    return "BLOB";
            }
        }

        public IEncodedString DataType(PropertySchema property)
        {
            return this.Raw(this.DataTypeString(property));
        }

        #endregion DataType
    }
}
