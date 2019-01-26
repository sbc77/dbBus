namespace dbBus.Extensions.AspNetCore
{
    using System;
    using System.Linq;
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

        public T GetService<T>() where T : class
        {
            return this.sp.GetRequiredService<T>();
        }

        public T TryGetService<T>() where T : class
        {
            return this.sp.GetServices<T>().FirstOrDefault();
        }

        public object GetService(Type type)
        {
            return this.sp.GetRequiredService(type);
        }

        public void SetConstraintService(Type abst, object impl)
        {
            this.sc.AddSingleton(abst, impl);
        }

        public void SetSingletonService(Type impl)
        {
            this.sc.AddSingleton(impl);
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

        public bool IsRegistered<T>() where T : class
        {
            return this.sc.Any(x => x.ServiceType == typeof(T));
        }
    }
}
