namespace storeAPI.Dto
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile image { get; set; }

        public float Price { get; set; }

        public double rating { get; set; }

        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

    }
}
