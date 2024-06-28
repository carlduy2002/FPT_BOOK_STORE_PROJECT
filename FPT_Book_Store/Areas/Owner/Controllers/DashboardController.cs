using FPT_Book_Store.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FPT_Book_Store.Constants;
using FPT_Book_Store.Areas.Owner.Models;

namespace FPT_Book_Store.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Route("Owner/[controller]/[action]")]
    [Authorize(Roles = "Owner")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Statistic> list = _db.OrdersDetail.
            Where(o => o.Order.Order_Status == Status.Received.ToString()).GroupBy(o => o.Book.Book_ID).
            Select(t => new Statistic
            {
                Book_Image = t.First().Book.Book_Image,
                Book_ID = t.Key,
                Book_Name = t.First().Book.Book_Name,
                Category_Type = t.First().Book.Category.Category_Type,
                Publisher_Name = t.First().Book.Publisher.Publisher_Name,
                Book_Quantity = t.First().Book.Book_Quantity,
                OrdersDetail_Quantity = t.Sum(o => o.OrderDetail_Quantity)

            }).ToList();
           

            return View(list);
        }
        
    }
}