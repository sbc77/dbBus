using Ninject;
using dbBus.Core;

namespace dbBus.Extensions.NInject
{
    public static class DbBusNinjectExtension
    {
        public static IBusConfiguration UseNinject(this IBusConfiguration config, IKernel kernel)
        {
            config.DependencyAdapter = new NinjectDependencyAdapter(kernel);
            return config;
        }
    }
}