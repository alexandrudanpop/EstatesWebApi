using DTO.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Api.IO
{
    public class ImageIoService
    {
        private readonly IHostingEnvironment hostingEnv;

        public ImageIoService(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }

        public void SaveOnDisk(IFormFile file, string fileName, string fileIdentifier)
        {
            var fileExtension = System.IO.Path.GetExtension(fileName);
            var path = hostingEnv.WebRootPath + $@"\control-f5.com-{fileIdentifier}{fileExtension}";

            using (var fs = System.IO.File.Create(path))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        public string CreateServerLink(HttpRequest request, string fileName, string fileIdentifier)
        {
            var fileExtension = System.IO.Path.GetExtension(fileName);

            return hostingEnv.IsDevelopment()
                ? request.Host.Host + $@":5000\images\control-f5.com-{fileIdentifier}{fileExtension}"
                : request.Host.Host + $@"\images\control-f5.com-{fileIdentifier}{fileExtension}";
        }

        public void DeleteFromDisk(IList<ImageDto> images)
        {
            var root = hostingEnv.WebRootPath;
            if (images.Any())
            {
                foreach (var image in images)
                {
                    var index = image.Link.IndexOf("control-f5.com");
                    var imagePath = $"{root}\\{image.Link.Substring(index)}";

                    System.IO.File.Delete(imagePath);
                }
            }
        }
    }
}
