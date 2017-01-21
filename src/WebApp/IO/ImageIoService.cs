using DTO.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.IO
{
    public class ImageIoService
    {
        private readonly IHostingEnvironment hostingEnv;

        public ImageIoService(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }

        public void Create(IFormFile file, string path)
        {

        }

        public void Delete(IList<ImageDto> images)
        {
            var root = hostingEnv.WebRootPath;
            if (images.Any())
            {
                foreach (var image in images)
                {
                    var index = image.Link.IndexOf("control");
                    var imagePath = $"{root}\\{image.Link.Substring(index)}";

                    System.IO.File.Delete(imagePath);
                }
            }
        }
    }
}
