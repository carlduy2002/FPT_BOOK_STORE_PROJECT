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
    [Authorize(Roles="User")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult ShowCart()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Cart> ds = _db.Carts.Where(c => c.Account_ID == user).Include(b => b.Book).ToList();

            decimal? total = 0;

            if (ds.Count() >= 1)
            {
                foreach (var i in ds)
                {
                   total += i.Book.Book_SalePrice * i.Cart_Quantity;
                }
            }

            TempData["total"] = total;

            return View(ds);
        }

        public IActionResult AddCart(int id)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = _db.Books.Where(b => b.Book_ID == id).FirstOrDefault();

            IEnumerable<Cart> ls = _db.Carts.Where(c => c.Account_ID == user && c.Cart_Quantity >= 1
            && c.Book_ID == product.Book_ID).ToList();

            if (ls.Count() > 0)
            {
                foreach (var item in ls)
                {
                    item.Cart_Quantity = item.Cart_Quantity + 1;
                    _db.Carts.Update(item);
                    _db.SaveChanges();
                    break;
                }
            }
            else
            {
                Cart a = new Cart();
                a.Account_ID = user;
                a.Book_ID = product.Book_ID;
                a.Cart_Quantity = 1;
                a.Cart_Deleted = Status.Existing.ToString();
                _db.Carts.Add(a);
                _db.SaveChanges();
            }
            return RedirectToAction("ShowCart");
        }


        public IActionResult UpdateIncrease(int id)
        {
            IEnumerable<Cart> ls = _db.Carts.Where(c => c.Cart_ID == id).ToList();

            if (ls.Count() > 0)
            {
                foreach (var item in ls)
                {
                    item.Cart_Quantity = item.Cart_Quantity + 1;
                    _db.Carts.Update(item);
                    _db.SaveChanges();
                    break;
                }
            }

            return RedirectToAction("ShowCart");
        }

        public IActionResult UpdateDescrease(int id)
        {
            IEnumerable<Cart> ls = _db.Carts.Where(c => c.Cart_ID == id).ToList();

            if (ls.Count() > 0)
            {
                foreach (var item in ls)
                {
                    if (item.Cart_Quantity > 0)
                    {
                        item.Cart_Quantity = item.Cart_Quantity - 1;
                        _db.Carts.Update(item);
                        _db.SaveChanges();
                    }

                    if (item.Cart_Quantity <= 0)
                    {
                        Cart cart = _db.Carts.Find(id);
                        _db.Carts.Remove(cart);
                        _db.SaveChanges();
                    }

                    break;
                }
            }

            return RedirectToAction("ShowCart");
        }

        public IActionResult Delete(int id)
        {
            Cart cart = _db.Carts.Find(id);
            if (cart != null)
            {
                _db.Carts.Remove(cart);
                _db.SaveChanges();
                TempData["message"] = "The book has been deleted successfully!";
            }
            return RedirectToAction("ShowCart");
        }


        public IActionResult Order(int id, string address, string phone)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart ds = _db.Carts.Where(c => c.Cart_ID == id).Include(a => a.Account).Include(c => c.Book).First();

            if (address == "" || phone == "")
            {
                TempData["error"] = "To place an order, please enter the address and phone number!";
                return RedirectToAction("ShowCart", "Cart");
            }
                
            Order orders = new Order();
                 
            orders.Account_ID = user;
            orders.Order_Address = address;
            orders.Order_Phone = phone;
            orders.Order_OrderDate = DateTime.Now;
            orders.Order_Status = Status.Pending.ToString();

            _db.Orders.Add(orders);
            _db.SaveChanges();
              
            OrderDetail od = new OrderDetail();
                         
            od.Order_ID = orders.Order_ID;
            od.Book_ID = ds.Book_ID;
            od.OrderDetail_Quantity = (int)ds.Cart_Quantity;

            _db.OrdersDetail.Add(od);
            _db.SaveChanges();

            IEnumerable<Book> lstBook = _db.Books.Where(b => b.Book_ID == ds.Book_ID).ToList();

            foreach(var book in lstBook){
                book.Book_Quantity = ((int)(book.Book_Quantity - ds.Cart_Quantity));
                _db.Books.Update(book);
                _db.SaveChanges();            
            }                   

            _db.Carts.Remove(ds);
            _db.SaveChanges();
               
            TempData["message"] = "Order is Successfully!";
        
            return RedirectToAction("ShowCart", "Cart");
        }


        public IActionResult OrderAll(string address, string phone)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Cart> ds = _db.Carts.Where(c => c.Account_ID == user).Include(a => a.Account).Include(c => c.Book).ToArray();

            if(address == "" || phone == ""){

                TempData["error"] = "To place an order, please enter the address and phone number!";
                return RedirectToAction("ShowCart", "Cart");
            }

            Order order = new Order();

                if (ds.Count() > 0)
                {
                    order.Account_ID = user;
                    order.Order_Phone = phone;
                    order.Order_Address = address;
                    order.Order_OrderDate = DateTime.Now;
                    order.Order_Status = Status.Pending.ToString();

                    _db.Orders.Add(order);
                    _db.SaveChanges();
                }

            Order lst = _db.Orders.Where(o => o.Order_ID == order.Order_ID).First();
            IEnumerable<Cart> carts = _db.Carts.Where(c => c.Account_ID == user).ToList();

                    foreach (var c in carts)
                    {                    
                        OrderDetail od = new OrderDetail();
                        od.Order_ID = lst.Order_ID;
                        od.Book_ID = c.Book_ID;
                        od.OrderDetail_Quantity = (int)c.Cart_Quantity;

                        _db.OrdersDetail.Add(od);
                        _db.SaveChanges();

                        Book book = _db.Books.Where(b => b.Book_ID == c.Book_ID).First();

                        if(book != null){

                            book.Book_Quantity = ((int)(book.Book_Quantity - c.Cart_Quantity));
                            _db.Books.Update(book);
                            _db.SaveChanges();
                        }                               
                    }
                        
                    foreach (var cart in carts)
                    {                       
                        _db.Carts.Remove(cart);
                        _db.SaveChanges();
                    }

                TempData["message"] = "Order is Successfully!";
                   
            return RedirectToAction("ShowCart", "Cart");
        }
  
        public IActionResult ConfirmFormOrder(int id)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart ds = _db.Carts.Where(c => c.Cart_ID == id && c.Account_ID == user).
            Include(a => a.Book).Include(c => c.Book.Category).First();

            ViewData["user"] = _db.Accounts.Where(a => a.Id == user).ToList();

            if (user == "")
            {
                TempData["error"] = "User ID Error!";
                return RedirectToAction("ShowCart", "Cart");
            }

            if (ds.Cart_Quantity > ds.Book.Book_Quantity)
            {
                TempData["message"] = "The " + "\"" + ds.Book.Book_Name + "\"" + " book in stock is only " + ds.Book.Book_Quantity;
                return RedirectToAction("ShowCart", "Cart");
            }

            return View(ds);
        }


        public IActionResult ConfirmFormOrderAll()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Cart> ds = _db.Carts.Where(c => c.Account_ID == user).
            Include(a => a.Account).Include(c => c.Book).Include(c => c.Book.Category).ToList();
            ViewData["user"] = _db.Accounts.Where(a => a.Id == user).ToList();

            if (user == "")
            {
                TempData["error"] = "User ID Error!";
                return RedirectToAction("ShowCart", "Cart");
            }

            if(ds.Count() <= 0){

                TempData["error"] = "Please add the book to the cart!";
                return RedirectToAction("ShowCart", "Cart");
            }

            foreach (var ls in ds)
            {
                if (ls.Cart_Quantity > ls.Book.Book_Quantity)
                {
                    TempData["error"] = "The " + ls.Book.Book_Name + " book in stock is only " + ls.Book.Book_Quantity;
                    return RedirectToAction("ShowCart", "Cart");
                }
            }

            return View(ds);
        }
        
    }
}