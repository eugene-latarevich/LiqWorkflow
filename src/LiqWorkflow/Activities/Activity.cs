using System;
using System.Threading;
using System.Threading.Tasks;
using LiqWorkflow.Abstractions.Models;

namespace LiqWorkflow.Activities
{
    public class Activity
    {
        protected Activity()
        {

        }

        protected async Task ExecuteAsync(Func<CancellationToken, Task<ActivityData>> processData, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await processData(cancellationToken);
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception exception)
            {

            }
        }
    }
}
