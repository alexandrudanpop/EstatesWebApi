using System.Collections.Generic;
using System.Linq;
using DTO.DTO;
using Api.Model;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Api.DAL.DataServices
{
    public class EstatesDataService : IDataService<EstateTempDto>
    {
        private readonly MongoDbContext<Estate> estatesDbCollection;
        private readonly MongoDbContext<Image> imageDbCollection;

        public EstatesDataService(MongoDbContext<Estate> estatesDbCollection, MongoDbContext<Image> imageDbCollection)
        {
            this.estatesDbCollection = estatesDbCollection;
            this.imageDbCollection = imageDbCollection;
        }

        public EstateTempDto GetById(string id)
        {
            var estate = estatesDbCollection.Collection.AsQueryable()
                .FirstOrDefault(e => e.Id == id);

            estate.Images = imageDbCollection.Collection.AsQueryable().Where(i => i.EstateId == id).ToList();

            if (estate != null)
            {
                return new EstateTempDto(estate.Id, estate.Title, estate.Price, estate.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList());
            }

            return new EstateTempDto(string.Empty, string.Empty, 0, new List<ImageDto>());
        }

        public IReadOnlyList<EstateTempDto> GetAll()
        {
            var estates = estatesDbCollection.Collection.Find(new BsonDocument()).Limit(50).ToList();
            var estateIds = estates.Select(e => e.Id).ToArray();

            var filter = Builders<Image>.Filter.Where(i => estateIds.Contains(i.EstateId));
            var images = imageDbCollection.Collection.Find(filter).ToList();

            estates.ForEach(e => e.Images.AddRange(images.Where(i => i.EstateId == e.Id)));

            if (estates != null && estates.Any())
            {
                return estates.Select(x =>
                                    new EstateTempDto(x.Id, x.Title, x.Price, x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
                                .ToList()
                                .AsReadOnly();
            }

            return new List<EstateTempDto>().AsReadOnly();
        }

        public IReadOnlyList<EstateTempDto> GetFilteredBy(string name)
        {
            var estates = estatesDbCollection.Collection.Find(x => x.Title.Equals(name) || x.Title.Contains(name)).ToList();

            var images = imageDbCollection.Collection.AsQueryable()
                .Where(i => estates.Select(e => e.Id).ToArray().Contains(i.EstateId))
                .ToList();

            estates.ForEach(e => e.Images.AddRange(images.Where(i => i.EstateId == e.Id)));

            return estates.Select(x => new EstateTempDto(x.Id, x.Title, x.Price, x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
                        .ToList()
                        .AsReadOnly();
        }

        public string Create(EstateTempDto estate)
        {
            var newEstate = new Estate
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = estate.Name,
                Price = estate.Price,
                Images = new List<Image>()
            };

            estatesDbCollection.Collection.InsertOne(newEstate);

            var newId = estatesDbCollection.Collection
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
                Images = estate.Images != null 
                ? estate.Images.Select(x => new Image
                {
                    Id = x.Id,
                    EstateId = x.EstateId,
                    Link = x.Link,
                    Name = x.Name
                }).ToList()
                : new List<Image>()
            };

            var filter = Builders<Estate>.Filter.Eq(e => e.Id, estate.Id);
            var result = estatesDbCollection.Collection.ReplaceOne(filter, estateToUpdate);

            return true;
        }

        public void Delete(string id)
        {
            var filter = Builders<Estate>.Filter.Eq("_id", id);
            estatesDbCollection.Collection.DeleteOne(filter);
        }
    }
}
