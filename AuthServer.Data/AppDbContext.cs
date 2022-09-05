using AuthServer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data
{
    /*
     * Bu sınıf sqlserver tarafında veri tabanına karşılık gelecek olan sınıftır.
     * 
     * Identity ile ilgili oluşacak tabloların ve diğer ürünler gibi ekstra olacak olan tabloların tek bir veri tabanında oluşması için AppDbContext sınıfı DbContext
     * sınıfından miras almak yerine IdentityDbContext sınıfından miras almaktadır. 
     * 
     * Bu sınıfa parametre olarak kendi oluşturduğumuz ve IdentityUser sınıfından miras alan UserApp.rol ve kullanıcılar için primerykey lerin tipleri olarak
     * string verildi
     */
    public class AppDbContext : IdentityDbContext<UserApp,IdentityRole,string>
    {
        /* Veri tabanı yolunun startup dosyasından verilebilmesi için gerekli kod parçacığı
         * 
         * AppDbContext in IdentityDbContext ten miras alması ile birlikte üyelikle ilgili tüm Dbsetler otomotik olarak oluşmaktadır. (UserApp)
         */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /*
         * Üyelikle alakası olmayan Dbsetler manuel olarak eklenir. (UserRefreshToken ve Produkt)
         */

        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }



        /*
         * Veri tabanında bu tablolar oluşurken colon özellikleri verilecek olan "OnModelCreating" isimli method oluşturulur.
         * 
         * "OnModelCreating" method içerisine entity lerin Configuration (colum özellikleri) lar kodlanır fakat bu işlem methodu şişirilmesine neden olacaktır.
         * 
         * Bu işlemi Data katmanında Configuration klasörü açarak kodlandıktan sonra "OnModelCreating" methodu içerisinde referans verilir.
         * 
         */

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Üzerinde bulunduğun Data katmanının Assembl i tara ve IEntityTypeConfiguration interface ini implemet eden classları dahil et.
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
        }

    }
}