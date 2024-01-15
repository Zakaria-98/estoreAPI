namespace storeAPI.Dto
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }


        public int Status { get; set; }

        public double Price { get; set; }


        public string ApplicationUserId { get; set; }





    }
}
