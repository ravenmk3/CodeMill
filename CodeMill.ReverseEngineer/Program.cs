using System;
using CodeMill.Core.Model;
using CodeMill.Core.Xml;
using CodeMill.ReverseEngineering.MySql;
using CodeMill.ReverseEngineering.MySql.Schema;
using CommandLineParser.Exceptions;

namespace CodeMill.ReverseEngineer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ReverseEngineer");
            var parser = new CommandLineParser.CommandLineParser
            {
                AcceptEqualSignSyntaxForValueArguments = true
            };
            var opts = new ArgumentsObject();
            parser.ExtractArgumentAttributes(opts);
            try
            {
                parser.ParseCommandLine(args);
                Console.WriteLine("DatabaseType: {0}", opts.DatabaseType);
                Console.WriteLine("ConnectionString: {0}", opts.ConnectionString);
                Console.WriteLine("Output: {0}", opts.OutputFilePath);
                var model = (DataModel)null;
                switch (opts.DatabaseType)
                {
                    case "mysql":
                        model = GetMySqlDataModel(opts.ConnectionString);
                        break;
                }
                if (model != null)
                {
                    DataModelWriter.Write(model, opts.OutputFilePath.FullName);
                    Console.WriteLine("File saved.");
                }
            }
            catch (CommandLineException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                parser.ShowUsage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static DataModel GetMySqlDataModel(string connectionString)
        {
            var db = new SchemaExporter().Export(connectionString);
            return MySqlReverseEngineer.Process(db);
        }
    }
}
