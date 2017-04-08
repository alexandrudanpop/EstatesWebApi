namespace Api.Model
{
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization.Attributes;

    public class Estate
    {
        // Total Surace (including garden, etc) in square meters
        public int Area { get; set; }

        [BsonId]
        public string Id { get; set; }

        [BsonRequired]
        public List<Image> Images { get; set; }

        public int LocationId { get; set; }

        [BsonRequired]
        public int Price { get; set; }

        [BsonRequired]
        public string Title { get; set; }

        public int TotalSurface { get; set; }

        public int UsableSurface { get; set; }

        public override string ToString()
        {
            return nameof(Estate);
        }
    }
}