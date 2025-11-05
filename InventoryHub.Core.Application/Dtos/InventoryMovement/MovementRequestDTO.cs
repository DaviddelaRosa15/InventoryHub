using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Core.Application.Dtos.InventoryMovement
{
    public class MovementRequestDTO
    {
        [SwaggerParameter(Description = "Id del producto")]
        [Required(ErrorMessage = "Debe ingresar el id del producto")]
        public string ProductId { get; set; }

        [SwaggerParameter(Description = "Cantidad a ingresar")]
        [Required(ErrorMessage = "Debe ingresar la cantidad a ingresar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        public int Quantity { get; set; }
    }
}
