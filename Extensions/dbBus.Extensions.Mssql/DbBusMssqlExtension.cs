using dbBus.Core;
using TheOne.OrmLite.Core;
using TheOne.OrmLite.SqlServer;

namespace dbBus.Extensions.Mssql
{
    public static class DbBusMssqlExtension
    {
        public static IBusConfiguration UseMssql(this IBusConfiguration cfg, string connectionString)
        {
            cfg.DbConnectionFactory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
            return cfg;
        }
    }
}
