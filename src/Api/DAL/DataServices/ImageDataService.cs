namespace Api.DAL.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Api.Model;

    using Core.Contracts.DataService;

    using DTO.DTO;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class ImageDataService : IDataService<ImageDto>
    {
        private readonly MongoDbContext<Image> db;

        public ImageDataService(MongoDbContext<Image> db)
        {
            this.db = db;
        }

        public string Create(ImageDto dto)
        {
            var image = new Image
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                EstateId = dto.EstateId,
                                Name = dto.Name,
                                Link = dto.Link
                            };
            this.db.Collection.InsertOne(image);

            return
                this.db.Collection.AsQueryable()
                    .FirstOrDefault(i => i.EstateId == dto.EstateId && i.Link == dto.Link)?.Id;
        }

        public void Delete(string id)
        {
            this.db.Collection.DeleteOne(x => x.Id == id);
        }

        public IReadOnlyList<ImageDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public ImageDto GetById(string id)
        {
            var image =  db.Collection.AsQueryable().FirstOrDefault(x => x.Id == id);

            return image != null
                ? new ImageDto(image.Id, image.EstateId, image.Name, image.Link)
                : new ImageDto(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public IReadOnlyList<ImageDto> GetFilteredBy(string name)
        {
            throw new NotImplementedException();
        }

        public bool Update(ImageDto dto)
        {
            throw new NotImplementedException();
        }
    }
}