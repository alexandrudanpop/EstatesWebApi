namespace Api.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using Api.DAL;
    using Api.Model;

    using DTO.DTO;

    using MongoDB.Driver;

    public class EstateValidator : IValidator<EstateTempDto>
    {
        private const string NameUniqueValidation = "name must be unique";

        private const string PriceLargerThanZeroValidation = "price must be larger than 0";

        private readonly MongoDbContext<Estate> db;

        public EstateValidator(MongoDbContext<Estate> db)
        {
            this.db = db;
        }

        public IReadOnlyList<string> Validate(EstateTempDto dto)
        {
            var validations = new List<string>();

            if (dto.Id == string.Empty && this.db.Collection.AsQueryable().Any(e => e.Title.Equals(dto.Name)))
            {
                validations.Add(NameUniqueValidation);
            }
            else if (dto.Id != string.Empty
                     && this.db.Collection.AsQueryable().Any(e => e.Id != dto.Id && e.Title.Equals(dto.Name)))
            {
                validations.Add(NameUniqueValidation);
            }

            if (dto.Price <= 0)
            {
                validations.Add(PriceLargerThanZeroValidation);
            }

            return validations;
        }
    }
}