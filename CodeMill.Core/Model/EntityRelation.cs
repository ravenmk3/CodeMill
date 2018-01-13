using System;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class EntityRelation
    {
        public string Name { get; set; }

        public string PropertyName { get; set; }

        public string RelatedEntityName { get; set; }

        public string RelatedPropertyName { get; set; }

        public PropertySchema Property { get; set; }

        public EntitySchema RelatedEntity { get; set; }

        public PropertySchema RelatedProperty { get; set; }
    }
}
