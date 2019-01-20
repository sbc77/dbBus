namespace dbBus.Extensions.Mssql
{
    using dbBus.Core;
    using TheOne.OrmLite.Core;
    using TheOne.OrmLite.SqlServer;

    public static class DbBusMssqlExtension
    {
        public static IBusConfiguration UseMssql(this IBusConfiguration cfg, string connectionString)
        {
            cfg.DbConnectionFactory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
            return cfg;
        }
    }
}
