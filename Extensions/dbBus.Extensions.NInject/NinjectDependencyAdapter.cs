using Ninject;
using dbBus.Core;
using System;
using System.Collections.Generic;

namespace dbBus.Extensions.NInject
{
    public class NinjectDependencyAdapter : IDependencyAdapter
    {
        private readonly IKernel kernel;

        public NinjectDependencyAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T GetService<T>()
        {
            return this.GetService<T>();
        }

        public object GetService(Type type)
        {
            return this.GetService(type);
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return this.GetServices(type);
        }

        public void SetConstraintService(Type abst, object impl)
        {
            kernel.Bind(abst).ToConstant(impl);
        }

        public void SetService(Type abst, Type impl)
        {
            kernel.Bind(abst).To(impl);
        }

        public void SetSingletonService(Type abst, Type impl)
        {
            kernel.Bind(abst).To(impl).InSingletonScope();
        }
    }
}