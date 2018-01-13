using System;
using System.Collections.Generic;

namespace CodeMill.Core.Model
{
    public interface INamedMap : IDictionary<string, string>
    {
        string Name { get; }
    }
}
