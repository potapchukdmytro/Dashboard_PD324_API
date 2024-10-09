using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.JwtService
{
    public interface IJwtService
    {
        Task<ServiceResponse> GenerateTokensAsync(User user);
        Task<ServiceResponse> RefreshTokensAsync(string refreshToken, string accessToken);
    }
}
