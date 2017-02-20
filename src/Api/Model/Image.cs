using MongoDB.Bson.Serialization.Attributes;

namespace Api.Model
{
    public class Image
    {
        [BsonId]
        public string Id { get; set; }

        [BsonRequired]
        public string EstateId { get; set; }

        public string Name { get; set; }

        [BsonRequired]
        public string Link { get; set; }

        public override string ToString()
        {
            return nameof(Image);
        }
    }
}