using System.ComponentModel.DataAnnotations.Schema;

namespace storeAPI.Models
{
    public class Order
    {

        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }


        public int Status { get; set; }

        public double Price { get; set; }


        public List<OrderProduct> Products { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
