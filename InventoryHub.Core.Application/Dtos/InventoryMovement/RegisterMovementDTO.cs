using InventoryHub.Core.Application.Dtos.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Core.Application.Dtos.InventoryMovement
{
    public class RegisterMovementDTO : ErrorDTO
    {
        [SwaggerSchema(Description = "Identificador único del movimiento de entrada.")]
        public string Id { get; set; }

        [SwaggerSchema(Description = "Identificador del producto asociado.")]
        public string ProductId { get; set; }

        [SwaggerSchema(Description = "Cantidad de productos ingresados al inventario.")]
        public int Quantity { get; set; }

        [SwaggerSchema(Description = "Fecha en la que se realizó la entrada de inventario.")]
        public DateTime MovementDate { get; set; }
    }
}
