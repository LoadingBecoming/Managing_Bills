using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.Entities
{
    public class AppDbContext:DbContext
    {
        public virtual DbSet<Products> Products { set; get; }
        public virtual DbSet<Properties> Properties { set; get; }
        public virtual DbSet<PropertyDetails> PropertyDetails { set; get; }
        public virtual DbSet<ProductDetails> ProductDetails { set; get; }
        public virtual DbSet<ProductDetailPropertyDetails> ProductDetailPropertyDetails { set; get; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server = DESKTOP-8LDGIN0\\SQLEXPRESS ; Database = QLSanPham; Trusted_Connection = True; " +
                $"TrustServerCertificate = True");
        }
    }
}
