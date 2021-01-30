using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Abstractions.Factories;
using LiqWorkflow.Events.Brokers;
using LiqWorkflow.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLiqWorkflow(this IServiceCollection services)
        {
            services.AddTransient<IWorkflowBranchFactory, WorkflowBranchFactory>();
            services.AddTransient<IWorkflowMessageEventBroker, WorkflowMessageEventBroker>();

            return services;
        }
    }
}
