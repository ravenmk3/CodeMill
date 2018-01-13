using System;
using System.Collections.Generic;

namespace CodeMill.ReverseEngineering.MySql.Schema
{
    [Serializable]
    public class TableSchema
    {
        private List<ColumnSchema> _columns;

        public string Name { get; set; }

        public string Type { get; set; }

        public string Engine { get; set; }

        public string RowFormat { get; set; }

        public string Collation { get; set; }

        public string Comment { get; set; }

        public List<ColumnSchema> Columns
        {
            get { return this._columns; }
            set { this._columns = value ?? new List<ColumnSchema>(); }
        }

        public TableSchema()
        {
            this.Columns = new List<ColumnSchema>();
        }

        public TableSchema(string name)
        {
            this.Name = name;
        }
    }
}
