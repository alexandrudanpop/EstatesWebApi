namespace DTO.DTO
{
    public class EstateTempDto
    {
        public EstateTempDto(int id, string name, int price)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
        }

        public int Id { get; }
        public string Name { get; }
        public int Price { get; }
    }
}
