using InventoryHub.Core.Domain.Common;
using InventoryHub.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<MovementType> MovementTypes { get; set; }
        public DbSet<Product> Products { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Id = Guid.NewGuid().ToString().Substring(5, 8);
                        entry.Entity.Created = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "DefaultBaseUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "DefaultBaseUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API
            base.OnModelCreating(modelBuilder);

            #region Tables
            modelBuilder.Entity<Inventory>()
                .ToTable("Inventories");

            modelBuilder.Entity<InventoryMovement>()
                .ToTable("InventoryMovements");

            modelBuilder.Entity<MovementType>()
                .ToTable("MovementTypes");

            modelBuilder.Entity<Product>()
                .ToTable("Products");
            #endregion

            #region Primary keys
            modelBuilder.Entity<Inventory>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<InventoryMovement>()
                .HasKey(im => im.Id);

            modelBuilder.Entity<MovementType>()
                .HasKey(mt => mt.Id);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            #endregion

            #region Relationships
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductId);

            modelBuilder.Entity<InventoryMovement>()
                .HasOne(im => im.Product)
                .WithMany(p => p.InventoryMovements)
                .HasForeignKey(im => im.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryMovement>()
                .HasOne(im => im.MovementType)
                .WithMany(mt => mt.InventoryMovements)
                .HasForeignKey(im => im.MovementTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Property configurations
            modelBuilder.Entity<Product>().HasIndex(i => i.Name);
            modelBuilder.Entity<MovementType>().HasIndex(i => i.Name);
            modelBuilder.Entity<InventoryMovement>().HasIndex(i => i.ProductId);
            #endregion
        }

        public void TruncateTables()
        {
            SaveChanges();
        }
    }
}
