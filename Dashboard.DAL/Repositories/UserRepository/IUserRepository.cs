namespace Dashboard.DAL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> CheckEmailAsync(string email);
        Task<bool> CheckUserNameAsync(string userName);
    }
}
