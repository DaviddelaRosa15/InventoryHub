using InventoryHub.Core.Application.Dtos.Auth;
using InventoryHub.Core.Application.Interfaces.Services;
using InventoryHub.Core.Domain.Settings;
using InventoryHub.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryHub.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly RefreshJWTSettings _refreshSettings;
        IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
              IOptions<JWTSettings> jwtSettings,
              IOptions<RefreshJWTSettings> refreshSettings,
              IHttpContextAccessor httpContextAccessor,
              ILogger<AccountService> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _refreshSettings = refreshSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    response.HasError = true;
                    response.Error = $"No existe una cuenta registrada con este usuario: {request.UserName}";
                    return response;
                }

                var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!isConfirmed)
                {
                    response.HasError = true;
                    response.Error = "El usuario no ha confirmado su cuenta. Revise su correo electrónico";
                    return response;
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    response.HasError = true;
                    response.Error = $"Usuario o contraseña inválidos";
                    return response;
                }

                response.JWToken = await GenerateJWToken(user.Id);
                response.ExpiresIn = (_jwtSettings.DurationInMinutes * 60).ToString();
                response.ExpiresAt = DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes);
                response.RefreshToken = GenerateRefreshToken(user.Id);
                response.RefreshExpiresIn = (_refreshSettings.DurationInMinutes * 60).ToString();
                response.RefreshExpiresAt = DateTime.Now.AddMinutes(_refreshSettings.DurationInMinutes);

                _logger.LogInformation("Inicio de sesión finalizado correctamente");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Un error ocurrió tratando de autenticar al usuario");
                response.HasError = true;
                response.Error = ex.Message;
                return response;
            }
        }

        public async Task<string> GenerateJWToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id),
                new Claim("UrlImage", user.UrlImage)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);


            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        public string GenerateRefreshToken(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", userId)
            };

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_refreshSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _refreshSettings.Issuer,
                audience: _refreshSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_refreshSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }

        public string ValidateRefreshToken()
        {
            string token = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (token == null)
            {
                return "Error: No existen token de actualización";
            }

            string userId = "";
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _refreshSettings.Issuer,
                ValidAudience = _refreshSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_refreshSettings.Key))
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                if (validatedToken == null)
                {
                    return "Error: El token no es válido";
                }
                var id = claimsPrincipal.FindFirst("uid");
                userId = id.Value;
            }
            catch (SecurityTokenValidationException ex)
            {
                return "Error de validación del token JWT: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error al decodificar el token JWT: " + ex.Message;
            }

            return userId;
        }

        #region Private Methods
        #endregion

    }
}
