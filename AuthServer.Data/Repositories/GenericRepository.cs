﻿using AuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        //Veri tabanı ilemleri için oluşturulur.
        private readonly DbContext _dbContext;

        //Tablolar üzerinde işlem yapılabilmesi için 
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyc()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsycn(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
           return _dbSet.Where(predicate);
        }

        IEnumerable<TEntity> IGenericRepository<TEntity>.GetAllAsyc()
        {
            throw new NotImplementedException();
        }
    }
}
