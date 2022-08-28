using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    // " IEnumerable " tipi veriyi önce belleğe atıp ardından bellekteki bu veri üzerinden belirtilen koşulları çalıştırır ve veriye uygular.
    // " IQueryable " tipinde ise belirtilen sorgular direk olarak server üzerinde çalıştırılır ve dönüş sağlar.

    // Ekleme güncelleme silme ve select işlemleri tanımlanır
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //Id ye göre getir
        Task<TEntity> GetByIdAsycn(int id);

        //Tüm verileri al
        IEnumerable<TEntity> GetAll();

        //Filitreleme koşuluna göre verileri al
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        //Veriyi sil
        void Remove(TEntity entity);

        //Veriyi güncelle
        TEntity Update(TEntity entity);
    }
}