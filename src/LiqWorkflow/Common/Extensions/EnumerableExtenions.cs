using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiqWorkflow.Common.Extensions
{
    static class EnumerableExtenions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func)
        {
            foreach (var item in source)
            {
                await func(item);
            }
        }
    }
}
