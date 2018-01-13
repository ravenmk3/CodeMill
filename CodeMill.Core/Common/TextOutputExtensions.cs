using System;

namespace CodeMill.Core.Common
{
    public static class TextOutputExtensions
    {
        public static void WriteLine(this ITextOutput output, string value)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            output.Write(value);
            output.Write(Environment.NewLine);
        }

        public static void WriteLine(this ITextOutput output, string format, params object[] args)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            output.Write(String.Format(format, args));
            output.Write(Environment.NewLine);
        }
    }
}
