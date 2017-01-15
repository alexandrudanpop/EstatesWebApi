using DTO.DTO;
using System;
using System.Collections.Generic;
using WebApp.Model;

namespace WebApp.DAL.DataServices
{
    public class ImageDataService : IDataService<ImageDto>
    {
        private readonly IRepository repository;

        public ImageDataService(IRepository repository)
        {
            this.repository = repository;
        }

        public int? Create(ImageDto dto)
        {
            var image = new Image { EstateId = dto.EstateId, Name = dto.Name, Link = dto.Link };
            repository.Add(image);
            repository.SaveChanges();

            return 0;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ImageDto> GetAll()
        {
            throw new NotImplementedException();
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
