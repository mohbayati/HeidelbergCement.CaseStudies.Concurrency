using System.Linq.Expressions;

namespace HeidelbergCement.CaseStudies.Concurrency.Domain.Common.Repository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> All();
    
    IQueryable<TEntity> FindByInclude
    (Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> GetAllIncluding
        (params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindByKey(int id, params Expression<Func<TEntity, object>>[] includeProperties);
    TEntity? Find(Expression<Func<TEntity?, bool>> predicate);
    Task<int> Insert(TEntity entity);
    Task<int> Update(TEntity entity);
    Task<int> Delete(int id);
}