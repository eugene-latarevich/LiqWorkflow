using System.Threading.Tasks;

namespace LiqWorkflow.Common.Extensions
{
    static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T item) => Task.FromResult(item);
    }
}
