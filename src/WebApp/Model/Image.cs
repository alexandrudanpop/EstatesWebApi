using System;

namespace WebApp.Model
{
    public class Image
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int OwnerTypeId { get; set; }
        public Guid Source { get; set; }
    }
}