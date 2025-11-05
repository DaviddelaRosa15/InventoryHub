using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Dtos.InventoryMovement;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Application.Features.InventoryMovement.Commands.RegisterEntry;
using InventoryHub.Core.Application.Features.Product.Command.Add;
using InventoryHub.Core.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Interface.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/inventorymovement")]
    [SwaggerTag("Movimientos de inventario")]
    public class InventoryMovementController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpPost("entry")]
        [SwaggerOperation(
           Summary = "Registrar movimiento de entrada",
           Description = "Nos permite crear existencia o aumentar existencia del inventario de un producto"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterEntryDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        public async Task<IActionResult> RegisterEntry([FromBody] RegisterEntryCommand command)
        {
            try
            {
                if (command == null)
                {
                    return BadRequest(ErrorMapperHelper.Error(ErrorMessages.BadRequest, "El cuerpo de la solicitud no puede estar vacío o tiene errores de formato."));
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList<string>();

                    return BadRequest(ErrorMapperHelper.ListError(errors));
                }

                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMapperHelper.Error(ErrorMessages.InternalServer, e.Message));
            }
        }
    }
}
