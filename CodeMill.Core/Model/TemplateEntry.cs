using System;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class TemplateEntry
    {
        public TemplateEntry(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public bool AllowKeyOnly { get; set; }

        public string OutputPattern { get; set; }
    }
}
