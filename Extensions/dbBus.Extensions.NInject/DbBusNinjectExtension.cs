namespace dbBus.Extensions.NInject
{
    using dbBus.Core;
    using Ninject;

    public static class DbBusNinjectExtension
    {
        public static IBusConfiguration UseNinject(this IBusConfiguration config, IKernel kernel)
        {
            config.DependencyAdapter = new NinjectDependencyAdapter(kernel);
            return config;
        }
    }
}