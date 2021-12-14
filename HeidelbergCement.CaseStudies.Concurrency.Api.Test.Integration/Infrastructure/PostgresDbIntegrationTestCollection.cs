using Xunit;

namespace HeidelbergCement.CaseStudies.Concurrency.Api.Test.Infrastructure;

[CollectionDefinition("PostgresDB Integration Tests")]
public class PostgresDbIntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
}