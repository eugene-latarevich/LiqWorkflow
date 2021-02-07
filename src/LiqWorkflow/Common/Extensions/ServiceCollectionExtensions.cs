using System;
using System.Linq;
using System.Reflection;
using LiqWorkflow.Abstractions;
using LiqWorkflow.Abstractions.Activities;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Events;
using LiqWorkflow.Attributes;
using LiqWorkflow.Branches;
using LiqWorkflow.Events.Brokers;
using LiqWorkflow.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static (IServiceCollection, IContainer) AddLiqWorkflow(this IServiceCollection services)
        {
            services.AddTransient<IContainer, Container>();
            services.AddTransient<IWorkflowBuilder, WorkflowBuilder>();
            services.AddTransient<IWorkflowBranchFactory, WorkflowBranchFactory>();
            services.AddTransient<IWorkflowMessageEventBroker, WorkflowMessageEventBroker>();

            return (services, services.BuildServiceProvider().GetService<IContainer>());
        }

        public static (IServiceCollection, IContainer) WithContainer<TImplementation>(this (IServiceCollection, IContainer) servicesData)
            where TImplementation : class, IContainer
        {
            var (services, container) = servicesData;

            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TImplementation));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }

            services.AddTransient<IContainer, TImplementation>();

            return servicesData;
        }

        public static (IServiceCollection, IContainer) WithExecutableActivity<TImplementation>(this (IServiceCollection, IContainer) servicesData)
            where TImplementation : class, IWorkflowExecutableActivity
        {
            var (services, container) = servicesData;

            if (container.Types.Any(type => type.Key.Equals(ServiceKeys.ExecutableActivity)))
            {
                throw new NotFoundException("Executable activity already exists in the workflow container.");
            }

            container.RegisterKeyed<IWorkflowExecutableActivity, TImplementation>(ServiceKeys.ExecutableActivity);

            return servicesData;
        }

        public static (IServiceCollection, IContainer) WithKeyedExecutableActivity<TImplementation>(this (IServiceCollection, IContainer) servicesData, object key)
            where TImplementation : class, IWorkflowExecutableActivity
        {
            var (services, container) = servicesData;

            if (container.Types.Any(type => type.Key.Equals(key)))
            {
                throw new NotFoundException("Executable activity already exists in the workflow container.");
            }

            container.RegisterKeyed<IWorkflowExecutableActivity, TImplementation>(key);

            return servicesData;
        }

        public static (IServiceCollection, IContainer) WithRestorableActivity<TImplementation>(this (IServiceCollection, IContainer) servicesData)
            where TImplementation : class, IRestorableWorkflowActivitity
        {
            var (services, container) = servicesData;

            if (container.Types.Any(type => type.Key.Equals(ServiceKeys.RestorableActivity)))
            {
                throw new NotFoundException("Restorable activity already exists in the workflow container.");
            }

            container.RegisterKeyed<IRestorableWorkflowActivitity, TImplementation>(ServiceKeys.RestorableActivity);

            return servicesData;
        }

        public static (IServiceCollection, IContainer) WithKeyedRestorableActivity<TImplementation>(this (IServiceCollection, IContainer) servicesData, object key)
            where TImplementation : class, IRestorableWorkflowActivitity
        {
            var (services, container) = servicesData;

            if (container.Types.Any(type => type.Key.Equals(key)))
            {
                throw new NotFoundException("Restorable activity already exists in the workflow container.");
            }

            container.RegisterKeyed<IRestorableWorkflowActivitity, TImplementation>(key);

            return servicesData;
        }

        public static (IServiceCollection, IContainer) WithKeyedActions(this (IServiceCollection, IContainer) servicesData, params Type[] actionAssemblyMarkerTypes)
        {
            var (services, container) = servicesData;

            foreach (var markerType in actionAssemblyMarkerTypes)
            {
                var assembly = Assembly.GetAssembly(markerType);

                var actionTypes = assembly
                    .GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(IWorkflowExecutableAction)))
                    .ToArray();

                foreach (var actionType in actionTypes)
                {
                    var keyAttribute = (KeyedWorkflowActionAttribute)actionType
                        .GetCustomAttributes(typeof(KeyedWorkflowActionAttribute), false)
                        .FirstOrDefault();
                    if (keyAttribute == null)
                    {
                        throw new NotFoundException($"Key attribute for action with Type={actionType} wasn't found. Every action class must have key Attribute {typeof(KeyedWorkflowActionAttribute)}.");
                    }

                    container.RegisterKeyed<IRestorableWorkflowActivitity>(keyAttribute.Key, actionType);
                }
            }

            return servicesData;
        }
    }
}
