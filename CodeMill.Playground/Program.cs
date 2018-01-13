using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using CodeMill.Core.Common;
using CodeMill.Core.Xml;
using CodeMill.Engine.Razor;
using CodeMill.ReverseEngineering.MySql;
using CodeMill.ReverseEngineering.MySql.Schema;
using Newtonsoft.Json;

namespace CodeMill.Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(StringUtility.ToPlural("dog"));
            Console.Read();
        }

        private static int Execute()
        {
            Console.WriteLine("Switching to RazorEngine AppDomain");
            var adSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
            };
            var current = AppDomain.CurrentDomain;
            var strongNames = new StrongName[0];
            var domain = AppDomain.CreateDomain(
                "RazorEngineDomain", null,
                current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                strongNames);
            var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
            AppDomain.Unload(domain);
            return exitCode;
        }

        private static void TestGen()
        {
            var dataModelPath = @"Data\DataModel.xml";
            var projectPath = @"Data\Project.xml";
            var outputPath = @"C:\Users\Raven\Desktop\DemoOutput";
            outputPath = null;
            Generator.Execute(dataModelPath, projectPath, outputPath);
        }

        private static void TestProject()
        {
            var path = @"Data\Project.xml";
            Console.WriteLine(File.Exists(path));
            var s = ProjectSchemaReader.Read(path);
            PrintJson(s);

            var path2 = @"Data\Data2\Project.xml";
            ProjectSchemaWriter.Write(s, path2);
            var s2 = ProjectSchemaReader.Read(path2);

            CompareJson(s, s2);
        }

        private static void TestDataModel()
        {
            var path = @"Data\DataModel.xml";
            Console.WriteLine(File.Exists(path));
            var s = DataModelReader.Read(path);
            PrintJson(s);

            var path2 = @"Data\Data2\DataModel.xml";
            DataModelWriter.Write(s, path2);
            var s2 = DataModelReader.Read(path2);

            CompareJson(s, s2);
        }

        private static void TestEntity()
        {
            var path = @"Data\Entities\User.xml";
            Console.WriteLine(File.Exists(path));
            var s = EntitySchemaReader.Read(path);
            PrintJson(s);

            var path2 = @"Data\Entities\User2.xml";
            EntitySchemaWriter.Write(s, path2);
            var s2 = EntitySchemaReader.Read(path2);
            PrintJson(s2);

            CompareJson(s, s2);
        }

        private static void TestMap()
        {
            var path = @"Data\Maps\DemoMap.xml";
            var m = MapReader.Read(path);
            Console.WriteLine(m.Name);
            foreach (var item in m)
            {
                Console.WriteLine("{0} = {1}", item.Key, item.Value);
            }
        }

        private static void TestEnum()
        {
            var path = @"Data\Enums\UserType.xml";
            Console.WriteLine(File.Exists(path));
            var s = EnumSchemaReader.Read(path);
            PrintJson(s);

            var path2 = @"Data\Enums\UserType2.xml";
            EnumSchemaWriter.Write(s, path2);
            var s2 = EnumSchemaReader.Read(path2);
            PrintJson(s2);

            CompareJson(s, s2);
        }

        private static void PrintJson<T>(T value)
        {
            var json = JsonConvert.SerializeObject(value, Formatting.Indented);
            Console.WriteLine(json);
        }

        private static void CompareJson<T>(T value1, T value2)
        {
            var json1 = JsonConvert.SerializeObject(value1, Formatting.Indented);
            var json2 = JsonConvert.SerializeObject(value2, Formatting.Indented);
            Console.WriteLine(json1 == json2);
        }

        private static void WriteTestFile(string path, string value)
        {
        }
    }
}
