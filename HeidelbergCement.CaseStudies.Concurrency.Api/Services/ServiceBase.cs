using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;

namespace HeidelbergCement.CaseStudies.Concurrency.Services;

public abstract class ServiceBase<TDbContext> where TDbContext : IDbContext
{
    protected TDbContext DbContext { get; }

    protected ServiceBase(TDbContext dbContext)
    {
        DbContext = dbContext;
    }
}