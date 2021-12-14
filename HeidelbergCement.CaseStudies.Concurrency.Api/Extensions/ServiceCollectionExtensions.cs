using System.Reflection;
using HeidelbergCement.CaseStudies.Concurrency.Configuration;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;
using Microsoft.EntityFrameworkCore;

namespace HeidelbergCement.CaseStudies.Concurrency.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var getSection = (string name) => configuration.GetSection($"Databases:Schedule:{name}").Value;
        var databaseInfoScheduler = new DatabaseInfo(
            host: getSection("Host"),
            port: getSection("Port"),
            username: getSection("UserName"),
            password: getSection("Password"),
            database: getSection("DatabaseName"),
            disableSsl: true
        );
        var migrationsAssembly = typeof(IScheduleDbContext).GetTypeInfo().Assembly.GetName().Name;

        services.AddDbContext<IScheduleDbContext, ScheduleDbContext>(
            builder => builder.UseNpgsql(databaseInfoScheduler.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(migrationsAssembly);
                optionsBuilder.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
            }));
    }
    public static void AddCors(this IServiceCollection services, string policyName)
    {
        services.AddCors(o => o.AddPolicy(policyName, builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
    }

}