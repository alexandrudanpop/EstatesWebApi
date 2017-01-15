using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System;
using WebApp.DAL;
using WebApp.Model;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment hostingEnv;

        private readonly IRepository repository;

        const int MaxFileLengthInBytes = 500000; // 500 Kb 

        public ImageController(IHostingEnvironment hostingEnv, IRepository repository)
        {
            this.hostingEnv = hostingEnv;
            this.repository = repository;
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
                    // validator - for now could only add 100 img so we don't get spammed on our server

                    var file = Request.Form.Files[0];

                    if (file.Length > MaxFileLengthInBytes)
                    {
                        return this.BadRequest("Image must be lower than 500 Kb in size");
                    }

                    var filename = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"');

                    var fileExtension = System.IO.Path.GetExtension(filename);

                    int estateId;
                    if (!int.TryParse(file.Name, out estateId))
                    {
                        return BadRequest();
                    }

                    filename = hostingEnv.WebRootPath + $@"\control-f5.com-{Guid.NewGuid().ToString()} {fileExtension}";

                    // todo - should resize images to a standard width? 
                    using (var fs = System.IO.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    // insert in DB 
                    repository.Add(new Image
                    {
                        EstateId = estateId,
                        Name = file.FileName,
                        Link = filename
                    });
                    repository.SaveChanges();

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
