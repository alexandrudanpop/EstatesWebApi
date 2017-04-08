namespace Api.Model
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Image
    {
        [BsonRequired]
        public string EstateId { get; set; }

        [BsonId]
        public string Id { get; set; }

        [BsonRequired]
        public string Link { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return nameof(Image);
        }
    }
}