using CommonLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    //Gelen TEntity ler TDto lara cast edilecekler.
    public interface IServiceGeneric<TEntity,TDto> where TEntity : class where TDto : class
    {
        /* Methodlar AutoServer.Core katmanında bulunan IGenericRepository e benzesede, methodların dönüş tipleri kendi oluşturduğumuz CommonLibrary katmanında
        bulunan Response olarak revize edilir.
        

        Methodların giriş ve çıkış parametreleri TDto olmasının sebebi API larla TDto nesneleri üzerinden, veri tabanı ile TEntity ler üzerinden iletişim kutulacak.
         
        */


        //Id ye göre getir
        Task<Response<TDto>> GetByIdAsycn(int id);

        //Tüm verileri al
        Task<Response<IEnumerable<TDto>>> GetAll();

        //Filitreleme koşuluna göre verileri al
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

        /* Veriyi sil (Bu aksiyonda geriye herhangi bir veri dönmeye gerek olmayacağı için CommonLibrary katmanının içinde bulunan Dtos klasörü içerisine
         NoDataDto adında boş bir sınıf oluşturularak Remove methodunun dönüş değerine verilir.) */
        Task<Response<NoDataDto>> Remove(int id);

        //Veriyi güncelle
        Task<Response<NoDataDto>> Update(TDto entity, int id);

        Task<Response<TDto>> AddAsync(TDto entity);
    }
}
