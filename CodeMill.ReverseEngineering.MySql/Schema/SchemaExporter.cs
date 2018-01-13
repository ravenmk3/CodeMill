using System;
using System.Linq;
using CodeMill.ReverseEngineering.MySql.Schema.RecordModel;
using Dapper;
using MySql.Data.MySqlClient;

namespace CodeMill.ReverseEngineering.MySql.Schema
{
    public class SchemaExporter
    {
        private const string SQL_SELECT_TABLES = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @schema;";
        private const string SQL_SELECT_COLUMNS = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table;";

        public DatabaseSchema Export(string host, int port, string user, string password, string schema)
        {
            var cnnStr = String.Format("Data Source={0};Port={1};User ID={2};Password={3};Database={4};CharSet=utf8;", host, port, user, password, schema);
            return this.Export(cnnStr, null);
        }

        public DatabaseSchema Export(string connectionString)
        {
            return this.Export(connectionString, null);
        }

        public DatabaseSchema Export(string connectionString, string schemaName)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                if (String.IsNullOrEmpty(schemaName))
                {
                    schemaName = connection.ExecuteScalar<string>("SELECT database();");
                }
                var tableRecords = connection.Query<TableRecord>(SQL_SELECT_TABLES, new { schema = schemaName });
                var tables = tableRecords.Select(tableRecord =>
                {
                    var columnRecords = connection.Query<ColumnRecord>(SQL_SELECT_COLUMNS, new { schema = schemaName, table = tableRecord.TABLE_NAME });
                    var table = this.ConvertTable(tableRecord);
                    table.Columns = columnRecords.OrderBy(x => x.ORDINAL_POSITION).Select(x => this.ConvertColumn(x)).ToList();
                    return table;
                }).ToList();
                return new DatabaseSchema(schemaName, tables);
            }
        }

        private TableSchema ConvertTable(TableRecord record)
        {
            return new TableSchema()
            {
                Name = record.TABLE_NAME,
                Collation = record.TABLE_COLLATION,
                Engine = record.ENGINE,
                RowFormat = record.ROW_FORMAT,
                Type = record.TABLE_TYPE,
                Comment = record.TABLE_COMMENT,
            };
        }

        private ColumnSchema ConvertColumn(ColumnRecord record)
        {
            return new ColumnSchema()
            {
                Name = record.COLUMN_NAME,
                Comment = record.COLUMN_COMMENT,
                IsNullable = "YES".Equals(record.IS_NULLABLE, StringComparison.InvariantCultureIgnoreCase),
                DataType = record.DATA_TYPE,
                DefaultValue = record.COLUMN_DEFAULT,
                Ordinal = record.ORDINAL_POSITION,
                MaxLength = record.CHARACTER_MAXIMUM_LENGTH,
                NumericPrecision = record.NUMERIC_PRECISION,
                NumericScale = record.NUMERIC_SCALE,
                IsKey = !String.IsNullOrWhiteSpace(record.COLUMN_KEY),
                IsPrimaryKey = record.COLUMN_KEY != null && record.COLUMN_KEY.ToUpperInvariant().Contains("PRI"),
                IsUniqueKey = record.COLUMN_KEY != null && record.COLUMN_KEY.ToUpperInvariant().Contains("UNI"),
                IsUnsigned = record.COLUMN_TYPE.ToUpperInvariant().Contains("UNSIGNED"),
                IsAutoIncrement = record.EXTRA != null && record.EXTRA.ToUpperInvariant().Contains("AUTO_INCREMENT"),
            };
        }
    }
}
