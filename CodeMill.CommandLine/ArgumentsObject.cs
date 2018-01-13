using System;
using System.IO;
using CommandLineParser.Arguments;

namespace CodeMill.CommandLine
{
    [Serializable]
    public class ArgumentsObject
    {
        [FileArgument('m', "model", Optional = false, FileMustExist = true, Description = "Data model xml file")]
        public FileInfo DataModelFile { get; set; }

        [FileArgument('p', "project", Optional = false, FileMustExist = true, Description = "Project xml file")]
        public FileInfo ProjectFile { get; set; }

        [DirectoryArgument('o', "output", Optional = true, DirectoryMustExist = false, Description = "Output directory")]
        public DirectoryInfo OutputDirectory { get; set; }
    }
}
