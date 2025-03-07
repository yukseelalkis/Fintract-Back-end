using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    // veri tabani baglanti noktasi
    public class ApplicationDBContex : IdentityDbContext<AppUsers>
    {   
        //VERI TABANI BAGLANTISI KURAR
        //CTOR YAZARAK BURASI GELMIS OLDU 
        ///DbContextOptions, Entity Framework Core (EF Core) içinde veritabanı bağlantısını ve yapılandırmasını yönetmek için kullanılan bir nesnedir.
        public ApplicationDBContex(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List <IdentityRole> roles =  new List<IdentityRole> {
                 new IdentityRole{
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                 },
                 new IdentityRole{
                    Name = "User",
                    NormalizedName = "USER"
                 },
            };
            builder.Entity<IdentityRole>().HasData(roles);  
        }
    }
}