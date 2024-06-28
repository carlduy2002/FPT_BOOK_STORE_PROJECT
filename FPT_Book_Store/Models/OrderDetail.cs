using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_Book_Store.Models
{
    public class OrderDetail
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int OrderDetail_ID {get;set;}

        public int Order_ID {get;set;}
        [ForeignKey("Order_ID")]
        public virtual Order? Order {get;set;}

        public int Book_ID {get;set;}
        [ForeignKey("Book_ID")]
        public virtual Book? Book {get;set;}

        [Display(Name = "Quantity")]
        [Range(1, 1000000000)]
        public int OrderDetail_Quantity {get;set;}
    }
}