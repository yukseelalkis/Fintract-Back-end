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
        public DbSet<Portfolio> Portfolios {get; set;}


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //✅ Aynı kullanıcının (AppUser) aynı hisseyi (StockId) birden fazla kez eklemesini engeller.
            //✅ Many-to-Many (Çoktan-Çoka) ilişkiyi yönetir.

            //1️⃣ Bileşik (Composite) Primary Key Belirleme
           // Portfolio tablosunda AppUser ve StockId alanlarını birlikte birincil anahtar (Primary Key) olarak tanımlar.
            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUsersId, p.StockId }));


            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser) //Portfolio -> AppUser ilişkisi
                .WithMany(u => u.Portfolios) // Bir AppUser, birden fazla Portfolio'ya sahip olabilir
                .HasForeignKey(p => p.AppUsersId);// Portfolio içindeki Foreign Key: AppUsersId

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);    

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