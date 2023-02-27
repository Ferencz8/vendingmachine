using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DAL.Repositories.Interfaces;

namespace VendingMachine.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DbSet<TEntity> _entitiySet;
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entitiySet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filterExpression = null)
        {
            IQueryable<TEntity> query = _entitiySet;
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }
            return query;
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await _entitiySet.FindAsync(id);
        }

        public async Task CreateInBulk(IEnumerable<TEntity> entities)
        {
            await _entitiySet.AddRangeAsync(entities);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await _entitiySet.AddAsync(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            _entitiySet.Update(entity);
        }

        public async Task Delete(object id)
        {
            var entity = await GetById(id);
            _entitiySet.Remove(entity);
        }
    }
}

