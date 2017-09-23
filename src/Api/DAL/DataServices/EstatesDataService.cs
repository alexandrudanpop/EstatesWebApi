namespace Api.DAL.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Api.Model;

    using Core.Contracts.DataService;
    using Core.Extensions;

    using DTO.DTO;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class EstatesDataService : IDataService<EstateTempDto>
    {
        private readonly MongoDbContext<Estate> estatesDbCollection;

        private readonly MongoDbContext<Image> imageDbCollection;

        public EstatesDataService(MongoDbContext<Estate> estatesDbCollection, MongoDbContext<Image> imageDbCollection)
        {
            this.estatesDbCollection = estatesDbCollection;
            this.imageDbCollection = imageDbCollection;
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

            this.estatesDbCollection.Collection.InsertOne(newEstate);

            var newId = this.estatesDbCollection.Collection.Find(e => e.Title == newEstate.Title).FirstOrDefault()?.Id;

            return newId;
        }

        public void Delete(string id)
        {
            var estateFilter = Builders<Estate>.Filter.Eq("_id", id);
            this.estatesDbCollection.Collection.DeleteOne(estateFilter);

            // todo should move this in image data service
            var imageFilter = Builders<Image>.Filter.Eq("EstateId", id);
            this.imageDbCollection.Collection.DeleteMany(imageFilter);
        }

        public IReadOnlyList<EstateTempDto> GetAll()
        {
            var estates = this.estatesDbCollection.Collection.Find(new BsonDocument()).Limit(50).ToList();
            var images = this.GetEstatesImages(estates);

            estates.ForEach(e => e.Images.AddRange(images.Where(i => i.EstateId == e.Id)));

            if (estates.Any())
            {
                return
                    estates.Select(
                            x =>
                                new EstateTempDto(
                                    x.Id,
                                    x.Title,
                                    x.Price,
                                    x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
                        .ToList()
                        .AsReadOnly();
            }

            return new List<EstateTempDto>().AsReadOnly();
        }

        public EstateTempDto GetById(string id)
        {
            var estate = this.estatesDbCollection.Collection.AsQueryable().FirstOrDefault(e => e.Id == id);

            estate.Images = this.imageDbCollection.Collection.AsQueryable().Where(i => i.EstateId == id).ToList();

            return new EstateTempDto(
                estate.Id,
                estate.Title,
                estate.Price,
                estate.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList());
        }

        public IReadOnlyList<EstateTempDto> GetFilteredBy(string name)
        {
            var seachName = name.ToLower();

            var caseInsensitiveFilter = Builders<Estate>.Filter.Regex(e => e.Title, seachName.ToCaseInsensitiveRegex());
            var estates = this.estatesDbCollection.Collection.Find(caseInsensitiveFilter).ToList();

            if (!estates.Any())
            {
                foreach (var word in name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrEmpty(word))
                    {
                        continue;
                    }

                    var wordFilter = Builders<Estate>.Filter.Regex(e => e.Title, word.ToCaseInsensitiveRegex());
                    estates = this.estatesDbCollection.Collection.Find(wordFilter).ToList();

                    if (!estates.Any())
                    {
                        estates =
                            this.estatesDbCollection.Collection.Find(e => e.Title.Contains(word)).ToList();
                    }

                    if (estates.Any())
                    {
                        break;
                    }
                }
            }

            if (!estates.Any())
            {
                estates =
                    this.estatesDbCollection.Collection.Find(e => e.Title.Equals(name) || e.Title.Contains(name))
                        .ToList();
            }

            var images = this.GetEstatesImages(estates);

            estates.ForEach(e => e.Images.AddRange(images.Where(i => i.EstateId == e.Id)));

            return
                estates.Select(
                        x =>
                            new EstateTempDto(
                                x.Id,
                                x.Title,
                                x.Price,
                                x.Images.Select(i => new ImageDto(i.Id, i.EstateId, i.Name, i.Link)).ToList()))
                    .ToList()
                    .AsReadOnly();
        }

        public bool Update(EstateTempDto estate)
        {
            var estateToUpdate = new Estate
                                     {
                                         Id = estate.Id,
                                         Title = estate.Name,
                                         Price = estate.Price,
                                         Images =
                                             estate.Images != null
                                                 ? estate.Images.Select(
                                                     x =>
                                                         new Image
                                                             {
                                                                 Id = x.Id,
                                                                 EstateId = x.EstateId,
                                                                 Link = x.Link,
                                                                 Name = x.Name
                                                             }).ToList()
                                                 : new List<Image>()
                                     };

            var filter = Builders<Estate>.Filter.Eq(e => e.Id, estate.Id);
            var restult = this.estatesDbCollection.Collection.ReplaceOne(filter, estateToUpdate);

            return restult.IsAcknowledged;
        }

        private List<Image> GetEstatesImages(IEnumerable<Estate> estates)
        {
            var estateIds = estates.Select(e => e.Id).ToArray();

            var filter = Builders<Image>.Filter.Where(i => estateIds.Contains(i.EstateId));
            var images = this.imageDbCollection.Collection.Find(filter).ToList();
            return images;
        }
    }
}