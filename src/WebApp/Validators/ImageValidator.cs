using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebApp.DAL;
using WebApp.Model;
using System.Linq;

namespace WebApp.Validators
{
    public class ImageValidator : IValidator<IFormFile>
    {
        const int MaxFileLengthInBytes = 500000; // 500 Kb 

        private readonly List<string> possibleImageExtensions = new List<string>
        {
            ".jpg",
            ".png",
            ".bmp",
            ".gif",
            ".tif",
            ".JPG",
            ".PNG",
            ".BMP",
            ".GIF",
            ".TIF"
        };

        private readonly IRepository repository;

        public ImageValidator(IRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyList<string> Validate(IFormFile dto)
        {
            var validations = new List<string>();

            if (dto.Length > MaxFileLengthInBytes)
            {
                validations.Add("Image must be lower than 500 Kb in size");
            }

            var fileExtension = System.IO.Path.GetExtension(dto.FileName.Trim());

            if (!possibleImageExtensions.Contains(fileExtension))
            {
                validations.Add("Provided file is not an image");
            }

            if (repository.GetEntities<Image>().Count() > 100)
            {
                validations.Add("Ups! Server is full of images.");
            }

            int id;
            if (!int.TryParse(dto.Name, out id))
            {
                validations.Add("No estate Id provided");
            }

            return validations;
        }
    }
}
