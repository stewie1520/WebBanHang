using System;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using WebBanHang.Extensions.DataContext;
using WebBanHang.Models;

namespace WebBanHang.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Basket> Orders { get; set; }
        public DbSet<BasketItem> ProductOrders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<WarehouseTransaction> WarehouseTransactions { get; set; }

        public DbSet<WarehouseTransactionItem> WarehouseTransactionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // this is to apply soft delete filter
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }

        public override int SaveChanges()
        {
            AddModifiedTimeStamp();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddModifiedTimeStamp();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddModifiedTimeStamp();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddModifiedTimeStamp()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseModel && x.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                ((BaseModel)entity.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
