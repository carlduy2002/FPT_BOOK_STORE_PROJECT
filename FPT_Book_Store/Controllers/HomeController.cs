using FPT_Book_Store.Constants;
using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_Book_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if(User.IsInRole("Admin")){
                return Redirect("Admin/Dashboard/Index");
            }else if(User.IsInRole("Owner")){
                return Redirect("Owner/Dashboard/Index");
            }
            IEnumerable<Book> ds = _db.Books.Where(b => b.Book_Deleted == Status.Existing.ToString()).ToList();
            return View(ds);
        }

        public IActionResult ShowDetail(int id)
        {

            Book obj = _db.Books.Where(b => b.Book_ID == id).Include(b => b.Category).Include(b =>b.Publisher).First();
            

            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Help()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult SearchBookName(Book name)
        {
            List<Book> books = _db.Books.ToList();

            if(name.Book_Name != null){
                List<Book> filteredBook = books.Where(m => m.Book_Name.ToLower().
                    Contains(name.Book_Name.ToLower())).OrderBy(quantity =>quantity.Book_Quantity).ToList();
                
                if(filteredBook.Count() == 0){

                    TempData["message"] = "Couldn't find any book titles matching your keywords!";
                    
                }else if((filteredBook.Count() == 1)){

                    TempData["message"] = "Found " + filteredBook.Count() + " book matching the keyword";
                }else{

                    TempData["message"] = "Found " + filteredBook.Count() + " books matching the keyword";
                }
                return View("Index", filteredBook);
            }
            else{
                return View("Index", books);
            }
        
        }

    }
}