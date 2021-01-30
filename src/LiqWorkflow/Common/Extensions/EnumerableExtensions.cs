using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiqWorkflow.Common.Extensions
{
    static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func)
        {
            foreach (var item in source)
            {
                await func(item);
            }
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> withKey)
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var value in source)
            {
                var key = withKey(value);
                result.Add(key, value);
            }

            return result;
        }
    }
}
