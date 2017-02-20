using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Api.Model;
using MongoDB.Driver;

namespace Api.Validators
{
    public class ImageValidator : IValidator<IFormFile>
    {
        const int MaxFileLengthInBytes = 500000; // 500 Kb 
        const string ImageMaxSizeValidation = "Image must be lower than 500 Kb in size";
        const string NotAnImageValidation = "Provided file is not an image";
        const string ServerFullValidation = "Ups! Server is full of images.";
        const string NoEstateIdValidation = "No estate Id provided";

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

        private readonly MongoDbContext<Image> db;

        public ImageValidator(MongoDbContext<Image> db)
        {
            this.db = db;
        }

        public IReadOnlyList<string> Validate(IFormFile dto)
        {
            var validations = new List<string>();

            if (dto.Length > MaxFileLengthInBytes)
            {
                validations.Add(ImageMaxSizeValidation);
            }

            var fileExtension = System.IO.Path.GetExtension(dto.FileName.Trim());

            if (!possibleImageExtensions.Contains(fileExtension))
            {
                validations.Add(NotAnImageValidation);
            }

            if (db.Collection.AsQueryable().Count() > 100)
            {
                validations.Add(ServerFullValidation);
            }

            if (string.IsNullOrEmpty(dto.Name))
            {
                validations.Add(NoEstateIdValidation);
            }

            return validations;
        }
    }
}
