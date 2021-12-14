using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts;

public interface IDbContext
{
    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void RemoveRange(IEnumerable<object> entities);

    DatabaseFacade Database { get; }

    void Dispose();

    DbSet<T> Set<T>() where T : class;

    void SetModified(object entity);
}