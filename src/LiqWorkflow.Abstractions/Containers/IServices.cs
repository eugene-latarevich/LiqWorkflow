using System;
using System.Collections.Generic;
using LiqWorkflow.Abstractions.Models.Containers;

namespace LiqWorkflow.Abstractions.Containers
{
    public interface IServices
    {
        IEnumerable<TypeConfiguration> Types { get; }

        void Register<TService, TImplementation>() where TImplementation : class;

        void RegisterKeyed<TService, TImplementation>(object key) where TImplementation : class;

        void RegisterKeyed<TService>(object key, Type implementation);
    }
}
