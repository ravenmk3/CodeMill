using System;
using System.Collections.Generic;

namespace CodeMill.ReverseEngineering.MySql.Schema
{
    [Serializable]
    public class DatabaseSchema
    {
        private List<TableSchema> _tables;

        public string Name { get; set; }

        public List<TableSchema> Tables
        {
            get { return this._tables; }
            set { this._tables = value ?? new List<TableSchema>(); }
        }

        public DatabaseSchema()
        {
            this.Tables = new List<TableSchema>();
        }

        public DatabaseSchema(string name) : this(name, null)
        {
        }

        public DatabaseSchema(string name, List<TableSchema> tables)
        {
            this.Name = name;
            this.Tables = tables;
        }
    }
}
