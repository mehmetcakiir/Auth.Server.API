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
    internal class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
    {
        public void Configure(EntityTypeBuilder<UserApp> builder)
        {
            builder.Property(x => x.City).HasMaxLength(20);
        }
    }
}
