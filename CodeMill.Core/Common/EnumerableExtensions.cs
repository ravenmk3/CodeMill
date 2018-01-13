using System;
using System.Collections.Generic;

namespace CodeMill.Core.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Func<T, bool> function)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            foreach (var item in source)
            {
                if (!function.Invoke(item))
                {
                    break;
                }
            }
        }
    }
}
