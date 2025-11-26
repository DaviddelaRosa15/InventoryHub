using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Dtos.Auth;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Features.Auth.Commands.Authenticate;
using InventoryHub.Core.Application.Features.Auth.Queries.GetRefreshAccessToken;
using InventoryHub.Core.Application.Helpers;
using InventoryHub.Core.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace InventoryHub.Interface.WebApi.Controllers
{
    public class AuthController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        private readonly RefreshJWTSettings _refreshSettings;

        public AuthController(IOptions<RefreshJWTSettings> refreshSettings)
        {
            _refreshSettings = refreshSettings.Value;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(AuthenticationResponse))]
        [SwaggerOperation(
           Summary = "Iniciar sesión",
           Description = "Autentica al usuario y devuelve un JWT Token"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ErrorMapperHelper.ListError(errors));
                }

                if (response.HasError)
                {
                    if (response.Error.Contains("No existe una cuenta registrada con este usuario"))
                    {
                        return BadRequest(ErrorMapperHelper.Error(ErrorMessages.BadRequest, ErrorMessages.LoginError));
                    }
                    if (response.Error.Contains("correo"))
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new ErrorDTO() { Status = "Fallido", Details = [new ErrorDetailsDTO { Code = "E002", Message = response.Error }] });
                    }
                    return BadRequest(ErrorMapperHelper.Error(ErrorMessages.InternalServer, response.Error));
                }

                Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(_refreshSettings.DurationInMinutes),
                    SameSite = SameSiteMode.None
                });

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMapperHelper.Error(ErrorMessages.InternalServer, e.Message));
            }

        }

        [HttpGet("refresh-access-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(AuthenticationResponse))]
        [SwaggerOperation(
           Summary = "Obtener nuevo access token",
           Description = "Valida el refresh token y devuelve un JWT Token de acceso nuevo"
        )]
        public async Task<IActionResult> RefreshAccesToken()
        {
            try
            {
                var response = await Mediator.Send(new GetRefreshAccessTokenQuery());

                if (response.HasError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ErrorMapperHelper.Error(ErrorMessages.InternalServer, response.Error));
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMapperHelper.Error(ErrorMessages.InternalServer, e.Message));
            }
        }
    }
}
