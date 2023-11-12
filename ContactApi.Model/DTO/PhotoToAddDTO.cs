using Microsoft.AspNetCore.Http;

namespace ContactApi.Model.DTO
{
    public class PhotoToAddDto
    {
        public IFormFile PhotoFile { get; set; }

    }
}
