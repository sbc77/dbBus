using System;
using System.Collections.Generic;

namespace dbBus.Core
{
    public interface IDependencyAdapter
    {
        void SetService(Type abst, Type impl);
        void SetSingletonService(Type abst, Type impl);
        void SetConstraintService(Type abst, object impl);

        T GetService<T>();
        object GetService(Type type);
        IEnumerable<object> GetServices(Type type);
    }
}