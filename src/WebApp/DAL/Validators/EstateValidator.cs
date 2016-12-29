using DTO.DTO;
using System.Collections.Generic;
using WebApp.Model;
using System.Linq;

namespace WebApp.DAL.Validators
{
    public class EstateValidator : IValidator<EstateTempDto>
    {
        const string nameUniqueValidation = "name must be unique";

        const string priceLargerThanZeroValidation = "price must be larger than 0";

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
                validations.Add(nameUniqueValidation);
            }
            else if (dto.Id != 0 && repository.GetEntities<Estate>().Any(e => e.Id != dto.Id && e.Title.Equals(dto.Name)))
            {
                validations.Add(nameUniqueValidation);
            }

            if (dto.Price <= 0)
            {
                validations.Add(priceLargerThanZeroValidation);
            }

            return validations;
        }
    }
}