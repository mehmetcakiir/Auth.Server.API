using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.UnitOfWork
{
    /*
     * IUnitOfWork interface inin amacı veri tabanına yapılan tüm işlemleri tek bit transaction olarak yapılmasıdır.
     * 
     * Veri tabanı işlemlerinin toplu bir şekilde tek transactionda yapılması veri tabanına yapılan istek sayısını azaltacağı için sistemi rahatlatacaktir.
     * 
     * İşlemlerin tek tarnsactionda yapılması veri tabanına yapılan isteklerden birinin dahi hata alınması durumunda EfCore rolback yapılarak veri güvenliği sağlanır
     * 
     * saveChange() methodu çağrılana kadar yapılan işlemler memory de tutulur.
     */


    /*
     * IUnitOfWork suz Örnek 
     * 
     * SaveProduct.add(Product);
     * saveChange();
     * 
     * SaveStock.add(stock);
     * saveChange();
     */
     

    /*
     * IUnitOfWork lü Örnek
     * 
     * SaveProduct.add(Product);
     * 
     * SaveStock.add(stock);
     * 
     * saveChange();
     */
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
    }
}
 