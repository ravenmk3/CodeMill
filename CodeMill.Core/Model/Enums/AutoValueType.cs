using System;

namespace CodeMill.Core.Model.Enums
{
    [Serializable]
    public enum AutoValueType
    {
        None = 0,

        Identity = 1,

        Guid = 2,

        Computed = 3,
    }
}
