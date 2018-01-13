using System;

namespace CodeMill.ReverseEngineering.MySql.Schema
{
    [Serializable]
    public class ColumnSchema
    {
        public string Name { get; set; }

        public int Ordinal { get; set; }

        public string DefaultValue { get; set; }

        public bool IsNullable { get; set; }

        public bool IsUnsigned { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsUniqueKey { get; set; }

        public bool IsAutoIncrement { get; set; }

        public bool IsKey { get; set; }

        public string DataType { get; set; }

        public int? MaxLength { get; set; }

        public int? NumericPrecision { get; set; }

        public int? NumericScale { get; set; }

        public string Comment { get; set; }

        public ColumnSchema()
        {
        }

        public ColumnSchema(string name)
        {
            this.Name = name;
        }
    }
}
