using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Features.Product.Queries.GetAll;
using InventoryHub.Core.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryHub.Interface.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/product")]
    [SwaggerTag("Manejo de afiliados")]
    public class ProductController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllProductQueryResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDTO))]
        [SwaggerOperation(
           Summary = "Obtener todos los productos",
           Description = "Nos permite obtener todos los productos disponibles en el sistema"
        )]
        public async Task<IActionResult> GetAffiliates()
        {
            try
            {
                var result = await Mediator.Send(new GetAllProductQuery());

                if (result.Products.Count == 0)
                {
                    return NotFound(ErrorMapperHelper.Error(ErrorMessages.NotFound, "No existen productos en el sistema"));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMapperHelper.Error(ErrorMessages.InternalServer, e.Message));
            }

        }
    }
}
