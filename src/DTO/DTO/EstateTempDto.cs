using System.Collections.Generic;

namespace DTO.DTO
{
    public class EstateTempDto
    {
        public EstateTempDto(int id, string name, int price, IList<ImageDto> images)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Images = images;
        }

        public int Id { get; }

        public string Name { get; }

        public int Price { get; }

        public IList<ImageDto> Images { get; }
    }
}
