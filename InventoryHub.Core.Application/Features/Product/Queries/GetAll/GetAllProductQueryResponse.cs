using InventoryHub.Core.Application.Dtos.Product;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Core.Application.Features.Product.Queries.GetAll
{
    public class GetAllProductQueryResponse
    {
        [SwaggerSchema("Listado de productos")]
        public List<ProductResponseDTO> Products { get; set; }
    }
}
