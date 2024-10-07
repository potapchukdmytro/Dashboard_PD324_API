using Dashboard.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Dashboard.BLL.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse> SaveImageAsync(IFormFile image)
        {
            if (image == null)
            {
                return ServiceResponse.GetBadRequestResponse(message: "Не вдалося зберегти зображення", errors: "Не знайдено файл");
            }

            var types = image.ContentType.Split('/');

            if (types.Length != 2 || types[0] != "image")
            {
                return ServiceResponse.GetBadRequestResponse(message: "Не вдалося зберегти зображення", errors: "Файл не є зображенням");
            }

            var ext = types[1];
            var imageName = Guid.NewGuid().ToString() + "." + ext;
            var rootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(rootPath, Settings.UsersImagePath, imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                using (var imageStream = image.OpenReadStream())
                {
                    await imageStream.CopyToAsync(stream);
                }
            }

            return ServiceResponse.GetOkResponse("Зображення успішно збережено", imageName);
        }
    }
}
