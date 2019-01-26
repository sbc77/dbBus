namespace dbBus.Extensions.NInject
{
    using System;
    using System.Collections.Generic;

    using dbBus.Core;
    using Ninject;

    public class NinjectDependencyAdapter : IDependencyAdapter
    {
        private readonly IKernel kernel;

        public NinjectDependencyAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T GetService<T>() where T : class
        {
            return this.kernel.Get<T>();
        }

        public T TryGetService<T>() where T : class
        {
            return this.kernel.TryGet<T>();
        }

        public object GetService(Type type)
        {
            return this.kernel.Get(type);
        }

        public void SetConstraintService(Type abst, object impl)
        {
            this.kernel.Bind(abst).ToConstant(impl);
        }

        public void SetService(Type impl)
        {
            this.kernel.Bind(impl).ToSelf();
        }

        public void SetSingletonService(Type impl)
        {
            this.kernel.Bind(impl).ToSelf().InSingletonScope();
        }

        public void SetService(Type abst, Type impl)
        {
            this.kernel.Bind(abst).To(impl);
        }

        public void SetSingletonService(Type abst, Type impl)
        {
            this.kernel.Bind(abst).To(impl).InSingletonScope();
        }

        public bool IsRegistered<T>() where T : class
        {
            return this.kernel.TryGet<T>() != null;
        }
    }
}