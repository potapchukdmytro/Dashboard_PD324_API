using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponse> SignUpAsync(SignUpVM model);
        Task<ServiceResponse> SignInAsync(SignInVM model);
        Task<ServiceResponse> EmailConfirmationAsync(string userId, string token);
        Task<ServiceResponse> ForgotPasswordAsync(string email);
        Task<ServiceResponse> ResetPasswordAsync(ResetPasswordVM model);
    }
}
