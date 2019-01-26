namespace dbBus.Extensions.AspNetCore
{
    using System;
    using dbBus.Core;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class DbBusAspNetCoreExtension
    {
        private static IBusConfiguration busCfg;

        public static void AddDbBus(this IServiceCollection service, Func<IBusConfiguration> configuration)
        {
            busCfg = configuration.Invoke();
        }

        public static void UseDbBus(this IApplicationBuilder app)
        {
            var da = (AspNetCoreDependencyAdapter)busCfg.DependencyAdapter;
            da.SetServiceScope(app.ApplicationServices);
            app.ApplicationServices.GetService<IBus>().Start();
        }

        public static IBusConfiguration UseAspNetCore(this IBusConfiguration bc, IServiceCollection service)
        {
            bc.DependencyAdapter = new AspNetCoreDependencyAdapter(service);
            bc.SetBindings();

            return bc;
        }
    }
}
