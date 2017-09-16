// tbd this is the model for Estate that I want in the future, that will replace EstateTempDto

namespace DTO.DTO
{
    using System.Collections.Generic;

    public class EstateDto
    {
        // Total Surace (including garden, etc) in square meters
        public int Area { get; set; }

        public int Id { get; set; }

        public List<ImageDto> Images { get; set; }

        public int LocationId { get; set; }

        public int Price { get; set; }

        public string Title { get; set; }

        public int TotalSurface { get; set; }

        public int UsableSurface { get; set; }
    }
}