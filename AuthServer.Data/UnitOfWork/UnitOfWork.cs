using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //saveChange() methodu çağrılabilmesi için DbContext e ihtiyaç vardır.
        private readonly DbContext _context;


        //AppDbContext sınıfı kendi oluşturduğumuz bir sınıf
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
