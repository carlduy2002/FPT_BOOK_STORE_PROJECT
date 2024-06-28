using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPT_Book_Store.Areas.Owner.Models
{
    public class Statistic
    {
        public string? Book_Image {get; set;}
        public int? Book_ID  { get; set; }
        public string? Book_Name { get; set; }
        public string? Category_Type {get; set;}
        public string? Publisher_Name {get; set;}
        public int? Book_Quantity { get; set; }
        public int? OrdersDetail_Quantity { get; set; }
    }
}