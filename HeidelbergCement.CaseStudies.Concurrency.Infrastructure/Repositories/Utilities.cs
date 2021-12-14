using System.Linq.Expressions;

namespace HeidelbergCement.CaseStudies.Concurrency.Infrastructure.Repositories;

public static class Utilities
{
    public static Expression<Func<TEntity, bool>> BuildLambdaForFindByKey<TEntity>(int id) {
        var item = Expression.Parameter(typeof(TEntity), "entity");
        var prop = Expression.Property(item, typeof(TEntity).Name[..^1] + "Id");
        var value = Expression.Constant(id);
        var equal = Expression.Equal(prop, value);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, item);
        return lambda;
    }
}