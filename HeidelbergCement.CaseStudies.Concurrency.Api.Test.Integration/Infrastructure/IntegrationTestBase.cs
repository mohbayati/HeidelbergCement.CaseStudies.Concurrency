using System.Net.Http;
using HeidelbergCement.CaseStudies.Concurrency.Api.Test.Utilities;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace HeidelbergCement.CaseStudies.Concurrency.Api.Test.Infrastructure;

[Collection("PostgresDB Integration Tests")]
public class IntegrationTestBase: IClassFixture<IntegrationTestFixture>
{
    protected ITestOutputHelper Output { get; }
    protected IScheduleDbContext? DbContext { get; }

    private IntegrationTestFixture _fixture;
    protected IntegrationTestBase(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output)
    {
        Output = output;
        DbContext = integrationTestFixture.DbContext;
        _fixture = integrationTestFixture;

        if (DbContext != null)
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }
    }

    public RequestBuilder NewRequest => new RequestBuilder(_fixture.HttpClient);
}