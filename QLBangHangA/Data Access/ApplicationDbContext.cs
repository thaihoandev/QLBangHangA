using Microsoft.EntityFrameworkCore;
using QLBangHangA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Data_Access
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ApplicationUser>ApplicationUsers{ get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<TransactStatus> TransactStatuses { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }
        public virtual DbSet<ProductVariantValue> ProductVariantValues { get; set; }

        public virtual DbSet<ProductAttribute> ProductAttribute { get; set; }




    }
}
