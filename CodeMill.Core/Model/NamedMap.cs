using System;
using System.Collections.Generic;

namespace CodeMill.Core.Model
{
    public class NamedMap : Dictionary<string, string>, INamedMap
    {
        private string _name;

        public NamedMap(string name)
        {
            this._name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name
        {
            get { return this._name; }
        }
    }
}
