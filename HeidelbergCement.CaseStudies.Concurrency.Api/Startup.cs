using HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Repositories;
using HeidelbergCement.CaseStudies.Concurrency.Extensions;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.Repositories.Schedule;
using HeidelbergCement.CaseStudies.Concurrency.Services;
using HeidelbergCement.CaseStudies.Concurrency.Services.Interfaces;

namespace HeidelbergCement.CaseStudies.Concurrency;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
        services.AddSingleton(_configuration);
        services.AddDatabase(_configuration);
        services.AddCors();
        services.AddScoped<IScheduleService, SchedulesService>();
        services.AddTransient<IScheduleRepository, ScheduleRepository>();

    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IScheduleDbContext dbContext)
    {
        app.UseConcurrentRequestsMiddleware();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles();
    }

}