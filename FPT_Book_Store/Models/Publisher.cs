using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FPT_Book_Store.Constants;

namespace FPT_Book_Store.Models
{
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Publisher_ID { get; set; }

        [Required(ErrorMessage = "Please, enter a publisher name!")]
        [StringLength(50, ErrorMessage = "Please, enter the publisher name length must be between {2} and {1}.", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z ]{1,50}$", 
        ErrorMessage = "Please, enter a valid publisher name!")]
        public string Publisher_Name { get; set; }

        [Required(ErrorMessage ="Please, enter the email!")]
        [StringLength(100)]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        + "@"+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", 
        ErrorMessage = "Please, enter a valid email address!")]
        public string Publisher_Email { get; set; }

        [Required(ErrorMessage ="Please, enter the phone number!")]
        [RegularExpression(@"^0[0-9]{9}", 
        ErrorMessage = "Please, enter a valid phone number!")]
        public string Publisher_Phone { get; set; }

        public string? Publisher_Deleted { get; set; } = Status.Existing.ToString();

        public virtual ICollection<Book>? Books { get; set; }
    }
}