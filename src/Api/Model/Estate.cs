using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Api.Model
{
    public class Estate
    {
        [BsonId]
		public string Id { get; set; }

        [BsonRequired]
		public string Title { get; set; }

		// Total Surace (including garden, etc) in square meters
		public int Area { get; set; }

        [BsonRequired]
        public int Price { get; set; }

		public int UsableSurface { get; set; }

		public int TotalSurface { get; set; }

		public int LocationId { get; set; }

        [BsonRequired]
        public List<Image> Images { get; set; }

        public override string ToString()
        {
            return nameof(Estate);
        }
    }
}