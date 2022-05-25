using Microsoft.Extensions.DependencyInjection;



namespace SmartCat.Extensions;

public static class FreeSqlExtensions
{
    public static IServiceCollection AddFreeSql(this IServiceCollection services)
    {
        // AppCore.AddDb("Main", new FreeSql.FreeSqlBuilder().UseConnectionString(FreeSql.DataType.Sqlite, "Data Source = GoogleBigQuery.db;").UseAutoSyncStructure(true).Build());

        return services;
    }
}
