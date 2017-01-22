using Api.DAL;
using Api.Model;
using DTO.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Api.Validators
{
    public class EstateValidator : IValidator<EstateTempDto>
    {
        const string NameUniqueValidation = "name must be unique";

        const string PriceLargerThanZeroValidation = "price must be larger than 0";

        private readonly IRepository repository;

        public EstateValidator(IRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyList<string> Validate(EstateTempDto dto)
        {
            var validations = new List<string>();

            if (dto.Id == 0 && repository.GetEntities<Estate>().Any(e => e.Title.Equals(dto.Name)))
            {
                validations.Add(NameUniqueValidation);
            }
            else if (dto.Id != 0 && repository.GetEntities<Estate>().Any(e => e.Id != dto.Id && e.Title.Equals(dto.Name)))
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