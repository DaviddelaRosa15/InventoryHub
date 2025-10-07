using InventoryHub.Core.Application.Dtos.Product;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Core.Application.Features.Product.Queries.GetById
{
    public class GetByIdProductQueryResponse
    {
        [SwaggerSchema("Producto")]
        public ProductResponseDTO Product { get; set; }
    }
}
