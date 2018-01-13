using System;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class PropertySchema
    {
        public PropertySchema()
        {
            this.Nullable = false;
            this.IsUnicode = true;
            this.Size = 0;
            this.Seed = 1;
            this.Increment = 1;
        }

        public string Name { get; set; }

        public PropertyDataType Type { get; set; }

        public bool Nullable { get; set; }

        public AutoValueType AutoValueType { get; set; }

        public int Seed { get; set; }

        public int Increment { get; set; }

        public bool IsUnique { get; set; }

        public int Size { get; set; }

        public bool IsFixedSize { get; set; }

        public bool IsUnicode { get; set; }

        public string EnumName { get; set; }

        public EnumSchema Enum { get; set; }

        public int Precision { get; set; }

        public int Scale { get; set; }

        public bool IsUnsigned { get; set; }

        public bool HasDefault { get; set; }

        public string DefaultValue { get; set; }

        public string StorageType { get; set; }

        public string StorageName { get; set; }

        public string DisplayName { get; set; }

        public string Comment { get; set; }

        public bool IsKeyMember { get; internal set; }

        public bool IsIdentity
        {
            get { return this.AutoValueType == AutoValueType.Identity; }
        }

        public bool IsComputed
        {
            get { return this.AutoValueType == AutoValueType.Computed; }
        }

        public bool IsNumber
        {
            get
            {
                return this.Type == PropertyDataType.Byte
                    || this.Type == PropertyDataType.Currency
                    || this.Type == PropertyDataType.Decimal
                    || this.Type == PropertyDataType.Double
                    || this.Type == PropertyDataType.Int16
                    || this.Type == PropertyDataType.Int32
                    || this.Type == PropertyDataType.Int64
                    || this.Type == PropertyDataType.Single;
            }
        }

        public bool IsInteger
        {
            get
            {
                return this.Type == PropertyDataType.Byte
                    || this.Type == PropertyDataType.Int16
                    || this.Type == PropertyDataType.Int32
                    || this.Type == PropertyDataType.Int64;
            }
        }

        public bool IsEnum
        {
            get { return this.Type == PropertyDataType.Enum; }
        }
    }
}
