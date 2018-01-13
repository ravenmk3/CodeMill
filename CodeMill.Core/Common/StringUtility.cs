using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMill.Core.Common
{
    public static class StringUtility
    {
        public static bool IsCapital(char c)
        {
            return ((c >= 'A') && (c <= 'Z'));
        }

        public static string ToCamelCase(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }
            var parts = SplitWords(word);
            bool first = true;
            var builder = new StringBuilder();
            foreach (var part in parts)
            {
                if (first)
                {
                    var tmp = part.ToCharArray();
                    tmp[0] = tmp[0].ToString().ToLowerInvariant()[0];
                    builder.Append(new string(tmp));
                    first = false;
                }
                else
                {
                    var tmp = part.ToCharArray();
                    tmp[0] = tmp[0].ToString().ToUpperInvariant()[0];
                    builder.Append(new string(tmp));
                }
            }
            return builder.ToString();
        }

        public static string ToPascalCase(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }
            var parts = SplitWords(word);
            var builder = new StringBuilder();
            foreach (var part in parts)
            {
                var tmp = part.ToCharArray();
                tmp[0] = tmp[0].ToString().ToUpperInvariant()[0];
                builder.Append(new string(tmp));
            }
            return builder.ToString();
        }

        public static string ToUnderscoreCase(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }
            var parts = SplitWords(word);
            return String.Join("_", parts);
        }

        public static string[] SplitWords(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return new string[0];
            }
            const int STATE_NONE = 0;
            const int STATE_DIGIT = 1;
            const int STATE_LLETTER = 2;
            const int STATE_ULETTER = 3;
            var list = new List<string>();
            var builder = new StringBuilder();
            var state = STATE_NONE;
            foreach (var c in value)
            {
                if (c >= '0' && c <= '9')
                {
                    switch (state)
                    {
                        case STATE_DIGIT:
                        case STATE_NONE:
                            builder.Append(c);
                            break;
                        case STATE_LLETTER:
                        case STATE_ULETTER:
                            list.Add(builder.ToString());
                            builder.Clear();
                            builder.Append(c);
                            break;
                    }
                    state = STATE_DIGIT;
                }
                else if (c >= 'a' && c <= 'z')
                {
                    switch (state)
                    {
                        case STATE_LLETTER:
                        case STATE_ULETTER:
                        case STATE_NONE:
                            builder.Append(c);
                            break;
                        case STATE_DIGIT:
                            list.Add(builder.ToString());
                            builder.Clear();
                            builder.Append(c);
                            break;
                    }
                    state = STATE_LLETTER;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (builder.Length > 0)
                    {
                        list.Add(builder.ToString());
                        builder.Clear();
                    }
                    builder.Append((char)(c + ('a' - 'A')));
                    state = STATE_ULETTER;
                }
                else
                {
                    if (builder.Length > 0)
                    {
                        list.Add(builder.ToString());
                        builder.Clear();
                    }
                    state = STATE_NONE;
                }
            }
            if (builder.Length > 0)
            {
                list.Add(builder.ToString());
            }
            return list.ToArray();
        }

        public static string ToPlural(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }
            var len = word.Length;
            if (len < 1)
            {
                return String.Empty;
            }
            const StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;
            if (word.EndsWith("y", comparison))
            {
                return String.Concat(word.Substring(0, len - 1), "ies");
            }
            if (word.EndsWith("f", comparison))
            {
                return String.Concat(word.Substring(0, len - 1), "ves");
            }
            if (word.EndsWith("fe", comparison))
            {
                return String.Concat(word.Substring(0, len - 2), "ves");
            }
            if (word.EndsWith("s", comparison) 
                || word.EndsWith("x", comparison)
                || word.EndsWith("sh", comparison) 
                || word.EndsWith("ch", comparison))
            {
                return String.Concat(word, "es");
            }
            return String.Concat(word, 's');
        }
    }
}
