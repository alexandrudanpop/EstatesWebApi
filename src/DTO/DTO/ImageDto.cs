namespace DTO.DTO
{
    public class ImageDto
    {
        public ImageDto(string id, string estateId, string name, string link)
        {
            this.Id = id;
            this.EstateId = estateId;
            this.Name = name;
            this.Link = link;
        }

        public string EstateId { get; }

        public string Id { get; }

        public string Link { get; }

        public string Name { get; }
    }
}