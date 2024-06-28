using System.Security.Claims;
using FPT_Book_Store.Constants;
using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_Book_Store.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/[controller]/[action]")]
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult ViewOrder()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Order> ds = _db.Orders.
            Where(o => o.Account_ID == user && o.Order_Status != Status.Canceled.ToString()).ToList();

            TempData["totalOrder"] = ds.Count();

            return View(ds);
        }
        

        public IActionResult CancelOrder(int id)
        {
            Order order = _db.Orders.Find(id);
            if(order != null){
                order.Order_Status = Status.Canceled.ToString();
                _db.Orders.Update(order);
                _db.SaveChanges();

                IEnumerable<OrderDetail> ds = _db.OrdersDetail.Where(od => od.Order_ID == order.Order_ID).Include(od => od.Book).ToList();
                foreach(var i in ds){
                    i.Book.Book_Quantity = (i.Book.Book_Quantity + i.OrderDetail_Quantity);
                    _db.Books.Update(i.Book);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("ViewOrder");
        }
        
        public IActionResult Detail(int id){
            IEnumerable<OrderDetail> ds = _db.OrdersDetail.Where(o => o.Order_ID == id).Include(a => a.Book).ToList();

            return View(ds);
        }
        
    }
}