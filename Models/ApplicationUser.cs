using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace storeAPI.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Id { get; set; }
        [Required,MaxLength(50)]
        public string Name {get; set;}

    public List<Order> Orders {get; set;}

        
    }
}
