using System.Collections.Generic;
using System.Linq;
using DTO.DTO;
using Api.Model;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Api.DAL.DataServices
{
    public class EstatesDataService : IDataService<EstateTempDto>
    {
        private readonly MongoDbContext<Estate> db;

        public EstatesDataService(MongoDbContext<Estate> db)
        {
            this.db = db;
        }

        public EstateTempDto GetById(int id)
        {
            var estate = db.Collection.AsQueryable()
                .FirstOrDefault(e => e.Id == id);
                //.Include(e => e.Images)

            if (estate != null)
            {
                return new EstateTempDto(estate.Id, estate.Title, estate.Price, estate.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList());
            }

            return new EstateTempDto(0, string.Empty, 0, new List<ImageDto>());
        }

        public IReadOnlyList<EstateTempDto> GetAll()
        {
            return db.Collection.Find(e => true).ToList()
                                .Select(x => 
                                    new EstateTempDto(x.Id, x.Title, x.Price, x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
                                .ToList()
                                .AsReadOnly();
        }

        public IReadOnlyList<EstateTempDto> GetFilteredBy(string name)
        {
            return db.Collection.Find(x => x.Title.Equals(name) || x.Title.Contains(name)).ToList()
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

            db.Collection.InsertOne(newEstate);

            var newId = db.Collection
                        .Find(e => e.Title == newEstate.Title)
                        .FirstOrDefault()?.Id;

            return newId;
        }

        public bool Update(EstateTempDto estate)
        {
            var estateToUpdate = new Estate
            {
                Id = estate.Id,
                Title = estate.Name,
                Price = estate.Price,
                Images = estate.Images.Select(x => new Image
                {
                    Id = x.Id,
                    EstateId = x.EstateId,
                    Link = x.Link,
                    Name = x.Name
                }).ToList()
            };

            var result = db.Collection.ReplaceOne(null, estateToUpdate);

            //if (editedEstate == null)
            //{
            //    return false;
            //}

            //editedEstate.Title = estate.Name;
            //editedEstate.Price = estate.Price;

            //_repository.SaveChanges();

            return true;
        }

        public void Delete(int id)
        {
            //var estateToDelete = _repository
            //            .GetEntities<Estate>()
            //            .FirstOrDefault(e => e.Id == id);

            //if (estateToDelete == null)
            //{
            //    return;
            //}

            //var imagesToDelete = _repository.GetEntities<Image>().Where(i => i.EstateId == id).ToList();
            //imagesToDelete.ForEach(i => _repository.Delete(i));

            //_repository.Delete(estateToDelete);
            //_repository.SaveChanges();
            var filter = Builders<Estate>.Filter.Eq("id", id);
            db.Collection.DeleteOne(filter);
        }
    }
}
