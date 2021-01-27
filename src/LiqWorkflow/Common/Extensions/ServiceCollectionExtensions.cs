using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Events.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLiqWorkflow(this IServiceCollection services)
        {
            services.AddTransient<IWorkflowMessageEventBroker, WorkflowMessageEventBroker>();

            return services;
        }
    }
}
