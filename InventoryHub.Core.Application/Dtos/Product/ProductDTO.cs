using InventoryHub.Core.Application.Dtos.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Core.Application.Dtos.Product
{
    public class ProductDTO : ErrorDTO
    {
        [SwaggerSchema(Description = "Identificador único")]
        public string Id { get; set; }

        [SwaggerSchema(Description = "Nombre del producto")]
        public string Name { get; set; }

        [SwaggerSchema(Description = "Descripción")]
        public string? Description { get; set; }

        [SwaggerSchema(Description = "Precio de venta")]
        public double SalePrice { get; set; }

        [SwaggerSchema(Description = "Minimo en stock")]
        public int MinimumStock { get; set; }

        [SwaggerSchema(Description = "Fecha de creación")]
        public DateTime Created { get; set; }

        [SwaggerSchema(Description = "Fecha de última actualización")]
        public DateTime? LastModified { get; set; }
    }
}
