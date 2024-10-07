using Microsoft.AspNetCore.Http;

namespace Dashboard.BLL.Services.ImageService
{
    public interface IImageService
    {
        Task<ServiceResponse> SaveImageAsync(IFormFile image);
    }
}
