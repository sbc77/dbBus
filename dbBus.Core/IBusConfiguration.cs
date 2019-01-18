using System;
using System.Collections.Generic;
using dbBus.Core.Model;
using TheOne.OrmLite.Core;

namespace dbBus.Core
{
    public interface IBusConfiguration
    {
        int PullInterval { get; }
        int PullMaxMessages { get; }
        int MaxRetry { get; }
        TimeSpan MessageLifetime { get; }

        IDbConnectionFactory DbConnectionFactory { get; set; }
        IDependencyAdapter DependencyAdapter { get; set; }

        IList<RegistrationInfo> RegistrationInfo { get; }

        IBusConfiguration RegisterHandler<T>();
        IBus Build();
    }
}