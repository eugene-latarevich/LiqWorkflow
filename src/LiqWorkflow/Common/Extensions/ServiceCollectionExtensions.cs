using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Branches;
using LiqWorkflow.Events.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLiqWorkflow(this IServiceCollection services)
        {
            services.AddTransient<IWorkflowBuilder, WorkflowBuilder>();
            services.AddTransient<IWorkflowBranchFactory, WorkflowBranchFactory>();
            services.AddTransient<IWorkflowMessageEventBroker, WorkflowMessageEventBroker>();

            return services;
        }
    }
}
