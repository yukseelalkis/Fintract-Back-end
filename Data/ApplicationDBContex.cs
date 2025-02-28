using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContex : DbContext
    {
        //CTOR YAZARAK BURASI GELMIS OLDU 
        ///DbContextOptions, Entity Framework Core (EF Core) içinde veritabanı bağlantısını ve yapılandırmasını yönetmek için kullanılan bir nesnedir.
        public ApplicationDBContex(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments {get; set;}
    }
}