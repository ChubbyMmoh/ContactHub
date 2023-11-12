using Microsoft.AspNetCore.Http;

namespace ContactApi.Core.Interface
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
