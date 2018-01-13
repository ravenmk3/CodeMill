using System;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class EnumField
    {
        public string Name { get; set; }

        public long Value { get; set; }

        public string DisplayName { get; set; }

        public string StorageName { get; set; }

        public string Comment { get; set; }
    }
}
