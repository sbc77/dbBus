namespace dbBus.Extensions.AspNetCore
{
    using System;
    using dbBus.Core;
    using Microsoft.Extensions.DependencyInjection;

    public class AspNetCoreDependencyAdapter : IDependencyAdapter
    {
        private readonly IServiceCollection sc;
        private IServiceProvider sp;

        public AspNetCoreDependencyAdapter(IServiceCollection sc)
        {
            this.sc = sc;
        }

        public void SetServiceScope(IServiceProvider serviceProvider)
        {
            this.sp = serviceProvider;
        }

        public T GetService<T>()
        {
            return this.sp.GetService<T>();
        }

        public object GetService(Type type)
        {
            return this.sp.GetService(type);
        }

        public void SetConstraintService(Type abst, object impl)
        {
            this.sc.AddSingleton(abst, impl);
        }

        public void SetSingletonService(Type abst, Type impl)
        {
            this.sc.AddSingleton(abst, impl);
        }

        public void SetService(Type impl)
        {
            this.sc.AddTransient(impl);
        }

        public void SetService(Type abst, Type impl)
        {
            this.sc.AddTransient(abst, impl);
        }
    }
}
