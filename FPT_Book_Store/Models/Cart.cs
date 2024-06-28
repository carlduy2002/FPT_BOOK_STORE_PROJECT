using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_Book_Store.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cart_ID { get; set; }


        public string Account_ID { get; set; }
        [ForeignKey("Account_ID")]
        public virtual Accounts? Account { get; set; }


        public int Book_ID { get; set; }
        [ForeignKey("Book_ID")]
        public virtual Book? Book { get; set;}

        public int? Cart_Quantity { get; set; }

        public string? Cart_Deleted { get; set; }
    }
}