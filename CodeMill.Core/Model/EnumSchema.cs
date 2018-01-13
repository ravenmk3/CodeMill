using System;
using System.Collections.Generic;
using CodeMill.Core.Model.Enums;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class EnumSchema
    {
        public EnumSchema()
        {
            this.ValueType = EnumValueType.None;
            this.Fields = new List<EnumField>();
        }

        public string Name { get; set; }

        public EnumValueType ValueType { get; set; }

        public string DisplayName { get; set; }

        public string StorageType { get; set; }

        public string StorageName { get; set; }

        public string Comment { get; set; }

        public IList<EnumField> Fields { get; private set; }
    }
}
