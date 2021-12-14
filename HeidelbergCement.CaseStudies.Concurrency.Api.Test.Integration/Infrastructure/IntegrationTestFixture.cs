using System;
using System.Net.Http;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace HeidelbergCement.CaseStudies.Concurrency.Api.Test.Infrastructure;

public class IntegrationTestFixture: IDisposable
{
    public readonly IScheduleDbContext? DbContext;
    private readonly WebApplicationFactory<Startup> _factory;

    public IntegrationTestFixture()
    {
        _factory = new CustomWebApplicationFactory<Startup>();
        DbContext = _factory.Services.GetService(typeof(IScheduleDbContext)) as IScheduleDbContext;
    }
    public void Dispose()
    {
        DbContext?.Dispose();
        _factory.Dispose();
    }

    public HttpClient HttpClient => _factory.CreateClient();
}