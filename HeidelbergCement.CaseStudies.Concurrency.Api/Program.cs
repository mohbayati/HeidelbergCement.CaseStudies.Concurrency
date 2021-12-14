
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;
using Microsoft.EntityFrameworkCore;

namespace HeidelbergCement.CaseStudies.Concurrency;

public static class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var db = services.GetRequiredService<ScheduleDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}