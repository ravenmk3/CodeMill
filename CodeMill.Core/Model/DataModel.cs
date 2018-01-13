using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMill.Core.Model
{
    [Serializable]
    public class DataModel
    {
        public DataModel()
        {
            this.EnumDirectory = "Enums";
            this.EntityDirectory = "Entities";
            this.EnumNames = new List<string>();
            this.EntityNames = new List<string>();
            this.Enums = new List<EnumSchema>();
            this.Entities = new List<EntitySchema>();
        }

        public string Name { get; set; }

        public string EnumDirectory { get; set; }

        public string EntityDirectory { get; set; }

        public string Description { get; set; }

        public IList<string> EnumNames { get; set; }

        public IList<string> EntityNames { get; set; }

        public IList<EnumSchema> Enums { get; set; }

        public IList<EntitySchema> Entities { get; set; }

        public EntitySchema[] GetOneToOneEntities(EntitySchema entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (this.Entities == null)
            {
                return new EntitySchema[0];
            }
            return this.Entities
                .Where(x => !x.Key.IsComposite &&
                    x.Relations.Any(a => a.RelatedEntity == entity && a.Property.IsKeyMember))
                .ToArray();
        }

        public EntitySchema[] GetOneToManyEntities(EntitySchema entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (this.Entities == null)
            {
                return new EntitySchema[0];
            }
            return this.Entities
                .Where(x => !x.Key.IsComposite
                    && x.Relations.Any(a => a.RelatedEntity == entity && !a.Property.IsKeyMember))
                .ToArray();
        }

        public EntitySchema[] GetManyToManyEntities(EntitySchema entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (this.Entities == null)
            {
                return new EntitySchema[0];
            }
            return this.Entities
                .Where(x => x.Key.IsComposite && x.Key.Properties.Count == 2 && x.Relations.Count == 2
                    && x.Relations.Any(a => a.RelatedEntity == entity && a.Property.IsKeyMember))
                .SelectMany(x => x.Relations)
                .Where(x => x.RelatedEntity != entity)
                .Select(x => x.RelatedEntity)
                .ToArray();
        }

        public EntitySchema[] GetManyToManyLeftEntities(EntitySchema entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (this.Entities == null)
            {
                return new EntitySchema[0];
            }
            return this.Entities
                .Where(x => x.Key.IsComposite && x.Key.Properties.Count == 2 && x.Relations.Count == 2
                    && x.Relations[1].RelatedEntity == entity && x.Relations[1].Property.IsKeyMember)
                .SelectMany(x => x.Relations)
                .Where(x => x.RelatedEntity != entity)
                .Select(x => x.RelatedEntity)
                .ToArray();
        }

        public EntitySchema[] GetManyToManyRightEntities(EntitySchema entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (this.Entities == null)
            {
                return new EntitySchema[0];
            }
            return this.Entities
                .Where(x => x.Key.IsComposite && x.Key.Properties.Count == 2 && x.Relations.Count == 2
                    && x.Relations[0].RelatedEntity == entity && x.Relations[0].Property.IsKeyMember)
                .SelectMany(x => x.Relations)
                .Where(x => x.RelatedEntity != entity)
                .Select(x => x.RelatedEntity)
                .ToArray();
        }
    }
}
