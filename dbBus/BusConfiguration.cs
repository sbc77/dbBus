namespace dbBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using dbBus.Core;
    using dbBus.Core.Model;
    using Microsoft.Extensions.Logging;
    using TheOne.OrmLite.Core;

    public class BusConfiguration : IBusConfiguration
    {
        public BusConfiguration()
        {
            this.PullInterval = 1000;
            this.PullMaxMessages = 100;
            this.MaxRetry = 3;
            this.MessageLifetime = TimeSpan.FromDays(7);
            this.RegistrationInfo = new List<RegistrationInfo>();
        }

        public int PullInterval { get; set; }

        public int PullMaxMessages { get; set; }

        public TimeSpan MessageLifetime { get; set; }

        public int MaxRetry { get; set; }

        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public IDependencyAdapter DependencyAdapter { get; set; }

        public IList<RegistrationInfo> RegistrationInfo { get; }

        public IBus Build()
        {
            this.SetBindings();
            return this.DependencyAdapter.GetService<IBus>();
        }

        public void SetBindings()
        {
            if (!this.DependencyAdapter.IsRegistered<IErrorHandler>())
            {
                this.DependencyAdapter.SetService(typeof(IErrorHandler), typeof(DefaultErrorHandler));
            }

            this.DependencyAdapter.SetSingletonService(typeof(PullMessagesJob));
            this.DependencyAdapter.SetConstraintService(typeof(IBusConfiguration), this);
            this.DependencyAdapter.SetSingletonService(typeof(IBus), typeof(Bus));
        }

        public IBusConfiguration RegisterHandler<T>()
        {
            var t = typeof(T).GetTypeInfo().ImplementedInterfaces.Single();
            var ga = t.GenericTypeArguments.Single();

            var ri = new RegistrationInfo
            {
                HandlerType = typeof(T),
                HandlerTypeName = typeof(T).FullName,
                MessageType = ga,
                MessageTypeName = ga.FullName
            };

            this.RegistrationInfo.Add(ri);
            this.DependencyAdapter.SetService(typeof(T));

            return this;
        }

        public IBusConfiguration UseDefaultConsoleLogger()
        {
            this.DependencyAdapter.SetService(typeof(ILogger<>), typeof(DefaultBusLogger<>));
            return this;
        }

        public IBusConfiguration RegisterErrorHandler<T>() where T : IErrorHandler
        {
            this.DependencyAdapter.SetService(typeof(IErrorHandler), typeof(T));
            return this;
        }
    }
}