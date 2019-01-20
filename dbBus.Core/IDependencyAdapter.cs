namespace dbBus.Core
{
    using System;
    
    public interface IDependencyAdapter
    {
        void SetService(Type impl);

        void SetSingletonService(Type abst, Type impl);

        void SetConstraintService(Type abst, object impl);

        T GetService<T>();

        object GetService(Type type);
    }
}