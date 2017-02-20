namespace DTO.DTO
{
    public class ImageDto
    {
        public ImageDto(string id, string estateId, string name, string link)
        {
            Id = id;
            EstateId = estateId;
            Name = name;
            Link = link;
        }

        public string Id { get; }

        public string EstateId { get; }

        public string Name { get; }

        public string Link { get; }
	}
}