using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class EntitySchema
    {
        public EntitySchema()
        {
            this.Key = new EntityKey();
            this.Properties = new List<PropertySchema>();
            this.Relations = new List<EntityRelation>();
            this.UniqueKeys = new List<EntityKey>();
        }

        public string Name { get; set; }

        public string PluralName { get; set; }

        public string DisplayName { get; set; }

        public string StorageName { get; set; }

        public string Comment { get; set; }

        public EntityKey Key { get; set; }

        public IList<PropertySchema> Properties { get; private set; }

        public IList<EntityKey> UniqueKeys { get; private set; }

        public IList<EntityRelation> Relations { get; private set; }

        public bool HasProperty(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return this.Properties != null
                && this.Properties.Any(p => p.Name == name);
        }

        public EntityRelation FindRelationByProperty(PropertySchema property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (this.Relations == null || this.Relations.Count < 1)
            {
                return null;
            }
            return this.Relations.FirstOrDefault(x => x.Property != null && x.Property == property);
        }

        public EntityRelation FindRelationByProperty(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (this.Relations == null || this.Relations.Count < 1)
            {
                return null;
            }
            return this.Relations.FirstOrDefault(x => x.Property != null && x.PropertyName == propertyName);
        }
    }
}
