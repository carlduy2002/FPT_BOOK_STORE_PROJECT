using FPT_Book_Store.Constants;
using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_Book_Store.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Route("Owner/[controller]/[action]")]
    [Authorize(Roles = "Owner")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult ShowOrder()
        {
            IEnumerable<Order> ds = _db.Orders.Where(o => o.Order_Status != Status.Canceled.ToString()).
            OrderBy(o => o.Order_Status).Include(a => a.Account).ToList();
            return View(ds);
        }
        

        public IActionResult ConfirmOrder(int id)
        {
            IEnumerable<Order> ds = _db.Orders.Where(o => o.Order_ID == id).ToList();
            if(ds.Count() > 0){
                foreach(var item in ds){
                    item.Order_Status = Status.Received.ToString();
                    item.Order_DeliveryDate = DateTime.Now;
                    _db.Orders.Update(item);
                    _db.SaveChanges();
                    break;
                }
            }
            return RedirectToAction("ShowOrder");
        }
        
    }
}