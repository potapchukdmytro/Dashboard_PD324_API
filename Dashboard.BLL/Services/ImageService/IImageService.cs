using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.ImageService
{
    public interface IImageService
    {
        Task<ServiceResponse> SaveImageAsync(UserImageVM model);
    }
}
