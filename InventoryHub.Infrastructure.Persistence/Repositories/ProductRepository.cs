using InventoryHub.Core.Application.Interfaces.Repositories;
using InventoryHub.Core.Domain.Entities;
using InventoryHub.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _dbContext;
        public ProductRepository(IDbContextFactory<ApplicationContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Product product)
        {
            using var dbContext = _dbContext.CreateDbContext();

            await dbContext.Database.ExecuteSqlRawAsync(
                "CALL sp_product_create({0}, {1}, {2}, {3}, {4}, {5})",
                product.Id, product.Name, product.Description, product.SalePrice, product.MinimumStock, product.CreatedBy);
        }

        public async Task UpdateAsync(Product product)
        {
            using var dbContext = _dbContext.CreateDbContext();

            await dbContext.Database.ExecuteSqlRawAsync(
                "CALL sp_product_update({0}, {1}, {2}, {3}, {4}, {5})",
                product.Id, product.Name, product.Description, product.SalePrice, product.MinimumStock, product.LastModifiedBy);
        }

        public async Task DeleteAsync(string id)
        {
            using var dbContext = _dbContext.CreateDbContext();

            await dbContext.Database.ExecuteSqlRawAsync(
                "CALL sp_product_delete({0})", id);
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            using var dbContext = _dbContext.CreateDbContext();

            return await dbContext.Products
                .FromSqlRaw("SELECT * FROM fn_product_get_by_id({0})", id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            using var dbContext = _dbContext.CreateDbContext();

            return await dbContext.Products
                .FromSqlRaw("SELECT * FROM fn_product_get_all()")
                .ToListAsync();
        }
    }
}
