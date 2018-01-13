using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using CodeMill.Engine.Razor;
using CommandLineParser.Exceptions;

namespace CodeMill.CommandLine
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                return Execute(args);
            }
            var parser = new CommandLineParser.CommandLineParser
            {
                AcceptEqualSignSyntaxForValueArguments = true
            };
            var opts = new ArgumentsObject();
            parser.ExtractArgumentAttributes(opts);
            try
            {
                parser.ParseCommandLine(args);
                Generator.Execute(
                    opts.DataModelFile.FullName,
                    opts.ProjectFile.FullName,
                    opts.OutputDirectory?.FullName);
                return 0;
            }
            catch (CommandLineException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                parser.ShowUsage();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }

        private static int Execute(string[] args)
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
            var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location, args);
            AppDomain.Unload(domain);
            return exitCode;
        }
    }
}
