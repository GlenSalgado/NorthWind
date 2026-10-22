namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;

internal class NorthWindDomainLogsContextFactory :
    IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
{
    public NorthWindDomainLogsContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("NORTHWIND_LOGS_DB_CONNECTION")
            ?? throw new InvalidOperationException(
                "Set NORTHWIND_LOGS_DB_CONNECTION before running NorthWind logs EF design-time commands.");
        IOptions<DBOptions> DBOptions =
            Microsoft.Extensions.Options.Options.Create(
            new DBOptions
            {
                DomainLogsConnectionString = connectionString
            });

        return new NorthWindDomainLogsContext(DBOptions);
    }
}
