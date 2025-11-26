using InventoryHub.Core.Application.Dtos.Auth;

namespace InventoryHub.Core.Application.Interfaces.Services
{
    public interface IAccountService
	{
		Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
		Task<string> GenerateJWToken(string userId);
		string GenerateRefreshToken(string userId);
		string ValidateRefreshToken();
    }
}
