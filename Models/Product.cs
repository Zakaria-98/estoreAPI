namespace storeAPI.Models
{
    public class Product
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] image { get; set; }

        public float Price { get; set; }

        public int ProductQty { get; set; }

        public double rating { get; set; }

        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }


        public List<OrderProduct> Orders { get; set; }

        


    }
}
