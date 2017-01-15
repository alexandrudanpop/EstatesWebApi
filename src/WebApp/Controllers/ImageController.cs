using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System;

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
        public IActionResult Post()
        {
            try
            {               
                if (Request.Form.Files.Count != 0)
                {
                    // todo - validations - should filer files by file type - only img types allowed

                    var file = Request.Form.Files[0];

                    int estateId;
                    if (!int.TryParse(file.Name, out estateId))
                    {
                        return BadRequest();
                    }

                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    filename = hostingEnv.WebRootPath + $@"\{filename}";

                    // todo do we need the file size?
                    var size = file.Length;

                    // todo - should resize images to a standard width? 
                    using (var fs = System.IO.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
