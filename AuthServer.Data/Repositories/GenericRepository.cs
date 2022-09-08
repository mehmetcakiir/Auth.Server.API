using AuthServer.Core.Repositories;
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
        //Veri tabanı işlemleri için oluşturulur.
        private readonly DbContext _dbContext;

        //Tablolar üzerinde işlem yapılabilmesi için 
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        //Verilen entity i veri tabanına ekler
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }


        //Veri tabanından tüm entityleri çeker
        public async Task<IEnumerable<TEntity>> GetAllAsyc()
        {
            return await _dbSet.ToListAsync();
        }

        //Verilen Id ye göre veri tabanından entity i çeker 
        public async Task<TEntity> GetByIdAsycn(int id)
        {
            var entity = await _dbSet.FindAsync(id); 

            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        //Verilen entity i veri tabanına ekler
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        //Verilen entity i veri tabanında günceller
        public TEntity Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        //Verilen linq sorgusuna göre veri tabanında filitreleme işlemi yapılır
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
           return _dbSet.Where(predicate);
        }
    }
}
