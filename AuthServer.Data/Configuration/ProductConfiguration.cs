using AuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Configuration
{
    /*
     * AppDbContext sınıfında bulunan OnModelCreating methodunda bu sınıfın kullanılabilmesi için ProductConfiguration sınıfı IEntityTypeConfiguration interface 
     * inden miras almaktadır
     * 
     */
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //Id alanı Product lar için bir primaryKey dir
            builder.HasKey(x => x.Id);

            //Name alanı boş geçilemez ve en fazla 200 karekter alabilir.
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            //Stock alanı boş geçilemez
            builder.Property(x => x.Stock).IsRequired();

            //Fiyat 18,2 formatında
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        }
    }
}
