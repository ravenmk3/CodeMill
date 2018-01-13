using System;
using System.IO;

namespace CodeMill.Core.Common
{
    public static class PathUtility
    {
        public static string ResolvePath(string baseDirectory, string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            return Path.Combine(baseDirectory, path);
        }

        public static string ResolvePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            return Path.Combine(Environment.CurrentDirectory, path);
        }

        public static void EnsureDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
