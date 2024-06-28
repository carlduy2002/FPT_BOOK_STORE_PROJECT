using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_Book_Store.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_ID { get; set; }


        public string Account_ID { get; set; }
        [ForeignKey("Account_ID")]
        public virtual Accounts? Account { get; set; }


        [Required(ErrorMessage = "Please, enter the phone number!")]
        [RegularExpression(@"^0[0-9]{9}",
        ErrorMessage = "Please, enter a valid phone number!")]
        public string Order_Phone { get; set; }


        [Required(ErrorMessage = "Please, enter the address!")]
        [StringLength(100)]
        public string Order_Address { get; set; }


        public DateTime? Order_OrderDate { get; set; }


        public DateTime? Order_DeliveryDate { get; set; }


        public string? Order_Status { get; set; }


        public virtual ICollection<OrderDetail>? OrdersDetail { get; set; }
    }
}
