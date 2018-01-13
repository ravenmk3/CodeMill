using System;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class KeyProperty
    {
        public string Name { get; set; }

        public SortDirection Order { get; set; }

        public PropertySchema Property { get; set; }
    }
}
