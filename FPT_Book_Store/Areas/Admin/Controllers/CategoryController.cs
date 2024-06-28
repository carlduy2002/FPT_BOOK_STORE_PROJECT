using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Book_Store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult ShowCategory()
        {
            IEnumerable<Category> ds = _db.Categories.ToList();
            return View(ds);
        }

        public IActionResult Confirm(int id)
        {
            IEnumerable<Category> ds = _db.Categories.Where(c => c.Category_ID == id).ToList();

            if(ds.Count() > 0){
                foreach(var item in ds){
                    item.Category_TypeConfirm = item.Category_Type;
                    item.Category_Status = Constants.Status.Approved.ToString();
                    _db.Categories.Update(item);
                    _db.SaveChanges();
                    break;
                }
            }
            return RedirectToAction("ShowCategory");
        }      
        
    }
}