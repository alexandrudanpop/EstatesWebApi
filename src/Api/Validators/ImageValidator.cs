namespace Api.Validators
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Api.DAL;
    using Api.Model;

    using Microsoft.AspNetCore.Http;

    using MongoDB.Driver;

    public class ImageValidator : IValidator<IFormFile>
    {
        private const string ImageMaxSizeValidation = "Image must be lower than 500 Kb in size";

        private const int MaxFileLengthInBytes = 500000; // 500 Kb 

        private const string NoEstateIdValidation = "No estate Id provided";

        private const string NotAnImageValidation = "Provided file is not an image";

        private const string ServerFullValidation = "Ups! Server is full of images.";

        private readonly MongoDbContext<Image> db;

        private readonly List<string> possibleImageExtensions = new List<string>
                                                                    {
                                                                        ".jpg",
                                                                        ".png",
                                                                        ".bmp",
                                                                        ".gif",
                                                                        ".tif",
                                                                    };

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

            var fileExtension = Path.GetExtension(dto.FileName.Trim());

            if (!this.possibleImageExtensions.Contains(fileExtension.ToLower()))
            {
                validations.Add(NotAnImageValidation);
            }

            if (this.db.Collection.AsQueryable().Count() > 100)
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