using System;
using System.IO;
using CommandLineParser.Arguments;

namespace CodeMill.ReverseEngineer
{
    [Serializable]
    public class ArgumentsObject
    {
        [EnumeratedValueArgument(typeof(string), 't', "type", AllowedValues = "mysql;mssql;postgre;sqlce;sqlite", Optional = false, Description = "Database type (mysql;mssql;postgre;sqlce;sqlite)")]
        public String DatabaseType { get; set; }

        [ValueArgument(typeof(string), 'c', "connection", Optional = false, Description = "ADO.NET connection string")]
        public String ConnectionString { get; set; }

        [FileArgument('o', "output", Optional = false, FileMustExist = false, Description = "Output xml file path")]
        public FileInfo OutputFilePath { get; set; }
    }
}
