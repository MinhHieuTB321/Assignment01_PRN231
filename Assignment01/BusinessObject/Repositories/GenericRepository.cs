using BusinessObject.IRepo;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> _dbSet;
        public GenericRepository(FstoreDbContext dbContext)
        {
            _dbSet=dbContext.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
           var result= await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<TEntity> FindByField(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
         => await includes
           .Aggregate(_dbSet!.AsQueryable(),
               (entity, property) => entity!.Include(property)).AsNoTracking()
           .FirstOrDefaultAsync(expression!);

        public async Task<List<TEntity>> FindListByField(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        => await includes
           .Aggregate(_dbSet!.AsQueryable(),
               (entity, property) => entity.Include(property)).AsNoTracking()
           .Where(expression!)
            .ToListAsync();

        public async Task<List<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includes)
        =>
            await includes
           .Aggregate(_dbSet.AsQueryable(),
               (entity, property) => entity.Include(property).IgnoreAutoIncludes())
           .ToListAsync();

        public void SoftRemove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void SoftRemoveRange(List<TEntity> entities)
        {
           _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet?.Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
