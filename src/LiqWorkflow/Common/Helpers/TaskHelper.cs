using System;
using System.Threading;
using System.Threading.Tasks;

namespace LiqWorkflow.Common.Helpers
{
    static class TaskHelper
    {
        public static Task<T> RetryOnConditionOrException<T>(
            Func<T, bool> condition,
            Func<Task<T>> retryFunc,
            int retryCount,
            TimeSpan delay,
            CancellationToken cancellationToken = default)
                => retryFunc()
                    .ContinueWith(async innerTask =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var result = innerTask.Result;
                        if (innerTask.Status != TaskStatus.Faulted && condition(result))
                        {
                            return innerTask.Result;
                        }

                        if (retryCount == 0)
                        {
                            throw innerTask.Exception ?? throw new Exception();
                        }

                        await Task.Delay(delay, cancellationToken);

                        return await RetryOnConditionOrException(condition, retryFunc, retryCount - 1, delay, cancellationToken);
                    })
                    .Unwrap();
    }
}
