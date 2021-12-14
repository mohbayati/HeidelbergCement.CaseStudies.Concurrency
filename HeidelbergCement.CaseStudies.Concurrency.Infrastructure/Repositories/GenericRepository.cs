using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using HeidelbergCement.CaseStudies.Concurrency.Domain.Common.Repository;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;

namespace HeidelbergCement.CaseStudies.Concurrency.Infrastructure.Repositories;

    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IScheduleDbContext _context;

        private readonly DbSet<TEntity> _dbSet;

        protected GenericRepository(IScheduleDbContext context) {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
   
        public IQueryable<TEntity> All() {
            return _dbSet.AsTracking();
        }
        

        public IQueryable<TEntity> FindByInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties) {
            var query = GetAllIncluding(includeProperties);
            return query.Where(predicate);
        }

        public IQueryable<TEntity> GetAllIncluding
            (params Expression<Func<TEntity, object>>[] includeProperties) {
            var queryable = _dbSet.AsTracking();

            return includeProperties.Aggregate
                (queryable, (current, includeProperty) => current.Include(includeProperty));
        }
        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> results = _dbSet.AsTracking()
                .Where(predicate);
            return results;
        }

        public Task<TEntity?> FindByKey(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var aggregate = includeProperties.Aggregate
                (_dbSet.AsQueryable(), (current, includeProperty) => current.Include(includeProperty));
            
            var lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            return aggregate.SingleOrDefaultAsync(lambda);
        }

        public TEntity? Find(Expression<Func<TEntity?, bool>> predicate)
        {
            return _dbSet.AsNoTracking()
                .SingleOrDefault(predicate);
        }

        public Task<int> Insert(TEntity entity) {
            _dbSet.Add(entity);
            return DelaySave();
        }

        public Task<int> Update(TEntity entity) 
        {
            _dbSet.Attach(entity);
            _context.SetModified(entity);
            return DelaySave();
        }

        //DO NOT  modify this. This delay is put in place to ensure that the concurrency problem is reproducible.
        private async Task<int> DelaySave()
        {
            await Task.Delay(1000);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) 
        {
            var entity = await FindByKey(id);
            _dbSet.Remove(entity);
            return await DelaySave();
        }
    }
