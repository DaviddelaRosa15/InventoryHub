using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Core.Application.Features.Product.Queries.GetAll
{
    public class GetAllProductQueryResponse
    {
        [SwaggerSchema("Listado de productos")]
        public List<GetAllProductQueryResponseChild> Products { get; set; }
    }

    public class GetAllProductQueryResponseChild
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
