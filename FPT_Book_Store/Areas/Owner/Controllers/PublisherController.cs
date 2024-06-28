using FPT_Book_Store.Data;
using Microsoft.AspNetCore.Mvc;
using FPT_Book_Store.Models;
using FPT_Book_Store.Constants;
using Microsoft.AspNetCore.Authorization;

namespace FPT_Book_Store.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Route("Owner/[controller]/[action]")]
    [Authorize(Roles = "Owner")]
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Publisher> publishers = _db.Publishers.Where(p => p.Publisher_Deleted == Status.Existing.ToString());

            return View(publishers);
        }

        public IActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public IActionResult Create(Publisher obj)
        {
            if(ModelState.IsValid){
                obj.Publisher_Deleted = Status.Existing.ToString();
                _db.Publishers.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }
      
         

        public IActionResult Edit(int id)
        {
            Publisher publisher = _db.Publishers.Find(id);

            if(publisher == null){

                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        [HttpPost]
        public IActionResult Edit(Publisher obj)
        {

            if(ModelState.IsValid){

                obj.Publisher_Deleted = Status.Existing.ToString();
                _db.Publishers.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }     


        public IActionResult Delete(int id)
        {
            Publisher publisher = _db.Publishers.Find(id);

            if(publisher != null){
                publisher.Publisher_Deleted = Status.Deleted.ToString();
                _db.Publishers.Update(publisher);
                _db.SaveChanges();

            }

            return RedirectToAction("Index");
        }
    }
}