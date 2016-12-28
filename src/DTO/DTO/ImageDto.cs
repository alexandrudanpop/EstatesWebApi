using System;

namespace DTO.DTO{
	public class ImageDto{
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int OwnerTypeId { get; set; }
        public Guid Source { get; set; }
	}
}