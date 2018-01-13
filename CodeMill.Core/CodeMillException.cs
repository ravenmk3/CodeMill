using System;
using System.Runtime.Serialization;

namespace CodeMill.Core
{
    [Serializable]
    public class CodeMillException : Exception
    {
        public CodeMillException()
        {
        }

        public CodeMillException(string message) : base(message)
        {
        }

        public CodeMillException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CodeMillException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
