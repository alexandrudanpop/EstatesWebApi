using System.Collections.Generic;

namespace Api.Model
{
    public class Estate
    {
			public int Id { get; set; }
			public string Title { get; set; }

			// Total Surace (including garden, etc) in square meters
			public int Area { get; set; }
			public int Price { get; set; }
			public int UsableSurface { get; set; }
			public int TotalSurface { get; set; }
			public int LocationId { get; set; }
			public List<Image> Images { get; set; }
    }
}