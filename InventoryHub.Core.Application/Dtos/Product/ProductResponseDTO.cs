using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Core.Application.Dtos.Product
{
    public class ProductResponseDTO 
    {
        [SwaggerSchema(Description = "Identificador único")]
        public string Id { get; set; }

        [SwaggerSchema(Description = "Nombre del producto")]
        public string Name { get; set; }

        [SwaggerSchema(Description = "Descipción")]
        public string? Description { get; set; }

        [SwaggerSchema(Description = "Precio de venta")]
        public double SalePrice { get; set; }

        [SwaggerSchema(Description = "Fecha de creación")]
        public DateTime Created { get; set; }

        [SwaggerSchema(Description = "Fecha de última actualización")]
        public DateTime? LastModified { get; set; }
    }
}
