﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;

using DTO.DTO;
using Api.DAL.DataServices;
using Api.Validators;
using Api.IO;

namespace Api.Controllers
{
    public class ImageController : Controller
    {
        private readonly IDataService<ImageDto> dataService;

        private readonly IValidator<IFormFile> validator;

        private readonly ImageIoService imageIoService;

        public ImageController(IDataService<ImageDto> dataService, IValidator<IFormFile> validator, ImageIoService imageIoService)
        {
            this.dataService = dataService;
            this.validator = validator;
            this.imageIoService = imageIoService;
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

                    int estateId;
                    GetEstateId(out estateId, file);

                    var filename = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"');

                    var fileIdentifier = Guid.NewGuid().ToString();
                    imageIoService.SaveOnDisk(file, filename, fileIdentifier);

                    var link = imageIoService.CreateServerLink(Request, filename, fileIdentifier);
                    var imageId = SaveInDb(file, estateId, link);

                    return Ok(new ImageDto(imageId, estateId, string.Empty, link));
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

        private static bool GetEstateId(out int estateId, IFormFile file)
        {
            return int.TryParse(file.Name, out estateId);
        }

        private int SaveInDb(IFormFile file, int estateId, string link)
        {
            var imageId =  dataService.Create(new ImageDto(0, estateId, file.FileName, link));

            if (!imageId.HasValue)
            {
                throw new Exception("Could not save iamge in DB");
            }

            return imageId.Value;
        }
    }
}
