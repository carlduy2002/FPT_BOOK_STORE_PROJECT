using FPT_Book_Store.Constants;
using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Book_Store.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Route("Owner/[controller]/[action]")]
    [Authorize(Roles = "Owner")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> ds = _db.Categories.Where(c => c.Category_Deleted == Status.Existing.ToString()).ToList();
            return View(ds);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                obj.Category_Status = Status.Pending.ToString();
                obj.Category_Deleted = Status.Existing.ToString();
                _db.Categories.Add(obj);
                _db.SaveChanges();
            
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int id)
        {
            Category obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                obj.Category_TypeConfirm = obj.Category_TypeConfirm;
                obj.Category_Status = Status.Pending.ToString();
                obj.Category_Deleted = Status.Existing.ToString();
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Delete(int id)
        {
            Category obj = _db.Categories.Find(id);
            if(obj != null){
                obj.Category_Deleted = Status.Deleted.ToString();
                _db.Categories.Update(obj);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}