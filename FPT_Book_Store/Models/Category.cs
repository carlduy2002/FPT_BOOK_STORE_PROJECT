using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FPT_Book_Store.Constants;

namespace FPT_Book_Store.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Category_ID { get; set; }


        [Required(ErrorMessage = "Please, enter the category!")]
        [StringLength(30, ErrorMessage = "Please, enter the category must be between {2} and {1}.", MinimumLength = 1)]
        public string Category_Type { get; set; }


        public string? Category_TypeConfirm { get; set; }


        public string? Category_Status { get; set; } = Status.Pending.ToString();


        public string? Category_Deleted { get; set; } = Status.Existing.ToString();

        
        public virtual ICollection<Book>? Books { get; set; }
    }
}