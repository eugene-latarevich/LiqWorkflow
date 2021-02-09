using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiqWorkflow.Abstractions.Containers;
using LiqWorkflow.Abstractions.Models.Containers;
using LiqWorkflow.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace LiqWorkflow
{
    class Container : IContainer
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentBag<TypeConfiguration> _types = new ConcurrentBag<TypeConfiguration>();

        public Container(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<TypeConfiguration> Types => _types;

        public void Register<TService, TImplementation>() 
            where TImplementation : class
                => _types.Add(new TypeConfiguration{Service = typeof(TService), Implementation = typeof(TImplementation)});

        public void RegisterKeyed<TService, TImplementation>(object key) 
            where TImplementation : class
                => _types.Add(new TypeConfiguration{Key = key, Service = typeof(TService), Implementation = typeof(TImplementation)});

        public void RegisterKeyed<TService>(object key, Type implementation) 
            => _types.Add(new TypeConfiguration{Key = key, Service = typeof(TService), Implementation = implementation});

        public object GetService(Type type) => _serviceProvider.GetService(type);

        public object GetService(Type type, object[] parameters)
        {
            parameters = GetConstructorParameters(type, parameters);
            return Activator.CreateInstance(type, parameters);
        }

        public TService GetService<TService>() => _serviceProvider.GetService<TService>();

        public TService GetService<TService>(object[] parameters) => (TService)GetService(typeof(TService), parameters);

        public TService GetKeyedService<TService>(object key, object[] parameters)
        {
            var serviceConfiguration = _types.FirstOrDefault(x => x.Key.Equals(key));
            return serviceConfiguration == null
                ? throw new Exception($"Service wih key={key} wasn't found.")
                : (TService) GetService(serviceConfiguration.Implementation, parameters);
        }

        private object[] GetConstructorParameters(Type type, object[] parameters)
        {
            bool canInvokeConstructor = false;

            var constructorsInfo = type.GetConstructors();
            foreach (var constructorInfo in constructorsInfo.OrderByDescending(x => x.GetParameters().Length))
            {
                var services = GetConstructorServices(parameters, constructorInfo.GetParameters());
                if (!services.Any())
                {
                    continue;
                }

                parameters = services.Concat(parameters).ToArray();

                canInvokeConstructor = true;
                break;
            }

            if (!canInvokeConstructor)
            {
                throw new SerivceImplementationException(type);
            }

            return parameters;
        }

        private IEnumerable<object> GetConstructorServices(object[] parameters, ParameterInfo[] constructorParameters)
        {
            var services = new List<object>();
            foreach (var parameter in constructorParameters)
            {
                if (parameters.All(x => x.GetType() != parameter.ParameterType))
                {
                    var service = GetService(parameter.ParameterType);
                    if (service == null)
                    {
                        return Enumerable.Empty<object>();
                    }

                    services.Add(service);
                }
            }

            return services;
        }
    }
}
