using System;

namespace CodeMill.Core.Model.Enums
{
    [Serializable]
    public enum PropertyDataType
    {
        None = 0,

        Boolean = 10,

        Byte = 20,

        Int16 = 21,

        Int24 = 22,

        Int32 = 23,

        Int64 = 24,

        Decimal = 30,

        Currency = 31,

        Single = 32,

        Double = 33,

        Enum = 40,

        DateTime = 50,

        Guid = 60,

        TimeStamp = 70,

        String = 80,

        Binary = 90,

        Json = 81,

        Xml = 82,
    }
}
