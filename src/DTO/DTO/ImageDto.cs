namespace DTO.DTO
{
    public class ImageDto
    {
        public ImageDto(int id, int estateId, string name, string link)
        {
            Id = id;
            EstateId = estateId;
            Name = name;
            Link = link;
        }

        public int Id { get; }

        public int EstateId { get; }

        public string Name { get; }

        public string Link { get; }
	}
}