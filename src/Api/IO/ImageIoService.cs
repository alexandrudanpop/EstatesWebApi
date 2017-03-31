namespace Api.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using DTO.DTO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    public class ImageIoService
    {
        private readonly IHostingEnvironment hostingEnv;

        public ImageIoService(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }

        public string CreateServerLink(HttpRequest request, string fileName, string fileIdentifier)
        {
            var fileExtension = Path.GetExtension(fileName);

            // return this.hostingEnv.IsDevelopment()
            // ? request.Host.Host + $@":5000\images\control-f5.com-{fileIdentifier}{fileExtension}"
            // :
            return request.Host.Host + $@"/img/control-f5.com-{fileIdentifier}{fileExtension}";
        }

        public void DeleteFromDisk(IList<ImageDto> images)
        {
            var root = this.hostingEnv.WebRootPath;
            if (images.Any())
            {
                foreach (var image in images)
                {
                    var index = image.Link.IndexOf("/img/control-f5.com", StringComparison.OrdinalIgnoreCase);
                    var imagePath = $@"{root}/{image.Link.Substring(index)}";

                    File.Delete(imagePath);
                }
            }
        }

        public void SaveOnDisk(IFormFile file, string fileName, string fileIdentifier)
        {
            var fileExtension = Path.GetExtension(fileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "img")
                       + $@"/control-f5.com-{fileIdentifier}{fileExtension}";

            using (var fs = File.Create(path))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}