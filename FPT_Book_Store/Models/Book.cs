using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FPT_Book_Store.Constants;

namespace FPT_Book_Store.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Book_ID { get; set; }


        public int Publisher_ID { get; set; }
        [ForeignKey("Publisher_ID")]
        public virtual Publisher? Publisher { get; set; }


        public int Category_ID { get; set; }
        [ForeignKey("Category_ID")]
        public virtual Category? Category { get; set; }


        [Required(ErrorMessage = "Please, enter the book name!")]
        [StringLength(50, ErrorMessage = "Please, enter the book name must be between {2} and {1}.", MinimumLength = 1)]
        public string Book_Name { get; set; }


        public string? Book_Description { get; set; }


        [Display(Name = "Quantity")]
        [Range(0, 10000)]
        public int Book_Quantity { get; set; }


        [Display(Name = "Original Price")]
        [Range(0, 1000000000)]
        public decimal Book_OriginalPrice { get; set; }


        [Display(Name = "Sale Price")]
        [Range(0, 1000000000)]
        public decimal Book_SalePrice { get; set; }


        public DateTime? Book_Date { get; set; }


        public string? Book_Image { get; set; }


        public string? Book_Deleted { get; set; } = Status.Existing.ToString();


        public virtual ICollection<Cart>? Carts { get; set; }


        public virtual ICollection<OrderDetail>? OrdersDetail { get; set; }
    }
}