using System;

namespace CodeMill.Core.Common
{
    public class ConsoleTextOutput : ITextOutput
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
