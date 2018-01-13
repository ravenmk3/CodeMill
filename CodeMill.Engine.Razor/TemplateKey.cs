using System;
using System.IO;
using System.Text;
using CodeMill.Core.Common;
using RazorEngine.Templating;

namespace CodeMill.Engine.Razor
{
    [Obsolete]
    public class TemplateKey : BaseTemplateKey
    {
        private readonly FileInfo _file;

        public TemplateKey(string name, FileInfo file) : base(name, ResolveType.Global, null)
        {
            this._file = file ?? throw new ArgumentNullException("file");
        }

        public override string GetUniqueKeyString()
        {
            return MD5Utility.HashString(String.Concat(this._file.FullName, ':', this._file.LastWriteTime.Ticks), Encoding.UTF8);
        }
    }
}
