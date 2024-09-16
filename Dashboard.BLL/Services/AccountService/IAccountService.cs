using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponse> SignUpAsync(SignUpVM model);
        Task<ServiceResponse> SignInAsync(SignInVM model);
    }
}
