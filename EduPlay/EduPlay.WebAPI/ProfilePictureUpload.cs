using Microsoft.AspNetCore.Http;

namespace EduPlay.WebAPI
{
    public class ProfilePictureUpload
    {
        public IFormFile files { get; set; }
    }
}
