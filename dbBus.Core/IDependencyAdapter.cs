namespace dbBus.Core
{
    using System;

    public interface IDependencyAdapter
    {
        void SetService(Type impl);

        void SetService(Type abst, Type impl);

        void SetSingletonService(Type impl);

        void SetSingletonService(Type abst, Type impl);

        void SetConstraintService(Type abst, object impl);

        T GetService<T>() where T : class;

        T TryGetService<T>() where T : class;

        object GetService(Type type);

        bool IsRegistered<T>() where T : class;
    }
}