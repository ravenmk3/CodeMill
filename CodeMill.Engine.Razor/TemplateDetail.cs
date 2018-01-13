using System;
using System.IO;
using System.Text;
using CodeMill.Core.Common;
using CodeMill.Core.Model;
using RazorEngine.Templating;

namespace CodeMill.Engine.Razor
{
    public class TemplateDetail
    {
        public TemplateDetail(TemplateEntry entry, FileInfo file)
        {
            this.Entry = entry ?? throw new ArgumentNullException("entry");
            this.File = file ?? throw new ArgumentNullException("file");
            this.Source = new LoadedTemplateSource(System.IO.File.ReadAllText(file.FullName, Encoding.UTF8), file.FullName);
            this.Key = MD5Utility.HashString(String.Concat(file.FullName, ':', file.LastWriteTime.Ticks), Encoding.UTF8);
        }

        public TemplateEntry Entry { get; protected set; }

        public FileInfo File { get; protected set; }

        public ITemplateSource Source { get; protected set; }

        public string Key { get; protected set; }
    }
}
