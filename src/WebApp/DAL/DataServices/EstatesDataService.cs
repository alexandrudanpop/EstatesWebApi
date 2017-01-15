using System.Collections.Generic;
using System.Linq;
using DTO.DTO;
using WebApp.Model;

namespace WebApp.DAL.DataServices
{
    public class EstatesDataService : IDataService<EstateTempDto>
    {
        private readonly IRepository _repository;

        public EstatesDataService(IRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<EstateTempDto> GetAll()
        {
            return _repository.GetEntities<Estate>()
                                .Select(x => 
                                    new EstateTempDto(x.Id, x.Title, x.Price, x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
                                .ToList()
                                .AsReadOnly();
        }

        public IReadOnlyList<EstateTempDto> GetFilteredBy(string name)
        {
            return _repository.GetEntities<Estate>()
                                .Where(x => x.Title.Equals(name) || x.Title.Contains(name))
                                .Select(x => 
                                    new EstateTempDto(x.Id, x.Title, x.Price, x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
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

            _repository.Add(newEstate);
            _repository.SaveChanges();

            var newId = _repository
                        .GetEntities<Estate>()
                        .FirstOrDefault(e => e.Title == newEstate.Title)?.Id;

            return newId;
        }

        public bool Update(EstateTempDto estate)
        {
            var editedEstate = _repository
                                  .GetEntities<Estate>()
                                  .FirstOrDefault(e => e.Id == estate.Id);
            if (editedEstate == null)
            {
                return false;
            }

            editedEstate.Title = estate.Name;
            editedEstate.Price = estate.Price;

            _repository.SaveChanges();

            return true;
        }

        public void Delete(int id)
        {
            var estateToDelete = _repository
                        .GetEntities<Estate>()
                        .FirstOrDefault(e => e.Id == id);

            if (estateToDelete == null)
            {
                return;
            }

            var imagesToDelete = _repository.GetEntities<Image>().Where(i => i.EstateId == id).ToList();
            imagesToDelete.ForEach(i => _repository.Delete(i));

            _repository.Delete(estateToDelete);
            _repository.SaveChanges();
        }
    }
}
