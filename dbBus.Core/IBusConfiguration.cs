namespace dbBus.Core
{
    using System;
    using System.Collections.Generic;
    using dbBus.Core.Model;
    using TheOne.OrmLite.Core;

    public interface IBusConfiguration
    {
        int PullInterval { get; set; }

        int PullMaxMessages { get; set; }

        TimeSpan MessageLifetime { get; set; }

        IDbConnectionFactory DbConnectionFactory { get; set; }

        IDependencyAdapter DependencyAdapter { get; set; }

        IList<RegistrationInfo> RegistrationInfo { get; }

        IBusConfiguration RegisterHandler<T>();

        IBus Build();
    }
}