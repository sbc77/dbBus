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
            return (T)this.kernel.GetService(typeof(T));
        }

        public object GetService(Type type)
        {
            return this.kernel.GetService(type);
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return this.kernel.GetAll(type);
        }

        public void SetConstraintService(Type abst, object impl)
        {
            this.kernel.Bind(abst).ToConstant(impl);
        }

        public void SetService(Type abst, Type impl)
        {
            this.kernel.Bind(abst).To(impl);
        }

        public void SetSingletonService(Type abst, Type impl)
        {
            this.kernel.Bind(abst).To(impl).InSingletonScope();
        }
    }
}