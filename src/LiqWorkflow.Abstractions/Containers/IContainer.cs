using System;

namespace LiqWorkflow.Abstractions.Containers
{
    public interface IContainer : IServices
    {
        object GetService(Type type);

        object GetService(Type type, params object[] parameters);

        TService GetService<TService>();

        TService GetService<TService>(params object[] parameters);

        TService GetKeyedService<TService>(object key, params object[] parameters);
    }
}
