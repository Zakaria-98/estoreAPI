using System.ComponentModel.DataAnnotations;

namespace storeAPI.Dto
{
    public class RegisterDto
    {
        [Required, StringLength(100)]
        public string  Name { get; set; }

        public string UserName { get; set; }



        [Required, StringLength(50)]
        public string Email { get; set; }



        [Required, StringLength(256)]
        public string Password { get; set; }
    }
}
