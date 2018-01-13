using System;
using System.Collections.Generic;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class EntityKey
    {
        public EntityKey()
        {
            this.Properties = new List<KeyProperty>();
        }

        public string Name { get; set; }

        public IList<KeyProperty> Properties { get; private set; }

        public bool IsComposite
        {
            get { return this.Properties != null && this.Properties.Count > 1; }
        }
    }
}
