using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System;
using WebApp.Validators;
using Microsoft.AspNetCore.Http;
using System.Linq;
using DTO.DTO;
using WebApp.DAL.DataServices;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment hostingEnv;

        private readonly IDataService<ImageDto> dataService;

        private readonly IValidator<IFormFile> validator;

        public ImageController(IHostingEnvironment hostingEnv, IDataService<ImageDto> dataService, IValidator<IFormFile> validator)
        {
            this.hostingEnv = hostingEnv;
            this.dataService = dataService;
            this.validator = validator;
        }

        [Route("api/images")]
        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                if (Request.Form.Files.Count != 0)
                {
                    var file = Request.Form.Files[0];
                    var validationErrors = validator.Validate(file);

                    if (validationErrors.Any())
                    {
                        return BadRequest(validationErrors);
                    }

                    var filename = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"');

                    int estateId;
                    if (!int.TryParse(file.Name, out estateId))
                    {
                        return BadRequest();
                    }

                    var link = GetLink(filename);

                    SaveOnDisk(file, link);
                    SaveInDb(file, estateId, link);

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

        private void SaveInDb(IFormFile file, int estateId, string link)
        {
            dataService.Create(new ImageDto(0, estateId, file.FileName, link));
        }

        private static void SaveOnDisk(IFormFile file, string link)
        {
            using (var fs = System.IO.File.Create(link))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        private string GetLink(string filename)
        {
            var fileExtension = System.IO.Path.GetExtension(filename);
            return hostingEnv.WebRootPath + $@"\control-f5.com-{Guid.NewGuid().ToString()} {fileExtension}";
        }
    }
}
