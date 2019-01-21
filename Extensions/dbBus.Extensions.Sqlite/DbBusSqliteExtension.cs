using System;
using dbBus.Core;
using TheOne.OrmLite.Core;
using TheOne.OrmLite.Sqlite;

namespace dbBus.Extensions.Sqlite
{
    public static class DbBusSqliteExtension
    {
        public static IBusConfiguration UseSqlite(this IBusConfiguration cfg, string connectionString)
        {
            cfg.DbConnectionFactory = new OrmLiteConnectionFactory(connectionString, SqliteDialect.Provider);
            return cfg;
        }
    }
}
