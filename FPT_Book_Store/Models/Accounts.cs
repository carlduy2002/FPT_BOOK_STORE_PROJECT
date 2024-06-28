using FPT_Book_Store.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FPT_Book_Store.Models
{
    public class Accounts : IdentityUser
    {
        [Required(ErrorMessage = "Please, enter the address!")]
        [StringLength(100)]
        public string Account_Address { get; set; }

        public string? Account_Image { get; set; }

        public string? Account_Deleted { get; set; } = Status.Existing.ToString();

        public virtual ICollection<Order>? Orders { get; set; }

        public virtual ICollection<Cart>? Carts { get; set; }
    }
}
