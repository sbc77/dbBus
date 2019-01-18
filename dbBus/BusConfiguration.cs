using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dbBus.Core;
using dbBus.Core.Model;
using TheOne.OrmLite.Core;

namespace dbBus
{
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
            this.DependencyAdapter.SetConstraintService(typeof(IBusConfiguration), this);
            this.DependencyAdapter.SetSingletonService(typeof(IBus), typeof(Bus));
            return this.DependencyAdapter.GetService<IBus>();
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

            var gt = typeof(IHandle<>).MakeGenericType(ga);

            this.DependencyAdapter.SetService(gt, typeof(T));

            return this;
        }
    }
}