using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        private IHostingEnvironment hostingEnv;

        public ImageController(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }

        [Route("api/images")]
        [HttpPost]
        public async Task Post()
        {
            if (Request.Form.Files.Count != 0)
            {
                var file = Request.Form.Files[0];
                long size = 0;
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = hostingEnv.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (var fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
        }
    }
}
