using DTO.DTO;
using WebApp.Model;
using System.Linq;
using System.Collections.Generic;

namespace WebApp.DAL.DataServices
{
    public class EstatesDataService : IDataService<EstateTempDto>
    {
        private readonly IRepository repository;

        public EstatesDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyList<EstateTempDto> GetAll()
        {
            return this.repository.GetEntities<Estate>()
                                .Select(x => new EstateTempDto(x.Id, x.Title, x.Price))
                                .ToList()
                                .AsReadOnly();
        }

        public IReadOnlyList<EstateTempDto> GetFilteredBy(string name)
        {
            return this.repository.GetEntities<Estate>()
                                .Where(x => x.Title.Equals(name) || x.Title.Contains(name))
                                .Select(x => new EstateTempDto(x.Id, x.Title, x.Price))
                                .ToList()
                                .AsReadOnly();
        }

        public int? Create(EstateTempDto estate)
        {
            var newEstate = new Estate
            {
                Title = estate.Name,
                Price = estate.Price
            };

            this.repository.Add(newEstate);
            this.repository.SaveChanges();

            var newId = this.repository.GetEntities<Estate>()
                        .Where(e => e.Title == newEstate.Title)
                        .FirstOrDefault()?.Id;

            return newId;
        }

        public bool Update(EstateTempDto estate)
        {
            var editedEstate = this.repository.GetEntities<Estate>()
                                  .Where(e => e.Id == estate.Id)
                                  .FirstOrDefault();
            if (editedEstate == null)
            {
                return false;
            }

            editedEstate.Title = estate.Name;
            editedEstate.Price = estate.Price;

            this.repository.SaveChanges();

            return true;
        }

        public void Delete(int id)
        {
            var estateToDelete = this.repository.GetEntities<Estate>()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();

            if (estateToDelete == null)
            {
                return;
            }

            this.repository.Delete(estateToDelete);
            this.repository.SaveChanges();
        }
    }
}
