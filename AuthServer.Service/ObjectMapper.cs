using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    
    /*ObjectMapper dan nesne örneği alınacak
     * 
     * Bir class ve methodun static olarak belirtilmesi çağrılacağı zaman " classİsmi.methodİsmi " olarak çağrılmaya olanak sağlamaktadır.
     * 
     * Static olarak belirtilmeyen methodlar öncelikle o sınıftan nesne üretildikten sonra o " üretilenNesneİsmi.methodİsmi " olarak çağrılır.
     */
    public static class ObjectMapper
    {
        //Uygulama ayağı kalkar kakmaz data memory e eklenmez. Kullanıcı çağırdığında data memorye yüklenir.(Geç İnitialization sağlar.)
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {

            //ObjectMapper.mapper denilene kadat bu methodun içerisindeki kod çalışmayacak.

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });

            return config.CreateMapper();
        });

        public static IMapper mapper => lazy.Value;
    }
}
